// <copyright file="Christoffel.cs" company="Mathematics.NET">
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
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a Christoffel symbol</summary>
/// <typeparam name="TCA">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TI1">The first index</typeparam>
/// <typeparam name="TI2N">The name of the second index</typeparam>
/// <typeparam name="TI3N">The name of the third index</typeparam>
/// <param name="array">A backing array</param>
public struct Christoffel<TCA, TN, TI1, TI2N, TI3N>(TCA array)
    : IRankThreeTensor<Christoffel<TCA, TN, TI1, TI2N, TI3N>, TCA, TN, TI1, Index<Lower, TI2N>, Index<Lower, TI3N>>
    where TCA : ICubicArray<TCA, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2N : ISymbol
    where TI3N : ISymbol
{
    private TCA _array = array;

    //
    // IRankThreeTensor interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => Index<Lower, TI2N>.Instance;

    public readonly IIndex I3 => Index<Lower, TI3N>.Instance;

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

    public static Christoffel<TCA, TN, TI1, TI2N, TI3N> operator -(Christoffel<TCA, TN, TI1, TI2N, TI3N> christoffel)
        => new(-christoffel._array);

    public static Christoffel<TCA, TN, TI1, TI2N, TI3N> operator +(Christoffel<TCA, TN, TI1, TI2N, TI3N> christoffel)
        => christoffel;

    public static Christoffel<TCA, TN, TI1, TI2N, TI3N> operator *(TN c, Christoffel<TCA, TN, TI1, TI2N, TI3N> christoffel)
        => new(c * christoffel._array);

    public static Christoffel<TCA, TN, TI1, TI2N, TI3N> operator *(Christoffel<TCA, TN, TI1, TI2N, TI3N> christoffel, TN c)
        => new(christoffel._array * c);

    //
    // Equality
    //

    public static bool operator ==(Christoffel<TCA, TN, TI1, TI2N, TI3N> left, Christoffel<TCA, TN, TI1, TI2N, TI3N> right)
        => left._array == right._array;

    public static bool operator !=(Christoffel<TCA, TN, TI1, TI2N, TI3N> left, Christoffel<TCA, TN, TI1, TI2N, TI3N> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Christoffel<TCA, TN, TI1, TI2N, TI3N> other && Equals(other);

    public readonly bool Equals(Christoffel<TCA, TN, TI1, TI2N, TI3N> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    public readonly void CopyTo(ref TN[,,] destination) => _array.CopyTo(ref destination);

    /// <summary>Reinterpret a reference to this Christoffel symbol as one with a new index in the first position.</summary>
    /// <typeparam name="TNI1">A new index</typeparam>
    /// <returns>A reference to this Christoffel symbol with a new index in the first position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Christoffel<TCA, TN, TNI1, TI2N, TI3N> WithIndex1<TNI1>()
        where TNI1 : IIndex
        => ref Unsafe.As<Christoffel<TCA, TN, TI1, TI2N, TI3N>, Christoffel<TCA, TN, TNI1, TI2N, TI3N>>(ref this);

    /// <summary>Reinterpret a reference to this Christoffel symbol as one with a new index name in the second position.</summary>
    /// <typeparam name="TNIN">A new index name</typeparam>
    /// <returns>A reference to this Christoffel symbol with a new index name in the second position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Christoffel<TCA, TN, TI1, TNIN, TI3N> WithIndex2Name<TNIN>()
        where TNIN : ISymbol
        => ref Unsafe.As<Christoffel<TCA, TN, TI1, TI2N, TI3N>, Christoffel<TCA, TN, TI1, TNIN, TI3N>>(ref this);

    /// <summary>Reinterpret a reference to this Christoffel symbol as one with a new index name in the third position.</summary>
    /// <typeparam name="TNIN">A new index name</typeparam>
    /// <returns>A reference to this Christoffel symbol with a new index name in the third position</returns>
    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Christoffel<TCA, TN, TI1, TI2N, TNIN> WithIndex3Name<TNIN>()
        where TNIN : ISymbol
        => ref Unsafe.As<Christoffel<TCA, TN, TI1, TI2N, TI3N>, Christoffel<TCA, TN, TI1, TI2N, TNIN>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Christoffel<TCA, TN, TI1, TI2N, TI3N>(TCA value) => new(value);
}
