// <copyright file="OpenCLService.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023-present Hamlet Tanyavong
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>

#pragma warning disable IDE0032
#pragma warning disable IDE0058

#if NET10_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.InteropServices;
using Mathematics.NET.Exceptions;
using Mathematics.NET.GPU.OpenCL.Core;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an Mathematics.NET OpenCL service.</summary>
public sealed partial class OpenCLService : IComputeService
{
    /// <summary>A class containing names of common vendors.</summary>
    public static class Vendors
    {
        public const string AMD = "Advanced Micro Devices, Inc.";
        public const string Intel = "Intel® Corporation";
        public const string NVIDIA = "NVIDIA Corporation";
    }

    private ILogger<OpenCLService> _logger;
    private CL _cl;

    private Platform _platform;
    private ReadOnlyMemory<Device> _devices;
    private Context _context;
    private Program _program;

#if NET10_0_OR_GREATER
    [Experimental("EXP0001", Message = "This is an experimental implementaion of OpenCL in Mathematics.NET", UrlFormat = "https://mathematics.hamlettanyavong.com/docs/diagnostic-messages/experimental/exp0001")]
#endif
    public unsafe OpenCLService(ILogger<OpenCLService> logger, string vendor, Func<Device, bool> filter, bool useFirst = false, params ReadOnlySpan<string> options)
    {
        _logger = logger;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            _cl = CL.GetApi();
        else
            throw new PlatformNotSupportedException("The OpenCL service is only available on Windows and Linux machines.");

        try
        {
            // OpenCL platform.
            _cl.GetPlatformIDs(0, null, out var platformsSize);
            Span<nint> platformsSpan = new nint[platformsSize];
            _cl.GetPlatformIDs(platformsSize, platformsSpan, []);

            var platforms = new Platform[platformsSpan.Length];
            for (int i = 0; i < platformsSpan.Length; i++)
            {
                platforms[i] = new(_cl, platformsSpan[i]);
            }

            if (platforms.FirstOrDefault(x => x.Vendor == vendor) is Platform platform)
            {
                _platform = platform;
                LogPlatformVendor(_platform.Vendor);
            }
            else
            {
                throw new ComputeServiceException($"There is no platform with the vendor {vendor}.");
            }

            // OpenCL context.
            _context = new Context(_logger, _cl, _platform);

            // OpenCL device.
            _devices = _context.GetDevices(filter, useFirst).ToArray();
            if (_devices.Length == 0)
                throw new ComputeServiceException("There are no devices available after filtering.");

            for (int i = 0; i < _devices.Length; i++)
            {
                LogDeviceUsed(i, _devices.Span[i].Name);
            }

            // OpenCL program.
            _program = new Program(_logger, _cl, _context, _devices.Span, options);
        }
        catch (Exception ex)
        {
            Dispose();
            CouldNotCreateOpenCLService(ex);
            throw;
        }
    }

    public void Dispose()
    {
        if (_program is Program program && program.IsValidInstance)
            program.Dispose();

        if (_context is Context context && context.IsValidInstance)
            context.Dispose();

        foreach (var device in _devices.Span)
        {
            if (device is not null && device.IsValidInstance)
                device.Dispose();
        }

        _cl.Dispose();
    }

    //
    // Logging
    //

    [LoggerMessage(LogLevel.Information, "Platform vendor: {Vendor}.")]
    private partial void LogPlatformVendor(string vendor);

    [LoggerMessage(LogLevel.Information, "Using the device at index, {Index}, with name, {Device}.")]
    private partial void LogDeviceUsed(int index, string device);

    [LoggerMessage(LogLevel.Error, "Unable to create the OpenCL service.")]
    private partial void CouldNotCreateOpenCLService(Exception ex);

    //
    // Properties
    //

    public ReadOnlySpan<Device> Devices => _devices.Span;

    public Program Program => _program;

    //
    // Methods
    //

    public KernelWorkGroupInformation GetKernelWorkGroupInfo(Device device, Kernel kernel) => new(_logger, _cl, device, kernel);
}
