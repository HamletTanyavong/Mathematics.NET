// <copyright file="RankFourTensor.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-four tensor</summary>
/// <typeparam name="T">A backing type that implements <see cref="IHyperCubic4DArray{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="V">The first index</typeparam>
/// <typeparam name="W">The second index</typeparam>
/// <typeparam name="X">The third index</typeparam>
/// <typeparam name="Y">The fourth index</typeparam>
/// <param name="array">A backing array</param>
[StructLayout(LayoutKind.Sequential)]
public struct RankFourTensor<T, U, V, W, X, Y>(T array)
    : IRankFourTensor<RankFourTensor<T, U, V, W, X, Y>, T, U, V, W, X, Y>
    where T : IHyperCubic4DArray<T, U>
    where U : IComplex<U>
    where V : IIndex
    where W : IIndex
    where X : IIndex
    where Y : IIndex
{
    private T _array = array;

    //
    // IRankFourTensor interface
    //

    public readonly IIndex I1 => V.Instance;

    public readonly IIndex I2 => W.Instance;

    public readonly IIndex I3 => X.Instance;

    public readonly IIndex I4 => Y.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => T.Components;

    public static int E1Components => T.E1Components;

    public static int E2Components => T.E2Components;

    public static int E3Components => T.E3Components;

    public static int E4Components => T.E4Components;

    //
    // Indexer
    //

    public U this[int i, int j, int k, int l]
    {
        get => _array[i, j, k, l];
        set => _array[i, j, k, l] = value;
    }

    //
    // Equality
    //

    public static bool operator ==(RankFourTensor<T, U, V, W, X, Y> left, RankFourTensor<T, U, V, W, X, Y> right)
    => left._array == right._array;

    public static bool operator !=(RankFourTensor<T, U, V, W, X, Y> left, RankFourTensor<T, U, V, W, X, Y> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is RankFourTensor<T, U, V, W, X, Y> other && Equals(other);

    public readonly bool Equals(RankFourTensor<T, U, V, W, X, Y> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a tensor with a new index in the first position.</summary>
    /// <typeparam name="Z">A new index</typeparam>
    /// <returns>A tensor with a new index in the first position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RankFourTensor<T, U, Z, W, X, Y> WithIndexOne<Z>()
        where Z : IIndex
        => Unsafe.As<RankFourTensor<T, U, V, W, X, Y>, RankFourTensor<T, U, Z, W, X, Y>>(ref this);

    /// <summary>Create a tensor with a new index in the second position.</summary>
    /// <typeparam name="Z">A new index</typeparam>
    /// <returns>A tensor with a new index in the second position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RankFourTensor<T, U, V, Z, X, Y> WithIndexTwo<Z>()
        where Z : IIndex
        => Unsafe.As<RankFourTensor<T, U, V, W, X, Y>, RankFourTensor<T, U, V, Z, X, Y>>(ref this);

    /// <summary>Create a tensor with a new index in the third position.</summary>
    /// <typeparam name="Z">A new index</typeparam>
    /// <returns>A tensor with a new index in the third position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RankFourTensor<T, U, V, W, Z, Y> WithIndexThree<Z>()
        where Z : IIndex
        => Unsafe.As<RankFourTensor<T, U, V, W, X, Y>, RankFourTensor<T, U, V, W, Z, Y>>(ref this);

    /// <summary>Create a tensor with a new index in the fourth position.</summary>
    /// <typeparam name="Z">A new index</typeparam>
    /// <returns>A tensor with a new index in the fourth position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RankFourTensor<T, U, V, W, X, Z> WithIndexFour<Z>()
        where Z : IIndex
        => Unsafe.As<RankFourTensor<T, U, V, W, X, Y>, RankFourTensor<T, U, V, W, X, Z>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator RankFourTensor<T, U, V, W, X, Y>(T value) => new(value);
}
