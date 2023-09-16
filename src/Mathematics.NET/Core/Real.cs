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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Mathematics.NET.Core;

/// <summary>Represents a real number</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
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

    //
    // Real number properties
    //

    public Real<T> Re => _value;
    public T Value => _value;

    //
    // Constants
    //

    static Real<T> IComplex<Real<T>, T>.Zero => Zero;
    static Real<T> IComplex<Real<T>, T>.One => One;
    static Real<T> IComplex<Real<T>, T>.NaN => NaN;
    static Real<T> IReal<Real<T>, T>.NegativeInfinity => NegativeInfinity;
    static Real<T> IReal<Real<T>, T>.PositiveInfinity => PositiveInfinity;

    //
    // Operators
    //

    public static Real<T> operator -(Real<T> value) => -value._value;
    public static Real<T> operator +(Real<T> left, Real<T> right) => left._value + right._value;
    public static Real<T> operator -(Real<T> left, Real<T> right) => left._value - right._value;
    public static Real<T> operator *(Real<T> left, Real<T> right) => left._value * right._value;
    public static Real<T> operator /(Real<T> left, Real<T> right) => left._value / right._value;

    //
    // Equality
    //

    public static bool operator ==(Real<T> left, Real<T> right) => left._value == right._value;

    public static bool operator !=(Real<T> left, Real<T> right) => left._value != right._value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Real<T> other && Equals(other);

    public bool Equals(Real<T> value) => _value.Equals(value._value);

    public override int GetHashCode() => HashCode.Combine(_value);

    //
    // Comparison
    //

    public static bool operator <(Real<T> left, Real<T> right) => left._value < right._value;
    public static bool operator >(Real<T> left, Real<T> right) => left._value > right._value;
    public static bool operator <=(Real<T> left, Real<T> right) => left._value <= right._value;
    public static bool operator >=(Real<T> left, Real<T> right) => left._value >= right._value;

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is Real<T> value)
        {
            return _value.CompareTo(value._value);
        }

        throw new ArgumentException("Argument is not a real number");
    }

    public int CompareTo(Real<T> value) => _value.CompareTo(value._value);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider) => string.Format(provider, "{0}", _value.ToString(format, provider));

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => _value.TryFormat(destination, out charsWritten, null, provider);

    //
    // Parsing
    //

    public static Real<T> Parse(string s, IFormatProvider? provider) => T.Parse(s, provider);

    public static Real<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => T.Parse(s, provider);

    public static Real<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
        => T.Parse(s, style, provider);

    public static Real<T> Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
        => T.Parse(s, style, provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Real<T> result)
    {
        var succeeded = T.TryParse(s, provider, out T? number);
        result = number!;
        return succeeded;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Real<T> result)
    {
        var succeeded = T.TryParse(s, provider, out T? number);
        result = number!;
        return succeeded;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Real<T> result)
    {
        if (s is null)
        {
            result = T.Zero;
            return false;
        }

        var succeeded = T.TryParse((ReadOnlySpan<char>)s, style, NumberFormatInfo.GetInstance(provider), out T? number);
        result = number!;
        return succeeded;
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Real<T> result)
    {
        if (s.IsEmpty)
        {
            result = T.Zero;
            return false;
        }

        var succeeded = T.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out T? number);
        result = number!;
        return succeeded;
    }

    //
    // Methods
    //

    public static Real<T> Conjugate(Real<T> x) => x;

    public static bool IsFinite(Real<T> x) => T.IsFinite(x._value);

    public static bool IsInfinity(Real<T> x) => T.IsInfinity(x._value);

    public static bool IsNaN(Real<T> x) => T.IsNaN(x._value);

    public static Real<T> Reciprocate(Real<T> x)
    {
        if (x._value == T.Zero)
        {
            return PositiveInfinity;
        }
        return T.One / x;
    }

    //
    // Implicit operators
    //

    public static implicit operator Real<T>(T x) => new(x);
}
