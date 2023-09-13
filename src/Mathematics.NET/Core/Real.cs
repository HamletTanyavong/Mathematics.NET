// <copyright file="Real.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0032

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Mathematics.NET.Core;

/// <summary>Represents a real number</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public readonly struct Real<T> : IReal<Real<T>, T>
    where T : IFloatingPointIeee754<T>
{
    public static readonly Real<T> Zero = T.Zero;
    public static readonly Real<T> One = T.One;
    public static readonly Real<T> NaN = T.NaN;
    public static readonly Real<T> NegativeInfinity = T.NegativeInfinity;
    public static readonly Real<T> PositiveInfinity = T.PositiveInfinity;

    private readonly T _value;

    public Real(T real)
    {
        _value = real;
    }

    // Real number properties

    public Real<T> Re => _value;
    public T Value => _value;

    // Constants

    static Real<T> IComplex<Real<T>, T>.Zero => Zero;
    static Real<T> IComplex<Real<T>, T>.One => One;
    static Real<T> IComplex<Real<T>, T>.NaN => NaN;
    static Real<T> IReal<Real<T>, T>.NegativeInfinity => NegativeInfinity;
    static Real<T> IReal<Real<T>, T>.PositiveInfinity => PositiveInfinity;

    // Operators

    public static Real<T> operator -(Real<T> value) => -value._value;
    public static Real<T> operator +(Real<T> left, Real<T> right) => left._value + right._value;
    public static Real<T> operator -(Real<T> left, Real<T> right) => left._value - right._value;
    public static Real<T> operator *(Real<T> left, Real<T> right) => left._value * right._value;
    public static Real<T> operator /(Real<T> left, Real<T> right) => left._value / right._value;

    // Methods

    public static Real<T> Conjugate(Real<T> x) => x;

    // Equality

    public static bool operator ==(Real<T> left, Real<T> right) => left._value == right._value;

    public static bool operator !=(Real<T> left, Real<T> right) => left._value != right._value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Real<T> other && Equals(other);

    public bool Equals(Real<T> value) => _value.Equals(value);

    public override int GetHashCode() => HashCode.Combine(_value);

    // Formatting

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider) => string.Format(provider, "{0}", _value.ToString(format, provider));

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => _value.TryFormat(destination, out charsWritten, null, provider);

    // Implicit operators

    public static implicit operator Real<T>(T x) => new(x);
}
