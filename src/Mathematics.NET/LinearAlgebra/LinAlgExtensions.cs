// <copyright file="LinAlgExtensions.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0058

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using Mathematics.NET.AutoDiff;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>A class containing linear algebra extension methods.</summary>
public static class LinAlgExtensions
{
    //
    // Casts and Reinterprets
    //

    /// <summary>Reinterprets a <see cref="Matrix2x2{T}"/> as a new <see cref="Span2D{T}"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="matrix">The matrix to reinterpret.</param>
    /// <returns><paramref name="matrix"/> reinterpreted as a new <see cref="Span2D{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this ref Matrix2x2<T> matrix)
        where T : IComplex<T>
        => new(Unsafe.AsPointer(ref matrix), Matrix2x2<T>.E1Components, Matrix2x2<T>.E2Components, 0);

    /// <summary>Reinterprets a <see cref="Matrix3x3{T}"/> as a new <see cref="Span2D{T}"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="matrix">The matrix to reinterpret.</param>
    /// <returns><paramref name="matrix"/> reinterpreted as a new <see cref="Span2D{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this ref Matrix3x3<T> matrix)
        where T : IComplex<T>
        => new(Unsafe.AsPointer(ref matrix), Matrix3x3<T>.E1Components, Matrix3x3<T>.E2Components, 0);

    /// <summary>Reinterprets a <see cref="Matrix4x4{T}"/> as a new <see cref="Span2D{T}"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="matrix">The matrix to reinterpret.</param>
    /// <returns><paramref name="matrix"/> reinterpreted as a new <see cref="Span2D{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this ref Matrix4x4<T> matrix)
        where T : IComplex<T>
        => new(Unsafe.AsPointer(ref matrix), Matrix4x4<T>.E1Components, Matrix4x4<T>.E2Components, 0);

    /// <summary>Reinterprets a <see cref="Vector4{Real}"/> as a new <see cref="Vector256{Double}"/>.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector256{Double}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<double> AsVector256(this Vector4<Real> value)
        => Unsafe.As<Vector4<Real>, Vector256<double>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector256{Real}"/> as a new <see cref="Vector4{Real}"/>.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4{Real}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<Real> AsVector4(this Vector256<double> value)
        => Unsafe.As<Vector256<double>, Vector4<Real>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector512{Real}"/> as a new <see cref="Vector4{Complex}"/>.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4{Complex}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<Complex> AsVector4(this Vector512<double> value)
        => Unsafe.As<Vector512<double>, Vector4<Complex>>(ref value);

    /// <summary>Reinterprets a <see cref="Vector4{Complex}"/> as a new <see cref="Vector512{Double}"/>.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector512{Double}"/>.</returns>
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
    // AutoDiff
    //

