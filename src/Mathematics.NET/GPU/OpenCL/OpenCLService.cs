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

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an Mathematics.NET OpenCL service.</summary>
public sealed class OpenCLService : IComputeService
{
    /// <summary>A class containing names of common vendors.</summary>
    public static class Vendors
    {
        public const string AMD = "Advanced Micro Devices, Inc.";
        public const string Intel = "Intel® Corporation";
        public const string NVIDIA = "NVIDIA Corporation";
    }

    private const string s_setKernelArgError = "Unable to set arguments for the kernel {Kernel}.";
    private const string s_enqueueNDRangeKernelError = "Problem enqueueing NDRange kernel.";

    private ILogger<OpenCLService> _logger;
    private CL _cl;

    private Platform _platform;
    private ReadOnlyMemory<Device> _devices;
    private Context _context;
    private Program _program;

    public unsafe OpenCLService(ILogger<OpenCLService> logger, string vendor, Func<Device, bool> filter, bool useFirst = false, params ReadOnlySpan<string> options)
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
#if DEBUG
                _logger.LogDebug("Platform vendor: {Vendor}.", _platform.Vendor);
#endif
            }
            else
            {
                throw new Exception($"There is no platform with the vendor {vendor}.");
            }

            // OpenCL context.
            _context = new Context(_logger, _cl, _platform);

            // OpenCL device.
            _devices = _context.GetDevices(filter, useFirst).ToArray();
            if (_devices.Length == 0)
                throw new Exception("There are no devices available after filtering.");
#if DEBUG
            for (int i = 0; i < _devices.Length; i++)
            {
                _logger.LogDebug("Using the device at index {Index} with name: {DeviceName}.", i, _devices.Span[i].Name);
            }
#endif

            // OpenCL program.
            _program = new Program(_logger, _cl, _context, _devices.Span, options);
        }
        catch (Exception e)
        {
            Dispose();
            _logger.LogCritical(e, "Unable to create the service.");
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

    public ReadOnlySpan<Device> Devices => _devices.Span;

    public Program Program => _program;

    //
    // Methods
    //

    public KernelWorkGroupInformation GetKernelWorkGroupInfo(Device device, Kernel kernel) => new(_logger, _cl, device, kernel);

    #region Interface Implementations

    // TODO: Consider putting this in another file.

    public unsafe void CompVecMulScalar(Device device, nuint globalWorkSize, nuint localWorkSize, Span<Complex> vector, in Complex scalar)
    {
        fixed (void* pVector = vector)
        {
            // Create buffers.
            nint vectorBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadWrite | MemFlags.HostReadOnly, (nuint)(sizeof(Complex) * vector.Length), pVector, null);

            // Set kernel arguments.
            var kernel = _program.Kernels["comp_vec_mul_scalar_overwrite"].Handle;
            var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &vectorBuffer);
            error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(Complex), in scalar);
#if DEBUG
            if (error != (int)ErrorCodes.Success)
                _logger.LogDebug(s_setKernelArgError, _program.Kernels["comp_vec_mul_scalar_overwrite"].Name);
#endif

            // Enqueue NDRange kernel.
            using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
            error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
#if DEBUG
            if (error != (int)ErrorCodes.Success)
                _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
            // Enqueue read buffer.
            _cl.EnqueueReadBuffer(commandQueue.Handle, vectorBuffer, true, 0, (nuint)(sizeof(Complex) * vector.Length), pVector, 0, null, null);

            // Release mem objects.
            _cl.ReleaseMemObject(vectorBuffer);
        }
    }

    public unsafe ReadOnlySpan<Complex> CompVecMulScalar(Device device, nuint globalWorkSize, nuint localWorkSize, ReadOnlySpan<Complex> vector, in Complex scalar)
    {
        var length = vector.Length;
        var result = new Complex[length];

        fixed (void* pVector = vector)
        {
            // Create buffers.
            nint vectorBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Complex) * length), pVector, null);

            fixed (void* pResult = result)
            {
                nint resultBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.WriteOnly | MemFlags.HostReadOnly, (nuint)(sizeof(Complex) * length), pResult, null);

                // Set kernel arguments.
                var kernel = _program.Kernels["comp_vec_mul_scalar"].Handle;
                var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &vectorBuffer);
                error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(Complex), in scalar);
                error |= _cl.SetKernelArg(kernel, 2, (nuint)sizeof(nint), &resultBuffer);
