// <copyright file="CompMatMul.cs" company="Mathematics.NET">
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
    public unsafe ReadOnlySpan2D<Complex> CompMatMul(Device device, WorkSize2D globalWorkSize, WorkSize2D localWorkSize, ReadOnlySpan2D<Complex> left, ReadOnlySpan2D<Complex> right)
    {
        var k = left.Width;
        if (k != right.Height)
            throw new MathematicsException("Cannot multiply two matrices with incompatible dimensions.");
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
                    var width = result.Width;
                    var error = _cl.SetKernelArg(kernel, 0, (nuint)sizeof(nint), &leftBuffer);
                    error |= _cl.SetKernelArg(kernel, 1, (nuint)sizeof(nint), &rightBuffer);
                    error |= _cl.SetKernelArg(kernel, 2, sizeof(int), &k);
                    error |= _cl.SetKernelArg(kernel, 3, sizeof(int), &width);
                    error |= _cl.SetKernelArg(kernel, 4, (nuint)sizeof(nint), &resultBuffer);
                    ComputeServiceException.ThrowIfCouldNotSetKernelArguments(error, device.Name, "comp_mat_mul");

                    // Enqueue NDRange kernel.
                    using var commandQueue = _context.CreateCommandQueue(device, CommandQueueProperties.None);
                    error = _cl.EnqueueNdrangeKernel(commandQueue.Handle, kernel, 2, null, (nuint*)&globalWorkSize, (nuint*)&localWorkSize, 0, null, null);
                    ComputeServiceException.ThrowIfCouldNotEnqueueNDRangeKernel(error, device.Name, "comp_mat_mul");

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
}
