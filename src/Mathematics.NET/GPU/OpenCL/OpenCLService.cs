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

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an Mathematics.NET OpenCL service.</summary>
public sealed class OpenCLService : IComputeService
{
    /// <summary>A class containing names of common vendors.</summary>
    public static class Vendors
    {
        public const string NVIDIA = "NVIDIA Corporation";
    }

    private ILogger<OpenCLService> _logger;
    private CL _cl;

    private ReadOnlyMemory<nint> _platforms;
    private ReadOnlyMemory<nint> _devices;
    private nint _context;
    private nint _commandQueue;
    private nint _program;
    private nint _kernel;

    private nint _platform;
    private ReadOnlyMemory<nint> _selectedDevices;

    private OpenCLService(ILogger<OpenCLService> logger)
    {
        _logger = logger;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _cl = CL.GetApi();
        }
        else
        {
            _logger.LogWarning("OpenCLService is not supported on this operating system.");
            throw new PlatformNotSupportedException();
        }
    }

    // TODO: Use params Span<string> devices when the feature becomes available in C#.
    public OpenCLService(ILogger<OpenCLService> logger, string vendor, params string[] deviceNames)
        : this(logger)
    {
        ConfigureOpenCL(vendor, deviceNames);
    }

    public void Dispose()
    {
        _ = _cl.ReleaseKernel(_kernel);

        if (_program != 0)
        {
            _ = _cl.ReleaseProgram(_program);
        }

        if (_commandQueue != 0)
        {
            _ = _cl.ReleaseCommandQueue(_commandQueue);
        }

        if (_context != 0)
        {
            _ = _cl.ReleaseContext(_context);
        }

        for (int i = 0; i < _devices.Length; i++)
        {
            if (_devices.Span[i] != 0)
            {
                _ = _cl.ReleaseDevice(_devices.Span[i]);
            }
        }
    }

    private void ConfigureOpenCL(string vendor, string[] deviceNames)
    {
        try
        {
            if (!TryGetPlatform(vendor))
            {
                throw new Exception("Unable to retrieve the specified platform.");
            }

            if (!TryCreateContext())
            {
                throw new Exception("Unable to create context.");
            }

            if (!TryGetDevices(deviceNames))
            {
                throw new Exception("Unable to retrieve the specified device.");
            }

            if (!TryCreateCommandQueue())
            {
                throw new Exception("Unable to create the command queue.");
            }

            if (!TryCreateProgram())
            {
                throw new Exception("Unable to create the program.");
            }

            if (!CreateKernel("vec_mul_scalar"))
            {
                throw new Exception($"Unable to create kernel.");
            }
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Unable to create the service.");
            Dispose();
        }
    }

    private unsafe bool TryGetPlatform(string vendor)
    {
        var error = _cl.GetPlatformIDs(0, null, out var numPlatforms);
        if (error != (int)ErrorCodes.Success || numPlatforms < 1)
        {
            return false;
        }
        Span<nint> platforms = new nint[numPlatforms];
        _ = _cl.GetPlatformIDs(numPlatforms, platforms, []);

        var platform = platforms.ToArray().FirstOrDefault(x => GetPlatformVendor(x) == vendor);
        if (platform == 0)
        {
            _logger.LogDebug("There is no platform vendor with name {PlatformVendor}.", vendor);
            return false;
        }
        _platform = platform;
        _logger.LogInformation("Platform vendor: {Vendor}", GetPlatformVendor(_platform));
        return true;
    }

    private unsafe bool TryCreateContext()
    {
        var contextProperties = stackalloc nint[3] { (nint)ContextProperties.Platform, _platform, 0 };
        var context = _cl.CreateContextFromType(contextProperties, DeviceType.Gpu, null, null, out var error);
        if (error != (int)ErrorCodes.Success)
        {
            _logger.LogDebug("Unable to create GPU context; attempting to create CPU context.");
            context = _cl.CreateContextFromType(contextProperties, DeviceType.Cpu, null, null, out error);
            if (error != (int)ErrorCodes.Success)
            {
                _logger.LogDebug("Unable to create CPU context.");
                return false;
            }
        }
        _context = context;
        return true;
    }

    private unsafe bool TryGetDevices(string[] deviceNames)
    {
        var error = _cl.GetContextInfo(_context, ContextInfo.Devices, 0, null, out var bufferSize);
        if (error != (int)ErrorCodes.Success || bufferSize <= 0)
        {
            return false;
        }

        var devices = new nint[bufferSize / (nuint)sizeof(nuint)];
        fixed (void* p = devices)
        {
            _ = _cl.GetContextInfo(_context, ContextInfo.Devices, bufferSize, p, null);
        }

        var selectedDevices = devices.Where(x => deviceNames.Contains(GetDeviceName(x))).ToArray();
        if (selectedDevices.Length == 0)
        {
            _logger.LogDebug("No devices found with the specified name/s.");
            return false;
        }
        _selectedDevices = selectedDevices;
        for (int i = 0; i < _selectedDevices.Length; i++)
        {
            _logger.LogInformation("Using device at index {Index} with name: {DeviceName}", i, GetDeviceName(_selectedDevices.Span[i]));
        }
        return true;
    }

    private unsafe bool TryCreateCommandQueue()
    {
        var commandQueue = _cl.CreateCommandQueue(_context, _selectedDevices.Span[0], CommandQueueProperties.None, out var error);
        if (error != (int)ErrorCodes.Success)
        {
            return false;
        }
        _commandQueue = commandQueue;
        return true;
    }

    private unsafe bool TryCreateProgram()
    {
        // TODO: Create a source generator that finds all the required kernels.
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Mathematics.NET.GPU.OpenCL.Kernels.vec_mul_scalar.cl");
        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            var kernelCode = reader.ReadToEnd();

            var program = _cl.CreateProgramWithSource(_context, 1, [kernelCode], null, out var error);
            if (error != (int)ErrorCodes.Success)
            {
                _logger.LogDebug("Unable to create the program from source.");
            }

            error = _cl.BuildProgram(program, (uint)_selectedDevices.Length, _selectedDevices.Span, (byte*)null, null, null);
            if (error != (int)ErrorCodes.Success)
            {
                _ = _cl.GetProgramBuildInfo(program, _selectedDevices.Span[0], ProgramBuildInfo.BuildLog, 0, null, out var bufferSize);
                Span<byte> log = new byte[bufferSize];
                _ = _cl.GetProgramBuildInfo(program, _selectedDevices.Span[0], ProgramBuildInfo.BuildLog, bufferSize, log, []);
                _ = _cl.ReleaseProgram(program);
                throw new Exception(Encoding.UTF8.GetString(log));
            }

            _program = program;
            return true;
        }
        else
        {
            _logger.LogDebug("Unable to find the embedded resource.");
        }
        return false;
    }

    private unsafe bool CreateKernel(string kernelName)
    {
        var kernel = _cl.CreateKernel(_program, kernelName, null);
        if (kernel == 0)
        {
            _logger.LogDebug("Unable to create kernel.");
        }
        _kernel = kernel;
        return true;
    }

    //
    // Helpers
    //

    private unsafe string GetPlatformVendor(nint platform)
    {
        var error = _cl.GetPlatformInfo(platform, PlatformInfo.Vendor, 0, null, out var bufferSize);
        if (error != (int)ErrorCodes.Success)
        {
            _logger.LogDebug("Unable to retrieve platform info.");
        }
        Span<byte> value = new byte[bufferSize];
        _ = _cl.GetPlatformInfo(platform, PlatformInfo.Vendor, bufferSize, value, []);
        return Encoding.UTF8.GetString(value[..^1]);
    }

    private unsafe string GetDeviceName(nint device)
    {
        var error = _cl.GetDeviceInfo(device, DeviceInfo.Name, 0, null, out var paramValueSize);
        if (error != (int)ErrorCodes.Success)
        {
            _logger.LogDebug("Unable to retrieve device info.");
        }
        Span<byte> value = new byte[paramValueSize];
        _ = _cl.GetDeviceInfo(device, DeviceInfo.Name, paramValueSize, value, []);
        return Encoding.UTF8.GetString(value[..^1]);
    }

    //
    // Methods
    //

    public unsafe ReadOnlySpan<Real> VecMulScalar(ReadOnlySpan<Real> vector, Real scalar)
    {
        var length = vector.Length;
        var result = new Real[length];

        // Since we are using MemFlags.UseHostPtr, keep memory pinned until finished.
        fixed (void* pVector = vector)
        {
            // Create buffers.
            nint vectorBuffer = _cl.CreateBuffer(_context, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Real) * length), pVector, out var errorInt);

            fixed (void* pResult = result)
            {
                nint resultBuffer = _cl.CreateBuffer(_context, MemFlags.UseHostPtr | MemFlags.WriteOnly | MemFlags.HostReadOnly, (nuint)(sizeof(Real) * length), pResult, null);

                // Set kernel arguments.
                var error = _cl.SetKernelArg(_kernel, 0, (nuint)sizeof(nint), &vectorBuffer);
                error |= _cl.SetKernelArg(_kernel, 1, (nuint)sizeof(Real), &scalar);
                error |= _cl.SetKernelArg(_kernel, 2, sizeof(int), &length);
                error |= _cl.SetKernelArg(_kernel, 3, (nuint)sizeof(nint), &resultBuffer);

                // Enqueue ndrange kernel.
                // TODO: Allow user to set work sizes.
                var globalWorkSize = stackalloc nuint[1] { (nuint)length };
                var localWorkSize = stackalloc nuint[1] { 16 };

                _ = _cl.EnqueueNdrangeKernel(_commandQueue, _kernel, 1, null, globalWorkSize, localWorkSize, 0, null, null);
                _ = _cl.Finish(_commandQueue);

                // Enqueue read buffer.
                _ = _cl.EnqueueReadBuffer(_commandQueue, resultBuffer, true, 0, (nuint)(sizeof(Real) * length), pResult, 0, null, null);

                // Release mem objects.
                _ = _cl.ReleaseMemObject(resultBuffer);
            }
            _ = _cl.ReleaseMemObject(vectorBuffer);
        }

        return result;
    }
}
