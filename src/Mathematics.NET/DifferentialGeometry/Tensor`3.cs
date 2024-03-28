// <copyright file="Tensor`3.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-one tensor or a similar mathematical object</summary>
/// <typeparam name="TVector">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex">An index</typeparam>
/// <param name="vector">A backing vector</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TVector, TNumber, TIndex>(TVector vector)
    : IRankOneTensor<Tensor<TVector, TNumber, TIndex>, TVector, TNumber, TIndex>,
      IAdditionOperation<Tensor<TVector, TNumber, TIndex>, Tensor<TVector, TNumber, TIndex>>,
      ISubtractionOperation<Tensor<TVector, TNumber, TIndex>, Tensor<TVector, TNumber, TIndex>>
    where TVector : IVector<TVector, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex : IIndex
{
    private TVector _vector = vector;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TVector.Components;

    public static int E1Components => TVector.E1Components;

    //
    // IRankOneTensor interface
    //

    public readonly IIndex I1 => TIndex.Instance;

    //
    // Indexer
    //

    public TNumber this[int index]
    {
        get => _vector[index];
        set => _vector[index] = value;
    }

    //
    // Operators
    //

    public static Tensor<TVector, TNumber, TIndex> operator -(Tensor<TVector, TNumber, TIndex> tensor)
        => new(-tensor._vector);

    public static Tensor<TVector, TNumber, TIndex> operator +(Tensor<TVector, TNumber, TIndex> left, Tensor<TVector, TNumber, TIndex> right)
        => new(left._vector + right._vector);

    public static Tensor<TVector, TNumber, TIndex> operator -(Tensor<TVector, TNumber, TIndex> left, Tensor<TVector, TNumber, TIndex> right)
        => new(left._vector - right._vector);

    //
    // Equality
    //

    public static bool operator ==(Tensor<TVector, TNumber, TIndex> left, Tensor<TVector, TNumber, TIndex> right)
        => left._vector == right._vector;

    public static bool operator !=(Tensor<TVector, TNumber, TIndex> left, Tensor<TVector, TNumber, TIndex> right)
        => left._vector != right._vector;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TVector, TNumber, TIndex> other && Equals(other);

    public readonly bool Equals(Tensor<TVector, TNumber, TIndex> value) => _vector.Equals(value._vector);

    public override readonly int GetHashCode() => HashCode.Combine(_vector);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _vector.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TVector, TNumber, TNewIndex> WithIndex<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TVector, TNumber, TIndex>, Tensor<TVector, TNumber, TNewIndex>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TVector, TNumber, TIndex>(TVector input) => new(input);
}
