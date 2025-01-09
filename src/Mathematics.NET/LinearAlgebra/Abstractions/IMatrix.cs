// <copyright file="IMatrix.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.LinearAlgebra.Abstractions;

/// <summary>Defines support for matrices.</summary>
/// <typeparam name="T">The type that implements the interface.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/>.</typeparam>
public interface IMatrix<T, U>
    : ITwoDimensionalArrayRepresentable<T, U>,
      IAdditionOperation<T, T>,
      ISubtractionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      IMultiplicationOperation<T, U, T>,
      IUnaryMinusOperation<T, T>,
      IUnaryPlusOperation<T, T>
    where T : IMatrix<T, U>
    where U : IComplex<U>
{
    /// <summary>Represents a value that is not a matrix.</summary>
    /// <remarks>This will be returned, for instance, when trying to invert a singular matrix.</remarks>
    static abstract T NaM { get; }

    /// <summary>Check if a value is not a matrix.</summary>
    /// <param name="matrix">The value to check.</param>
    /// <returns><see langword="true"/> if the value is not a matrix; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsNaM(T matrix);

    /// <summary>Compute the transpose of the matrix.</summary>
    /// <returns>The transpose.</returns>
    T Transpose();
}
