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
using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for complex numbers</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public interface IComplex<T, U>
    : INegationOperation<T, T>,
      IAdditionOperation<T, T>,
      IDivisionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      ISubtractionOperation<T, T>,
      IEqualityRelation<T, bool>,
      IEquatable<T>,
      ISpanFormattable,
      ISpanParsable<T>
    where T : IComplex<T, U>
    where U : IFloatingPointIeee754<U>
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
    /// <summary>Represents NaN for the type</summary>
    static abstract T NaN { get; }
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
    /// <summary>Compute the complex conjugate of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The complex conjugate</returns>
    static abstract T Conjugate(T z);
    /// <summary>Compute the reciprocal of a number</summary>
    /// <param name="z">A complex number</param>
    /// <returns>The reciprocal</returns>
    static abstract T Reciprocate(T z);
    /// <summary>Convert a value of type <see cref="IFloatingPointIeee754{TSelf}"/> to one of type <typeparamref name="T"/></summary>
    /// <param name="x">The value to convert</param>
    static abstract implicit operator T(U x);
}
