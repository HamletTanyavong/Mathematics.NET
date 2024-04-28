// <copyright file="Extensions.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Mathematics.NET.Core;

/// <summary>Extension methods for Mathematics.NET</summary>
public static class Extensions
{
    //
    // Casts and reinterprets
    //

    /// <summary>Reinterprets a <see cref="Real"/> as a new <see cref="double"/></summary>
    /// <param name="value">The real number to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="double"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AsDouble(this Real value) => Unsafe.As<Real, double>(ref value);

    /// <summary>Reinterprets a <see cref="double"/> as a new <see cref="Real"/></summary>
    /// <param name="value">The double to reinterpret</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Real"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Real AsReal(this double value) => Unsafe.As<double, Real>(ref value);

    // Do not make the following methods public.

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double AsDouble<T>(this T value)
        where T : IComplex<T>
        => Unsafe.As<T, double>(ref value);

    //
    // Rational
    //

    /// <inheritdoc cref="IRational{T, U}.Reduce(T)" />
    public static Rational<T> Reduce<T>(this Rational<T> value)
        where T : IBinaryInteger<T>
        => Rational<T>.Reduce(value);
}
