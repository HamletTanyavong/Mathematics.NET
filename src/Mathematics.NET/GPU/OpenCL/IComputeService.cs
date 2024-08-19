// <copyright file="IComputeService.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Defines support for GPU compute services.</summary>
public interface IComputeService : IDisposable
{
    /// <summary>Get a span of available devices.</summary>
    ReadOnlySpan<Device> Devices { get; }

    /// <summary>Get the OpenCL program associated with this service.</summary>
    Program Program { get; }

    /// <summary>Get the kernel work group information.</summary>
    /// <param name="device">A device.</param>
    /// <param name="kernel">A kernel associated with the device.</param>
    /// <returns>The kernel work group information.</returns>
    KernelWorkGroupInformation GetKernelWorkGroupInfo(Device device, Kernel kernel);

    //
    // Methods.
    //

    /// <summary>Multipy two matrices.</summary>
    /// <remarks>Padded matrices should have zeros added to their right and bottom.</remarks>
    /// <param name="device">The device to use.</param>
    /// <param name="globalWorkSize">A struct containing global work size information.</param>
    /// <param name="localWorkSize">A struct contaning local work size information.</param>
    /// <param name="left">The left matrix.</param>
    /// <param name="right">The right matrix.</param>
    /// <returns>A new matrix.</returns>
    ReadOnlySpan2D<Real> MatMul(Device device, WorkSize2D globalWorkSize, WorkSize2D localWorkSize, ReadOnlySpan2D<Real> left, ReadOnlySpan2D<Real> right);

    /// <summary>Multiply a vector by a scalar.</summary>
    /// <remarks>Padded vectors should have zeros added to their ends.</remarks>
    /// <param name="device">The device to use.</param>
    /// <param name="globalWorkSize">The global work size.</param>
    /// <param name="localWorkSize">The local work size.</param>
    /// <param name="vector">A vector.</param>
    /// <param name="scalar">A scalar.</param>
    /// <returns>A new vector.</returns>
    ReadOnlySpan<Real> VecMulScalar(Device device, nuint globalWorkSize, nuint localWorkSize, ReadOnlySpan<Real> vector, Real scalar);
}
