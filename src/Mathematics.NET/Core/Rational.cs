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
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Rational<T> : IRational<Rational<T>, T>
    where T : IBinaryInteger<T>
{
    public static readonly Rational<T> Zero = T.Zero;
    public static readonly Rational<T> One = T.One;

    public static readonly Rational<T> MaxValue = T.CreateSaturating(double.MaxValue);
    public static readonly Rational<T> MinValue = T.CreateSaturating(double.MinValue);

    public static readonly Rational<T> NaN = new(T.Zero, T.Zero);
    public static readonly Rational<T> NegativeInfinity = new(-T.One, T.Zero);
    public static readonly Rational<T> PositiveInfinity = new(T.One, T.Zero);

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
            _denominator = T.One;
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

    public Real Re => (Real)this;
    public double Value => (double)this;

    //
    // Constants
    //

    static Rational<T> IComplex<Rational<T>>.Zero => Zero;
    static Rational<T> IComplex<Rational<T>>.One => One;
    static Rational<T> IComplex<Rational<T>>.NaN => NaN;
    static Rational<T> IMinMaxValue<Rational<T>>.MaxValue => MaxValue;
    static Rational<T> IMinMaxValue<Rational<T>>.MinValue => MinValue;

    //
    // Operators
    //

    public static Rational<T> operator +(Rational<T> x) => x;

    public static Rational<T> operator -(Rational<T> x) => x + One;

    public static Rational<T> operator --(Rational<T> x) => new(x._numerator, x._denominator);

    public static Rational<T> operator ++(Rational<T> x) => new(x._numerator, x._denominator);

    public static Rational<T> operator +(Rational<T> x, Rational<T> y)
    {
        var lcm = LCM(x._denominator, y._denominator);
        var num = lcm / x._denominator * x._numerator + lcm / y._denominator * y._numerator;
        var gcd = GCD(num, lcm);
        return new(num / gcd, lcm / gcd);
    }

    public static Rational<T> operator -(Rational<T> x, Rational<T> y)
    {
        var lcm = LCM(x._denominator, y._denominator);
        var num = lcm / x._denominator * x._numerator - lcm / y._denominator * y._numerator;
        var gcd = GCD(num, lcm);
        return new(num / gcd, lcm / gcd);
    }

    public static Rational<T> operator *(Rational<T> x, Rational<T> y)
    {
        var num = x._numerator * y._numerator;
        var den = x._denominator * y._denominator;
        var gcd = GCD(num, den);
        return new(num / gcd, den / gcd);
    }

    public static Rational<T> operator /(Rational<T> x, Rational<T> y)
    {
        if (y._numerator == T.Zero)
        {
            return NaN;
        }

        var num = x._numerator * y._denominator;
        var den = x._denominator * y._numerator;
        var gcd = GCD(num, den);
        return new(num / gcd, den / gcd);
    }

    public static Rational<T> operator %(Rational<T> x, Rational<T> y)
    {
        if (y._denominator == T.Zero)
        {
            return NaN;
        }

        var q = x / y;
        return T.DivRem(q._numerator, q._denominator).Remainder;
    }

    //
    // Equality
    //

    public static bool operator ==(Rational<T> left, Rational<T> right)
    {
        return left._numerator == right._numerator && left._denominator == right._denominator;
    }

    public static bool operator !=(Rational<T> left, Rational<T> right)
    {
        return left._numerator != right._numerator || left._denominator != right._denominator;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Rational<T> other && Equals(other);

    public bool Equals(Rational<T> value)
    {
        return _numerator.Equals(value._numerator) && _denominator.Equals(value._denominator);
    }

    public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

    //
    // Comparison
    //

    public static bool operator <(Rational<T> x, Rational<T> y)
    {
        return x._numerator * y._denominator < y._numerator * x._denominator;
    }

    public static bool operator >(Rational<T> x, Rational<T> y)
    {
        return x._numerator * y._denominator > y._numerator * x._denominator;
    }

    public static bool operator <=(Rational<T> x, Rational<T> y)
    {
        return x._numerator * y._denominator <= y._numerator * x._denominator;
    }

    public static bool operator >=(Rational<T> x, Rational<T> y)
    {
        return x._numerator * y._denominator >= y._numerator * x._denominator;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is Rational<T> other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Argument is not a rational number");
    }

    public int CompareTo(Rational<T> value)
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
        if (IsNaN(this))
        {
            return "NaN";
        }

        if (IsPositiveInfinity(this))
        {
            return "∞";
        }

        if (IsNegativeInfinity(this))
        {
            return "-∞";
        }

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
        if (IsNaN(this))
        {
            if (destination.Length < 3)
            {
                charsWritten = 0;
                return false;
            }

            "NaN".CopyTo(destination);
            charsWritten = 3;
            return true;
        }

        if (IsPositiveInfinity(this))
        {
            if (destination.Length < 1)
            {
                charsWritten = 0;
                return false;
            }

            destination[0] = '∞';
            charsWritten = 1;
            return true;
        }

        if (IsNegativeInfinity(this))
        {
            if (destination.Length < 2)
            {
                charsWritten = 0;
                return false;
            }

            "-∞".CopyTo(destination);
            charsWritten = 2;
            return true;
        }

        format = format.IsEmpty ? "MINIMAL" : format.ToString().ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "MINIMAL")
        {
            int charsCurrentlyWritten = 0;

            bool tryFormatSucceeded;

            if (_numerator == T.Zero)
            {
                if (destination.Length < 1)
                {
                    charsWritten = charsCurrentlyWritten;
                    return false;
                }

                destination[0] = '0';
                charsWritten = 1;
                return true;
            }

            if (_denominator == T.One)
            {
                if (destination.Length < 1)
                {
                    charsWritten = charsCurrentlyWritten;
                    return false;
                }

                tryFormatSucceeded = _numerator.TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
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

    public static Rational<T> Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Rational<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Rational<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse((ReadOnlySpan<char>)s, style, provider);
    }

    public static Rational<T> Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (!TryParse(s, style, provider, out Rational<T> result))
        {
            return NaN;
        }
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Rational<T> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rational<T> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Rational<T> result)
        => TryParse((ReadOnlySpan<char>)s, style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Rational<T> result)
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

    public static Rational<T> Abs(Rational<T> x) => new(T.Abs(x._numerator), T.Abs(x._denominator));

    public static Real Atan2(Rational<T> y, Rational<T> x) => Real.Atan2(ToReal(y), ToReal(x));

    public static Rational<T> Ceiling(Rational<T> x)
    {
        var (quotient, remainder) = T.DivRem(x._numerator, x._denominator);
        return remainder == T.Zero ? quotient : quotient + T.One;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rational<T> Clamp(Rational<T> value, Rational<T> min, Rational<T> max)
    {
        if (min > max)
        {
            throw new ArgumentException("Minimum value must be less than or equal to maximum value");
        }

        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }

        return value;
    }

    public static Rational<T> Conjugate(Rational<T> x) => x;

    public static Rational<T> Floor(Rational<T> x) => T.DivRem(x._numerator, x._denominator).Quotient;

    public static Rational<T> FromDouble(double x) => (Rational<T>)x;

    public static Rational<T> FromReal(Real x) => (Rational<T>)x;

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

    public static Rational<T> InverseLerp(Rational<T> start, Rational<T> end, Rational<T> weight) => end - (end - start) * weight;

    public static bool IsFinite(Rational<T> x) => !T.IsZero(x._denominator);

    public static bool IsInfinity(Rational<T> x) => T.IsZero(x._denominator);

    public static bool IsNaN(Rational<T> x) => T.IsZero(x._numerator) && T.IsZero(x._denominator);

    public static bool IsZero(Rational<T> x) => T.IsZero(x._numerator) && x._denominator == T.One;

    public static bool IsNegativeInfinity(Rational<T> x) => x._numerator == -T.One && T.IsZero(x._denominator);

    public static bool IsPositiveInfinity(Rational<T> x) => x._numerator == T.One && T.IsZero(x._denominator);

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

    public static Rational<T> Lerp(Rational<T> start, Rational<T> end, Rational<T> weight) => start + (end - start) * weight;

    // TODO: Find a better implementation for Max and Min
    public static Rational<T> Max(Rational<T> x, Rational<T> y)
    {
        var u = x.Reduce();
        var v = y.Reduce();

        return u._numerator * v._denominator >= v._numerator * u._denominator ? x : y;
    }

    public static Rational<T> Min(Rational<T> x, Rational<T> y)
    {
        var u = x.Reduce();
        var v = y.Reduce();

        return u._numerator * v._denominator <= v._numerator * u._denominator ? x : y;
    }

    public static Rational<T> Reciprocate(Rational<T> x)
    {
        if (x._numerator == T.Zero)
        {
            return NaN;
        }
        return new(x._denominator, x._numerator);
    }

    public static Rational<T> Reduce(Rational<T> x)
    {
        var gcd = GCD(x._numerator, x._denominator);
        if (gcd == T.One)
        {
            return x;
        }
        return new(x._numerator / gcd, x._denominator / gcd);
    }

    public static int Sign(Rational<T> x)
    {
        if (x == Rational<T>.Zero)
        {
            return 0;
        }
        return x._numerator > T.Zero ? 1 : -1;
    }

    public static Real ToReal(Rational<T> x)
        => checked(double.CreateChecked(x._numerator) / double.CreateChecked(x._denominator));

    public static bool TryConvertFromChecked<U>(U value, out Rational<T> result)
        where U : INumberBase<U>
    {
        if (Real.TryConvertFromChecked(value, out var intermediateResult))
        {
            result = (Rational<T>)intermediateResult;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromSaturating<U>(U value, out Rational<T> result)
        where U : INumberBase<U>
    {
        if (Real.TryConvertFromSaturating(value, out var intermediateResult))
        {
            result = (Rational<T>)intermediateResult;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromTruncating<U>(U value, out Rational<T> result)
        where U : INumberBase<U>
    {
        if (Real.TryConvertFromTruncating(value, out var intermediateResult))
        {
            result = (Rational<T>)intermediateResult;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertToChecked<U>(Rational<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateChecked(checked(double.CreateChecked(value._numerator) / double.CreateChecked(value._denominator)));
        return true;
    }

    public static bool TryConvertToSaturating<U>(Rational<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateSaturating(double.CreateSaturating(value._numerator) / double.CreateSaturating(value._denominator));
        return true;
    }

    public static bool TryConvertToTruncating<U>(Rational<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateTruncating(double.CreateTruncating(value._numerator) / double.CreateTruncating(value._denominator));
        return true;
    }

    //
    // Implicit operators
    //

    public static implicit operator Rational<T>(T p) => new(p);

    //
    // Explicit operators
    //

    // TODO: Find a better implementation
    public static explicit operator Rational<T>(double x)
    {
        var value = x;
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return NaN;
        }

        var n = 0.0;
        while (x != double.Floor(value))
        {
            x *= 10.0;
            n++;
        }

        T num = T.CreateChecked(value);
        T den = T.CreateChecked(Math.Pow(10.0, n));
        var gcd = GCD(num, den);

        return new(num / gcd, den / gcd);
    }

    public static explicit operator Rational<T>(Real x) => (Rational<T>)x.Value;

    public static explicit operator checked Real(Rational<T> x) => checked(double.CreateChecked(x._numerator) / double.CreateChecked(x._denominator));

    public static explicit operator Real(Rational<T> x) => ToReal(x);

    public static explicit operator checked double(Rational<T> x) => double.CreateChecked(x._numerator) / double.CreateChecked(x._denominator);

    public static explicit operator double(Rational<T> x) => double.CreateSaturating(x._numerator) / double.CreateSaturating(x._denominator);
}
