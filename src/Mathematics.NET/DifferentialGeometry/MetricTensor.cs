// <copyright file="MetricTensor.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a metric tensor.</summary>
/// <typeparam name="TSM">A backing type that implements <see cref="ISquareMatrix{T, U}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
/// <typeparam name="TIP">An index position.</typeparam>
/// <typeparam name="TI1N">The name of the first index.</typeparam>
/// <typeparam name="TI2N">The name of the second index.</typeparam>
/// <param name="matrix">A backing matrix.</param>
[StructLayout(LayoutKind.Sequential)]
public struct MetricTensor<TSM, TN, TIP, TI1N, TI2N>(TSM matrix)
    : IRankTwoTensor<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, TSM, TN, Index<TIP, TI1N>, Index<TIP, TI2N>>,
      IAdditionOperation<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, Tensor<TSM, TN, Index<TIP, TI1N>, Index<TIP, TI2N>>>,
      ISubtractionOperation<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, Tensor<TSM, TN, Index<TIP, TI1N>, Index<TIP, TI2N>>>
    where TSM : ISquareMatrix<TSM, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TIP : IIndexPosition
    where TI1N : IIndexName
    where TI2N : IIndexName
{
    public static readonly MetricTensor<TSM, TN, TIP, TI1N, TI2N> Euclidean = TSM.Identity;

    private TSM _matrix = matrix;

    //
    // IRankTwoTensor interface
    //

    public readonly IIndex I1 => Index<TIP, TI1N>.Instance;

    public readonly IIndex I2 => Index<TIP, TI2N>.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TSM.Components;

    public static int E1Components => TSM.E1Components;

    public static int E2Components => TSM.E2Components;

    //
    // Indexer
    //

    public TN this[int i, int j]
    {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }

    //
    // Operators
    //

    public static MetricTensor<TSM, TN, TIP, TI1N, TI2N> operator -(MetricTensor<TSM, TN, TIP, TI1N, TI2N> metric)
        => new(-metric._matrix);

    public static MetricTensor<TSM, TN, TIP, TI1N, TI2N> operator +(MetricTensor<TSM, TN, TIP, TI1N, TI2N> metric)
        => metric;

    public static Tensor<TSM, TN, Index<TIP, TI1N>, Index<TIP, TI2N>> operator +(MetricTensor<TSM, TN, TIP, TI1N, TI2N> left, MetricTensor<TSM, TN, TIP, TI1N, TI2N> right)
        => left._matrix + right._matrix;

    public static Tensor<TSM, TN, Index<TIP, TI1N>, Index<TIP, TI2N>> operator -(MetricTensor<TSM, TN, TIP, TI1N, TI2N> left, MetricTensor<TSM, TN, TIP, TI1N, TI2N> right)
        => left._matrix - right._matrix;

    public static MetricTensor<TSM, TN, TIP, TI1N, TI2N> operator *(TN c, MetricTensor<TSM, TN, TIP, TI1N, TI2N> metric)
        => new(c * metric._matrix);

    public static MetricTensor<TSM, TN, TIP, TI1N, TI2N> operator *(MetricTensor<TSM, TN, TIP, TI1N, TI2N> metric, TN c)
        => new(metric._matrix * c);

    //
    // Equality
    //

    public static bool operator ==(MetricTensor<TSM, TN, TIP, TI1N, TI2N> left, MetricTensor<TSM, TN, TIP, TI1N, TI2N> right)
        => left._matrix == right._matrix;

    public static bool operator !=(MetricTensor<TSM, TN, TIP, TI1N, TI2N> left, MetricTensor<TSM, TN, TIP, TI1N, TI2N> right)
        => left._matrix == right._matrix;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is MetricTensor<TSM, TN, TIP, TI1N, TI2N> other && Equals(other);

    public readonly bool Equals(MetricTensor<TSM, TN, TIP, TI1N, TI2N> value) => _matrix.Equals(value._matrix);

    public override readonly int GetHashCode() => HashCode.Combine(_matrix);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _matrix.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Compute the determinant of the metric.</summary>
    /// <returns>The determinant.</returns>
    public TN Determinant() => _matrix.Determinant();

    /// <summary>Compute the inverse of this metric tensor.</summary>
    /// <remarks>New index names must be supplied.</remarks>
    /// <typeparam name="TNI1N">The name of the first index.</typeparam>
    /// <typeparam name="TNI2N">The name of the second index.</typeparam>
    /// <returns>The inverse of this metric tensor with new index names.</returns>
    public MetricTensor<TSM, TN, Upper, TNI1N, TNI2N> Inverse<TNI1N, TNI2N>()
        where TNI1N : IIndexName
        where TNI2N : IIndexName
        => new(_matrix.Inverse());

    /// <summary>Compute the square root of the determinant of this metric tensor.</summary>
    /// <returns>The square root of the determinant.</returns>
    public TN SquareRootOfDeterminant() => TN.Sqrt(Determinant());

    public TN[,] ToArray() => _matrix.ToArray();

    /// <summary>Compute the trace of this metric tensor.</summary>
    /// <remarks>Note that the trace of an <c>n x n</c> metric tensor is equal to <c>n</c>.</remarks>
    /// <returns>The trace.</returns>
    public readonly TN Trace() => TSM.E1Components;

    /// <summary>Reinterpret this metric tensor as one with new index names.</summary>
    /// <typeparam name="TNI1N">The name of the first index.</typeparam>
    /// <typeparam name="TNI2N">The name of the second index.</typeparam>
    /// <returns>A tensor with new index names.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MetricTensor<TSM, TN, TIP, TNI1N, TNI2N> WithIndices<TNI1N, TNI2N>()
        where TNI1N : IIndexName
        where TNI2N : IIndexName
        => Unsafe.As<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, MetricTensor<TSM, TN, TIP, TNI1N, TNI2N>>(ref this);

    /// <summary>Reinterpret this metric tensor as one with a new index name in the first position.</summary>
    /// <typeparam name="TNIN">A new index name.</typeparam>
    /// <returns>A tensor with a new index name in the first position.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MetricTensor<TSM, TN, TIP, TNIN, TI2N> WithIndex1Name<TNIN>()
        where TNIN : IIndexName
        => Unsafe.As<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, MetricTensor<TSM, TN, TIP, TNIN, TI2N>>(ref this);

    /// <summary>Reinterpret this metric tensor as one with a new index name in the second position.</summary>
    /// <typeparam name="TNIN">A new index name.</typeparam>
    /// <returns>A tensor with a new index name in the second position.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MetricTensor<TSM, TN, TIP, TI1N, TNIN> WithIndex2Name<TNIN>()
        where TNIN : IIndexName
        => Unsafe.As<MetricTensor<TSM, TN, TIP, TI1N, TI2N>, MetricTensor<TSM, TN, TIP, TI1N, TNIN>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator MetricTensor<TSM, TN, TIP, TI1N, TI2N>(TSM input) => new(input);
}
