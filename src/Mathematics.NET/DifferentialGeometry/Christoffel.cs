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
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a Christoffel symbol</summary>
/// <typeparam name="T">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="V">The first index</typeparam>
/// <typeparam name="W">The name of the second index</typeparam>
/// <typeparam name="X">The name of the third index</typeparam>
/// <param name="array">A backing array</param>
public struct Christoffel<T, U, V, W, X>(T array) : IRankThreeTensor<Christoffel<T, U, V, W, X>, T, U, V, Index<Lower, W>, Index<Lower, X>>
    where T : ICubicArray<T, U>
    where U : IComplex<U>
    where V : IIndex
    where W : ISymbol
    where X : ISymbol
{
    private T _array = array;

    //
    // IRankThreeTensor interface
    //

    public readonly IIndex I1 => V.Instance;

    public readonly IIndex I2 => Index<Lower, W>.Instance;

    public readonly IIndex I3 => Index<Lower, X>.Instance;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => T.Components;

    public static int E1Components => T.E1Components;

    public static int E2Components => T.E2Components;

    public static int E3Components => T.E3Components;

    //
    // Indexer
    //

    public U this[int i, int j, int k]
    {
        get => _array[i, j, k];
        set => _array[i, j, k] = value;
    }

    //
    // Equality
    //

    public static bool operator ==(Christoffel<T, U, V, W, X> left, Christoffel<T, U, V, W, X> right)
        => left._array == right._array;

    public static bool operator !=(Christoffel<T, U, V, W, X> left, Christoffel<T, U, V, W, X> right)
        => left._array != right._array;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Christoffel<T, U, V, W, X> other && Equals(other);

    public bool Equals(Christoffel<T, U, V, W, X> value) => _array.Equals(value._array);

    public override int GetHashCode() => HashCode.Combine(_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => _array.ToString(format, provider);

    //
    // Implicit operators
    //

    public static implicit operator Christoffel<T, U, V, W, X>(T value) => new(value);
}