#if DEBUG
                if (error != (int)ErrorCodes.Success)
                    _logger.LogDebug(s_setKernelArgError, _program.Kernels["comp_vec_mul_scalar"].Name);
#endif

                // Enqueue NDRange kernel.
                using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
#if DEBUG
                if (error != (int)ErrorCodes.Success)
                    _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
                // Enqueue read buffer.
                _cl.EnqueueReadBuffer(commandQueue.Handle, resultBuffer, true, 0, (nuint)(sizeof(Complex) * length), pResult, 0, null, null);

                // Release mem objects.
                _cl.ReleaseMemObject(resultBuffer);
            }
            _cl.ReleaseMemObject(vectorBuffer);
        }

        return result;
    }

    public unsafe ReadOnlySpan2D<Complex> CompMatMul(Device device, WorkSize2D globalWorkSize, WorkSize2D localWorkSize, ReadOnlySpan2D<Complex> left, ReadOnlySpan2D<Complex> right)
    {
        var k = left.Width;
        if (k != right.Height)
            throw new Exception("Cannot multiply two matrices with incompatible dimensions.");
        Span2D<Complex> result = new Complex[left.Height, right.Width];

        fixed (void* pLeft = left)
        {
            // Create buffers.
            nint leftBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Complex) * left.Length), pLeft, null);

            fixed (void* pRight = right)
            {
                nint rightBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Complex) * right.Length), pRight, null);

                fixed (void* pResult = result)
                {
                    nint resultBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.WriteOnly | MemFlags.HostReadOnly, (nuint)(sizeof(Complex) * result.Length), pResult, null);

                    // Set kernel arguments.
                    var kernel = _program.Kernels["comp_mat_mul"].Handle;
                    var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &leftBuffer);
                    error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(nint), &rightBuffer);
                    error |= _cl.SetKernelArg(kernel, 2, sizeof(int), &k);
                    error |= _cl.SetKernelArg(kernel, 3, (nuint)sizeof(nint), &resultBuffer);
#if DEBUG
                    if (error != (int)ErrorCodes.Success)
                        _logger.LogDebug(s_setKernelArgError, _program.Kernels["comp_mat_mul"].Name);
#endif

                    // Enqueue NDRange kernel.
                    using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                    error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 2, null, (nuint*)&globalWorkSize, (nuint*)&localWorkSize, 0, null, null);
#if DEBUG
                    if (error != (int)ErrorCodes.Success)
                        _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
                    // Enqueue read buffer.
                    _cl.EnqueueReadBuffer(commandQueue.Handle, resultBuffer, true, 0, (nuint)(sizeof(Complex) * result.Length), pResult, 0, null, null);

                    // Release mem objects.
                    _cl.ReleaseMemObject(resultBuffer);
                }
                _cl.ReleaseMemObject(rightBuffer);
            }
            _cl.ReleaseMemObject(leftBuffer);
        }

        return result;
    }

    public unsafe ReadOnlySpan2D<Real> MatMul(Device device, WorkSize2D globalWorkSize, WorkSize2D localWorkSize, ReadOnlySpan2D<Real> left, ReadOnlySpan2D<Real> right)
    {
        var k = left.Width;
        if (k != right.Height)
            throw new Exception("Cannot multiply two matrices with incompatible dimensions.");
        Span2D<Real> result = new Real[left.Height, right.Width];

        fixed (void* pLeft = left)
        {
            // Create buffers.
            nint leftBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Real) * left.Length), pLeft, null);

            fixed (void* pRight = right)
            {
                nint rightBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Real) * right.Length), pRight, null);

                fixed (void* pResult = result)
                {
                    nint resultBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.WriteOnly | MemFlags.HostReadOnly, (nuint)(sizeof(Real) * result.Length), pResult, null);

                    // Set kernel arguments.
                    var kernel = _program.Kernels["mat_mul"].Handle;
                    var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &leftBuffer);
                    error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(nint), &rightBuffer);
                    error |= _cl.SetKernelArg(kernel, 2, sizeof(int), &k);
                    error |= _cl.SetKernelArg(kernel, 3, (nuint)sizeof(nint), &resultBuffer);
