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
/// <typeparam name="TCubicArray">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">The first index</typeparam>
/// <typeparam name="TIndex2Name">The name of the second index</typeparam>
/// <typeparam name="TIndex3Name">The name of the third index</typeparam>
/// <param name="array">A backing array</param>
public struct Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>(TCubicArray array)
    : IRankThreeTensor<Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>, TCubicArray, TNumber, TIndex1, Index<Lower, TIndex2Name>, Index<Lower, TIndex3Name>>
    where TCubicArray : ICubicArray<TCubicArray, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2Name : ISymbol
    where TIndex3Name : ISymbol
{
    private TCubicArray _array = array;

    //
    // IRankThreeTensor interface
    //

    public readonly IIndex I1 => TIndex1.Instance;

    public readonly IIndex I2 => Index<Lower, TIndex2Name>.Instance;

    public readonly IIndex I3 => Index<Lower, TIndex3Name>.Instance;

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

    public static Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> operator -(Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> christoffel)
        => new(-christoffel._array);

    public static Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> operator +(Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> christoffel)
        => christoffel;

    //
    // Equality
    //

    public static bool operator ==(Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> left, Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> right)
        => left._array == right._array;

    public static bool operator !=(Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> left, Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> right)
        => left._array != right._array;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> other && Equals(other);

    public readonly bool Equals(Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name> value) => _array.Equals(value._array);

    public override readonly int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a Christoffel symbol with a new index in the first position.</summary>
    /// <typeparam name="TNewIndex">An index</typeparam>
    /// <returns>A Christoffel symbol with a new index in the first position</returns>
    public Christoffel<TCubicArray, TNumber, TNewIndex, TIndex2Name, TIndex3Name> WithIndexOne<TNewIndex>()
        where TNewIndex : IIndex
        => Unsafe.As<Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>, Christoffel<TCubicArray, TNumber, TNewIndex, TIndex2Name, TIndex3Name>>(ref this);

    /// <summary>Create a Christoffel symbol with a new index in the second position.</summary>
    /// <typeparam name="TNewIndexName">A symbol</typeparam>
    /// <returns>A Christoffel symbol with a new index in the second position</returns>
    public Christoffel<TCubicArray, TNumber, TIndex1, TNewIndexName, TIndex3Name> WithIndexTwo<TNewIndexName>()
        where TNewIndexName : ISymbol
        => Unsafe.As<Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>, Christoffel<TCubicArray, TNumber, TIndex1, TNewIndexName, TIndex3Name>>(ref this);

    /// <summary>Create a Christoffel symbol with a new index in the third position.</summary>
    /// <typeparam name="TNewIndexName">A symbol</typeparam>
    /// <returns>A Christoffel symbol with a new index in the third position</returns>
    public Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TNewIndexName> WithIndexThree<TNewIndexName>()
        where TNewIndexName : ISymbol
        => Unsafe.As<Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>, Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TNewIndexName>>(ref this);

    //
    // Implicit operators
    //

    public static implicit operator Christoffel<TCubicArray, TNumber, TIndex1, TIndex2Name, TIndex3Name>(TCubicArray value) => new(value);
}
