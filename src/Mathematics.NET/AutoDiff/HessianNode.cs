// <copyright file="HessianNode.cs" company="Mathematics.NET">
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

/// <summary>Represents a node on a Hessian tape</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
internal readonly record struct HessianNode<T>
    where T : IComplex<T>
{
    /// <summary>The first derivative of the left component of the binary operation</summary>
    public readonly T DX;
    /// <summary>The second derivative of the left component of the binary operation</summary>
    public readonly T DXX;
    /// <summary>The derivative of the left or right component of the binary operation with respect to the left and right variables</summary>
    public readonly T DXY;
    /// <summary>The first derivative of the right component of the binary operation</summary>
    public readonly T DY;
    /// <summary>The second derivative of the right component of the binary operation</summary>
    public readonly T DYY;

    /// <summary>The parent index of the left node</summary>
    public readonly int PX;
    /// <summary>The parent index of the right node</summary>
    public readonly int PY;

    public HessianNode(int index)
    {
        DX = T.Zero;
        DXX = T.Zero;
        DXY = T.Zero;
        DY = T.Zero;
        DYY = T.Zero;

        PX = index;
        PY = index;
    }

    public HessianNode(T dfx, T dfxx, int px, int py)
    {
        DX = dfx;
        DXX = dfxx;
        DXY = T.Zero;
        DY = T.Zero;
        DYY = T.Zero;

        PX = px;
        PY = py;
    }

    public HessianNode(T dfx, T dfxx, T dfxy, T dfy, T dfyy, int px, int py)
    {
        DX = dfx;
        DXX = dfxx;
        DXY = dfxy;
        DY = dfy;
        DYY = dfyy;

        PX = px;
        PY = py;
    }
}