#if DEBUG
                    if (error != (int)ErrorCodes.Success)
                        _logger.LogDebug(s_setKernelArgError, _program.Kernels["mat_mul"].Name);
#endif

                    // Enqueue NDRange kernel.
                    using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                    error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 2, null, (nuint*)&globalWorkSize, (nuint*)&localWorkSize, 0, null, null);
#if DEBUG
                    if (error != (int)ErrorCodes.Success)
                        _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
                    // Enqueue read buffer.
                    _cl.EnqueueReadBuffer(commandQueue.Handle, resultBuffer, true, 0, (nuint)(sizeof(Real) * result.Length), pResult, 0, null, null);

                    // Release mem objects.
                    _cl.ReleaseMemObject(resultBuffer);
                }
                _cl.ReleaseMemObject(rightBuffer);
            }
            _cl.ReleaseMemObject(leftBuffer);
        }

        return result;
    }

    public unsafe void VecMulScalar(Device device, nuint globalWorkSize, nuint localWorkSize, Span<Real> vector, in Real scalar)
    {
        fixed (void* pVector = vector)
        {
            // Create buffers.
            nint vectorBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadWrite | MemFlags.HostReadOnly, (nuint)(sizeof(Real) * vector.Length), pVector, null);

            // Set kernel arguments.
            var kernel = _program.Kernels["vec_mul_scalar_overwrite"].Handle;
            var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &vectorBuffer);
            error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(Real), in scalar);
#if DEBUG
            if (error != (int)ErrorCodes.Success)
                _logger.LogDebug(s_setKernelArgError, _program.Kernels["vec_mul_scalar_overwrite"].Name);
#endif

            // Enqueue NDRange kernel.
            using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
            error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
#if DEBUG
            if (error != (int)ErrorCodes.Success)
                _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
            // Enqueue read buffer.
            _cl.EnqueueReadBuffer(commandQueue.Handle, vectorBuffer, true, 0, (nuint)(sizeof(Real) * vector.Length), pVector, 0, null, null);

            // Release mem objects.
            _cl.ReleaseMemObject(vectorBuffer);
        }
    }

    public unsafe ReadOnlySpan<Real> VecMulScalar(Device device, nuint globalWorkSize, nuint localWorkSize, ReadOnlySpan<Real> vector, Real scalar)
    {
        var length = vector.Length;
        var result = new Real[length];

        fixed (void* pVector = vector)
        {
            // Create buffers.
            nint vectorBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.ReadOnly | MemFlags.HostNoAccess, (nuint)(sizeof(Real) * length), pVector, null);

            fixed (void* pResult = result)
            {
                nint resultBuffer = _cl.CreateBuffer(_context.Handle, MemFlags.UseHostPtr | MemFlags.WriteOnly | MemFlags.HostReadOnly, (nuint)(sizeof(Real) * length), pResult, null);

                // Set kernel arguments.
                var kernel = _program.Kernels["vec_mul_scalar"].Handle;
                var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &vectorBuffer);
                error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(Real), &scalar);
                error |= _cl.SetKernelArg(kernel, 2, (nuint)sizeof(nint), &resultBuffer);
#if DEBUG
                if (error != (int)ErrorCodes.Success)
                    _logger.LogDebug(s_setKernelArgError, _program.Kernels["vec_mul_scalar"].Name);
#endif

                // Enqueue NDRange kernel.
                using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
#if DEBUG
                if (error != (int)ErrorCodes.Success)
                    _logger.LogDebug(s_enqueueNDRangeKernelError);
#endif
                // Enqueue read buffer.
                _cl.EnqueueReadBuffer(commandQueue.Handle, resultBuffer, true, 0, (nuint)(sizeof(Real) * length), pResult, 0, null, null);

                // Release mem objects.
                _cl.ReleaseMemObject(resultBuffer);
            }
            _cl.ReleaseMemObject(vectorBuffer);
        }

        return result;
    }

    #endregion
}
