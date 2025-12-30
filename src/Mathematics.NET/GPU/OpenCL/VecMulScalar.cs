// <copyright file="VecMulScalar.cs" company="Mathematics.NET">
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
using Mathematics.NET.GPU.OpenCL.Core;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

public sealed partial class OpenCLService
{
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
                ComputeServiceException.ThrowIfCouldNotSetKernelArguments(error, device.Name, "vec_mul_scalar");

                // Enqueue NDRange kernel.
                using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
                ComputeServiceException.ThrowIfCouldNotEnqueueNDRangeKernel(error, device.Name, "vec_mul_scalar");

                // Enqueue read buffer.
                _cl.EnqueueReadBuffer(commandQueue.Handle, resultBuffer, true, 0, (nuint)(sizeof(Real) * length), pResult, 0, null, null);

                // Release mem objects.
                _cl.ReleaseMemObject(resultBuffer);
            }
            _cl.ReleaseMemObject(vectorBuffer);
        }

        return result;
    }
}
