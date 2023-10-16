﻿// <copyright file="Matrix4x4.cs" company="Mathematics.NET">
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
using CommunityToolkit.HighPerformance;
using Mathematics.NET.Core;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

[StructLayout(LayoutKind.Sequential)]
public struct Matrix4x4<T>
    : ITwoDimensionalArrayRepresentable<Matrix4x4<T>, T>,
      ISquareMatrix<Matrix4x4<T>, T>
    where T : IComplex<T>
{
    public static readonly Matrix4x4<T> NaM = CreateDiagonal(T.NaN, T.NaN, T.NaN, T.NaN);

    public T E11;
    public T E12;
    public T E13;
    public T E14;

    public T E21;
    public T E22;
    public T E23;
    public T E24;

    public T E31;
    public T E32;
    public T E33;
    public T E34;

    public T E41;
    public T E42;
    public T E43;
    public T E44;

    public Matrix4x4(
        T e11, T e12, T e13, T e14,
        T e21, T e22, T e23, T e24,
        T e31, T e32, T e33, T e34,
        T e41, T e42, T e43, T e44)
    {
        E11 = e11;
        E12 = e12;
        E13 = e13;
        E14 = e14;

        E21 = e21;
        E22 = e22;
        E23 = e23;
        E24 = e24;

        E31 = e31;
        E32 = e32;
        E33 = e33;
        E34 = e34;

        E41 = e41;
        E42 = e42;
        E43 = e43;
        E44 = e44;
    }

    public static int Components => 16;

    public static int E1Components => 4;

    public static int E2Components => 4;

    //
    // Indexer
    //

    public T this[int row, int column]
    {
        get
        {
            if ((uint)row >= 4)
            {
                throw new ArgumentOutOfRangeException();
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
    // Constants
    //

    static Matrix4x4<T> ISquareMatrix<Matrix4x4<T>, T>.NaM => NaM;

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

        var trimLength = Environment.NewLine.Length;
        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < 4; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < 4; j++)
            {
                string value = j != 3 ? $"{strings[i, j]}, " : strings[i, j];
                builder.Append(value.PadRight(maxElementLength));
            }
            builder.Remove(builder.Length - trimLength, trimLength);
            builder.Append($"],{Environment.NewLine}");
        }
        builder.Remove(builder.Length - trimLength - 1, trimLength + 1); // Trim last comma
        builder.Append(']');
        return string.Format(provider, builder.ToString());
    }

    //
    // Methods
    //

    public static Matrix4x4<T> CreateDiagonal(T e11, T e22, T e33, T e44)
    {
        return new(
            e11, T.Zero, T.Zero, T.Zero,
            T.Zero, e22, T.Zero, T.Zero,
            T.Zero, T.Zero, e33, T.Zero,
            T.Zero, T.Zero, T.Zero, e44);
    }

    public T Determinant()
    {
        throw new NotImplementedException();
    }

    public Matrix4x4<T> Inverse()
    {
        var det = Determinant();
        if (det == T.Zero)
        {
            return NaM;
        }
        throw new NotImplementedException();
    }

    public static bool IsNaM(Matrix4x4<T> matrix)
        => matrix.E11 == T.NaN && matrix.E22 == T.NaN && matrix.E33 == T.NaN && matrix.E44 == T.NaN;

    public T Trace() => E11 + E22 + E33 + E44;

    public Matrix4x4<T> Transpose()
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.E11 = E11; result.E12 = E21; result.E13 = E31; result.E14 = E41;
        result.E21 = E12; result.E22 = E22; result.E23 = E32; result.E24 = E42;
        result.E31 = E13; result.E32 = E23; result.E33 = E33; result.E34 = E43;
        result.E41 = E14; result.E42 = E24; result.E43 = E34; result.E44 = E44;

        return result;
    }
}