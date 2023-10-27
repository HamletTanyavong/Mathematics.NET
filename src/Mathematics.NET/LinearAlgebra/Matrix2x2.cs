// <copyright file="Matrix2x2.cs" company="Mathematics.NET">
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

/// <summary>Represents a 2x2 matrix</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix2x2<T> : ISquareMatrix<Matrix2x2<T>, T>
    where T : IComplex<T>
{
    private static readonly Matrix2x2<T> s_identity = CreateDiagonal(T.One, T.One);

    public const int Components = 4;
    public const int E1Components = 2;
    public const int E2Components = 2;

    public static readonly Matrix2x2<T> NaM = CreateDiagonal(T.NaN, T.NaN);

    public T E11;
    public T E12;

    public T E21;
    public T E22;

    public Matrix2x2(T e11, T e12, T e21, T e22)
    {
        E11 = e11;
        E12 = e12;

        E21 = e21;
        E22 = e22;
    }

    //
    // Constants
    //

    static int IArrayRepresentable<T>.Components => Components;
    static int ITwoDimensionalArrayRepresentable<Matrix2x2<T>, T>.E1Components => E1Components;
    static int ITwoDimensionalArrayRepresentable<Matrix2x2<T>, T>.E2Components => E2Components;
    static Matrix2x2<T> ISquareMatrix<Matrix2x2<T>, T>.Identity => s_identity;
    static Matrix2x2<T> IMatrix<Matrix2x2<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        get
        {
            if ((uint)row >= 2)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector2<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector2<T>>(ref E11), row);
            return vrow[column];
        }
        set
        {
            if ((uint)row >= 2)
            {
                throw new IndexOutOfRangeException();
            }

            ref Vector2<T> vrow = ref Unsafe.Add(ref Unsafe.As<T, Vector2<T>>(ref E11), row);
            var temp = Vector2<T>.WithElement(vrow, column, value);
            vrow = temp;
        }
    }

    //
    // Operators
    //

    public static Matrix2x2<T> operator +(Matrix2x2<T> a, Matrix2x2<T> b)
    {
        return new(
            a.E11 + b.E11, a.E12 + b.E12,
            a.E21 + b.E21, a.E22 + b.E22);
    }

    public static Matrix2x2<T> operator -(Matrix2x2<T> a, Matrix2x2<T> b)
    {
        return new(
            a.E11 - b.E11, a.E12 - b.E12,
            a.E21 - b.E21, a.E22 - b.E22);
    }

    public static Matrix2x2<T> operator *(Matrix2x2<T> a, Matrix2x2<T> b)
    {
        return new(
            a.E11 * b.E11 + a.E12 * b.E21, a.E11 * b.E12 + a.E12 * b.E22,
            a.E21 * b.E11 + a.E22 * b.E21, a.E21 * b.E12 + a.E22 * b.E22);
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix2x2<T> a, Matrix2x2<T> b)
    {
        return a.E11 == b.E11 && a.E22 == b.E22 // Check diagonal first
            && a.E12 == b.E12
            && a.E21 == b.E21;
    }

    public static bool operator !=(Matrix2x2<T> a, Matrix2x2<T> b)
    {
        return a.E11 != b.E11 || a.E22 != b.E22 // Check diagonal first
            || a.E12 != b.E12
            || a.E21 != b.E21;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix2x2<T> other && Equals(other);

    public bool Equals(Matrix2x2<T> value)
    {
        return E11.Equals(value.E11) && E22.Equals(value.E22) // Check diagonal first
            && E12.Equals(value.E12)
            && E21.Equals(value.E21);
    }

    public override readonly int GetHashCode() => HashCode.Combine(E11, E12, E21, E22);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
    {
        Span2D<string> strings = new string[2, 2];
        var maxElementLength = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
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
        for (int i = 0; i < 2; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < 2; j++)
            {
                string value = j != 1 ? $"{strings[i, j]}, " : strings[i, j];
                builder.Append(value.PadRight(maxElementLength));
            }
            LinAlgExtensions.CloseGroup(builder, newlineChars);
        }
        LinAlgExtensions.CloseGroup(builder, newlineChars, true);
        return string.Format(provider, builder.ToString());
    }

    public static Matrix2x2<T> CreateDiagonal(T e11, T e22)
        => new(e11, T.Zero, T.Zero, e22);

    public readonly T Determinant() => E11 * E22 - E12 * E21;

    public Matrix2x2<T> Inverse()
    {
        var det = Determinant();
        if (det == T.Zero)
        {
            return NaM;
        }
        T invDet = T.One / det;

        return new(E22 * invDet, -E12 * invDet, -E21 * invDet, E11 * invDet);
    }

    public static bool IsNaM(Matrix2x2<T> matrix)
        => T.IsNaN(matrix.E11) && T.IsNaN(matrix.E22);

    public T Trace() => E11 + E22;

    public Matrix2x2<T> Transpose() => new(E11, E21, E12, E22);
}
