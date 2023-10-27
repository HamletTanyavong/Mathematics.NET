// <copyright file="Matrix3x3.cs" company="Mathematics.NET">
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

/// <summary>Represents a 3x3 matrix</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix3x3<T> : ISquareMatrix<Matrix3x3<T>, T>
    where T : IComplex<T>
{
    private static readonly Matrix3x3<T> s_identity = CreateDiagonal(T.One, T.One, T.One);

    public const int Components = 9;
    public const int E1Components = 3;
    public const int E2Components = 3;

    public static readonly Matrix3x3<T> NaM = CreateDiagonal(T.NaN, T.NaN, T.NaN);

    public T E11;
    public T E12;
    public T E13;

    public T E21;
    public T E22;
    public T E23;

    public T E31;
    public T E32;
    public T E33;

    public Matrix3x3(
        T e11, T e12, T e13,
        T e21, T e22, T e23,
        T e31, T e32, T e33)
    {
        E11 = e11;
        E12 = e12;
        E13 = e13;

        E21 = e21;
        E22 = e22;
        E23 = e23;

        E31 = e31;
        E32 = e32;
        E33 = e33;
    }

    //
    // Constants
    //

    static int IArrayRepresentable<T>.Components => Components;
    static int ITwoDimensionalArrayRepresentable<Matrix3x3<T>, T>.E1Components => E1Components;
    static int ITwoDimensionalArrayRepresentable<Matrix3x3<T>, T>.E2Components => E2Components;
    static Matrix3x3<T> ISquareMatrix<Matrix3x3<T>, T>.Identity => s_identity;
    static Matrix3x3<T> IMatrix<Matrix3x3<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        get
        {
            if ((uint)row >= 3)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector3<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector3<T>>(ref E11), row);
            return vrow[column];
        }
        set
        {
            if ((uint)row >= 3)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector3<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector3<T>>(ref E11), row);
            var temp = Vector3<T>.WithElement(vrow, column, value);
            vrow = temp;
        }
    }

    //
    // Operators
    //

    public static Matrix3x3<T> operator +(Matrix3x3<T> a, Matrix3x3<T> b)
    {
        return new(
            a.E11 + b.E11, a.E12 + b.E12, a.E13 + b.E13,
            a.E21 + b.E21, a.E22 + b.E22, a.E23 + b.E23,
            a.E31 + b.E31, a.E32 + b.E32, a.E33 + b.E33);
    }

    public static Matrix3x3<T> operator -(Matrix3x3<T> a, Matrix3x3<T> b)
    {
        return new(
            a.E11 - b.E11, a.E12 - b.E12, a.E13 - b.E13,
            a.E21 - b.E21, a.E22 - b.E22, a.E23 - b.E23,
            a.E31 - b.E31, a.E32 - b.E32, a.E33 - b.E33);
    }

    public static Matrix3x3<T> operator *(Matrix3x3<T> a, Matrix3x3<T> b)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.E11 = a.E11 * b.E11 + a.E12 * b.E21 + a.E13 * b.E31;
        result.E12 = a.E11 * b.E12 + a.E12 * b.E22 + a.E13 * b.E32;
        result.E13 = a.E11 * b.E13 + a.E12 * b.E23 + a.E13 * b.E33;

        result.E21 = a.E21 * b.E11 + a.E22 * b.E21 + a.E23 * b.E31;
        result.E22 = a.E21 * b.E12 + a.E22 * b.E22 + a.E23 * b.E32;
        result.E23 = a.E21 * b.E13 + a.E22 * b.E23 + a.E23 * b.E33;

        result.E31 = a.E31 * b.E11 + a.E32 * b.E21 + a.E33 * b.E31;
        result.E32 = a.E31 * b.E12 + a.E32 * b.E22 + a.E33 * b.E32;
        result.E33 = a.E31 * b.E13 + a.E32 * b.E23 + a.E33 * b.E33;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix3x3<T> a, Matrix3x3<T> b)
    {
        return a.E11 == b.E11 && a.E22 == b.E22 && a.E33 == b.E33 // Check diagonal first
            && a.E12 == b.E12 && a.E13 == b.E13
            && a.E21 == b.E21 && a.E23 == b.E23
            && a.E31 == b.E31 && a.E32 == b.E32;
    }

    public static bool operator !=(Matrix3x3<T> a, Matrix3x3<T> b)
    {
        return a.E11 != b.E11 || a.E22 != b.E22 || a.E33 != b.E33 // Check diagonal first
            || a.E12 != b.E12 || a.E13 != b.E13
            || a.E21 != b.E21 || a.E23 != b.E23
            || a.E31 != b.E31 || a.E32 != b.E32;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix3x3<T> other && Equals(other);

    public bool Equals(Matrix3x3<T> value)
    {
        return E11.Equals(value.E11) && E22.Equals(value.E22) && E33.Equals(value.E33) // Check diagonal first
            && E12.Equals(value.E12) && E13.Equals(value.E13)
            && E21.Equals(value.E21) && E23.Equals(value.E23)
            && E31.Equals(value.E31) && E32.Equals(value.E32);
    }

    public override readonly int GetHashCode()
    {
        HashCode hash = default;

        hash.Add(E11);
        hash.Add(E12);
        hash.Add(E13);

        hash.Add(E21);
        hash.Add(E22);
        hash.Add(E23);

        hash.Add(E31);
        hash.Add(E32);
        hash.Add(E33);

        return hash.ToHashCode();
    }

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
    {
        Span2D<string> strings = new string[3, 3];
        var maxElementLength = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
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
        for (int i = 0; i < 3; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < 3; j++)
            {
                string value = j != 2 ? $"{strings[i, j]}, " : strings[i, j];
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

    public static Matrix3x3<T> CreateDiagonal(T e11, T e22, T e33)
    {
        return new(
            e11, T.Zero, T.Zero,
            T.Zero, e22, T.Zero,
            T.Zero, T.Zero, e33);
    }

    public readonly T Determinant()
    {
        T a = E11, b = E12, c = E13;
        T d = E21, e = E22, f = E23;
        T g = E31, h = E32, i = E33;

        T ei_fh = e * i - f * h;
        T di_fg = d * i - f * g;
        T dh_eg = d * h - e * g;

        return a * ei_fh - b * di_fg + c * dh_eg;
    }

    public Matrix3x3<T> Inverse()
    {
        T a = E11, b = E12, c = E13;
        T d = E21, e = E22, f = E23;
        T g = E31, h = E32, i = E33;

        T ei_fh = e * i - f * h;
        T di_fg = d * i - f * g;
        T dh_eg = d * h - e * g;

        T det = a * ei_fh - b * di_fg + c * dh_eg;
        if (det == T.Zero)
        {
            return NaM;
        }
        var invDet = T.One / det;

        Matrix3x3<T> result;

        result.E11 = ei_fh * invDet;
        result.E21 = -di_fg * invDet;
        result.E31 = dh_eg * invDet;

        T bi_ch = b * i - c * h;
        T ai_cg = a * i - c * g;
        T ah_bg = a * h - b * g;

        result.E12 = -bi_ch * invDet;
        result.E22 = ai_cg * invDet;
        result.E32 = -ah_bg * invDet;

        T bf_ce = b * f - c * e;
        T af_cd = a * f - c * d;
        T ae_bd = a * e - b * d;

        result.E13 = bf_ce * invDet;
        result.E23 = -af_cd * invDet;
        result.E33 = ae_bd * invDet;

        return result;
    }

    public static bool IsNaM(Matrix3x3<T> matrix)
        => T.IsNaN(matrix.E11) && T.IsNaN(matrix.E22) && T.IsNaN(matrix.E33);

    public T Trace() => E11 + E22 + E33;

    public Matrix3x3<T> Transpose()
    {
        return new(
            E11, E21, E31,
            E12, E22, E32,
            E13, E23, E33);
    }
}
