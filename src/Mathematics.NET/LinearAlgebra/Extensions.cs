// <copyright file="Extensions.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using System.Text;
using CommunityToolkit.HighPerformance;
using Mathematics.NET.Core;

namespace Mathematics.NET.LinearAlgebra;

public static class Extensions
{
    /// <summary>Create a new <see cref="Span2D{T}"/> over a 4x4 matrix of <typeparamref name="T"/> numbers</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">The input matrix</param>
    /// <returns>A <see cref="Span2D{T}"/> with elements from the input matrix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this Matrix4x4<T> matrix)
        where T : IComplex<T>
    {
        return new Span2D<T>(Unsafe.AsPointer(ref matrix.E11), Matrix4x4<T>.E1Components, Matrix4x4<T>.E2Components, 0);
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
            CloseGroup(builder, newlineChars);
        }
        CloseGroup(builder, newlineChars, true);
        return string.Format(provider, builder.ToString());
    }

    internal static void CloseGroup(StringBuilder builder, char[]? unwantedChars, bool isEnd = false)
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
