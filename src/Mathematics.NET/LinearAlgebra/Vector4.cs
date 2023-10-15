// <copyright file="Vector4.cs" company="Mathematics.NET">
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
using Mathematics.NET.Core;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector with four components</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Vector4<T> : IOneDimensionalArrayRepresentable<Vector4<T>, T>
    where T : IComplex<T>
{
    /// <summary>The first element of the vector</summary>
    public T X1;

    /// <summary>The second element of the vector</summary>
    public T X2;

    /// <summary>The third element of the vector</summary>
    public T X3;

    /// <summary>The fourth element of the vector</summary>
    public T X4;

    public Vector4(T x1, T x2, T x3, T x4)
    {
        X1 = x1;
        X2 = x2;
        X3 = x3;
        X4 = x4;
    }

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => 4;

    public static int E1Components => 4;

    //
    // Indexer
    //

    public T this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(Vector4<T> vector, int index)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref Vector4<T> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 4);
        return Unsafe.Add(ref Unsafe.As<Vector4<T>, T>(ref vector), index);
    }

    // Set

    internal static Vector4<T> WithElement(Vector4<T> vector, int index, T value)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        Vector4<T> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref Vector4<T> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 4);
        Unsafe.Add(ref Unsafe.As<Vector4<T>, T>(ref vector), index) = value;
    }

    //
    // Operators
    //

    public static Vector4<T> operator +(Vector4<T> left, Vector4<T> right)
    {
        return new(
            left.X1 + right.X1,
            left.X2 + right.X2,
            left.X3 + right.X3,
            left.X4 + right.X4);
    }

    public static Vector4<T> operator -(Vector4<T> left, Vector4<T> right)
    {
        return new(
            left.X1 - right.X1,
            left.X2 - right.X2,
            left.X3 - right.X3,
            left.X4 - right.X4);
    }

    public static Vector4<T> operator *(Vector4<T> left, Vector4<T> right)
    {
        return new(
            left.X1 * right.X1,
            left.X2 * right.X2,
            left.X3 * right.X3,
            left.X4 * right.X4);
    }

    //
    // Equality
    //

    public static bool operator ==(Vector4<T> left, Vector4<T> right)
        => left.X1 == right.X1
        && left.X2 == right.X2
        && left.X3 == right.X3
        && left.X4 == right.X4;

    public static bool operator !=(Vector4<T> left, Vector4<T> right)
        => left.X1 != right.X1
        || left.X2 != right.X2
        || left.X3 != right.X3
        || left.X4 != right.X4;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector4<T> other && Equals(other);

    public bool Equals(Vector4<T> value)
        => X1.Equals(value.X1)
        && X2.Equals(value.X2)
        && X3.Equals(value.X3)
        && X4.Equals(value.X4);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2, X3, X4);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
        => string.Format(provider, "({0}, {1}, {2}, {3})",
            X1.ToString(format, provider),
            X2.ToString(format, provider),
            X3.ToString(format, provider),
            X4.ToString(format, provider));

    //
    // Methods
    //

    public static T InnerProduct(Vector4<T> left, Vector4<T> right)
        => T.Conjugate(left.X1) * right.X1 + T.Conjugate(left.X2) * right.X2 + T.Conjugate(left.X3) * right.X3 + T.Conjugate(left.X4) * right.X4;

    public Real Norm()
    {
        Span<Real> components = stackalloc Real[4];

        components[0] = (X1 * T.Conjugate(X1)).Re;
        components[1] = (X2 * T.Conjugate(X2)).Re;
        components[2] = (X3 * T.Conjugate(X3)).Re;
        components[3] = (X4 * T.Conjugate(X4)).Re;

        Real max = components[0];
        for (int i = 1; i < 4; i++)
        {
            if (components[i] > max)
            {
                max = components[i];
            }
        }

        var partialSum = Real.Zero;
        var maxSquared = max * max;
        partialSum += components[0] / maxSquared;
        partialSum += components[1] / maxSquared;
        partialSum += components[2] / maxSquared;
        partialSum += components[3] / maxSquared;

        return max * Real.Sqrt(partialSum);
    }

    public Vector4<T> Normalize()
    {
        var norm = Norm().Value;
        return new(X1 / norm, X2 / norm, X3 / norm, X4 / norm);
    }
}
