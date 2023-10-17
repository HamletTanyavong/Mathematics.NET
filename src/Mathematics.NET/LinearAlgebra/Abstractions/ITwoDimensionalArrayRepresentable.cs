// <copyright file="ITwoDimensionalArrayRepresentable.cs" company="Mathematics.NET">
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

using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.LinearAlgebra.Abstractions;

/// <summary>Defines support for mathematical objects that can be represented by two-dimensional arrays</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
public interface ITwoDimensionalArrayRepresentable<T, U>
    : IArrayRepresentable<U>,
      IAdditionOperation<T, T>,
      ISubtractionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      IEqualityRelation<T, bool>,
      IFormattable
    where T : ITwoDimensionalArrayRepresentable<T, U>
    where U : IComplex<U>
{
    /// <summary>The number of rows in the array</summary>
    static abstract int E1Components { get; }

    /// <summary>The number of columns in the array</summary>
    static abstract int E2Components { get; }

    /// <summary>Get the element at the specified row and column</summary>
    /// <param name="row">The row</param>
    /// <param name="column">The column</param>
    /// <returns>The element at the specified row and column</returns>
    U this[int row, int column] { get; set; }
}
