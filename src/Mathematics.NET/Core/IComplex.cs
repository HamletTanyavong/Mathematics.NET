// <copyright file="IComplex.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for complex numbers.</summary>
/// <typeparam name="T">The type that implements the interface.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryNumber{TSelf}"/>.</typeparam>
/// <typeparam name="V">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
public interface IComplex<T, U, V>
    : IAdditionOperation<T, T>,
      IDivisionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      IUnaryMinusOperation<T, T>,
      IUnaryPlusOperation<T, T>,
      ISubtractionOperation<T, T>,
      IEqualityRelation<T, bool>,
      IEquatable<T>,
      ISpanFormattable,
      ISpanParsable<T>
    where T : IComplex<T, U, V>
    where U : IBinaryNumber<U>
    where V : IBinaryFloatingPointIeee754<V>, IMinMaxValue<V>
{
    /// <summary>The real part of the complex number.</summary>
    Real<V> Re { get; }

    /// <summary>The imaginary part of the complex number.</summary>
    Real<V> Im => V.Zero;

    /// <summary>The magnitude of the complex number in polar coordinates.</summary>
    Real<V> Magnitude => Real<V>.Hypot(Re, Im);

    /// <summary>The phase of the complex number in polar coordinates.</summary>
    Real<V> Phase => Real<V>.Atan2(Im, Re);

    /// <summary>Represents zero for the type.</summary>
    static abstract T Zero { get; }

    /// <summary>Represents one for the type.</summary>
    static abstract T One { get; }

    /// <summary>Represents NaN for the type.</summary>
    static abstract T NaN { get; }

    /// <summary>Represents the radix, base, of the type.</summary>
    static abstract int Radix { get; }

    /// <summary>Compute the absolute value of a number.</summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The absolute value.</returns>
    static virtual Real<V> Abs(T z) => Real<V>.Hypot(z.Re, z.Im);

    /// <summary>Compute the complex conjugate of a number.</summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The complex conjugate.</returns>
    static abstract T Conjugate(T z);

    /// <summary>Convert a value to one of the current type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The result of the conversion.</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="W"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateChecked<W>(W value)
        where W : INumberBase<W>
    {
        T? result;

        if (typeof(W) == typeof(T))
            result = (T)(object)value;
        else if (!T.TryConvertFromChecked(value, out result))
            throw new NotSupportedException();

        return result;
    }

    /// <summary>Convert a value to one of the current type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The result of the conversion.</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="W"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateSaturating<W>(W value)
        where W : INumberBase<W>
    {
        T? result;

        if (typeof(W) == typeof(T))
            result = (T)(object)value;
        else if (!T.TryConvertFromSaturating(value, out result))
            throw new NotSupportedException();

        return result;
    }

    /// <summary>Convert a value to one of another type, and truncate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The result of the conversion.</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="W"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateTruncating<W>(W value)
        where W : INumberBase<W>
    {
        T? result;

        if (typeof(W) == typeof(T))
            result = (T)(object)value;
        else if (!T.TryConvertFromTruncating(value, out result))
            throw new NotSupportedException();

        return result;
    }

    /// <summary>Create an instance of type <typeparamref name="T"/> from one of type <typeparamref name="V"/>.</summary>
    /// <param name="x">A value of type <typeparamref name="V"/>.</param>
    /// <returns>An instance of type <typeparamref name="T"/> created from <paramref name="x"/>.</returns>
    static abstract T FromFloat(V x);

    /// <summary>Create an instance of type <typeparamref name="T"/> from one of type <see cref="Real{T}"/>.</summary>
    /// <remarks>If the value to convert from is also of type <see cref="Real{T}"/>, return itself.</remarks>
    /// <param name="x">A value of type <see cref="Real{T}"/>.</param>
    /// <returns>An instance of type <typeparamref name="T"/> created from <paramref name="x"/>.</returns>
    static abstract T FromReal(Real<V> x);

    /// <summary>Check if a value is in canonical form.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is in canonical form; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsCanonical(T z);

    /// <summary>Check if a value is complex.</summary>
    /// <remarks>This method will return <see langword="false"/> if the value is purely real or imaginary.</remarks>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is complex; otherwise, <see langword="false"/>.</returns>
    static virtual bool IsComplex(T z) => !T.IsReal(z) && !T.IsImaginary(z);

    /// <summary>Check if a value is finite.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is finite; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsFinite(T z);

    /// <summary>Check if a value is purely imaginary.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is purely imaginary; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsImaginary(T z);

    /// <summary>Check if a value is infinity.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is infinity; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsInfinity(T z);

    /// <summary>Check if a value is an integer.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is an integer; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsInteger(T z);

    /// <summary>Checks if a value is not a number.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is not a number; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsNaN(T z);

    /// <summary>Check if a value is purely real.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is purely real; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsReal(T z);

    /// <summary>Check if a value is zero.</summary>
    /// <param name="z">The value to check.</param>
    /// <returns><see langword="true"/> if the value is zero; otherwise, <see langword="false"/>.</returns>
    static abstract bool IsZero(T z);

    /// <summary>Compares two values and returns whichever has the greatest magnitude.</summary>
    /// <param name="z">The left value.</param>
    /// <param name="w">The right value.</param>
    /// <returns>The value with the greatest magnitude.</returns>
    static abstract T MaxMagnitude(T z, T w);

    /// <summary>Compares two values and returns whichever has the greatest magnitude or returns the non-<c>NaN</c> value.</summary>
    /// <param name="z">The left value.</param>
    /// <param name="w">The right value.</param>
    /// <returns>The value with the greatest magnitude or non-<c>NaN</c> value.</returns>
    static abstract T MaxMagnitudeNumber(T z, T w);

    /// <summary>Compares two values and returns whichever has the lesser magnitude.</summary>
    /// <param name="z">The left value.</param>
    /// <param name="w">The right value.</param>
    /// <returns>The value with the lesser magnitude.</returns>
    static abstract T MinMagnitude(T z, T w);

    /// <summary>Compares two values and returns whichever has the lesser magnitude or returns the non-<c>NaN</c> value.</summary>
    /// <param name="z">The left value.</param>
    /// <param name="w">The right value.</param>
    /// <returns>The value with the lesser magnitude or non-<c>NaN</c> value.</returns>
    static abstract T MinMagnitudeNumber(T z, T w);

    /// <summary>Parse a string into a value.</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="style">The number style.</param>
    /// <param name="provider">The culture-specific formatting information about the string.</param>
    /// <returns>The parse result.</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c></exception>
    static abstract T Parse(string s, NumberStyles style, IFormatProvider? provider);

    /// <summary>Parse a span of characters into a value.</summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="style">The number style.</param>
    /// <param name="provider">The culture-specific formatting information about the span of characters.</param>
    /// <returns>The parse result.</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c></exception>
    static abstract T Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);

    /// <summary>Compute the reciprocal of a number.</summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The reciprocal.</returns>
    static abstract T Reciprocate(T z);

    /// <summary>Try to convert a value to one of the current type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="OverflowException">The value is not representable by the type <typeparamref name="T"/>.</exception>
    protected static abstract bool TryConvertFromChecked<W>(W value, [MaybeNullWhen(false)] out T result)
        where W : INumberBase<W>;

    /// <summary>Try to convert a value to one of the current type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    protected static abstract bool TryConvertFromSaturating<W>(W value, [MaybeNullWhen(false)] out T result)
        where W : INumberBase<W>;

    /// <summary>Try to convert a value to one of the current type, and truncate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The type from which to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    protected static abstract bool TryConvertFromTruncating<W>(W value, [MaybeNullWhen(false)] out T result)
        where W : INumberBase<W>;

    /// <summary>Try to convert a value to one of another type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="W">The target type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="OverflowException">The value is not representable by the target type.</exception>
    protected static abstract bool TryConvertToChecked<W>(T value, [MaybeNullWhen(false)] out W result)
        where W : INumberBase<W>;

    /// <summary>Try to convert a value to one of another type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The target type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    protected static abstract bool TryConvertToSaturating<W>(T value, [MaybeNullWhen(false)] out W result)
        where W : INumberBase<W>;

    /// <summary>Try to convert a value to one of another type, and truncate values that fall outside the representable range.</summary>
    /// <typeparam name="W">The target type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
    protected static abstract bool TryConvertToTruncating<W>(T value, [MaybeNullWhen(false)] out W result)
        where W : INumberBase<W>;

    /// <summary>Try to parse a string into a value.</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="style">The number style.</param>
    /// <param name="provider">The culture-specific formatting information about the string.</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful.</param>
    /// <returns><see langword="true"/> if the parse was successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported.</exception>
    static abstract bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Try to parse a span of characters into a value.</summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="style">The number style.</param>
    /// <param name="provider">The culture-specific formatting information about the span of characters.</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful.</param>
    /// <returns><see langword="true"/> if the parse was successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported.</exception>
    static abstract bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Convert a value of type <typeparamref name="U"/> to one of type <typeparamref name="T"/>.</summary>
    /// <param name="x">The value to convert.</param>
    static abstract implicit operator T(U x);

    /// <summary>Convert a value of type <typeparamref name="T"/> to one of type <typeparamref name="U"/>.</summary>
    /// <param name="x">The value to convert.</param>
    static abstract explicit operator U(T x);
}
