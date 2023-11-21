// <copyright file="Matrix4x4.cs" company="Mathematics.NET">
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
using System.Text;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a 4x4 matrix</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <param name="e11">The $ e_{11} $ component</param>
/// <param name="e12">The $ e_{12} $ component</param>
/// <param name="e13">The $ e_{13} $ component</param>
/// <param name="e14">The $ e_{14} $ component</param>
/// <param name="e21">The $ e_{21} $ component</param>
/// <param name="e22">The $ e_{22} $ component</param>
/// <param name="e23">The $ e_{23} $ component</param>
/// <param name="e24">The $ e_{24} $ component</param>
/// <param name="e31">The $ e_{31} $ component</param>
/// <param name="e32">The $ e_{32} $ component</param>
/// <param name="e33">The $ e_{33} $ component</param>
/// <param name="e34">The $ e_{34} $ component</param>
/// <param name="e41">The $ e_{41} $ component</param>
/// <param name="e42">The $ e_{42} $ component</param>
/// <param name="e43">The $ e_{43} $ component</param>
/// <param name="e44">The $ e_{44} $ component</param>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix4x4<T>(
    T e11, T e12, T e13, T e14,
    T e21, T e22, T e23, T e24,
    T e31, T e32, T e33, T e34,
    T e41, T e42, T e43, T e44) : ISquareMatrix<Matrix4x4<T>, T>
    where T : IComplex<T>
{
    private static readonly Matrix4x4<T> s_identity = CreateDiagonal(T.One, T.One, T.One, T.One);

    public const int Components = 16;
    public const int E1Components = 4;
    public const int E2Components = 4;

    public static readonly Matrix4x4<T> NaM = CreateDiagonal(T.NaN, T.NaN, T.NaN, T.NaN);

    public T E11 = e11;
    public T E12 = e12;
    public T E13 = e13;
    public T E14 = e14;

    public T E21 = e21;
    public T E22 = e22;
    public T E23 = e23;
    public T E24 = e24;

    public T E31 = e31;
    public T E32 = e32;
    public T E33 = e33;
    public T E34 = e34;

    public T E41 = e41;
    public T E42 = e42;
    public T E43 = e43;
    public T E44 = e44;

    //
    // Constants
    //

    static int IArrayRepresentable<T>.Components => Components;
    static int ITwoDimensionalArrayRepresentable<Matrix4x4<T>, T>.E1Components => E1Components;
    static int ITwoDimensionalArrayRepresentable<Matrix4x4<T>, T>.E2Components => E2Components;
    static Matrix4x4<T> ISquareMatrix<Matrix4x4<T>, T>.Identity => s_identity;
    static Matrix4x4<T> IMatrix<Matrix4x4<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        get
        {
            if ((uint)row >= 4)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector4<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector4<T>>(ref E11), row);
            return vrow[column];
        }
        set
        {
            if ((uint)row >= 4)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector4<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector4<T>>(ref E11), row);
            var temp = Vector4<T>.WithElement(vrow, column, value);
            vrow = temp;
        }
    }

    //
    // Operators
    //

    public static Matrix4x4<T> operator +(Matrix4x4<T> a, Matrix4x4<T> b)
    {
        return new(
            a.E11 + b.E11, a.E12 + b.E12, a.E13 + b.E13, a.E14 + b.E14,
            a.E21 + b.E21, a.E22 + b.E22, a.E23 + b.E23, a.E24 + b.E24,
            a.E31 + b.E31, a.E32 + b.E32, a.E33 + b.E33, a.E34 + b.E34,
            a.E41 + b.E41, a.E42 + b.E42, a.E43 + b.E43, a.E44 + b.E44);
    }

    public static Matrix4x4<T> operator -(Matrix4x4<T> a, Matrix4x4<T> b)
    {
        return new(
            a.E11 - b.E11, a.E12 - b.E12, a.E13 - b.E13, a.E14 - b.E14,
            a.E21 - b.E21, a.E22 - b.E22, a.E23 - b.E23, a.E24 - b.E24,
            a.E31 - b.E31, a.E32 - b.E32, a.E33 - b.E33, a.E34 - b.E34,
            a.E41 - b.E41, a.E42 - b.E42, a.E43 - b.E43, a.E44 - b.E44);
    }

    public static Matrix4x4<T> operator *(Matrix4x4<T> a, Matrix4x4<T> b)
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.E11 = a.E11 * b.E11 + a.E12 * b.E21 + a.E13 * b.E31 + a.E14 * b.E41;
        result.E12 = a.E11 * b.E12 + a.E12 * b.E22 + a.E13 * b.E32 + a.E14 * b.E42;
        result.E13 = a.E11 * b.E13 + a.E12 * b.E23 + a.E13 * b.E33 + a.E14 * b.E43;
        result.E14 = a.E11 * b.E14 + a.E12 * b.E24 + a.E13 * b.E34 + a.E14 * b.E44;

        result.E21 = a.E21 * b.E11 + a.E22 * b.E21 + a.E23 * b.E31 + a.E24 * b.E41;
        result.E22 = a.E21 * b.E12 + a.E22 * b.E22 + a.E23 * b.E32 + a.E24 * b.E42;
        result.E23 = a.E21 * b.E13 + a.E22 * b.E23 + a.E23 * b.E33 + a.E24 * b.E43;
        result.E24 = a.E21 * b.E14 + a.E22 * b.E24 + a.E23 * b.E34 + a.E24 * b.E44;

        result.E31 = a.E31 * b.E11 + a.E32 * b.E21 + a.E33 * b.E31 + a.E34 * b.E41;
        result.E32 = a.E31 * b.E12 + a.E32 * b.E22 + a.E33 * b.E32 + a.E34 * b.E42;
        result.E33 = a.E31 * b.E13 + a.E32 * b.E23 + a.E33 * b.E33 + a.E34 * b.E43;
        result.E34 = a.E31 * b.E14 + a.E32 * b.E24 + a.E33 * b.E34 + a.E34 * b.E44;

        result.E41 = a.E41 * b.E11 + a.E42 * b.E21 + a.E43 * b.E31 + a.E44 * b.E41;
        result.E42 = a.E41 * b.E12 + a.E42 * b.E22 + a.E43 * b.E32 + a.E44 * b.E42;
        result.E43 = a.E41 * b.E13 + a.E42 * b.E23 + a.E43 * b.E33 + a.E44 * b.E43;
        result.E44 = a.E41 * b.E14 + a.E42 * b.E24 + a.E43 * b.E34 + a.E44 * b.E44;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix4x4<T> a, Matrix4x4<T> b)
    {
        return a.E11 == b.E11 && a.E22 == b.E22 && a.E33 == b.E33 && a.E44 == b.E44 // Check diagonal first
            && a.E12 == b.E12 && a.E13 == b.E13 && a.E14 == b.E14
            && a.E21 == b.E21 && a.E23 == b.E23 && a.E24 == b.E24
            && a.E31 == b.E31 && a.E32 == b.E32 && a.E34 == b.E34
            && a.E41 == b.E41 && a.E42 == b.E42 && a.E43 == b.E43;
    }

    public static bool operator !=(Matrix4x4<T> a, Matrix4x4<T> b)
    {
        return a.E11 != b.E11 || a.E22 != b.E22 || a.E33 != b.E33 || a.E44 == b.E44 // Check diagonal first
            || a.E12 != b.E12 || a.E13 != b.E13 || a.E14 != b.E14
            || a.E21 != b.E21 || a.E23 != b.E23 || a.E24 != b.E24
            || a.E31 != b.E31 || a.E32 != b.E32 || a.E34 != b.E34
            || a.E41 != b.E41 || a.E42 != b.E42 || a.E43 != b.E43;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix4x4<T> other && Equals(other);

    public bool Equals(Matrix4x4<T> value)
    {
        return E11.Equals(value.E11) && E22.Equals(value.E22) && E33.Equals(value.E33) && E44.Equals(value.E44) // Check diagonal first
            && E12.Equals(value.E12) && E13.Equals(value.E13) && E14.Equals(value.E14)
            && E21.Equals(value.E21) && E23.Equals(value.E23) && E24.Equals(value.E24)
            && E31.Equals(value.E31) && E32.Equals(value.E32) && E34.Equals(value.E34)
            && E41.Equals(value.E41) && E42.Equals(value.E42) && E43.Equals(value.E43);
    }

    public override readonly int GetHashCode()
    {
        HashCode hash = default;

        hash.Add(E11);
        hash.Add(E12);
        hash.Add(E13);
        hash.Add(E14);

        hash.Add(E21);
        hash.Add(E22);
        hash.Add(E23);
        hash.Add(E24);

        hash.Add(E31);
        hash.Add(E32);
        hash.Add(E33);
        hash.Add(E34);

        hash.Add(E41);
        hash.Add(E42);
        hash.Add(E43);
        hash.Add(E44);

        return hash.ToHashCode();
    }

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
    {
        Span2D<string> strings = new string[4, 4];
        var maxElementLength = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var s = this[i, j].ToString(format, provider);
                strings[i, j] = s;
                var length = s.Length + 2;
                if (maxElementLength < length)
                {
                    maxElementLength = length;
                }
            }
        }

        StringBuilder builder = new();
        var newlineChars = Environment.NewLine.ToCharArray();
        builder.Append('[');
        for (int i = 0; i < 4; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < 4; j++)
            {
                string value = j != 3 ? $"{strings[i, j]}, " : strings[i, j];
                builder.Append(value.PadRight(maxElementLength));
            }
            LinAlgExtensions.CloseGroup(builder, newlineChars);
        }
        LinAlgExtensions.CloseGroup(builder, newlineChars, true);
        return string.Format(provider, builder.ToString());
    }

    //
    // Methods
    //

    /// <summary>Create a diagonal matrix from specified values along the diagonal.</summary>
    /// <param name="e11">The $ e_{11} $ component</param>
    /// <param name="e22">The $ e_{22} $ component</param>
    /// <param name="e33">The $ e_{33} $ component</param>
    /// <param name="e44">The $ e_{44} $ component</param>
    /// <returns>A diagonal matrix</returns>
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
        T a = E11, b = E12, c = E13, d = E14;
        T e = E21, f = E22, g = E23, h = E24;
        T i = E31, j = E32, k = E33, l = E34;
        T m = E41, n = E42, o = E43, p = E44;

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

    // TODO: Optimize
    public readonly Matrix4x4<T> Inverse()
    {
        T a = E11, b = E12, c = E13, d = E14;
        T e = E21, f = E22, g = E23, h = E24;
        T i = E31, j = E32, k = E33, l = E34;
        T m = E41, n = E42, o = E43, p = E44;

        T kp_lo = k * p - l * o;
        T jp_ln = j * p - l * n;
        T jo_kn = j * o - k * n;
        T ip_lm = i * p - l * m;
        T io_km = i * o - k * m;
        T in_jm = i * n - j * m;

        T a11 = f * kp_lo - g * jp_ln + h * jo_kn;
        T a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        T a13 = e * jp_ln - f * ip_lm + h * in_jm;
        T a14 = -(e * jo_kn - f * io_km + g * in_jm);

        T det = a * a11 + b * a12 + c * a13 + d * a14;
        if (det == T.Zero)
        {
            return NaM;
        }
        T invDet = T.One / det;

        Matrix4x4<T> result;

        result.E11 = a11 * invDet;
        result.E21 = a12 * invDet;
        result.E31 = a13 * invDet;
        result.E41 = a14 * invDet;

        result.E12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
        result.E22 = (a * kp_lo - c * ip_lm + d * io_km) * invDet;
        result.E32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
        result.E42 = (a * jo_kn - b * io_km + c * in_jm) * invDet;

        T gp_ho = g * p - h * o;
        T fp_hn = f * p - h * n;
        T fo_gn = f * o - g * n;
        T ep_hm = e * p - h * m;
        T eo_gm = e * o - g * m;
        T en_fm = e * n - f * m;

        result.E13 = (b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
        result.E23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
        result.E33 = (a * fp_hn - b * ep_hm + d * en_fm) * invDet;
        result.E43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

        T gl_hk = g * l - h * k;
        T fl_hj = f * l - h * j;
        T fk_gj = f * k - g * j;
        T el_hi = e * l - h * i;
        T ek_gi = e * k - g * i;
        T ej_fi = e * j - f * i;

        result.E14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
        result.E24 = (a * gl_hk - c * el_hi + d * ek_gi) * invDet;
        result.E34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
        result.E44 = (a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

        return result;
    }

    public static bool IsNaM(Matrix4x4<T> matrix)
        => T.IsNaN(matrix.E11) && T.IsNaN(matrix.E22) && T.IsNaN(matrix.E33) && T.IsNaN(matrix.E44);

    public readonly T Trace() => E11 + E22 + E33 + E44;

    public readonly Matrix4x4<T> Transpose()
    {
        return new(
            E11, E21, E31, E41,
            E12, E22, E32, E42,
            E13, E23, E33, E43,
            E14, E24, E34, E44);
    }
}
