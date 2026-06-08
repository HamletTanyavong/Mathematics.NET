// <copyright file="Vector2.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector with two components.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="x1">The <c>x_1</c> component.</param>
/// <param name="x2">The <c>x_2</c> component.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Vector2<T, U>(T x1, T x2) : IVector<Vector2<T, U>, T, U, U>
    where T : IComplex<T, U, U>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    public static readonly Vector2<T, U> Zero = new(T.Zero, T.Zero);

    /// <summary>The first element of the vector.</summary>
    public T X1 = x1;

    /// <summary>The second element of the vector.</summary>
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
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(Vector2<T, U> vector, int index)
    {
        if ((uint)index >= 2)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref Vector2<T, U> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 2);
        return Unsafe.Add(ref Unsafe.As<Vector2<T, U>, T>(ref vector), index);
    }

    // Set

    internal static Vector2<T, U> WithElement(Vector2<T, U> vector, int index, T value)
    {
        if ((uint)index >= 2)
            throw new IndexOutOfRangeException();
        Vector2<T, U> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref Vector2<T, U> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 2);
        Unsafe.Add(ref Unsafe.As<Vector2<T, U>, T>(ref vector), index) = value;
    }

    //
    // Constants
    //

    static Vector2<T, U> IVector<Vector2<T, U>, T, U, U>.Zero => Zero;

    //
    // Operators
    //

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator -(Vector2<T, U> vector)
        => new(-vector.X1, -vector.X2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator +(Vector2<T, U> vector)
        => vector;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator +(Vector2<T, U> left, Vector2<T, U> right)
        => new(left.X1 + right.X1, left.X2 + right.X2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator -(Vector2<T, U> left, Vector2<T, U> right)
        => new(left.X1 - right.X1, left.X2 - right.X2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator *(Vector2<T, U> left, Vector2<T, U> right)
        => new(left.X1 * right.X1, left.X2 * right.X2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator *(T c, Vector2<T, U> vector)
        => new(c * vector.X1, c * vector.X2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T, U> operator *(Vector2<T, U> vector, T c)
        => new(vector.X1 * c, vector.X2 * c);

    //
    // Equality
    //

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2<T, U> left, Vector2<T, U> right)
        => left.X1 == right.X1 && left.X2 == right.X2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2<T, U> left, Vector2<T, U> right)
        => left.X1 != right.X1 || left.X2 != right.X2;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2<T, U> other && Equals(other);

    public bool Equals(Vector2<T, U> value)
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

    public static T InnerProduct(Vector2<T, U> left, Vector2<T, U> right)
        => T.Conjugate(left.X1) * right.X1 + T.Conjugate(left.X2) * right.X2;

    public readonly Real<U> Norm()
    {
        var x0 = (X1 * T.Conjugate(X1)).Re;
        var x1 = (X2 * T.Conjugate(X2)).Re;

        Real<U> max = Real<U>.Max(x0, x1);
        var maxSquared = max * max;

        return max * Real<U>.Sqrt(x0 / maxSquared + x1 / maxSquared);
    }

    public readonly Vector2<T, U> Normalize()
    {
        var norm = Norm();
        return new(X1 / (U)norm, X2 / (U)norm);
    }

    public readonly T[] ToArray() => [X1, X2];
}
