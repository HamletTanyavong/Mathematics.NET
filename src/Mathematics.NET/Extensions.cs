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

namespace Mathematics.NET;

/// <summary>Core extension methods for Mathematics.NET.</summary>
public static class Extensions
{
    //
    // Casts and Reinterprets
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static U AsFloat<T, U>(this T value)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        where U : IBinaryFloatingPointIeee754<U>
        => Unsafe.As<T, U>(ref value);

    // The real part of any type that implements IComplex<T> should be aligned at zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static V AsFloat<T, U, V>(this T value)
        where T : IComplex<T, U, U>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        where V : IBinaryFloatingPointIeee754<V>
        => Unsafe.As<T, V>(ref value);

    //
    // Rational
    //

    /// <inheritdoc cref="IRational{T, U, V}.Reduce(T)" />
    public static Rational<T, U> Reduce<T, U>(this Rational<T, U> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        => Rational<T, U>.Reduce(value);

    #endregion
}

internal static class BinaryFloatingPointExtensionsIeee754
{
    extension<T>(IBinaryFloatingPointIeee754<T> source)
        where T : IBinaryFloatingPointIeee754<T>
    {
        public static T DblMinPositive => T.CreateChecked(Precision.DblMinPositive);

        public static T Half => T.CreateChecked(0.5);
        public static T OneAndHalf => T.CreateChecked(1.5);
        public static T Two => T.CreateChecked(2);
        public static T Three => T.CreateChecked(3);
        public static T Four => T.CreateChecked(4);
        public static T Six => T.CreateChecked(6);
        public static T Ten => T.CreateChecked(10);
    }
}

internal static class BinaryIntegerExtensions
{
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
}
