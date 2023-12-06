// <copyright file="LinAlgExtensions.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0058

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>A class containing linear algebra extension methods</summary>
public static class LinAlgExtensions
{
    //
    // Reinterpret
    //

    /// <summary>Reinterprets a <see cref="Matrix4x4{T}"/> as a new <see cref="Span2D{T}"/></summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">The matrix to reinterpret</param>
    /// <returns><paramref name="matrix"/> reinterpreted as a new <see cref="Span2D{T}"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this Matrix4x4<T> matrix)
        where T : IComplex<T>
        => new(Unsafe.AsPointer(ref matrix.X1.X1), Matrix4x4<T>.E1Components, Matrix4x4<T>.E2Components, 0);

    /// <summary>Reinterprets a <see cref="Vector4{Real}"/> as a new <see cref="Vector256{Double}"/></summary>
    /// <param name="value">The vector to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector256{Double}"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<double> AsVector256(this Vector4<Real> value)
        => Unsafe.As<Vector4<Real>, Vector256<double>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector256{Real}"/> as a new <see cref="Vector4{Real}"/></summary>
    /// <param name="value">The vector to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4{Real}"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<Real> AsVector4(this Vector256<double> value)
        => Unsafe.As<Vector256<double>, Vector4<Real>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector512{Real}"/> as a new <see cref="Vector4{Complex}"/></summary>
    /// <param name="value">The vector to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4{Complex}"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<Complex> AsVector4(this Vector512<double> value)
        => Unsafe.As<Vector512<double>, Vector4<Complex>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector4{Complex}"/> as a new <see cref="Vector512{Double}"/></summary>
    /// <param name="value">The vector to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector512{Double}"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector512<double> AsVector512(this Vector4<Complex> value)
        => Unsafe.As<Vector4<Complex>, Vector512<double>>(ref value);

    // Do not make the following methods public. AsVector256 must only be used with Vector4<Real>
    // and AsVector256 must only be used with Vector4<Complex>.

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<double> AsVector256<T>(this Vector4<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector4<T>, Vector256<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4<T> AsVector4<T>(this Vector256<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector256<double>, Vector4<T>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector512<double> AsVector512<T>(this Vector4<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector4<T>, Vector512<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4<T> AsVector4<T>(this Vector512<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector512<double>, Vector4<T>>(ref value);

    //
    // Formatting
    //

    /// <summary>Get the string representation of this <see cref="ReadOnlySpan{T}"/> object</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="readOnlySpan">The read-only span to format</param>
    /// <param name="format">The format to use</param>
    /// <param name="provider">The provider to use to format the value</param>
    /// <returns>A string representation of this object</returns>
    public static string ToDisplayString<T>(this ReadOnlySpan<T> readOnlySpan, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        Span<T> span = new T[readOnlySpan.Length];
        readOnlySpan.CopyTo(span);
        return span.ToDisplayString(format, provider);
    }

    /// <summary>Get the string representation of this <see cref="Span{T}"/> object</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="span">The span to format</param>
    /// <param name="format">The format to use</param>
    /// <param name="provider">The provider to use to format the value</param>
    /// <returns>A string representation of this object</returns>
    public static string ToDisplayString<T>(this Span<T> span, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        var count = span.Length;

        var maxElementLength = 0;
        Span<string> strings = new string[count];
        for (int i = 0; i < count; i++)
        {
            var s = span[i].ToString(format, provider);
            strings[i] = s;
            var length = s.Length + 2;
            if (maxElementLength < length)
            {
                maxElementLength = length;
            }
        }

        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < count; i++)
        {
            string value = i != count - 1 ? $"{strings[i]}, " : strings[i];
            builder.Append(value.PadRight(maxElementLength));
        }
        builder.Append(']');
        return string.Format(provider, builder.ToString());
    }

    /// <summary>Get the string representation of this <see cref="ReadOnlySpan2D{T}"/></summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="readOnlySpan2D">A 2D read-only span to format</param>
    /// <param name="format">The format to use</param>
    /// <param name="provider">The provider to use to format the value</param>
    /// <returns>A string representation of this object</returns>
    public static string ToDisplayString<T>(this ReadOnlySpan2D<T> readOnlySpan2D, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        Span2D<T> span = new T[readOnlySpan2D.Width, readOnlySpan2D.Height];
        readOnlySpan2D.CopyTo(span);
        return span.ToDisplayString(format, provider);
    }

    /// <summary>Get the string representation of this <see cref="Span2D{T}"/> object</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="span">The span to format</param>
    /// <param name="format">The format to use</param>
    /// <param name="provider">The provider to use to format the value</param>
    /// <returns>A string representation of this object</returns>
    public static string ToDisplayString<T>(this Span2D<T> span, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        var rows = span.Height;
        var cols = span.Width;

        Span2D<string> strings = new string[rows, cols];
        var maxElementLength = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var s = span[i, j].ToString(format, provider);
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
        for (int i = 0; i < rows; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < cols; j++)
            {
                var value = j != cols - 1 ? $"{strings[i, j]}, " : strings[i, j];
                builder.Append(value.PadRight(maxElementLength));
            }
            builder.CloseGroup(newlineChars);
        }
        builder.CloseGroup(newlineChars, true);
        return string.Format(provider, builder.ToString());
    }

    internal static void CloseGroup(this StringBuilder builder, char[]? unwantedChars, bool isEnd = false)
    {
        builder.TrimEnd(unwantedChars);
        if (!isEnd)
        {
            builder.AppendLine("]");
        }
        else
        {
            builder.Append(']');
        }
    }
}
