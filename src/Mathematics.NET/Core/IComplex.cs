// <copyright file="IComplex.cs" company="Mathematics.NET">
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
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for complex numbers</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/></typeparam>
public interface IComplex<T, U>
    : IAdditionOperation<T, T>,
      IDivisionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      INegationOperation<T, T>,
      ISubtractionOperation<T, T>,
      IEqualityRelation<T, bool>,
      IEquatable<T>,
      ISpanFormattable,
      ISpanParsable<T>
    where T : IComplex<T, U>
    where U : IFloatingPointIeee754<U>, IMinMaxValue<U>
{
    /// <summary>The real part of the complex number</summary>
    public Real<U> Re { get; }

    /// <summary>The imaginary part of the complex number</summary>
    public virtual Real<U> Im => Real<U>.Zero;

    /// <summary>The magnitude of the complex number in polar coordinates</summary>
    public virtual Real<U> Magnitude => U.Hypot(Re.Value, Im.Value);

    /// <summary>The phase of the complex number in polar coordinates</summary>
    public virtual Real<U> Phase => U.Atan2(Im.Value, Re.Value);

    /// <summary>Reprsents zero for the type</summary>
    static abstract T Zero { get; }

    /// <summary>Represents one for the type</summary>
    static abstract T One { get; }

    /// <summary>Represents two for the type</summary>
    static abstract T Two { get; }

    /// <summary>Represents NaN for the type</summary>
    static abstract T NaN { get; }

    /// <summary>Compute the absolute value of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The absolute value</returns>
    static abstract T Abs(T z);

    /// <summary>Compute the complex conjugate of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The complex conjugate</returns>
    static abstract T Conjugate(T z);

    /// <summary>Convert a value to one of the current type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="V"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateChecked<V>(V value)
        where V : INumberBase<V>
    {
        T? result;

        if (typeof(V) == typeof(T))
        {
            result = (T)(object)value;
        }
        else if (!T.TryConvertFromChecked(value, out result))
        {
            throw new NotSupportedException();
        }

        return result;
    }

    /// <summary>Convert a value to one of the current type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="V"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateSaturating<V>(V value)
        where V : INumberBase<V>
    {
        T? result;

        if (typeof(V) == typeof(T))
        {
            result = (T)(object)value;
        }
        else if (!T.TryConvertFromSaturating(value, out result))
        {
            throw new NotSupportedException();
        }

        return result;
    }

    /// <summary>Convert a value to one of another type, and truncate values that fall outside of the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="V"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateTruncating<V>(V value)
        where V : INumberBase<V>
    {
        T? result;

        if (typeof(V) == typeof(T))
        {
            result = (T)(object)value;
        }
        else if (!T.TryConvertFromTruncating(value, out result))
        {
            throw new NotSupportedException();
        }

        return result;
    }

    /// <summary>Check if a value is finite</summary>
    /// <param name="z">The value to check</param>
    /// <returns><c>true</c> if the value is finite; otherwise, <c>false</c></returns>
    static abstract bool IsFinite(T z);

    /// <summary>Check if a value is infinity</summary>
    /// <param name="z">The value to check</param>
    /// <returns><c>true</c> if the value is infinity; otherwise, <c>false</c></returns>
    static abstract bool IsInfinity(T z);

    /// <summary>Checks if a value is not a number</summary>
    /// <param name="z">The value to check</param>
    /// <returns><c>true</c> if the value is not a number; otherwise, <c>false</c></returns>
    static abstract bool IsNaN(T z);

    /// <summary>Check if a value is zero</summary>
    /// <param name="z">The value to check</param>
    /// <returns><c>true</c> if the value is zero; otherwise, <c>false</c></returns>
    static abstract bool IsZero(T z);

    /// <summary>Parse a string into a value</summary>
    /// <param name="s">The string to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the string</param>
    /// <returns>The parse result</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c></exception>
    static abstract T Parse(string s, NumberStyles style, IFormatProvider? provider);

    /// <summary>Parse a span of characters into a value</summary>
    /// <param name="s">The span of characters to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the span of characters</param>
    /// <returns>The parse result</returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c></exception>
    static abstract T Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);

    /// <summary>Compute the reciprocal of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The reciprocal</returns>
    static abstract T Reciprocate(T z);

    /// <summary>Try to convert a value to one of the current type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    /// <exception cref="OverflowException">The value is not representable by the type <typeparamref name="T"/>.</exception>
    protected static abstract bool TryConvertFromChecked<V>(V value, [MaybeNullWhen(false)] out T result)
        where V : INumberBase<V>;

    /// <summary>Try to convert a value to one of the current type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    protected static abstract bool TryConvertFromSaturating<V>(V value, [MaybeNullWhen(false)] out T result)
        where V : INumberBase<V>;

    /// <summary>Try to convert a value to one of the current type, and truncate values that fall outside the representable range.</summary>
    /// <typeparam name="V">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    protected static abstract bool TryConvertFromTruncating<V>(V value, [MaybeNullWhen(false)] out T result)
        where V : INumberBase<V>;

    /// <summary>Try to convert a value to one of another type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="V">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    /// <exception cref="OverflowException">The value is not representable by the type <typeparamref name="V"/>.</exception>
    protected static abstract bool TryConvertToChecked<V>(T value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>;

    /// <summary>Try to convert a value to one of another type, and saturate values that fall outside of the representable range.</summary>
    /// <typeparam name="V">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    protected static abstract bool TryConvertToSaturating<V>(T value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>;

    /// <summary>Try to convert a value to one of another type, and truncate values that fall outside of the representable range.</summary>
    /// <typeparam name="V">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c></returns>
    protected static abstract bool TryConvertToTruncating<V>(T value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>;

    /// <summary>Try to parse a string into a value</summary>
    /// <param name="s">The string to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the string</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful</param>
    /// <returns><c>True</c> if the parse was successful; otherwise, <c>false</c></returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    static abstract bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Try to parse a span of characters into a value</summary>
    /// <param name="s">The span of characters to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the span of characters</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful</param>
    /// <returns><c>True</c> if the parse was successful; otherwise, <c>false</c></returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    static abstract bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Convert a value of type <see cref="IFloatingPointIeee754{TSelf}"/> to one of type <typeparamref name="T"/></summary>
    /// <param name="x">The value to convert</param>
    static abstract implicit operator T(U x);
}
