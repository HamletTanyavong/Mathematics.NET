// <copyright file="Tensor`5.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-three tensor or a similar mathematical object</summary>
/// <typeparam name="TCubicArray">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">The first index</typeparam>
/// <typeparam name="TIndex2">The second index</typeparam>
/// <typeparam name="TIndex3">The third index</typeparam>
/// <param name="array">A backing array</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>(TCubicArray array)
    : IRankThreeTensor<Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>, TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>
    where TCubicArray : ICubicArray<TCubicArray, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2 : IIndex
    where TIndex3 : IIndex
{
    private TCubicArray _array = array;

    //
    // IRankThreeTensor interface
    //

    public readonly IIndex I1 => TIndex1.Instance;

    public readonly IIndex I2 => TIndex2.Instance;

    public readonly IIndex I3 => TIndex3.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TCubicArray.Components;

    public static int E1Components => TCubicArray.E1Components;

    public static int E2Components => TCubicArray.E2Components;

    public static int E3Components => TCubicArray.E3Components;

    //
    // Indexer
    //

    public TNumber this[int i, int j, int k]
    {
        get => _array[i, j, k];
        set => _array[i, j, k] = value;
    }

    //
    // Operators
    //

    public static Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> operator -(Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> tensor)
        => new(-tensor._array);

    public static Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> operator +(Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> tensor)
        => tensor;

    //
    // Equality
    //

    public static bool operator ==(Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> left, Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> right)
        => left._array == right._array;

    public static bool operator !=(Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> left, Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> other && Equals(other);

    public readonly bool Equals(Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> value) => _array.Equals(value._array);

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
    public Tensor<TCubicArray, TNumber, TNewIndex, TIndex2, TIndex3> WithIndexOne<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>, Tensor<TCubicArray, TNumber, TNewIndex, TIndex2, TIndex3>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TCubicArray, TNumber, TIndex1, TNewIndex, TIndex3> WithIndexTwo<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>, Tensor<TCubicArray, TNumber, TIndex1, TNewIndex, TIndex3>>(ref this);

    /// <summary>Create a tensor with a new index in the third position.</summary>
    /// <typeparam name="TNewIndex">A new index</typeparam>
    /// <returns>A tensor with a new index in the third position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TNewIndex> WithIndexThree<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>, Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TNewIndex>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>(TCubicArray value) => new(value);
}
