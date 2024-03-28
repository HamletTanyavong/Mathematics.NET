// <copyright file="MetricTensor.cs" company="Mathematics.NET">
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
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a metric tensor</summary>
/// <typeparam name="TSquareMatrix">A backing type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndexPosition">An index position</typeparam>
/// <typeparam name="TIndex1Name">The name of the first index</typeparam>
/// <typeparam name="TIndex2Name">The name of the second index</typeparam>
/// <param name="matrix">A backing matrix</param>
[StructLayout(LayoutKind.Sequential)]
public struct MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>(TSquareMatrix matrix)
    : IRankTwoTensor<MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>, TSquareMatrix, TNumber, Index<TIndexPosition, TIndex1Name>, Index<TIndexPosition, TIndex2Name>>,
      IAdditionOperation<MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>, Tensor<TSquareMatrix, TNumber, Index<TIndexPosition, TIndex1Name>, Index<TIndexPosition, TIndex2Name>>>,
      ISubtractionOperation<MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>, Tensor<TSquareMatrix, TNumber, Index<TIndexPosition, TIndex1Name>, Index<TIndexPosition, TIndex2Name>>>
    where TSquareMatrix : ISquareMatrix<TSquareMatrix, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndexPosition : IIndexPosition
    where TIndex1Name : ISymbol
    where TIndex2Name : ISymbol
{
    public static readonly MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> Euclidean = TSquareMatrix.Identity;

    private TSquareMatrix _matrix = matrix;

    //
    // IRankTwoTensor interface
    //

    public readonly IIndex I1 => Index<TIndexPosition, TIndex1Name>.Instance;

    public readonly IIndex I2 => Index<TIndexPosition, TIndex2Name>.Instance;

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

    public static MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> operator -(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> metric)
        => new(-metric._matrix);

    public static MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> operator +(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> metric)
        => metric;

    public static Tensor<TSquareMatrix, TNumber, Index<TIndexPosition, TIndex1Name>, Index<TIndexPosition, TIndex2Name>> operator +(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> left, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> right)
        => left._matrix + right._matrix;

    public static Tensor<TSquareMatrix, TNumber, Index<TIndexPosition, TIndex1Name>, Index<TIndexPosition, TIndex2Name>> operator -(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> left, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> right)
        => left._matrix - right._matrix;

    //
    // Equality
    //

    public static bool operator ==(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> left, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> right)
        => left._matrix == right._matrix;

    public static bool operator !=(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> left, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> right)
        => left._matrix == right._matrix;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> other && Equals(other);

    public readonly bool Equals(MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name> value) => _matrix.Equals(value._matrix);

    public override readonly int GetHashCode() => HashCode.Combine(_matrix);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _matrix.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index in the first position.</summary>
    /// <typeparam name="TNewIndexName">A symbol</typeparam>
    /// <returns>A tensor with a new index in the first position</returns>
    public MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TNewIndexName, TIndex2Name> WithIndexOne<TNewIndexName>()
        where TNewIndexName : ISymbol
        => Unsafe.As<MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TNewIndexName, TIndex2Name>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="TNewIndexName">A symbol</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    public MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TNewIndexName> WithIndexTwo<TNewIndexName>()
        where TNewIndexName : ISymbol
        => Unsafe.As<MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>, MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TNewIndexName>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator MetricTensor<TSquareMatrix, TNumber, TIndexPosition, TIndex1Name, TIndex2Name>(TSquareMatrix input) => new(input);
}
