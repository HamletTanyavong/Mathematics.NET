// <copyright file="Tensor`6.cs" company="Mathematics.NET">
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
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-four tensor or a similar mathematical object</summary>
/// <typeparam name="TH4DA">A backing type that implements <see cref="IHypercubic4DArray{T, U}"/></typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TI1">The first index</typeparam>
/// <typeparam name="TI2">The second index</typeparam>
/// <typeparam name="TI3">The third index</typeparam>
/// <typeparam name="TI4">The fourth index</typeparam>
/// <param name="array">A backing array</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>(TH4DA array)
    : IRankFourTensor<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, TH4DA, TN, TI1, TI2, TI3, TI4>
    where TH4DA : IHypercubic4DArray<TH4DA, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2 : IIndex
    where TI3 : IIndex
    where TI4 : IIndex
{
    private TH4DA _array = array;

    //
    // IRankFourTensor interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => TI2.Instance;

    public readonly IIndex I3 => TI3.Instance;

    public readonly IIndex I4 => TI4.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TH4DA.Components;

    public static int E1Components => TH4DA.E1Components;

    public static int E2Components => TH4DA.E2Components;

    public static int E3Components => TH4DA.E3Components;

    public static int E4Components => TH4DA.E4Components;

    //
    // Indexer
    //

    public TN this[int i, int j, int k, int l]
    {
        get => _array[i, j, k, l];
        set => _array[i, j, k, l] = value;
    }

    //
    // Operators
    //

    public static Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> operator -(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> tensor)
        => new(-tensor._array);

    public static Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> operator +(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> tensor)
        => tensor;

    public static Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> operator *(TN c, Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> tensor)
        => new(c * tensor._array);

    public static Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> operator *(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> tensor, TN c)
        => new(tensor._array * c);

    //
    // Equality
    //

    public static bool operator ==(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> left, Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> right)
        => left._array == right._array;

    public static bool operator !=(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> left, Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> other && Equals(other);

    public readonly bool Equals(Tensor<TH4DA, TN, TI1, TI2, TI3, TI4> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    public readonly void CopyTo(ref TN[,,,] destination) => _array.CopyTo(ref destination);

    /// <summary>Reinterpret a reference to this tensor as one with new indices.</summary>
    /// <typeparam name="TNI1">A new first index</typeparam>
    /// <typeparam name="TNI2">A new second index</typeparam>
    /// <typeparam name="TNI3">A new third index</typeparam>
    /// <typeparam name="TNI4">A new fourth index</typeparam>
    /// <returns>A reference to this tensor with new indices</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TH4DA, TN, TNI1, TNI2, TNI3, TNI4> WithIndices<TNI1, TNI2, TNI3, TNI4>()
        where TNI1 : IIndex
        where TNI2 : IIndex
        where TNI3 : IIndex
        where TNI4 : IIndex
        => ref Unsafe.As<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, Tensor<TH4DA, TN, TNI1, TNI2, TNI3, TNI4>>(ref this);

    /// <summary>Reinterpret a reference to this tensor as one with a new index in the first position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A reference to this tensor with a new index in the first position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TH4DA, TN, TNI, TI2, TI3, TI4> WithIndex1<TNI>()
        where TNI : IIndex
        => ref Unsafe.As<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, Tensor<TH4DA, TN, TNI, TI2, TI3, TI4>>(ref this);

    /// <summary>Reinterpret a reference to this tensor as one with a new index in the second position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A reference to this tensor with a new index in the second position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TH4DA, TN, TI1, TNI, TI3, TI4> WithIndex2<TNI>()
        where TNI : IIndex
        => ref Unsafe.As<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, Tensor<TH4DA, TN, TI1, TNI, TI3, TI4>>(ref this);

    /// <summary>Reinterpret a reference to this tensor as one with a new index in the third position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A reference to this tensor with a new index in the third position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TH4DA, TN, TI1, TI2, TNI, TI4> WithIndex3<TNI>()
        where TNI : IIndex
        => ref Unsafe.As<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, Tensor<TH4DA, TN, TI1, TI2, TNI, TI4>>(ref this);

    /// <summary>Reinterpret a reference to this tensor as one with a new index in the fourth position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A reference to this tensor with a new index in the fourth position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Tensor<TH4DA, TN, TI1, TI2, TI3, TNI> WithIndex4<TNI>()
        where TNI : IIndex
        => ref Unsafe.As<Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>, Tensor<TH4DA, TN, TI1, TI2, TI3, TNI>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TH4DA, TN, TI1, TI2, TI3, TI4>(TH4DA value) => new(value);
}
