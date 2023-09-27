// <copyright file="Rational.cs" company="Mathematics.NET">
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
using System.Runtime.InteropServices;

namespace Mathematics.NET.Core;

/// <summary>Represents a rational number</summary>
/// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/></typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Rational<T, U> : IRational<Rational<T, U>, T, U>
    where T : IBinaryInteger<T>
    where U : IFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private static U s_ten = U.Exp10(U.One);

    public static readonly Rational<T, U> Zero = T.Zero;
    public static readonly Rational<T, U> One = T.One;
    public static readonly Rational<T, U> Two = T.One + T.One;

    public static readonly Rational<T, U> MaxValue = T.CreateSaturating(U.MaxValue);
    public static readonly Rational<T, U> MinValue = T.CreateSaturating(U.MinValue);

    public static readonly Rational<T, U> NaN = new(T.Zero, T.Zero);
    public static readonly Rational<T, U> NegativeInfinity = new(-T.One, T.Zero);
    public static readonly Rational<T, U> PositiveInfinity = new(T.One, T.Zero);

    private readonly T _numerator;
    private readonly T _denominator;

    public Rational(T p)
    {
        _numerator = p;
        _denominator = T.One;
    }

    /// <summary>This construct prevents negative numbers from being in the denominator; some methods, such as the ones inherited from <see cref="IComparable"/>, have been written to rely on this fact.</summary>
    /// <param name="p">The numerator</param>
    /// <param name="q">The denominator</param>
    public Rational(T p, T q)
    {
        if (q == T.Zero)
        {
            _numerator = T.Zero;
            _denominator = T.Zero;
        }
        else
        {
            if (q > T.Zero)
            {
                _numerator = p;
                _denominator = q;
            }
            else
            {
                _numerator = -p;
                _denominator = -q;
            }
        }
    }

    //
    // Rational number properties
    //

    public T Num => _numerator;
    public T Den => _denominator;

    public Real<U> Re => (U)this;
    public U Value => (U)this;

    //
    // Constants
    //

    static Rational<T, U> IComplex<Rational<T, U>, U>.Zero => Zero;
    static Rational<T, U> IComplex<Rational<T, U>, U>.One => One;
    static Rational<T, U> IComplex<Rational<T, U>, U>.Two => Zero;
    static Rational<T, U> IComplex<Rational<T, U>, U>.NaN => NaN;
    static Rational<T, U> IMinMaxValue<Rational<T, U>>.MaxValue => MaxValue;
    static Rational<T, U> IMinMaxValue<Rational<T, U>>.MinValue => MinValue;

    //
    // Operators
    //

    public static Rational<T, U> operator -(Rational<T, U> x) => x + One;

    public static Rational<T, U> operator --(Rational<T, U> x) => new(x._numerator, x._denominator);

    public static Rational<T, U> operator ++(Rational<T, U> x) => new(x._numerator, x._denominator);

    public static Rational<T, U> operator +(Rational<T, U> x, Rational<T, U> y)
    {
        var lcm = LCM(x._denominator, y._denominator);
        var num = lcm / x._denominator * x._numerator + lcm / y._denominator * y._numerator;
        var gcd = GCD(num, lcm);
        return new(num / gcd, lcm / gcd);
    }

    public static Rational<T, U> operator -(Rational<T, U> x, Rational<T, U> y)
    {
        var lcm = LCM(x._denominator, y._denominator);
        var num = lcm / x._denominator * x._numerator - lcm / y._denominator * y._numerator;
        var gcd = GCD(num, lcm);
        return new(num / gcd, lcm / gcd);
    }

    public static Rational<T, U> operator *(Rational<T, U> x, Rational<T, U> y)
    {
        var num = x._numerator * y._numerator;
        var den = x._denominator * y._denominator;
        var gcd = GCD(num, den);
        return new(num / gcd, den / gcd);
    }

    public static Rational<T, U> operator /(Rational<T, U> x, Rational<T, U> y)
    {
        if (y._denominator == T.Zero)
        {
            return NaN;
        }

        var num = x._numerator * y._denominator;
        var den = x._denominator * y._numerator;
        var gcd = GCD(num, den);
        return new(num / gcd, den / gcd);
    }

    //
    // Equality
    //

    public static bool operator ==(Rational<T, U> left, Rational<T, U> right)
    {
        return left._numerator == right._numerator && left._denominator == right._denominator;
    }

    public static bool operator !=(Rational<T, U> left, Rational<T, U> right)
    {
        return left._numerator != right._numerator || left._denominator != right._denominator;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Rational<T, U> other && Equals(other);

    public bool Equals(Rational<T, U> value)
    {
        return _numerator.Equals(value._numerator) && _denominator.Equals(value._denominator);
    }

    public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

    //
    // Comparison
    //

    public static bool operator <(Rational<T, U> x, Rational<T, U> y)
    {
        return x._numerator * y._denominator < y._numerator * x._denominator;
    }

    public static bool operator >(Rational<T, U> x, Rational<T, U> y)
    {
        return x._numerator * y._denominator > y._numerator * x._denominator;
    }

    public static bool operator <=(Rational<T, U> x, Rational<T, U> y)
    {
        return x._numerator * y._denominator <= y._numerator * x._denominator;
    }

    public static bool operator >=(Rational<T, U> x, Rational<T, U> y)
    {
        return x._numerator * y._denominator >= y._numerator * x._denominator;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is Rational<T, U> other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Argument is not a rational number");
    }

    public int CompareTo(Rational<T, U> value)
    {
        if (this < value)
        {
            return -1;
        }
        else if (this > value)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? "MINIMAL" : format.ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "MINIMAL")
        {
            if (_numerator == T.Zero || _denominator == T.One)
            {
                return string.Format(provider, "{0}", _numerator.ToString(null, provider));
            }
            return string.Format(provider, "({0} / {1})", _numerator.ToString(null, provider), _denominator.ToString(null, provider));
        }
        else if (format is "ALL")
        {
            return string.Format(provider, "({0} / {1})", _numerator.ToString(null, provider), _denominator.ToString(null, provider));
        }
        else
        {
            throw new FormatException($"The \"{format}\" format is not supported.");
        }
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        format = format.IsEmpty ? "MINIMAL" : format.ToString().ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "MINIMAL")
        {
            int charsCurrentlyWritten = 0;

            bool tryFormatSucceeded;
            int tryFormatCharsWritten;

            if (_numerator == T.Zero)
            {
                if (destination.Length < 1)
                {
                    charsWritten = charsCurrentlyWritten;
                    return false;
                }

                charsWritten = 1;
                destination[0] = '0';
                return true;
            }

            if (_denominator == T.One)
            {
                if (destination.Length < 1)
                {
                    charsWritten = charsCurrentlyWritten;
                    return false;
                }

                tryFormatSucceeded = _numerator.TryFormat(destination[charsCurrentlyWritten..], out tryFormatCharsWritten, null, provider);
                charsCurrentlyWritten += tryFormatCharsWritten;
                if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
                {
                    charsWritten = charsCurrentlyWritten;
                    return false;
                }

                charsWritten = charsCurrentlyWritten;
                return true;
            }

            return TryFormatAllInternal(_numerator, _denominator, destination, out charsWritten, provider);
        }
        else if (format is "ALL")
        {
            return TryFormatAllInternal(_numerator, _denominator, destination, out charsWritten, provider);
        }
        else
        {
            throw new FormatException($"The \"{format}\" format is not supported.");
        }

        static bool TryFormatAllInternal(T num, T den, Span<char> destination, out int charsWritten, IFormatProvider? provider)
        {
            var charsCurrentlyWritten = 0;

            // There are a minimum of 7 characters for "(0 / 0)"
            if (destination.Length < 7)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = '(';

            bool tryFormatSucceeded = num.TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
            charsCurrentlyWritten += tryFormatCharsWritten;
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = ' ';
            if (destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }
            destination[charsCurrentlyWritten++] = '/';
            if (destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }
            destination[charsCurrentlyWritten++] = ' ';
            if (destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            tryFormatSucceeded = den.TryFormat(destination[charsCurrentlyWritten..], out tryFormatCharsWritten, null, provider);
            charsCurrentlyWritten += tryFormatCharsWritten;
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = ')';

            charsWritten = charsCurrentlyWritten;
            return true;
        }
    }

    //
    // Parsing
    //

    public static Rational<T, U> Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Rational<T, U> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Rational<T, U> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse((ReadOnlySpan<char>)s, style, provider);
    }

    public static Rational<T, U> Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (!TryParse(s, style, provider, out Rational<T, U> result))
        {
            return NaN;
        }
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Rational<T, U> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rational<T, U> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Rational<T, U> result)
        => TryParse((ReadOnlySpan<char>)s, style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Rational<T, U> result)
    {
        s = s.Trim();
        int openParenthesis = s.IndexOf('(');
        int split = s.IndexOf('/');
        int closeParenthesis = s.IndexOf(')');

        // There a minimum of 5 characters for "(0/0)".
        if (s.Length < 5 || openParenthesis == -1 || split == -1 || closeParenthesis == -1 || openParenthesis > split || openParenthesis > closeParenthesis || split > closeParenthesis)
        {
            result = Zero;
            return false;
        }

        if (!T.TryParse(s.Slice(openParenthesis + 1, split - 1), style, provider, out T? numerator))
        {
            result = Zero;
            return false;
        }

        if (!T.TryParse(s.Slice(split + 1, closeParenthesis - split - 1), style, provider, out T? denominator))
        {
            result = Zero;
            return false;
        }

        result = new(numerator, denominator);
        return true;
    }

    //
    // Methods
    //

    public static Rational<T, U> Abs(Rational<T, U> x) => new(T.Abs(x._numerator), T.Abs(x._denominator));

    public static Rational<T, U> Conjugate(Rational<T, U> x) => x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GCD(T p, T q)
    {
        p = T.Abs(p);
        q = T.Abs(q);
        while (p != T.Zero && q != T.Zero)
        {
            if (p > q)
            {
                p %= q;
            }
            else
            {
                q %= p;
            }
        }
        return p | q;
    }

    public static bool IsFinite(Rational<T, U> x) => !T.IsZero(x._denominator);

    public static bool IsInfinity(Rational<T, U> x) => T.IsZero(x._denominator);

    public static bool IsNaN(Rational<T, U> x) => T.IsZero(x._numerator) && T.IsZero(x._denominator);

    public static bool IsZero(Rational<T, U> x) => T.IsZero(x._numerator);

    public static bool IsNegativeInfinity(Rational<T, U> x) => x._numerator == -T.One && T.IsZero(x._denominator);

    public static bool IsPositiveInfinity(Rational<T, U> x) => x._numerator == T.One && T.IsZero(x._denominator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T LCM(T p, T q)
    {
        p = T.Abs(p);
        q = T.Abs(q);
        T holdP = p;
        T holdQ = q;
        while (p != T.Zero && q != T.Zero)
        {
            if (p > q)
            {
                p %= q;
            }
            else
            {
                q %= p;
            }
        }
        return holdP / (p | q) * holdQ;
    }

    public static Rational<T, U> Reciprocate(Rational<T, U> x)
    {
        if (x._numerator == T.Zero)
        {
            return NaN;
        }
        return new(x._denominator, x._numerator);
    }

    public static Rational<T, U> Reduce(Rational<T, U> x)
    {
        var gcd = GCD(x._numerator, x._denominator);
        if (gcd == T.One)
        {
            return x;
        }
        return new(x._numerator / gcd, x._denominator / gcd);
    }

    public static bool TryConvertFromChecked<V>(V value, out Rational<T, U> result)
        where V : INumberBase<V>
    {
        if (V.IsInteger(value))
        {
            result = T.CreateChecked(value);
            return true;
        }
        else if (value is IFloatingPointIeee754<U> floatingPointNumber)
        {
            result = (Rational<T, U>)(U)floatingPointNumber;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromSaturating<V>(V value, out Rational<T, U> result)
        where V : INumberBase<V>
    {
        if (V.IsInteger(value))
        {
            result = T.CreateSaturating(value);
            return true;
        }
        else if (value is IFloatingPointIeee754<U> floatingPointNumber)
        {
            result = (Rational<T, U>)(U)floatingPointNumber;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromTruncating<V>(V value, out Rational<T, U> result)
        where V : INumberBase<V>
    {
        if (V.IsInteger(value))
        {
            result = T.CreateTruncating(value);
            return true;
        }
        else if (value is IFloatingPointIeee754<U> floatingPointNumber)
        {
            result = (Rational<T, U>)(U)floatingPointNumber;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertToChecked<V>(Rational<T, U> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateChecked(checked((U)value));
        return true;
    }

    public static bool TryConvertToSaturating<V>(Rational<T, U> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateSaturating(U.CreateSaturating(value._numerator) / U.CreateSaturating(value._denominator));
        return true;
    }

    public static bool TryConvertToTruncating<V>(Rational<T, U> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateTruncating(U.CreateTruncating(value._numerator) / U.CreateTruncating(value._denominator));
        return true;
    }

    //
    // Implicit operators
    //

    public static implicit operator Rational<T, U>(T p) => new(p);

    //
    // Explicit operators
    //

    // TODO: Find a better implementation
    public static explicit operator Rational<T, U>(U x)
    {
        if (U.IsNaN(x) || U.IsInfinity(x))
        {
            return NaN;
        }

        var n = U.Zero;
        while (x != U.Floor(x))
        {
            x *= s_ten;
            n++;
        }

        T num = T.CreateChecked(x);
        T den = T.CreateChecked(U.Pow(s_ten, n));
        var gcd = GCD(num, den);

        return new(num / gcd, den / gcd);
    }

    public static explicit operator checked U(Rational<T, U> x) => checked(U.CreateChecked(x._numerator) / U.CreateChecked(x._denominator));

    public static explicit operator U(Rational<T, U> x) => U.CreateChecked(x._numerator) / U.CreateChecked(x._denominator);
}
