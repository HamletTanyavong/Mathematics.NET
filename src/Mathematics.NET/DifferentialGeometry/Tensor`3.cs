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

/// <summary>Represents a rank-one tensor</summary>
/// <typeparam name="T">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="V">An index</typeparam>
/// <param name="vector">A backing vector</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<T, U, V>(T vector)
    : IRankOneTensor<Tensor<T, U, V>, T, U, V>,
      IAdditionOperation<Tensor<T, U, V>, Tensor<T, U, V>>,
      ISubtractionOperation<Tensor<T, U, V>, Tensor<T, U, V>>
    where T : IVector<T, U>
    where U : IComplex<U>
    where V : IIndex
{
    private T _vector = vector;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => T.Components;

    public static int E1Components => T.E1Components;

    //
    // IRankOneTensor interface
    //

    public readonly IIndex I1 => V.Instance;

    //
    // Indexer
    //

    public U this[int index]
    {
        get => _vector[index];
        set => _vector[index] = value;
    }

    //
    // Operators
    //

    public static Tensor<T, U, V> operator +(Tensor<T, U, V> left, Tensor<T, U, V> right)
        => new(left._vector + right._vector);

    public static Tensor<T, U, V> operator -(Tensor<T, U, V> left, Tensor<T, U, V> right)
        => new(left._vector - right._vector);

    //
    // Equality
    //

    public static bool operator ==(Tensor<T, U, V> left, Tensor<T, U, V> right)
        => left._vector == right._vector;

    public static bool operator !=(Tensor<T, U, V> left, Tensor<T, U, V> right)
        => left._vector != right._vector;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<T, U, V> other && Equals(other);

    public readonly bool Equals(Tensor<T, U, V> value) => _vector.Equals(value._vector);

    public override readonly int GetHashCode() => HashCode.Combine(_vector);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _vector.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index.</summary>
    /// <typeparam name="W">A new index</typeparam>
    /// <returns>A tensor with a new index</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<T, U, W> WithIndex<W>()
        where W : IIndex
        => Unsafe.As<Tensor<T, U, V>, Tensor<T, U, W>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<T, U, V>(T input) => new(input);
}
