// <copyright file="GradientNode.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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

using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a node on a gradient tape</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
internal readonly record struct GradientNode<T>
    where T : IComplex<T>
{
    /// <summary>The derivative of the left component of the binary operation</summary>
    public readonly T DX;
    /// <summary>The derivative of the right component of the binary operation</summary>
    public readonly T DY;

    /// <summary>The parent index of the left node</summary>
    public readonly int PX;
    /// <summary>The parent index of the right node</summary>
    public readonly int PY;

    public GradientNode(int index)
    {
        DX = T.Zero;
        DY = T.Zero;

        PX = index;
        PY = index;
    }

    public GradientNode(T dx, int px, int py)
    {
        DX = dx;
        DY = T.Zero;

        PX = px;
        PY = py;
    }

    public GradientNode(T dx, T dy, int px, int py)
    {
        DX = dx;
        DY = dy;

        PX = px;
        PY = py;
    }
}
