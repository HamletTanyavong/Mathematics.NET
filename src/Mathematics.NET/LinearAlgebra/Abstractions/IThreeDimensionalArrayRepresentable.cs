// <copyright file="IThreeDimensionalArrayRepresentable.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.LinearAlgebra.Abstractions;

/// <summary>Defines support for mathematical objects that can be represented by three-dimensional arrays</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
public interface IThreeDimensionalArrayRepresentable<T, U>
    : IArrayRepresentable<T, U>,
      IFormattable
    where T : IThreeDimensionalArrayRepresentable<T, U>
    where U : IComplex<U>
{
    /// <summary>The number of elements in the first dimension of the array</summary>
    static abstract int E1Components { get; }

    /// <summary>The number of elements in the second dimension of the array</summary>
    static abstract int E2Components { get; }

    /// <summary>The number of elements in the third dimension of the array</summary>
    static abstract int E3Components { get; }

    /// <summary>Convert the matrix into a 3D array.</summary>
    /// <returns>A 3D array</returns>
    U[,,] ToArray3D();

    /// <summary>Get the element at the specified indices.</summary>
    /// <param name="i">The first index</param>
    /// <param name="j">The second index</param>
    /// <param name="k">The third index</param>
    /// <returns>The element at the specified indices</returns>
    U this[int i, int j, int k] { get; set; }
}
