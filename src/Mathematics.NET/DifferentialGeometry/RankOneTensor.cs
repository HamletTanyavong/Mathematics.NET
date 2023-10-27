// <copyright file="RankOneTensor.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-one tensor</summary>
/// <typeparam name="T">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="V">An index</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct RankOneTensor<T, U, V>
    : IRankOneTensor<RankOneTensor<T, U, V>, T, U, V>,
      IAdditionOperation<RankOneTensor<T, U, V>, RankOneTensor<T, U, V>>,
      ISubtractionOperation<RankOneTensor<T, U, V>, RankOneTensor<T, U, V>>
    where T : IVector<T, U>
    where U : IComplex<U>
    where V : IIndex
{
    private T _vector;

    public RankOneTensor(T vector)
    {
        _vector = vector;
    }

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => T.Components;

    public static int E1Components => T.E1Components;

    //
    // IRankOneTensor interface
    //

    public readonly IIndex Index => V.Instance;

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

    public static RankOneTensor<T, U, V> operator +(RankOneTensor<T, U, V> left, RankOneTensor<T, U, V> right)
        => new(left._vector + right._vector);

    public static RankOneTensor<T, U, V> operator -(RankOneTensor<T, U, V> left, RankOneTensor<T, U, V> right)
        => new(left._vector - right._vector);

    //
    // Equality
    //

    public static bool operator ==(RankOneTensor<T, U, V> left, RankOneTensor<T, U, V> right)
        => left._vector == right._vector;

    public static bool operator !=(RankOneTensor<T, U, V> left, RankOneTensor<T, U, V> right)
        => left._vector != right._vector;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is RankOneTensor<T, U, V> other && Equals(other);

    public bool Equals(RankOneTensor<T, U, V> value) => _vector.Equals(value._vector);

    public override int GetHashCode() => HashCode.Combine(_vector);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _vector.ToString(format, provider);

    //
    // Implicit operators
    //

    public static implicit operator RankOneTensor<T, U, V>(T input) => new(input);
}
