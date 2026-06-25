// <copyright file="Tensor`4.cs" company="Mathematics.NET">
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Operations;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-one tensor or a similar mathematical object.</summary>
/// <typeparam name="TV">A backing type that implements <see cref="IVector{T, U, V, W}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TI">An index.</typeparam>
/// <param name="vector">A backing vector.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TV, TN, U, TI>(TV vector)
    : IRankOneTensor<Tensor<TV, TN, U, TI>, TV, TN, U, U, TI>,
      IMultiplicationOperation<Tensor<TV, TN, U, TI>, TN, Tensor<TV, TN, U, TI>>,
      IUnaryMinusOperation<Tensor<TV, TN, U, TI>, Tensor<TV, TN, U, TI>>,
      IUnaryPlusOperation<Tensor<TV, TN, U, TI>, Tensor<TV, TN, U, TI>>
    where TV : IVector<TV, TN, U, U>
    where TN : IComplex<TN, U, U>, IDifferentiableFunctions<TN>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    where TI : IIndex
{
    private TV _vector = vector;

    //
    // IArrayRepresentable & Relevant Interfaces
    //

    public static int Components => TV.Components;

    public static int E1Components => TV.E1Components;

    //
    // IRankOneTensor Interface
    //

    public readonly IIndex I1 => TI.Instance;

    //
    // Indexer
    //

    public TN this[int i]
    {
        get => _vector[i];
        set => _vector[i] = value;
    }

    //
    // Operators
    //

    public static Tensor<TV, TN, U, TI> operator -(Tensor<TV, TN, U, TI> tensor)
        => new(-tensor._vector);

    public static Tensor<TV, TN, U, TI> operator +(Tensor<TV, TN, U, TI> tensor)
        => tensor;

    public static Tensor<TV, TN, U, TI> operator +(Tensor<TV, TN, U, TI> left, Tensor<TV, TN, U, TI> right)
        => new(left._vector + right._vector);

    public static Tensor<TV, TN, U, TI> operator -(Tensor<TV, TN, U, TI> left, Tensor<TV, TN, U, TI> right)
        => new(left._vector - right._vector);

    public static Tensor<TV, TN, U, TI> operator *(TN c, Tensor<TV, TN, U, TI> tensor)
        => new(c * tensor._vector);

    public static Tensor<TV, TN, U, TI> operator *(Tensor<TV, TN, U, TI> tensor, TN c)
        => new(tensor._vector * c);

    //
    // Equality
    //

    public static bool operator ==(Tensor<TV, TN, U, TI> left, Tensor<TV, TN, U, TI> right)
        => left._vector == right._vector;

    public static bool operator !=(Tensor<TV, TN, U, TI> left, Tensor<TV, TN, U, TI> right)
        => left._vector != right._vector;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TV, TN, U, TI> other && Equals(other);

    public readonly bool Equals(Tensor<TV, TN, U, TI> value) => _vector.Equals(value._vector);

    public override readonly int GetHashCode() => HashCode.Combine(_vector);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _vector.ToString(format, provider);

    //
    // Methods
    //

    public readonly TN[] ToArray() => _vector.ToArray();

    /// <summary>Reinterpret this tensor as one with a new index.</summary>
    /// <typeparam name="TNI">An index.</typeparam>
    /// <returns>A tensor with a new index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TV, TN, U, TNI> WithIndex<TNI>()
        where TNI : IIndex
        => Unsafe.As<Tensor<TV, TN, U, TI>, Tensor<TV, TN, U, TNI>>(ref this);

    //
    // Implicit Operators
    //

    public static implicit operator Tensor<TV, TN, U, TI>(TV input) => new(input);
}
