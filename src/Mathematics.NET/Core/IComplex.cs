// <copyright file="IComplex.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for complex numbers</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
public interface IComplex<T>
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
    where T : IComplex<T>
{
    /// <summary>The real part of the complex number</summary>
    public Real Re { get; }

    /// <summary>The imaginary part of the complex number</summary>
    public virtual Real Im => Real.Zero;

    /// <summary>The magnitude of the complex number in polar coordinates</summary>
    public virtual Real Magnitude => Real.Hypot(Re, Im);

    /// <summary>The phase of the complex number in polar coordinates</summary>
    public virtual Real Phase => Real.Atan2(Im, Re);

    /// <summary>Reprsents zero for the type</summary>
    static abstract T Zero { get; }

    /// <summary>Represents one for the type</summary>
    static abstract T One { get; }

    /// <summary>Represents NaN for the type</summary>
    static abstract T NaN { get; }

    /// <summary>Represents the radix, base, of the type</summary>
    static abstract int Radix { get; }

    /// <summary>Compute the absolute value of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The absolute value</returns>
    static virtual Real Abs(T z) => Real.Hypot(z.Re, z.Im);

    /// <summary>Compute the complex conjugate of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The complex conjugate</returns>
    static abstract T Conjugate(T z);

    /// <summary>Convert a value to one of the current type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="U"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateChecked<U>(U value)
        where U : INumberBase<U>
    {
        T? result;

        if (typeof(U) == typeof(T))
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
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="U"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateSaturating<U>(U value)
        where U : INumberBase<U>
    {
        T? result;

        if (typeof(U) == typeof(T))
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
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <returns>The result of the conversion</returns>
    /// <exception cref="NotSupportedException">Conversions from the type <typeparamref name="U"/> are not supported.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static virtual T CreateTruncating<U>(U value)
        where U : INumberBase<U>
    {
        T? result;

        if (typeof(U) == typeof(T))
        {
            result = (T)(object)value;
        }
        else if (!T.TryConvertFromTruncating(value, out result))
        {
            throw new NotSupportedException();
        }

        return result;
    }

    /// <summary>Create an instance of type <typeparamref name="T"/> from one of type <see cref="double"/></summary>
    /// <param name="x">A value of type <see cref="double"/></param>
    /// <returns>An instance of type <typeparamref name="T"/> created from <paramref name="x"/></returns>
    static abstract T FromDouble(double x);

    /// <summary>Create an instance of type <typeparamref name="T"/> from one of type <see cref="Real"/></summary>
    /// <remarks>If the value to convert from is also of type <see cref="Real"/>, return itself</remarks>
    /// <param name="x">A value of type <see cref="Real"/></param>
    /// <returns>An instance of type <typeparamref name="T"/> created from <paramref name="x"/></returns>
    static abstract T FromReal(Real x);

    /// <summary>Check if a value is finite</summary>
    /// <param name="z">The value to check</param>
    /// <returns><see langword="true"/> if the value is finite; otherwise, <see langword="false"/></returns>
    static abstract bool IsFinite(T z);

    /// <summary>Check if a value is infinity</summary>
    /// <param name="z">The value to check</param>
    /// <returns><see langword="true"/> if the value is infinity; otherwise, <see langword="false"/></returns>
    static abstract bool IsInfinity(T z);

    /// <summary>Checks if a value is not a number</summary>
    /// <param name="z">The value to check</param>
    /// <returns><see langword="true"/> if the value is not a number; otherwise, <see langword="false"/></returns>
    static abstract bool IsNaN(T z);

    /// <summary>Check if a value is zero</summary>
    /// <param name="z">The value to check</param>
    /// <returns><see langword="true"/> if the value is zero; otherwise, <see langword="false"/></returns>
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
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    /// <exception cref="OverflowException">The value is not representable by the type <typeparamref name="T"/>.</exception>
    protected static abstract bool TryConvertFromChecked<U>(U value, [MaybeNullWhen(false)] out T result)
        where U : INumberBase<U>;

    /// <summary>Try to convert a value to one of the current type, and saturate values that fall outside the representable range.</summary>
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    protected static abstract bool TryConvertFromSaturating<U>(U value, [MaybeNullWhen(false)] out T result)
        where U : INumberBase<U>;

    /// <summary>Try to convert a value to one of the current type, and truncate values that fall outside the representable range.</summary>
    /// <typeparam name="U">The type from which to convert</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    protected static abstract bool TryConvertFromTruncating<U>(U value, [MaybeNullWhen(false)] out T result)
        where U : INumberBase<U>;

    /// <summary>Try to convert a value to one of another type, and throw and overflow exception if the value falls outside the representable range.</summary>
    /// <typeparam name="U">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    /// <exception cref="OverflowException">The value is not representable by the target type.</exception>
    protected static abstract bool TryConvertToChecked<U>(T value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>;

    /// <summary>Try to convert a value to one of another type, and saturate values that fall outside of the representable range.</summary>
    /// <typeparam name="U">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    protected static abstract bool TryConvertToSaturating<U>(T value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>;

    /// <summary>Try to convert a value to one of another type, and truncate values that fall outside of the representable range.</summary>
    /// <typeparam name="U">The target type</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="result">The result of the conversion</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/></returns>
    protected static abstract bool TryConvertToTruncating<U>(T value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>;

    /// <summary>Try to parse a string into a value</summary>
    /// <param name="s">The string to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the string</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful</param>
    /// <returns><see langword="true"/> if the parse was successful; otherwise, <see langword="false"/></returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    static abstract bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Try to parse a span of characters into a value</summary>
    /// <param name="s">The span of characters to parse</param>
    /// <param name="style">The number style</param>
    /// <param name="provider">The culture-specific formatting information about the span of characters</param>
    /// <param name="result">The result of the parse or a default value if the parse was unsuccessful</param>
    /// <returns><see langword="true"/> if the parse was successful; otherwise, <see langword="false"/></returns>
    /// <exception cref="ArgumentException"><paramref name="style" /> is not a supported</exception>
    static abstract bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out T result);

    /// <summary>Convert a value of type <see cref="double"/> to one of type <typeparamref name="T"/></summary>
    /// <param name="x">The value to convert</param>
    static virtual implicit operator T(double x) => T.FromDouble(x);

    /// <summary>Convert a value of type <see cref="Real"/> to one of type <typeparamref name="T"/></summary>
    /// <remarks>If the type to convert from is also <see cref="Real"/>, return itself</remarks>
    /// <param name="x">The value to convert</param>
    static virtual implicit operator T(Real x) => T.FromReal(x);
}
