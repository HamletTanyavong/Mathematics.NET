﻿// <copyright file="WorkSize2D.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents a struct of 2D work sizes.</summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct WorkSize2D
{
    private readonly nuint _gws0;
    private readonly nuint _gws1;
    private readonly nuint _lws0;
    private readonly nuint _lws1;

    public WorkSize2D(ReadOnlySpan<nuint> globalWorkSize, ReadOnlySpan<nuint> localWorkSize)
    {
        Debug.Assert(globalWorkSize.Length == 2 && localWorkSize.Length == 2, "The work sizes specified must be of length two.");

        _gws0 = globalWorkSize[0];
        _gws1 = globalWorkSize[1];
        _lws0 = localWorkSize[0];
        _lws1 = localWorkSize[1];
    }
}
