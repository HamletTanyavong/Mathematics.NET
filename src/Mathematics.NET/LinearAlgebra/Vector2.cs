// <copyright file="Vector2.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector with two components</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <param name="x1">The $ x_1 $ component</param>
/// <param name="x2">The $ x_2 $ component</param>
[StructLayout(LayoutKind.Sequential)]
public struct Vector2<T>(T x1, T x2) : IVector<Vector2<T>, T>
    where T : IComplex<T>
{
    /// <summary>The first element of the vector</summary>
    public T X1 = x1;

    /// <summary>The second element of the vector</summary>
    public T X2 = x2;

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => 2;

    public static int E1Components => 2;

    //
    // Indexer
    //

    public T this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(Vector2<T> vector, int index)
    {
        if ((uint)index >= 2)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref Vector2<T> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 2);
        return Unsafe.Add(ref Unsafe.As<Vector2<T>, T>(ref vector), index);
    }

    // Set

    internal static Vector2<T> WithElement(Vector2<T> vector, int index, T value)
    {
        if ((uint)index >= 2)
        {
            throw new IndexOutOfRangeException();
        }

        Vector2<T> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref Vector2<T> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 2);
        Unsafe.Add(ref Unsafe.As<Vector2<T>, T>(ref vector), index) = value;
    }

    //
    // Operators
    //

    public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right)
        => new(left.X1 + right.X1, left.X2 + right.X2);

    public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
        => new(left.X1 - right.X1, left.X2 - right.X2);

    //
    // Equality
    //

    public static bool operator ==(Vector2<T> left, Vector2<T> right)
        => left.X1 == right.X1 && left.X2 == right.X2;

    public static bool operator !=(Vector2<T> left, Vector2<T> right)
        => left.X1 != right.X1 || left.X2 != right.X2;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2<T> other && Equals(other);

    public bool Equals(Vector2<T> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
        => string.Format(provider, "({0}, {1})", X1.ToString(format, provider), X2.ToString(format, provider));

    //
    // Methods
    //

    public static T InnerProduct(Vector2<T> left, Vector2<T> right)
        => T.Conjugate(left.X1) * right.X1 + T.Conjugate(left.X2) * right.X2;

    public readonly Real Norm()
    {
        var x0 = (X1 * T.Conjugate(X1)).Re;
        var x1 = (X2 * T.Conjugate(X2)).Re;

        Real max = Real.Max(x0, x1);
        var maxSquared = max * max;

        return max * Real.Sqrt(x0 / maxSquared + x1 / maxSquared);
    }

    public readonly Vector2<T> Normalize()
    {
        var norm = Norm();
        return new(X1 / norm, X2 / norm);
    }
}
