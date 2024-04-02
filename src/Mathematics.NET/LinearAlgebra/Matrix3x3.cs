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

    public Vector3<T> X1;
    public Vector3<T> X2;
    public Vector3<T> X3;

    /// <summary>Create a 3x3 matrix given a set of 9 values</summary>
    /// <param name="e11">The $ e_{11} $ component</param>
    /// <param name="e12">The $ e_{12} $ component</param>
    /// <param name="e13">The $ e_{13} $ component</param>
    /// <param name="e21">The $ e_{21} $ component</param>
    /// <param name="e22">The $ e_{22} $ component</param>
    /// <param name="e23">The $ e_{23} $ component</param>
    /// <param name="e31">The $ e_{31} $ component</param>
    /// <param name="e32">The $ e_{32} $ component</param>
    /// <param name="e33">The $ e_{33} $ component</param>
    public Matrix3x3(
        T e11, T e12, T e13,
        T e21, T e22, T e23,
        T e31, T e32, T e33)
    {
        X1 = new(e11, e12, e13);
        X2 = new(e21, e22, e23);
        X3 = new(e31, e32, e33);
    }

    //
    // Constants
    //

    static int IArrayRepresentable<Matrix3x3<T>, T>.Components => Components;
    static int ITwoDimensionalArrayRepresentable<Matrix3x3<T>, T>.E1Components => E1Components;
    static int ITwoDimensionalArrayRepresentable<Matrix3x3<T>, T>.E2Components => E2Components;
    static Matrix3x3<T> ISquareMatrix<Matrix3x3<T>, T>.Identity => s_identity;
    static Matrix3x3<T> IMatrix<Matrix3x3<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if ((uint)row >= 3)
            {
                throw new IndexOutOfRangeException();
            }
            return Unsafe.Add(ref Unsafe.AsRef(in X1), row)[column];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if ((uint)row >= 3)
            {
                throw new IndexOutOfRangeException();
            }
            Unsafe.Add(ref X1, row)[column] = value;
        }
    }

    //
    // Operators
    //

    public static Matrix3x3<T> operator -(Matrix3x3<T> matrix)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = -matrix.X1;
        result.X2 = -matrix.X2;
        result.X3 = -matrix.X3;

        return result;
    }

    public static Matrix3x3<T> operator +(Matrix3x3<T> matrix)
        => matrix;

    public static Matrix3x3<T> operator +(Matrix3x3<T> left, Matrix3x3<T> right)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = left.X1 + left.X1;
        result.X2 = left.X2 + left.X2;
        result.X3 = left.X3 + left.X3;

        return result;
    }

    public static Matrix3x3<T> operator -(Matrix3x3<T> left, Matrix3x3<T> right)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = left.X1 - left.X1;
        result.X2 = left.X2 - left.X2;
        result.X3 = left.X3 - left.X3;

        return result;
    }

    public static Matrix3x3<T> operator *(Matrix3x3<T> left, Matrix3x3<T> right)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = right.X1 * left.X1.X1;
        result.X1 += right.X2 * left.X1.X2;
        result.X1 += right.X3 * left.X1.X3;

        result.X2 = right.X1 * left.X2.X1;
        result.X2 += right.X2 * left.X2.X2;
        result.X2 += right.X3 * left.X2.X3;

        result.X3 = right.X1 * left.X3.X1;
        result.X3 += right.X2 * left.X3.X2;
        result.X3 += right.X3 * left.X3.X3;

        return result;
    }

    public static Matrix3x3<T> operator *(T c, Matrix3x3<T> matrix)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = c * matrix.X1;
        result.X2 = c * matrix.X2;
        result.X3 = c * matrix.X3;

        return result;
    }

    public static Matrix3x3<T> operator *(Matrix3x3<T> matrix, T c)
    {
        Unsafe.SkipInit(out Matrix3x3<T> result);

        result.X1 = matrix.X1 * c;
        result.X2 = matrix.X2 * c;
        result.X3 = matrix.X3 * c;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix3x3<T> left, Matrix3x3<T> right)
        => left.X1 == right.X1 && left.X2 == right.X2 && left.X3 == right.X3;

    public static bool operator !=(Matrix3x3<T> left, Matrix3x3<T> right)
        => left.X1 != right.X1 || left.X2 != right.X2 || left.X3 != right.X3;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix3x3<T> other && Equals(other);

    public bool Equals(Matrix3x3<T> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2) && X3.Equals(value.X3);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2, X3);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
        => this.AsSpan2D().ToDisplayString();

    //
    // Methods
    //

    /// <summary>Create a diagonal matrix from specified values along the diagonal.</summary>
    /// <param name="e11">The $ e_{11} $ component</param>
    /// <param name="e22">The $ e_{22} $ component</param>
    /// <param name="e33">The $ e_{33} $ component</param>
    /// <returns>A diagonal matrix</returns>
    public static Matrix3x3<T> CreateDiagonal(T e11, T e22, T e33)
    {
        return new(
            e11, T.Zero, T.Zero,
            T.Zero, e22, T.Zero,
            T.Zero, T.Zero, e33);
    }

    public readonly T Determinant()
    {
        T a = X1.X1, b = X1.X2, c = X1.X3;
        T d = X2.X1, e = X2.X2, f = X2.X3;
        T g = X3.X1, h = X3.X2, i = X3.X3;

        T ei_fh = e * i - f * h;
        T di_fg = d * i - f * g;
        T dh_eg = d * h - e * g;

        return a * ei_fh - b * di_fg + c * dh_eg;
    }

    public readonly Matrix3x3<T> Inverse()
    {
        T a = X1.X1, b = X1.X2, c = X1.X3;
        T d = X2.X1, e = X2.X2, f = X2.X3;
        T g = X3.X1, h = X3.X2, i = X3.X3;

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

        result.X1.X1 = ei_fh * invDet;
        result.X2.X1 = -di_fg * invDet;
        result.X3.X1 = dh_eg * invDet;

        T bi_ch = b * i - c * h;
        T ai_cg = a * i - c * g;
        T ah_bg = a * h - b * g;

        result.X1.X2 = -bi_ch * invDet;
        result.X2.X2 = ai_cg * invDet;
        result.X3.X2 = -ah_bg * invDet;

        T bf_ce = b * f - c * e;
        T af_cd = a * f - c * d;
        T ae_bd = a * e - b * d;

        result.X1.X3 = bf_ce * invDet;
        result.X2.X3 = -af_cd * invDet;
        result.X3.X3 = ae_bd * invDet;

        return result;
    }

    public static bool IsNaM(Matrix3x3<T> matrix)
        => T.IsNaN(matrix.X1.X1) && T.IsNaN(matrix.X2.X2) && T.IsNaN(matrix.X3.X3);

    public readonly T Trace() => X1.X1 + X2.X2 + X3.X3;

    public readonly Matrix3x3<T> Transpose()
    {
        return new(
            X1.X1, X2.X1, X3.X1,
            X1.X2, X2.X2, X3.X2,
            X1.X3, X2.X3, X3.X3);
    }
}
