// <copyright file="Extensions.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023 Hamlet Tanyavong
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
using CommunityToolkit.HighPerformance;
using Mathematics.NET.Core;

namespace Mathematics.NET.LinearAlgebra;

public static class Extensions
{
    /// <summary>Create a new <see cref="Span2D{T}"/> over a 4x4 matrix of <typeparamref name="T"/> numbers</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">The input matrix</param>
    /// <returns>A <see cref="Span2D{T}"/> with elements from the input matrix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this Matrix4x4<T> matrix)
        where T : IComplex<T>
    {
        return new Span2D<T>(Unsafe.AsPointer(ref matrix.E11), Matrix4x4<T>.E1Components, Matrix4x4<T>.E2Components, 0);
    }
}
