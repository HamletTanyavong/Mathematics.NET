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
using System.Runtime.InteropServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a metric tensor</summary>
/// <typeparam name="T">A backing type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="V">An index position</typeparam>
/// <typeparam name="W">The first index</typeparam>
/// <typeparam name="X">The second index</typeparam>
/// <param name="matrix">A backing matrix</param>
[StructLayout(LayoutKind.Sequential)]
public struct MetricTensor<T, U, V, W, X>(T matrix)
    : IRankTwoTensor<MetricTensor<T, U, V, W, X>, T, U, Index<V, W>, Index<V, X>>,
      IAdditionOperation<MetricTensor<T, U, V, W, X>, RankTwoTensor<T, U, Index<V, W>, Index<V, X>>>,
      ISubtractionOperation<MetricTensor<T, U, V, W, X>, RankTwoTensor<T, U, Index<V, W>, Index<V, X>>>
    where T : ISquareMatrix<T, U>
    where U : IComplex<U>
    where V : IIndexPosition
    where W : ISymbol
    where X : ISymbol
{
    public static readonly MetricTensor<T, U, V, W, X> Euclidean = T.Identity;

    private T _matrix = matrix;

    //
    // IRankTwoTensor interface
    //

    public readonly IIndex[] Indices => new IIndex[2] { Index<V, W>.Instance, Index<V, X>.Instance };

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => T.Components;

    public static int E1Components => T.E1Components;

    public static int E2Components => T.E2Components;

    //
    // Indexer
    //

    public U this[int row, int column]
    {
        get => _matrix[row, column];
        set => _matrix[row, column] = value;
    }

    //
    // Operators
    //

    public static RankTwoTensor<T, U, Index<V, W>, Index<V, X>> operator +(MetricTensor<T, U, V, W, X> left, MetricTensor<T, U, V, W, X> right)
        => left._matrix + right._matrix;

    public static RankTwoTensor<T, U, Index<V, W>, Index<V, X>> operator -(MetricTensor<T, U, V, W, X> left, MetricTensor<T, U, V, W, X> right)
        => left._matrix - right._matrix;

    //
    // Equality
    //

    public static bool operator ==(MetricTensor<T, U, V, W, X> left, MetricTensor<T, U, V, W, X> right)
        => left._matrix == right._matrix;

    public static bool operator !=(MetricTensor<T, U, V, W, X> left, MetricTensor<T, U, V, W, X> right)
        => left._matrix == right._matrix;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is MetricTensor<T, U, V, W, X> other && Equals(other);

    public bool Equals(MetricTensor<T, U, V, W, X> value) => _matrix.Equals(value._matrix);

    public override int GetHashCode() => HashCode.Combine(_matrix);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _matrix.ToString(format, provider);

    //
    // Implicit operators
    //

    public static implicit operator MetricTensor<T, U, V, W, X>(T input) => new(input);
}

