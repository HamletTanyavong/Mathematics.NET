// <copyright file="IOneDimensionalArrayRepresentable.cs" company="Mathematics.NET">
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

/// <summary>Defines support for mathematical objects that can be represented by one-dimensional arrays</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
public interface IOneDimensionalArrayRepresentable<T, U>
    : IArrayRepresentable<T, U>,
      IFormattable
    where T : IOneDimensionalArrayRepresentable<T, U>
    where U : IComplex<U>
{
    /// <summary>The number of components in the one-dimensional array</summary>
    static abstract int E1Components { get; }

    /// <summary>Get the element at the specified index.</summary>
    /// <param name="index">An index</param>
    /// <returns>The element at the index</returns>
    U this[int index] { get; set; }
}
