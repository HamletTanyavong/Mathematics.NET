// <copyright file="Context.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0058

using Mathematics.NET.Exceptions;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an OpenCL context.</summary>
public sealed partial class Context : IOpenCLObject
{
    private readonly ILogger<OpenCLService> _logger;
    private readonly CL _cl;

    public unsafe Context(ILogger<OpenCLService> logger, CL cl, Platform platform)
    {
        _logger = logger;
        _cl = cl;

        // Attempt to create a GPU context. If that does not work, attempt to create a CPU context.
        var contextProperties = stackalloc nint[3] { (nint)ContextProperties.Platform, platform.Handle, 0 };
        Handle = _cl.CreateContextFromType(contextProperties, DeviceType.Gpu, null, null, out var error);
        if (error != (int)ErrorCodes.Success)
        {
            CouldNotCreateGPUContext();
            Handle = _cl.CreateContextFromType(contextProperties, DeviceType.Cpu, null, null, out error);
            ComputeServiceException.ThrowIfNotSuccess(error, "Unable to create a context on either the GPU or CPU.");
        }
    }

    public void Dispose()
    {
        if (Handle != 0)
        {
            _cl.ReleaseContext(Handle);
            Handle = 0;
        }
    }

    [LoggerMessage(LogLevel.Warning, "Unable to create a GPU context; attempting to create a CPU context instead.")]
    private partial void CouldNotCreateGPUContext();

    public nint Handle { get; private set; }

    public bool IsValidInstance => Handle != 0;

    /// <summary>Create a command queue.</summary>
    /// <param name="device">A device.</param>
    /// <param name="commandQueueProperties">Command queue properties.</param>
    /// <returns>A command queue.</returns>
    public CommandQueue CreateCommandQueue(Device device, CommandQueueProperties commandQueueProperties)
        => new(_cl, this, device, commandQueueProperties);

    /// <summary>Get all the devices available.</summary>
    /// <returns>A read-only span of devices.</returns>
    public unsafe Device[] GetDevices()
    {
        _cl.GetContextInfo(Handle, ContextInfo.Devices, 0, null, out var devicesSize);
        Span<nint> devicesSpan = new nint[devicesSize / (nuint)sizeof(nuint)];
        _cl.GetContextInfo(Handle, ContextInfo.Devices, devicesSize, devicesSpan, []);

        var result = new Device[devicesSpan.Length];
        for (int i = 0; i < devicesSpan.Length; i++)
        {
            result[i] = new(_cl, devicesSpan[i]);
        }
        return result;
    }

    /// <summary>Get all the devices that match our selection criteria.</summary>
    /// <param name="filter">A delegate that filters devices.</param>
    /// <param name="useFirst">Should only get the first device found.</param>
    /// <returns>An array of devices.</returns>
    public Device[] GetDevices(Func<Device, bool> filter, bool useFirst)
    {
        var devices = GetDevices();
        return useFirst
            ? devices.FirstOrDefault(filter) is Device device ? [device] : []
            : devices.Where(filter).ToArray();
    }
}
