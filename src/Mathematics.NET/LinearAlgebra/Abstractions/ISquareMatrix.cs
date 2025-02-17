// <copyright file="ISquareMatrix.cs" company="Mathematics.NET">
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

using Mathematics.NET.Core.Operations;
using Mathematics.NET.LinearAlgebra.Operations;

namespace Mathematics.NET.LinearAlgebra.Abstractions;

/// <summary>Defines support for square matrices.</summary>
/// <typeparam name="T">The type that implements the interface.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/>.</typeparam>
public interface ISquareMatrix<T, U>
    : IMatrix<T, U>,
      IMultiplicationOperation<T, T>,
      ITransposeOperation<T, U>
    where T : ISquareMatrix<T, U>
    where U : IComplex<U>
{
    /// <summary>Gets the multiplicative identiy matrix.</summary>
    static abstract T Identity { get; }

    /// <summary>Compute the determinant of the matrix.</summary>
    /// <returns>The determinant.</returns>
    U Determinant();

    /// <summary>Compute the inverse of the matrix.</summary>
    /// <returns>The inverse if the matrix is invertible; otherwise, <see cref="IMatrix{T, U}.NaM"/>.</returns>
    T Inverse();

    /// <summary>Compute the trace of the matrix.</summary>
    /// <returns>The trace.</returns>
    U Trace();
}
