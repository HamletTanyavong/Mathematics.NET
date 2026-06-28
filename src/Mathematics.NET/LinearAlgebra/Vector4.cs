// <copyright file="Vector4.cs" company="Mathematics.NET">
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
using System.Runtime.Intrinsics;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector with four components.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="x1">The <c>x_1</c> component.</param>
/// <param name="x2">The <c>x_2</c> component.</param>
/// <param name="x3">The <c>x_3</c> component.</param>
/// <param name="x4">The <c>x_4</c> component.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Vector4<T, U>(T x1, T x2, T x3, T x4) : IVector<Vector4<T, U>, T, U, U>
    where T : IComplex<T, U, U>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    public static readonly Vector4<T, U> Zero = new(T.Zero, T.Zero, T.Zero, T.Zero);

    /// <summary>The first element of the vector.</summary>
    public T X1 = x1;

    /// <summary>The second element of the vector.</summary>
    public T X2 = x2;

    /// <summary>The third element of the vector.</summary>
    public T X3 = x3;

    /// <summary>The fourth element of the vector.</summary>
    public T X4 = x4;

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
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(Vector4<T, U> vector, int index)
    {
        if ((uint)index >= 4)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref Vector4<T, U> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 4);
        return Unsafe.Add(ref Unsafe.As<Vector4<T, U>, T>(ref vector), index);
    }

    // Set

    internal static Vector4<T, U> WithElement(Vector4<T, U> vector, int index, T value)
    {
        if ((uint)index >= 4)
            throw new IndexOutOfRangeException();
        Vector4<T, U> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref Vector4<T, U> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 4);
        Unsafe.Add(ref Unsafe.As<Vector4<T, U>, T>(ref vector), index) = value;
    }

    //
    // Constants
    //

    static Vector4<T, U> IVector<Vector4<T, U>, T, U, U>.Zero => Zero;

    //
    // Operators
    //

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator -(Vector4<T, U> vector)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Negate(vector.AsVector128()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Negate(vector.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<float>) => Vector256.Negate(vector.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<double>) => Vector512.Negate(vector.AsVector512()).AsVector4<T, U>(),
            _ => new(-vector.X1, -vector.X2, -vector.X3, -vector.X4)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator +(Vector4<T, U> vector)
        => vector;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator +(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Add(left.AsVector128(), right.AsVector128()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Add(left.AsVector256(), right.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<float>) => Vector256.Add(left.AsVector256(), right.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<double>) => Vector512.Add(left.AsVector512(), right.AsVector512()).AsVector4<T, U>(),
            _ => new(
                left.X1 + right.X1,
                left.X2 + right.X2,
                left.X3 + right.X3,
                left.X4 + right.X4)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator -(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Subtract(left.AsVector128(), right.AsVector128()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Subtract(left.AsVector256(), right.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<float>) => Vector256.Subtract(left.AsVector256(), right.AsVector256()).AsVector4<T, U>(),
            var t when t == typeof(Complex<double>) => Vector512.Subtract(left.AsVector512(), right.AsVector512()).AsVector4<T, U>(),
            _ => new(
                left.X1 + right.X1,
                left.X2 + right.X2,
                left.X3 + right.X3,
                left.X4 + right.X4)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator *(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Multiply(left.AsVector128(), right.AsVector128()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Multiply(left.AsVector256(), right.AsVector256()).AsVector4<T, U>(),
            _ => new(
                left.X1 * right.X1,
                left.X2 * right.X2,
                left.X3 * right.X3,
                left.X4 * right.X4)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator *(T c, Vector4<T, U> vector)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Multiply(c.AsFloat<T, U, U>(), vector.AsVector128()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Multiply(c.AsFloat<T, U, U>(), vector.AsVector256()).AsVector4<T, U>(),
            _ => new(
                c * vector.X1,
                c * vector.X2,
                c * vector.X3,
                c * vector.X4)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T, U> operator *(Vector4<T, U> vector, T c)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Multiply(vector.AsVector128(), c.AsFloat<T, U, U>()).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Multiply(vector.AsVector256(), c.AsFloat<T, U, U>()).AsVector4<T, U>(),
            _ => new(
                vector.X1 * c,
                vector.X2 * c,
                vector.X3 * c,
                vector.X4 * c)
        };
    }

    //
    // Equality
    //

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.EqualsAll(left.AsVector128(), right.AsVector128()),
            var t when t == typeof(Real<double>) => Vector256.EqualsAll(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<float>) => Vector256.EqualsAll(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<double>) => Vector512.EqualsAll(left.AsVector512(), right.AsVector512()),
            _ =>
                left.X1 == right.X1 &&
                left.X2 == right.X2 &&
                left.X3 == right.X3 &&
                left.X4 == right.X4
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => !Vector128.EqualsAll(left.AsVector128(), right.AsVector128()),
            var t when t == typeof(Real<double>) => !Vector256.EqualsAll(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<float>) => !Vector256.EqualsAll(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<double>) => !Vector512.EqualsAll(left.AsVector512(), right.AsVector512()),
            _ =>
                left.X1 != right.X1 ||
                left.X2 != right.X2 ||
                left.X3 != right.X3 ||
                left.X4 != right.X4
        };
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector4<T, U> other && Equals(other);

    public bool Equals(Vector4<T, U> value)
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

    public static T InnerProduct(Vector4<T, U> left, Vector4<T, U> right)
    {
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Dot(left.AsVector128(), right.AsVector128()),
            var t when t == typeof(Real<double>) => Vector256.Dot(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<float>) => Vector256.Dot(left.AsVector256(), right.AsVector256()),
            var t when t == typeof(Complex<double>) => Vector512.Dot(left.AsVector512(), right.AsVector512()),
            _ => T.Conjugate(left.X1) * right.X1 + T.Conjugate(left.X2) * right.X2 + T.Conjugate(left.X3) * right.X3 + T.Conjugate(left.X4) * right.X4
        };
    }

    public readonly Real<U> Norm()
    {
        switch (typeof(U))
        {
            case Type t when t == typeof(float):
                {
                    var components = Vector128.Create(
                        (X1 * T.Conjugate(X1)).AsFloat<T, U, float>(),
                        (X2 * T.Conjugate(X2)).AsFloat<T, U, float>(),
                        (X3 * T.Conjugate(X3)).AsFloat<T, U, float>(),
                        (X4 * T.Conjugate(X4)).AsFloat<T, U, float>());
                    var vector = Vector64.Max(components.GetLower(), components.GetUpper());
                    var max = MathF.Max(vector[0], vector[1]);
                    return (max * MathF.Sqrt(Vector128.Sum(Vector128.Divide(components, max * max)))).AsBackingType<U>();
                }
            case Type t when t == typeof(double):
                {
                    var components = Vector256.Create(
                        (X1 * T.Conjugate(X1)).AsFloat<T, U, double>(),
                        (X2 * T.Conjugate(X2)).AsFloat<T, U, double>(),
                        (X3 * T.Conjugate(X3)).AsFloat<T, U, double>(),
                        (X4 * T.Conjugate(X4)).AsFloat<T, U, double>());
                    var vector = Vector128.Max(components.GetLower(), components.GetUpper());
                    var max = Math.Max(vector[0], vector[1]);
                    return (max * Math.Sqrt(Vector256.Sum(Vector256.Divide(components, max * max)))).AsBackingType<U>();
                }
            default:
                {
                    var x1 = (X1 * T.Conjugate(X1)).Re;
                    var x2 = (X2 * T.Conjugate(X2)).Re;
                    var x3 = (X3 * T.Conjugate(X3)).Re;
                    var x4 = (X4 * T.Conjugate(X4)).Re;

                    var max = Real<U>.Max(Real<U>.Max(x1, x2), Real<U>.Max(x3, x4));
                    var maxSquared = max * max;
                    return max * Real<U>.Sqrt((x1 + x2 + x3 + x4) / maxSquared);
                }
        }
    }

    public readonly Vector4<T, U> Normalize()
    {
        var norm = (U)Norm();
        return typeof(T) switch
        {
            var t when t == typeof(Real<float>) => Vector128.Divide(this.AsVector128(), norm).AsVector4<T, U>(),
            var t when t == typeof(Real<double>) => Vector256.Divide(this.AsVector256(), norm).AsVector4<T, U>(),
            var t when t == typeof(Complex<float>) => Vector256.Divide(this.AsVector256(), norm).AsVector4<T, U>(),
            var t when t == typeof(Complex<double>) => Vector512.Divide(this.AsVector512(), norm).AsVector4<T, U>(),
            _ => new(X1 / norm, X2 / norm, X3 / norm, X4 / norm)
        };
    }

    public readonly T[] ToArray() => [X1, X2];
}
