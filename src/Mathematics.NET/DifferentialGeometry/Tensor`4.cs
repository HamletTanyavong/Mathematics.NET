﻿// <copyright file="Tensor`4.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-two tensor or a similar mathematical object</summary>
/// <typeparam name="TSquareMatrix">A backing type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">The first index</typeparam>
/// <typeparam name="TIndex2">The second index</typeparam>
/// <param name="matrix">A backing matrix</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>(TSquareMatrix matrix)
    : IRankTwoTensor<Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>, TSquareMatrix, TNumber, TIndex1, TIndex2>,
      IAdditionOperation<Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>>,
      ISubtractionOperation<Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>>
    where TSquareMatrix : ISquareMatrix<TSquareMatrix, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2 : IIndex
{
    private TSquareMatrix _matrix = matrix;

    //
    // IRankTwoTensor interface
    //

    public readonly IIndex I1 => TIndex1.Instance;

    public readonly IIndex I2 => TIndex2.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TSquareMatrix.Components;

    public static int E1Components => TSquareMatrix.E1Components;

    public static int E2Components => TSquareMatrix.E2Components;

    //
    // Indexer
    //

    public TNumber this[int row, int column]
    {
        get => _matrix[row, column];
        set => _matrix[row, column] = value;
    }

    //
    // Operators
    //

    public static Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> operator +(Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> left, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> right)
        => left._matrix + right._matrix;

    public static Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> operator -(Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> left, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> right)
        => left._matrix - right._matrix;

    //
    // Equality
    //

    public static bool operator ==(Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> left, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> right)
        => left._matrix == right._matrix;

    public static bool operator !=(Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> left, Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> right)
        => left._matrix != right._matrix;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> other && Equals(other);

    public readonly bool Equals(Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2> value) => _matrix.Equals(value._matrix);

    public override readonly int GetHashCode() => HashCode.Combine(_matrix);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _matrix.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index in the first position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the first position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TSquareMatrix, TNumber, TNewIndex, TIndex2> WithIndexOne<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>, Tensor<TSquareMatrix, TNumber, TNewIndex, TIndex2>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TSquareMatrix, TNumber, TIndex1, TNewIndex> WithIndexTwo<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>, Tensor<TSquareMatrix, TNumber, TIndex1, TNewIndex>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TSquareMatrix, TNumber, TIndex1, TIndex2>(TSquareMatrix input) => new(input);
}
