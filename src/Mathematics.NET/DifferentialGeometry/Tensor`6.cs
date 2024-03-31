// <copyright file="Tensor`6.cs" company="Mathematics.NET">
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
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-four tensor or a similar mathematical object</summary>
/// <typeparam name="THypercubic4DArray">A backing type that implements <see cref="IHypercubic4DArray{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">The first index</typeparam>
/// <typeparam name="TIndex2">The second index</typeparam>
/// <typeparam name="TIndex3">The third index</typeparam>
/// <typeparam name="TIndex4">The fourth index</typeparam>
/// <param name="array">A backing array</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>(THypercubic4DArray array)
    : IRankFourTensor<Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>, THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>
    where THypercubic4DArray : IHypercubic4DArray<THypercubic4DArray, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2 : IIndex
    where TIndex3 : IIndex
    where TIndex4 : IIndex
{
    private THypercubic4DArray _array = array;

    //
    // IRankFourTensor interface
    //

    public readonly IIndex I1 => TIndex1.Instance;

    public readonly IIndex I2 => TIndex2.Instance;

    public readonly IIndex I3 => TIndex3.Instance;

    public readonly IIndex I4 => TIndex4.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => THypercubic4DArray.Components;

    public static int E1Components => THypercubic4DArray.E1Components;

    public static int E2Components => THypercubic4DArray.E2Components;

    public static int E3Components => THypercubic4DArray.E3Components;

    public static int E4Components => THypercubic4DArray.E4Components;

    //
    // Indexer
    //

    public TNumber this[int i, int j, int k, int l]
    {
        get => _array[i, j, k, l];
        set => _array[i, j, k, l] = value;
    }

    //
    // Operators
    //

    public static Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> operator -(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> tensor)
        => new(-tensor._array);

    public static Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> operator +(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> tensor)
        => tensor;

    public static Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> operator *(TNumber c, Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> tensor)
        => new(c * tensor._array);

    public static Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> operator *(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> tensor, TNumber c)
        => new(tensor._array * c);

    //
    // Equality
    //

    public static bool operator ==(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> left, Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> right)
    => left._array == right._array;

    public static bool operator !=(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> left, Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> other && Equals(other);

    public readonly bool Equals(Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index in the first position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the first position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<THypercubic4DArray, TNumber, TNewIndex, TIndex2, TIndex3, TIndex4> WithIndexOne<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>, Tensor<THypercubic4DArray, TNumber, TNewIndex, TIndex2, TIndex3, TIndex4>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<THypercubic4DArray, TNumber, TIndex1, TNewIndex, TIndex3, TIndex4> WithIndexTwo<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>, Tensor<THypercubic4DArray, TNumber, TIndex1, TNewIndex, TIndex3, TIndex4>>(ref this);

    /// <summary>Create a tensor with a new index in the third position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the third position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TNewIndex, TIndex4> WithIndexThree<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>, Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TNewIndex, TIndex4>>(ref this);

    /// <summary>Create a tensor with a new index in the fourth position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the fourth position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TNewIndex> WithIndexFour<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>, Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TNewIndex>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<THypercubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>(THypercubic4DArray value) => new(value);
}
