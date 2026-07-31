// <copyright file="Extensions.cs" company="Mathematics.NET">
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

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mathematics.NET;

/// <summary>Core extension methods for Mathematics.NET.</summary>
public static class Extensions
{
    private static readonly ImmutableDictionary<char, char> s_subscripts = new Dictionary<char, char>()
    {
        { '0', '\u2080' },
        { '1', '\u2081' },
        { '2', '\u2082' },
        { '3', '\u2083' },
        { '4', '\u2084' },
        { '5', '\u2085' },
        { '6', '\u2086' },
        { '7', '\u2087' },
        { '8', '\u2088' },
        { '9', '\u2089' }
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<char, char> s_superscripts = new Dictionary<char, char>()
    {
        { '0', '\u2070' },
        { '1', '\u00b9' },
        { '2', '\u00b2' },
        { '3', '\u00b3' },
        { '4', '\u2074' },
        { '5', '\u2075' },
        { '6', '\u2076' },
        { '7', '\u2077' },
        { '8', '\u2078' },
        { '9', '\u2079' },
    }.ToImmutableDictionary();

    //
    // .NET Casts and Reinterprets
    //

    internal static int AsInt<T>(this T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        return typeof(T) switch
        {
            var t when t == typeof(sbyte) => Unsafe.As<T, sbyte>(ref value),
            var t when t == typeof(short) => Unsafe.As<T, short>(ref value),
            var t when t == typeof(int) => Unsafe.As<T, int>(ref value),
            _ => int.CreateChecked(value)
        };
    }

    //
    // Mathematics.NET Casts and Reinterprets
    //

    /// <summary>Reinterprets a <see cref="Real{T}"/> as a new <typeparamref name="T"/>.</summary>
    /// <param name="value">The real number to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <typeparamref name="T"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T AsBackingType<T>(this Real<T> value)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => Unsafe.As<Real<T>, T>(ref value);

    /// <summary>Reinterprets a <typeparamref name="T"/> as a new <see cref="Real{T}"/>.</summary>
    /// <param name="value">The <typeparamref name="T"/> to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Real{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Real<T> AsReal<T>(this T value)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => Unsafe.As<T, Real<T>>(ref value);

    #region Keep Private

    //
    // Do not make the following methods public.
    //

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T AsBackingType<T>(this float value)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => Unsafe.As<float, T>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T AsBackingType<T>(this double value)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => Unsafe.As<double, T>(ref value);

    // The real part of any type that implements IComplex<T> should be aligned at zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static V AsFloat<T, U, V>(this T value)
        where T : IComplex<T, U, U>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        where V : IBinaryFloatingPointIeee754<V>
        => Unsafe.As<T, V>(ref value);

    #endregion

    //
    // Rational
    //

    /// <inheritdoc cref="IRational{T, U, V}.Reduce(T)" />
    public static Rational<T, U> Reduce<T, U>(this Rational<T, U> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        => Rational<T, U>.Reduce(value);

    //
    // Formatting
    //

    internal static string ToSubscript(this string s)
    {
        StringBuilder builder = new();
        foreach (var c in s)
        {
            _ = builder.Append(s_subscripts[c]);
        }
        return builder.ToString();
    }

    internal static string ToSuperscript(this string s)
    {
        StringBuilder builder = new();
        foreach (var c in s)
        {
            _ = builder.Append(s_superscripts[c]);
        }
        return builder.ToString();
    }
}

/// <summary>Extensions for integers.</summary>
public static class BinaryIntegerExtensions
{
    extension<T>(IBinaryInteger<T> source)
        where T : IBinaryInteger<T>, ISignedNumber<T>, IBitwiseOperators<T, T, T>
    {
        /// <summary>Compute the floor of the log of <paramref name="x"/> base <paramref name="b"/>.</summary>
        /// <param name="x">An integer.</param>
        /// <param name="b">A base.</param>
        /// <returns>The floor of the log of <paramref name="x"/> base <paramref name="b"/>.</returns>
        public static T FloorLog(T x, T b)
        {
            if (x < T.One || b < T.CreateSaturating(2))
                return -T.One;
            var result = T.Zero;
            while (x >= b)
            {
                result += T.One;
                x /= b;
            }
            return result;
        }

        /// <summary>Compute the floor of the square root of <paramref name="x"/>.</summary>
        /// <param name="x">An integer.</param>
        /// <returns>The floor of the square root of <paramref name="x"/>.</returns>
        /// <exception cref="MathematicsException">Thrown when <paramref name="x"/> is negative.</exception>
        public static T FloorSqrt(T x)
        {
            if (x < T.Zero)
                throw new MathematicsException("Cannot take the square root of a negative integer.");

            var powerOfFour = T.One;
            while (powerOfFour <= x)
            {
                powerOfFour <<= 2;
            }
            powerOfFour >>= 2;

            var result = T.Zero;
            while (powerOfFour != T.Zero)
            {
                var delta = result + powerOfFour;
                if (x >= delta)
                {
                    x -= delta;
                    result = (result >> 1) + powerOfFour;
                }
                else
                {
                    result >>= 1;
                }
                powerOfFour >>= 2;
            }

            return result;
        }

        /// <summary>Compute <paramref name="x"/> raised to the power of <paramref name="n"/>.</summary>
        /// <param name="x">An integer.</param>
        /// <param name="n">A positive power.</param>
        /// <returns><paramref name="x"/> to the power of <paramref name="n"/>.</returns>
        public static T Pow(T x, int n)
        {
            if (n == 0)
                return T.One;
            if (T.IsZero(x))
                return T.Zero;
            if (x == T.One)
                return T.One;
            if (x == T.NegativeOne)
                return (n & 1) == 0 ? T.One : T.NegativeOne;

            var y = T.One;
            while (n > 1)
            {
                if ((n & 1) == 1)
                {
                    y *= x;
                    n--;
                }
                x *= x;
                n /= 2;
            }
            return x * y;
        }

        /// <inheritdoc cref="Pow{T}(T, int)"/>
        public static T Pow(T x, T n)
        {
            if (T.IsZero(n))
                return T.One;
            if (T.IsZero(x))
                return T.Zero;
            if (x == T.One)
                return T.One;
            if (x == T.NegativeOne)
                return T.IsEvenInteger(n) ? T.One : T.NegativeOne;

            var y = T.One;
            while (n > T.One)
            {
                if ((n & T.One) == T.One)
                {
                    y *= x;
                    n--;
                }
                x *= x;
                n /= T.CreateSaturating(2);
            }
            return x * y;
        }
    }
}
