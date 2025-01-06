﻿// <copyright file="Kernel.cs" company="Mathematics.NET">
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
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an OpenCL kernel.</summary>
public sealed partial class Kernel : IOpenCLObject
{
    private CL _cl;

    public readonly string Name;

    public unsafe Kernel(CL cl, Program program, string name)
    {
        _cl = cl;

        Name = name;
        Handle = _cl.CreateKernel(program.Handle, name, out var error);

        ComputeServiceException.ThrowIfNotSuccess(error, "Unable to create the kernel", kernel: name);
    }

    public void Dispose()
    {
        if (Handle != 0)
        {
            _cl.ReleaseKernel(Handle);
            Handle = 0;
        }
    }

    public nint Handle { get; private set; }

    public bool IsValidInstance => Handle != 0;
}
