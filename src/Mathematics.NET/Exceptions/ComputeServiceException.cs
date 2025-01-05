// <copyright file="ComputeServiceException.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using Silk.NET.OpenCL;

namespace Mathematics.NET.Exceptions;

/// <summary>Represents compute-related errors.</summary>
public sealed class ComputeServiceException : Exception
{
    public ComputeServiceException() { }

    public ComputeServiceException(string message)
        : base(message) { }

    public ComputeServiceException(string message, Exception innerException)
        : base(message, innerException) { }

    public ComputeServiceException(string message, string? device = default, string? kernel = default)
        : base(message)
    {
        Device = device;
        Kernel = kernel;
    }

    /// <summary>The device where the exception occured.</summary>
    public string? Device { get; }

    /// <summary>The kernel where the exception occured.</summary>
    public string? Kernel { get; }

    //
    // Throw Helpers
    //

    /// <summary>Throw a <see cref="ComputeServiceException"/> if a compute method did not return a success error code.</summary>
    /// <param name="errorCode">The error code returned from a compute method.</param>
    /// <param name="message">The error message.</param>
    /// <param name="device">The device where the error occured.</param>
    /// <param name="kernel">The kernel where the error occured.</param>
    /// <exception cref="ComputeServiceException">Thrown when the compute service encounters an error.</exception>
    public static void ThrowIfNotSuccess(int errorCode, string message, string? device = default, string? kernel = default)
    {
        if (errorCode != (int)ErrorCodes.Success)
            throw new ComputeServiceException(message, device, kernel);
    }

    /// <summary>Throw a <see cref="ComputeServiceException"/> if the argument of a kernel could not be set.</summary>
    /// <param name="errorCode">The error code returned from the argument-setting method.</param>
    /// <param name="device">The device where the error occured.</param>
    /// <param name="kernel">The kernel where the error occured.</param>
    /// <exception cref="ComputeServiceException">Thrown when the compute service encounters an error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfCouldNotSetKernelArguments(int errorCode, string device, string kernel)
    {
        if (errorCode != (int)ErrorCodes.Success)
            throw new ComputeServiceException("Unable to set arguments for the kernel.", device, kernel);
    }

    /// <summary>Throw a <see cref="ComputeServiceException"/> if an n-dimensional range kernel could not be enqueued.</summary>
    /// <param name="errorCode">The error code returned from the kernel-enqueing method.</param>
    /// <param name="device">The device where the error occured.</param>
    /// <param name="kernel">The kernel where the error occured.</param>
    /// <exception cref="ComputeServiceException">Thrown when the compute service encounters an error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfCouldNotEnqueueNDRangeKernel(int errorCode, string device, string kernel)
    {
        if (errorCode != (int)ErrorCodes.Success)
            throw new ComputeServiceException("Unable to enqueue the n-dimensional range kernel.", device, kernel);
    }
}
