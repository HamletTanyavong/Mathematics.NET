// <copyright file="Matrix4x4.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a 4x4 matrix.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix4x4<T> : ISquareMatrix<Matrix4x4<T>, T>
    where T : IComplex<T>
{
    private static readonly Matrix4x4<T> s_identity = CreateDiagonal(T.One, T.One, T.One, T.One);

    public const int Components = 16;
    public const int E1Components = 4;
    public const int E2Components = 4;

    public static readonly Matrix4x4<T> NaM = CreateDiagonal(T.NaN, T.NaN, T.NaN, T.NaN);

    public Vector4<T> X1;
    public Vector4<T> X2;
    public Vector4<T> X3;
    public Vector4<T> X4;

    /// <summary>Create a 4x4 matrix given a set of 16 values.</summary>
    /// <param name="e11">The $ e_{11} $ component.</param>
    /// <param name="e12">The $ e_{12} $ component.</param>
    /// <param name="e13">The $ e_{13} $ component.</param>
    /// <param name="e14">The $ e_{14} $ component.</param>
    /// <param name="e21">The $ e_{21} $ component.</param>
    /// <param name="e22">The $ e_{22} $ component.</param>
    /// <param name="e23">The $ e_{23} $ component.</param>
    /// <param name="e24">The $ e_{24} $ component.</param>
    /// <param name="e31">The $ e_{31} $ component.</param>
    /// <param name="e32">The $ e_{32} $ component.</param>
    /// <param name="e33">The $ e_{33} $ component.</param>
    /// <param name="e34">The $ e_{34} $ component.</param>
    /// <param name="e41">The $ e_{41} $ component.</param>
    /// <param name="e42">The $ e_{42} $ component.</param>
    /// <param name="e43">The $ e_{43} $ component.</param>
    /// <param name="e44">The $ e_{44} $ component.</param>
    public Matrix4x4(
        T e11, T e12, T e13, T e14,
        T e21, T e22, T e23, T e24,
        T e31, T e32, T e33, T e34,
        T e41, T e42, T e43, T e44)
    {
        X1 = new(e11, e12, e13, e14);
        X2 = new(e21, e22, e23, e24);
        X3 = new(e31, e32, e33, e34);
        X4 = new(e41, e42, e43, e44);
    }

    //
    // Constants
    //

    static int IArrayRepresentable<Matrix4x4<T>, T>.Components => Components;
    static int ITwoDimensionalArrayRepresentable<Matrix4x4<T>, T>.E1Components => E1Components;
    static int ITwoDimensionalArrayRepresentable<Matrix4x4<T>, T>.E2Components => E2Components;
    static Matrix4x4<T> ISquareMatrix<Matrix4x4<T>, T>.Identity => s_identity;
    static Matrix4x4<T> IMatrix<Matrix4x4<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if ((uint)row >= 4)
                throw new IndexOutOfRangeException();
            return Unsafe.Add(ref Unsafe.AsRef(in X1), row)[column];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if ((uint)row >= 4)
                throw new IndexOutOfRangeException();
            Unsafe.Add(ref X1, row)[column] = value;
        }
    }

    //
    // Operators
    //

    public static Matrix4x4<T> operator -(Matrix4x4<T> matrix)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = -matrix.X1;
        result.X2 = -matrix.X2;
        result.X3 = -matrix.X3;
        result.X4 = -matrix.X4;

        return result;
    }

    public static Matrix4x4<T> operator +(Matrix4x4<T> matrix)
        => matrix;

    public static Matrix4x4<T> operator +(Matrix4x4<T> left, Matrix4x4<T> right)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = left.X1 + right.X1;
        result.X2 = left.X2 + right.X2;
        result.X3 = left.X3 + right.X3;
        result.X4 = left.X4 + right.X4;

        return result;
    }

    public static Matrix4x4<T> operator -(Matrix4x4<T> left, Matrix4x4<T> right)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = left.X1 - right.X1;
        result.X2 = left.X2 - right.X2;
        result.X3 = left.X3 - right.X3;
        result.X4 = left.X4 - right.X4;

        return result;
    }

    public static Matrix4x4<T> operator *(Matrix4x4<T> left, Matrix4x4<T> right)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = right.X1 * left.X1.X1;
        result.X1 += right.X2 * left.X1.X2;
        result.X1 += right.X3 * left.X1.X3;
        result.X1 += right.X4 * left.X1.X4;

        result.X2 = right.X1 * left.X2.X1;
        result.X2 += right.X2 * left.X2.X2;
        result.X2 += right.X3 * left.X2.X3;
        result.X2 += right.X4 * left.X2.X4;

        result.X3 = right.X1 * left.X3.X1;
        result.X3 += right.X2 * left.X3.X2;
        result.X3 += right.X3 * left.X3.X3;
        result.X3 += right.X4 * left.X3.X4;

        result.X4 = right.X1 * left.X4.X1;
        result.X4 += right.X2 * left.X4.X2;
        result.X4 += right.X3 * left.X4.X3;
        result.X4 += right.X4 * left.X4.X4;

        return result;
    }

    public static Matrix4x4<T> operator *(T c, Matrix4x4<T> matrix)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = c * matrix.X1;
        result.X2 = c * matrix.X2;
        result.X3 = c * matrix.X3;
        result.X4 = c * matrix.X4;

        return result;
    }

    public static Matrix4x4<T> operator *(Matrix4x4<T> matrix, T c)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = matrix.X1 * c;
        result.X2 = matrix.X2 * c;
        result.X3 = matrix.X3 * c;
        result.X4 = matrix.X4 * c;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix4x4<T> left, Matrix4x4<T> right)
        => left.X1 == right.X1 && left.X2 == right.X2 && left.X3 == right.X3 && left.X4 == right.X4;

    public static bool operator !=(Matrix4x4<T> left, Matrix4x4<T> right)
        => left.X1 != right.X1 || left.X2 != right.X2 || left.X3 != right.X3 || left.X4 != right.X4;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix4x4<T> other && Equals(other);

    public bool Equals(Matrix4x4<T> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2) && X3.Equals(value.X3) && X4.Equals(value.X4);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2, X3, X4);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
        => this.AsSpan2D().ToDisplayString(format, provider);

    //
    // Methods
    //

    /// <summary>Create a diagonal matrix from specified values along the diagonal.</summary>
    /// <param name="e11">The $ e_{11} $ component.</param>
    /// <param name="e22">The $ e_{22} $ component.</param>
    /// <param name="e33">The $ e_{33} $ component.</param>
    /// <param name="e44">The $ e_{44} $ component.</param>
    /// <returns>A diagonal matrix.</returns>
    public static Matrix4x4<T> CreateDiagonal(T e11, T e22, T e33, T e44)
    {
        return new(
            e11, T.Zero, T.Zero, T.Zero,
            T.Zero, e22, T.Zero, T.Zero,
            T.Zero, T.Zero, e33, T.Zero,
            T.Zero, T.Zero, T.Zero, e44);
    }

    public readonly T Determinant()
    {
        T a = X1.X1, b = X1.X2, c = X1.X3, d = X1.X4;
        T e = X2.X1, f = X2.X2, g = X2.X3, h = X2.X4;
        T i = X3.X1, j = X3.X2, k = X3.X3, l = X3.X4;
        T m = X4.X1, n = X4.X2, o = X4.X3, p = X4.X4;

        T kp_lo = k * p - l * o;
        T jp_ln = j * p - l * n;
        T jo_kn = j * o - k * n;
        T ip_lm = i * p - l * m;
        T io_km = i * o - k * m;
        T in_jm = i * n - j * m;

        return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
               b * (e * kp_lo - g * ip_lm + h * io_km) +
               c * (e * jp_ln - f * ip_lm + h * in_jm) -
               d * (e * jo_kn - f * io_km + g * in_jm);
    }

    public readonly Matrix4x4<T> Inverse()
    {
        T a = X1.X1, b = X1.X2, c = X1.X3, d = X1.X4;
        T e = X2.X1, f = X2.X2, g = X2.X3, h = X2.X4;
        T i = X3.X1, j = X3.X2, k = X3.X3, l = X3.X4;
        T m = X4.X1, n = X4.X2, o = X4.X3, p = X4.X4;

        T kp_lo = k * p - l * o;
        T jp_ln = j * p - l * n;
        T jo_kn = j * o - k * n;
        T ip_lm = i * p - l * m;
        T io_km = i * o - k * m;
        T in_jm = i * n - j * m;

        T a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
        T a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        T a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
        T a14 = -(e * jo_kn - f * io_km + g * in_jm);

        T det = a * a11 + b * a12 + c * a13 + d * a14;

        Matrix4x4<T> result;
        if (T.Abs(det) < Precision.DblMinPositive)
            return NaM;
        T invDet = T.One / det;

        result.X1.X1 = a11 * invDet;
        result.X2.X1 = a12 * invDet;
        result.X3.X1 = a13 * invDet;
        result.X4.X1 = a14 * invDet;

        result.X1.X2 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
        result.X2.X2 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
        result.X3.X2 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
        result.X4.X2 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

        T gp_ho = g * p - h * o;
        T fp_hn = f * p - h * n;
        T fo_gn = f * o - g * n;
        T ep_hm = e * p - h * m;
        T eo_gm = e * o - g * m;
        T en_fm = e * n - f * m;

        result.X1.X3 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
        result.X2.X3 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
        result.X3.X3 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
        result.X4.X3 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

        T gl_hk = g * l - h * k;
        T fl_hj = f * l - h * j;
        T fk_gj = f * k - g * j;
        T el_hi = e * l - h * i;
        T ek_gi = e * k - g * i;
        T ej_fi = e * j - f * i;

        result.X1.X4 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
        result.X2.X4 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
        result.X3.X4 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
        result.X4.X4 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

        return result;
    }

    public static bool IsNaM(Matrix4x4<T> matrix)
        => T.IsNaN(matrix.X1.X1) && T.IsNaN(matrix.X2.X2) && T.IsNaN(matrix.X3.X3) && T.IsNaN(matrix.X4.X4);

    public readonly T Trace() => X1.X1 + X2.X2 + X3.X3 + X3.X4;

    public readonly Matrix4x4<T> Transpose()
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        if (typeof(T) == typeof(Real))
        {
            if (Avx.IsSupported)
            {
                var x1 = X1.AsVector256();
                var x2 = X2.AsVector256();
                var x3 = X3.AsVector256();
                var x4 = X4.AsVector256();

                var l12 = Avx.UnpackLow(x1, x2);
                var l34 = Avx.UnpackLow(x3, x4);
                var u12 = Avx.UnpackHigh(x1, x2);
                var u34 = Avx.UnpackHigh(x3, x4);

                result.X1 = l12.WithUpper(l34.GetLower()).AsVector4<T>();
                result.X2 = u12.WithUpper(u34.GetLower()).AsVector4<T>();
                result.X3 = l34.WithLower(l12.GetUpper()).AsVector4<T>();
                result.X4 = u34.WithLower(u12.GetUpper()).AsVector4<T>();

                return result;
            }
        }

        result.X1 = new(X1.X1, X2.X1, X3.X1, X4.X1);
        result.X2 = new(X1.X2, X2.X2, X3.X2, X4.X2);
        result.X3 = new(X1.X3, X2.X3, X3.X3, X4.X3);
        result.X4 = new(X1.X4, X2.X4, X3.X4, X4.X4);

        return result;
    }
}