    /// <summary>Create an autodiff vector from a seed vector.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A variable vector of length two.</returns>
    public static AutoDiffVector2<T> CreateAutoDiffVector<T>(this ITape<T> tape, Vector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2));

    /// <summary>Create an autodiff vector from seed values.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <param name="x2Seed">The second seed value.</param>
    /// <returns>A variable vector of length two.</returns>
    public static AutoDiffVector2<T> CreateAutoDiffVector<T>(this ITape<T> tape, T x1Seed, T x2Seed)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed));

    /// <summary>Create an autodiff vector from a seed vector.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A variable vector of length three.</returns>
    public static AutoDiffVector3<T> CreateAutoDiffVector<T>(this ITape<T> tape, Vector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3));

    /// <summary>Create an autodiff vector from seed values.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <param name="x2Seed">The second seed value.</param>
    /// <param name="x3Seed">The third seed value.</param>
    /// <returns>A variable vector of length three.</returns>
    public static AutoDiffVector3<T> CreateAutoDiffVector<T>(this ITape<T> tape, T x1Seed, T x2Seed, T x3Seed)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed));

    /// <summary>Create an autodiff vector from a seed vector.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A variable vector of length four.</returns>
    public static AutoDiffVector4<T> CreateAutoDiffVector<T>(this ITape<T> tape, Vector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3), tape.CreateVariable(x.X4));

    /// <summary>Create an autodiff vector from seed values.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/>.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <param name="x2Seed">The second seed value.</param>
    /// <param name="x3Seed">The third seed value.</param>
    /// <param name="x4Seed">The fourth seed value.</param>
    /// <returns>A variable vector of length four.</returns>
    public static AutoDiffVector4<T> CreateAutoDiffVector<T>(this ITape<T> tape, T x1Seed, T x2Seed, T x3Seed, T x4Seed)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed), tape.CreateVariable(x4Seed));

    //
    // Formatting
    //

    /// <summary>Get the string representation of this <see cref="ReadOnlySpan{T}"/> object.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="readOnlySpan">The read-only span to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
    public static string ToDisplayString<T>(this ReadOnlySpan<T> readOnlySpan, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        Span<T> span = new T[readOnlySpan.Length];
        readOnlySpan.CopyTo(span);
#pragma warning disable EPS06
        return span.ToDisplayString(format, provider);
#pragma warning restore EPS06
    }

    /// <summary>Get the string representation of this <see cref="Span{T}"/> object.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="span">The span to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
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
                maxElementLength = length;
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

    /// <summary>Get the string representation of this <see cref="ReadOnlySpan2D{T}"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="readOnlySpan2D">A 2D read-only span to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
    public static string ToDisplayString<T>(this ReadOnlySpan2D<T> readOnlySpan2D, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        Span2D<T> span = new T[readOnlySpan2D.Height, readOnlySpan2D.Width];
        readOnlySpan2D.CopyTo(span);
#pragma warning disable EPS06
        return span.ToDisplayString(format, provider);
#pragma warning restore EPS06
    }

    /// <summary>Get the string representation of this <see cref="Span2D{T}"/> object.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="span2D">The span to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
    public static string ToDisplayString<T>(this Span2D<T> span2D, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        var rows = span2D.Height;
        var cols = span2D.Width;

        Span2D<string> strings = new string[rows, cols];
        var maxElementLength = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var s = span2D[i, j].ToString(format, provider);
                strings[i, j] = s;
                var length = s.Length + 2;
                if (maxElementLength < length)
                    maxElementLength = length;
            }
        }

        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < rows; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < cols; j++)
            {
                var value = j != cols - 1 ? $"{strings[i, j]}, " : strings[i, j];
                builder.Append(value.PadRight(maxElementLength));
            }
            builder.CloseGroup();
        }
        builder.CloseGroup(true);
        return string.Format(provider, builder.ToString());
    }

    /// <summary>Get the string representation of this <see cref="Array"/> object.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="array">An array to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
    public static string ToDisplayString<T>(this T[,,] array, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        var e0Length = array.GetLength(0);
        var e1Length = array.GetLength(1);
        var e2Length = array.GetLength(2);

        var maxElementLength = 0;
        var strings = new string[e0Length, e1Length, e2Length];
        for (int i = 0; i < e0Length; i++)
        {
            for (int j = 0; j < e1Length; j++)
            {
                for (int k = 0; k < e2Length; k++)
                {
                    var s = array[i, j, k].ToString(format, provider);
                    strings[i, j, k] = s;
                    var length = s.Length + 2;
                    if (maxElementLength < length)
                        maxElementLength = length;
                }
            }
        }

        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < e0Length; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < e1Length; j++)
            {
                builder.Append(j != 0 ? "  [" : "[");
                for (int k = 0; k < e2Length; k++)
                {
                    string value = k != e2Length - 1 ? $"{strings[i, j, k]}, " : strings[i, j, k];
                    builder.Append(value.PadRight(maxElementLength));
                }
                builder.CloseGroup();
            }
            builder.CloseGroup();
        }
        builder.CloseGroup(true);
        return string.Format(provider, builder.ToString());
    }

    /// <summary>Get the string representation of this <see cref="Array"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="array">An array to format.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>A string representation of this object.</returns>
    public static string ToDisplayString<T>(this T[,,,] array, string? format = null, IFormatProvider? provider = null)
        where T : IComplex<T>
    {
        var e0Length = array.GetLength(0);
        var e1Length = array.GetLength(1);
        var e2Length = array.GetLength(2);
        var e3Length = array.GetLength(3);

        var maxElementLength = 0;
        var strings = new string[e0Length, e1Length, e2Length, e3Length];
        for (int i = 0; i < e0Length; i++)
        {
            for (int j = 0; j < e1Length; j++)
            {
                for (int k = 0; k < e2Length; k++)
                {
                    for (int l = 0; l < e3Length; l++)
                    {
                        var s = array[i, j, k, l].ToString(format, provider);
                        strings[i, j, k, l] = s;
                        var length = s.Length + 2;
                        if (maxElementLength < length)
                            maxElementLength = length;
                    }
                }
            }
        }

        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < e0Length; i++)
        {
            builder.Append(i != 0 ? " [" : "[");
            for (int j = 0; j < e1Length; j++)
            {
                builder.Append(j != 0 ? "  [" : "[");
                for (int k = 0; k < e2Length; k++)
                {
                    builder.Append(k != 0 ? "   [" : "[");
                    for (int l = 0; l < e3Length; l++)
                    {
                        string value = l != e3Length - 1 ? $"{strings[i, j, k, l]}, " : strings[i, j, k, l];
                        builder.Append(value.PadRight(maxElementLength));
                    }
                    builder.CloseGroup();
                }
                builder.CloseGroup();
            }
            builder.CloseGroup();
        }
        builder.CloseGroup(true);
        return string.Format(provider, builder.ToString());
    }

    private static void CloseGroup(this StringBuilder builder, bool isEnd = false)
    {
        builder.TrimEnd(Environment.NewLine.ToCharArray());
        if (!isEnd)
            builder.AppendLine("]");
        else
            builder.Append(']');
    }

    //
    // Spans
    //

    /// <summary>Pad a span of numbers with zeros.</summary>
    /// <remarks>This method allocates a new array and its performance impacts should be considered.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="span">The original span.</param>
    /// <param name="length">A new length.</param>
    /// <returns>A padded span.</returns>
    public static ReadOnlySpan<T> Pad<T>(this ReadOnlySpan<T> span, int length)
        where T : IComplex<T>
    {
        if (span.Length > length)
            throw new Exception("The new size of the padded span must be greater or equal than the original size.");

        var result = new T[length];
        span.CopyTo(result);
        return result;
    }

    /// <inheritdoc cref="Pad{T}(ReadOnlySpan{T}, int)"/>
    public static Span<T> Pad<T>(this Span<T> span, int length)
        where T : IComplex<T>
    {
        if (span.Length > length)
            throw new Exception("The new size of the padded span must be greater or equal than the original size.");

        var result = new T[length];
        span.CopyTo(result);
        return result;
    }

    /// <summary>Pad a 2D span of numbers with zeros.</summary>
    /// <remarks>This method allocates a new array and its performance impacts should be considered.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <param name="span">The original span.</param>
    /// <param name="height">A new height.</param>
    /// <param name="width">A new width.</param>
    /// <returns>A padded 2D span.</returns>
    public static ReadOnlySpan2D<T> Pad<T>(this ReadOnlySpan2D<T> span, int height, int width)
        where T : IComplex<T>
    {
        if (span.Height > height || span.Width > width)
            throw new Exception("The new size of the padded span must be greater or equal than the original size.");

        var result = new T[height, width];
        for (int i = 0; i < span.Height; i++)
        {
            span.GetRow(i).CopyTo(result.GetRow(i));
        }
        return result;
    }

    /// <inheritdoc cref="Pad{T}(ReadOnlySpan2D{T}, int, int)"/>
    public static Span2D<T> Pad<T>(this Span2D<T> span, int height, int width)
        where T : IComplex<T>
    {
        if (span.Height > height || span.Width > width)
            throw new Exception("The new size of the padded span must be greater or equal than the original size.");

        var result = new T[height, width];
        for (int i = 0; i < span.Height; i++)
        {
            span.GetRow(i).CopyTo(result.GetRow(i));
        }
        return result;
    }
}
