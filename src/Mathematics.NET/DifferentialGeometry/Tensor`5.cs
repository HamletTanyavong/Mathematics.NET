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
/// <typeparam name="TCA">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TI1">The first index</typeparam>
/// <typeparam name="TI2">The second index</typeparam>
/// <typeparam name="TI3">The third index</typeparam>
/// <param name="array">A backing array</param>
[StructLayout(LayoutKind.Sequential)]
public struct Tensor<TCA, TN, TI1, TI2, TI3>(TCA array)
    : IRankThreeTensor<Tensor<TCA, TN, TI1, TI2, TI3>, TCA, TN, TI1, TI2, TI3>
    where TCA : ICubicArray<TCA, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2 : IIndex
    where TI3 : IIndex
{
    private TCA _array = array;

    //
    // IRankThreeTensor interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => TI2.Instance;

    public readonly IIndex I3 => TI3.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => TCA.Components;

    public static int E1Components => TCA.E1Components;

    public static int E2Components => TCA.E2Components;

    public static int E3Components => TCA.E3Components;

    //
    // Indexer
    //

    public TN this[int i, int j, int k]
    {
        get => _array[i, j, k];
        set => _array[i, j, k] = value;
    }

    //
    // Operators
    //

    public static Tensor<TCA, TN, TI1, TI2, TI3> operator -(Tensor<TCA, TN, TI1, TI2, TI3> tensor)
        => new(-tensor._array);

    public static Tensor<TCA, TN, TI1, TI2, TI3> operator +(Tensor<TCA, TN, TI1, TI2, TI3> tensor)
        => tensor;

    public static Tensor<TCA, TN, TI1, TI2, TI3> operator *(TN c, Tensor<TCA, TN, TI1, TI2, TI3> tensor)
        => new(c * tensor._array);

    public static Tensor<TCA, TN, TI1, TI2, TI3> operator *(Tensor<TCA, TN, TI1, TI2, TI3> tensor, TN c)
        => new(tensor._array * c);

    //
    // Equality
    //

    public static bool operator ==(Tensor<TCA, TN, TI1, TI2, TI3> left, Tensor<TCA, TN, TI1, TI2, TI3> right)
        => left._array == right._array;

    public static bool operator !=(Tensor<TCA, TN, TI1, TI2, TI3> left, Tensor<TCA, TN, TI1, TI2, TI3> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Tensor<TCA, TN, TI1, TI2, TI3> other && Equals(other);

    public readonly bool Equals(Tensor<TCA, TN, TI1, TI2, TI3> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index in the first position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A tensor with a new index in the first position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TCA, TN, TNI, TI2, TI3> WithIndexOne<TNI>()
        where TNI : IIndex
        => Unsafe.As<Tensor<TCA, TN, TI1, TI2, TI3>, Tensor<TCA, TN, TNI, TI2, TI3>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TCA, TN, TI1, TNI, TI3> WithIndexTwo<TNI>()
        where TNI : IIndex
        => Unsafe.As<Tensor<TCA, TN, TI1, TI2, TI3>, Tensor<TCA, TN, TI1, TNI, TI3>>(ref this);

    /// <summary>Create a tensor with a new index in the third position.</summary>
    /// <typeparam name="TNI">A new index</typeparam>
    /// <returns>A tensor with a new index in the third position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tensor<TCA, TN, TI1, TI2, TNI> WithIndexThree<TNI>()
        where TNI : IIndex
        => Unsafe.As<Tensor<TCA, TN, TI1, TI2, TI3>, Tensor<TCA, TN, TI1, TI2, TNI>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Tensor<TCA, TN, TI1, TI2, TI3>(TCA value) => new(value);
}
