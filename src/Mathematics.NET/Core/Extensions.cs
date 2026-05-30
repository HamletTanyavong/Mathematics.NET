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

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Mathematics.NET.Core;

/// <summary>Core extension methods for Mathematics.NET.</summary>
public static class Extensions
{
    //
    // Casts and Reinterprets
    //

    /// <summary>Reinterprets a <see cref="Real"/> as a new <see cref="double"/>.</summary>
    /// <param name="value">The real number to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="double"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AsDouble(this Real value) => Unsafe.As<Real, double>(ref value);

    /// <summary>Reinterprets a <see cref="double"/> as a new <see cref="Real"/>.</summary>
    /// <param name="value">The double to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Real"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Real AsReal(this double value) => Unsafe.As<double, Real>(ref value);

    // Do not make the following methods public.

    // The real part of any type that implements IComplex<T> should be aligned at zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double AsDouble<T>(this T value)
        where T : IComplex<T>
        => Unsafe.As<T, double>(ref value);

    //
    // .NET Types
    //

    extension<T>(IBinaryInteger<T> source)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        // The first 5 prime numbers.

        /// <summary>Represents the number 2.</summary>
        public static T Two => T.CreateChecked(2);
        /// <summary>Represents the number 3.</summary>
        public static T Three => T.CreateChecked(3);
        /// <summary>Represents the number 5.</summary>
        public static T Five => T.CreateChecked(5);
        /// <summary>Represents the number 7.</summary>
        public static T Seven => T.CreateChecked(7);
        /// <summary>Represents the number 11.</summary>
        public static T Eleven => T.CreateChecked(11);

        /// <summary>Compute <paramref name="x"/> raised to the power of <paramref name="n"/>.</summary>
        /// <param name="x">An integer.</param>
        /// <param name="n">A positive power.</param>
        /// <returns><paramref name="x"/> to the power of <paramref name="n"/>.</returns>
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
                if (T.IsOddInteger(n))
                {
                    y *= x;
                    n--;
                }
                x *= x;
                n /= IBinaryInteger<T>.Two;
            }
            return x * y;
        }
    }

    //
    // Rational
    //

    /// <inheritdoc cref="IRational{T, U}.Reduce(T)" />
    public static Rational<T> Reduce<T>(this Rational<T> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => Rational<T>.Reduce(value);
}
