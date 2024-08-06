// <copyright file="Tensor`3.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-one tensor or a similar mathematical object.</summary>
/// <typeparam name="TV">A backing type that implements <see cref="IVector{T, U}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
/// <typeparam name="TI">An index.</typeparam>
/// <param name="vector">A backing vector.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TV, TN, TI>(TV vector)
    : IRankOneTensor<Tensor<TV, TN, TI>, TV, TN, TI>,
      IAdditionOperation<Tensor<TV, TN, TI>, Tensor<TV, TN, TI>>,
      ISubtractionOperation<Tensor<TV, TN, TI>, Tensor<TV, TN, TI>>
    where TV : IVector<TV, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI : IIndex
{
    private TV _vector = vector;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TV.Components;

    public static int E1Components => TV.E1Components;

    //
    // IRankOneTensor interface
    //

    public readonly IIndex I1 => TI.Instance;

    //
    // Indexer
    //

    public TN this[int index]
    {
        get => _vector[index];
        set => _vector[index] = value;
    }

    //
    // Operators
    //

    public static Tensor<TV, TN, TI> operator -(Tensor<TV, TN, TI> tensor)
        => new(-tensor._vector);

    public static Tensor<TV, TN, TI> operator +(Tensor<TV, TN, TI> tensor)
        => tensor;

    public static Tensor<TV, TN, TI> operator +(Tensor<TV, TN, TI> left, Tensor<TV, TN, TI> right)
        => new(left._vector + right._vector);

    public static Tensor<TV, TN, TI> operator -(Tensor<TV, TN, TI> left, Tensor<TV, TN, TI> right)
        => new(left._vector - right._vector);

    public static Tensor<TV, TN, TI> operator *(TN c, Tensor<TV, TN, TI> tensor)
        => new(c * tensor._vector);

    public static Tensor<TV, TN, TI> operator *(Tensor<TV, TN, TI> tensor, TN c)
        => new(tensor._vector * c);

    //
    // Equality
    //

    public static bool operator ==(Tensor<TV, TN, TI> left, Tensor<TV, TN, TI> right)
        => left._vector == right._vector;

    public static bool operator !=(Tensor<TV, TN, TI> left, Tensor<TV, TN, TI> right)
        => left._vector != right._vector;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TV, TN, TI> other && Equals(other);

    public readonly bool Equals(Tensor<TV, TN, TI> value) => _vector.Equals(value._vector);

    public override readonly int GetHashCode() => HashCode.Combine(_vector);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _vector.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Reinterpret a reference to this tensor as one with a new index.</summary>
    /// <typeparam name="TNI">An index.</typeparam>
    /// <returns>A reference to this tensor with a new index.</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TV, TN, TNI> WithIndex<TNI>()
        where TNI : IIndex
        => ref Unsafe.As<Tensor<TV, TN, TI>, Tensor<TV, TN, TNI>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TV, TN, TI>(TV input) => new(input);
}
