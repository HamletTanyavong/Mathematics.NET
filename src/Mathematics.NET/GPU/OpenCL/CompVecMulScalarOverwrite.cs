// <copyright file="CompVecMulScalarOverwrite.cs" company="Mathematics.NET">
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
            ComputeServiceException.ThrowIfCouldNotSetKernelArguments(error, device.Name, "comp_vec_mul_scalar_overwrite");

            // Enqueue NDRange kernel.
            using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
            error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 1, null, &globalWorkSize, &localWorkSize, 0, null, null);
            ComputeServiceException.ThrowIfCouldNotEnqueueNDRangeKernel(error, device.Name, "comp_vec_mul_scalar_overwrite");

            // Enqueue read buffer.
            _cl.EnqueueReadBuffer(commandQueue.Handle, vectorBuffer, true, 0, (nuint)(sizeof(Complex) * vector.Length), pVector, 0, null, null);

            // Release mem objects.
            _cl.ReleaseMemObject(vectorBuffer);
        }
    }
}
