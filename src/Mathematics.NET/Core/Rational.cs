// <copyright file="Rational.cs" company="Mathematics.NET">
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
using System.Runtime.InteropServices;

namespace Mathematics.NET.Core;

/// <summary>Represents a rational number.</summary>
/// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
[Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Rational<T> : IRational<Rational<T>, T>
    where T : IBinaryInteger<T>, ISignedNumber<T>
{
    public static readonly Rational<T> Zero = T.Zero;
    public static readonly Rational<T> One = T.One;
    public static readonly Rational<T> NegativeOne = T.NegativeOne;

    public static readonly Rational<T> MaxValue = T.CreateSaturating(double.MaxValue);
    public static readonly Rational<T> MinValue = T.CreateSaturating(double.MinValue);
    public static readonly Rational<T> Epsilon = new(T.One, T.CreateSaturating(double.MaxValue));

    public static readonly Rational<T> NaN = new(T.Zero, T.Zero);
    public static readonly Rational<T> NegativeInfinity = new(-T.One, T.Zero);
    public static readonly Rational<T> PositiveInfinity = new(T.One, T.Zero);

    private readonly T _numerator;
    private readonly T _denominator;

    public Rational()
    {
        _numerator = T.Zero;
        _denominator = T.One;
    }

    public Rational(T p)
    {
        _numerator = p;
        _denominator = T.One;
    }

    /// <summary>This construct prevents negative numbers from being in the denominator; some methods, such as the ones inherited from <see cref="IComparable"/>, have been written to rely on this fact.</summary>
    /// <param name="p">The numerator.</param>
    /// <param name="q">The denominator.</param>
    public Rational(T p, T q)
    {
        if (q != T.Zero)
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
        else
        {
            if (p == T.Zero)
                _numerator = p;
            else if (p > T.Zero)
                _numerator = T.One;
            else
                _numerator = -T.One;
            _denominator = q;
        }
    }

    //
    // Rational Number Properties
    //

    public T Num => _numerator;
    public T Den => _denominator;

    public Real Re => ToReal(this);

    //
    // Constants
    //

    static Rational<T> IComplex<Rational<T>>.Zero => Zero;
    static Rational<T> IComplex<Rational<T>>.One => One;
    static Rational<T> IComplex<Rational<T>>.NaN => NaN;
    static int IComplex<Rational<T>>.Radix => 2;
    static Rational<T> IMinMaxValue<Rational<T>>.MaxValue => MaxValue;
    static Rational<T> IMinMaxValue<Rational<T>>.MinValue => MinValue;
    static Rational<T> IReal<Rational<T>>.Epsilon => Epsilon;

    //
    // Operators
    //

    public static Rational<T> operator +(Rational<T> x) => x;

    public static Rational<T> operator -(Rational<T> x) => new(-x._numerator, x._denominator);

    public static Rational<T> operator --(Rational<T> x) => Reduce(new(x._numerator - x._denominator, x._denominator));

    public static Rational<T> operator ++(Rational<T> x) => Reduce(new(x._numerator + x._denominator, x._denominator));

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
            return NaN;

        var num = x._numerator * y._denominator;
        var den = x._denominator * y._numerator;
        var gcd = GCD(num, den);
        return new(num / gcd, den / gcd);
    }

    public static Rational<T> operator %(Rational<T> x, Rational<T> y)
    {
        if (y._denominator == T.Zero)
            return NaN;

        var q = x / y;
        return T.DivRem(q._numerator, q._denominator).Remainder;
    }

    //
    // Equality
    //

    public static bool operator ==(Rational<T> left, Rational<T> right)
        => left._numerator == right._numerator && left._denominator == right._denominator;

    public static bool operator !=(Rational<T> left, Rational<T> right)
        => left._numerator != right._numerator || left._denominator != right._denominator;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Rational<T> other && Equals(other);

    public bool Equals(Rational<T> value)
        => _numerator.Equals(value._numerator) && _denominator.Equals(value._denominator);

    public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

    //
    // Comparison
    //

    public static bool operator <(Rational<T> x, Rational<T> y) => x._numerator * y._denominator < y._numerator * x._denominator;

    public static bool operator >(Rational<T> x, Rational<T> y) => x._numerator * y._denominator > y._numerator * x._denominator;

    public static bool operator <=(Rational<T> x, Rational<T> y) => x._numerator * y._denominator <= y._numerator * x._denominator;

    public static bool operator >=(Rational<T> x, Rational<T> y) => x._numerator * y._denominator >= y._numerator * x._denominator;

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return 1;
        if (obj is Rational<T> other)
            return CompareTo(other);
        throw new ArgumentException("The argument is not a rational number.");
    }

    public int CompareTo(Rational<T> value)
    {
        if (this < value)
            return -1;
        else if (this > value)
            return 1;
        else
            return 0;
    }

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider)
    {
        if (IsNaN(this))
            return "NaN";
        if (IsPositiveInfinity(this))
            return "∞";
        if (IsNegativeInfinity(this))
            return "-∞";

        format = string.IsNullOrEmpty(format) ? string.Empty : format.ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "N")
            return _numerator.ToString(null, provider);
        else if (format is "D")
            return _denominator.ToString(null, provider);
        else
            return $"({_numerator.ToString(null, provider)}, {_denominator.ToString(null, provider)})";
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

        format = format.IsEmpty ? string.Empty : format.ToString().ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        var charsCurrentlyWritten = 0;
        if (format is "N")
        {
            if (destination.Length < 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            var tryFormatSucceeded = _numerator.TryFormat(destination, out charsCurrentlyWritten, null, provider);
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = charsCurrentlyWritten;
            return true;
        }
        else if (format is "D")
        {
            if (destination.Length < 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            var tryFormatSucceeded = _denominator.TryFormat(destination, out charsCurrentlyWritten, null, provider);
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = charsCurrentlyWritten;
            return true;
        }
        else
        {
            // There are a minimum of 7 characters for "(0 / 0)".
            if (destination.Length < 7)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = '(';

            bool tryFormatSucceeded = _numerator.TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
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

            tryFormatSucceeded = _denominator.TryFormat(destination[charsCurrentlyWritten..], out tryFormatCharsWritten, null, provider);
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
            return NaN;
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
#pragma warning disable EPS06
        s = s.Trim();
        int openParenthesis = s.IndexOf('(');
        int split = s.IndexOf('/');
        int closeParenthesis = s.IndexOf(')');
#pragma warning restore EPS06

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

    public static Rational<T> Abs(Rational<T> x) => new(T.Abs(x._numerator), x._denominator);

    public static Rational<T> Ceiling(Rational<T> x)
    {
        var (quotient, remainder) = T.DivRem(x._numerator, x._denominator);
        return remainder == T.Zero ? quotient : quotient + T.One;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rational<T> Clamp(Rational<T> value, Rational<T> min, Rational<T> max)
    {
        if (min > max)
            throw new ArgumentException("The minimum value must be less than or equal to maximum value.");

        if (value < min)
            return min;
        else if (value > max)
            return max;

        return value;
    }

    public static Rational<T> Conjugate(Rational<T> x) => x;

    public static (T Quotient, Rational<T> Remainder) QuoRem(Rational<T> dividend, Rational<T> divisor)
        => ToMixed(dividend / divisor);

    public static Rational<T> Floor(Rational<T> x) => T.DivRem(x._numerator, x._denominator).Quotient;

    /// <summary>Create an instance of type rational of <typeparamref name="T"/> from one of type <see cref="double"/>.</summary>
    /// <param name="x">A value of type <see cref="double"/>.</param>
    /// <returns>An instance of type rational of <typeparamref name="T"/> created from <paramref name="x"/>.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="x"/> cannot be represented as a rational of <typeparamref name="T"/>.</exception>
    public static Rational<T> FromDouble(double x)
    {
        if (double.IsNaN(x))
            return NaN;
        if (double.IsPositiveInfinity(x))
            return PositiveInfinity;
        if (double.IsNegativeInfinity(x))
            return NegativeInfinity;

        checked
        {
            var n = 0;

            // We are comparing floats here, but it is okay in just this case.
            while (x != Math.Floor(x))
            {
                x *= 10.0;
                n++;
            }

            T num = T.CreateChecked(x);
            T den = T.CreateChecked(Math.Pow(10.0, n));
            T gcd = GCD(num, den);

            return new(num / gcd, den / gcd);
        }
    }

    public static Rational<T> FromReal(Real x) => FromDouble(x.AsDouble());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GCD(T p, T q)
    {
        p = T.Abs(p);
        q = T.Abs(q);
        while (p != T.Zero && q != T.Zero)
        {
            if (p > q)
                p %= q;
            else
                q %= p;
        }
        return p | q;
    }

    public static Rational<T> InverseLerp(Rational<T> start, Rational<T> end, Rational<T> weight) => (One - weight) * end + weight * start;

    // T.IsPositive will return false for -0 so we check again using T.Zero; we accept a zero in the denominator for Rational<T>.NaN;
    public static bool IsCanonical(Rational<T> x) => T.IsPositive(x._denominator) || T.IsZero(x._denominator);

    public static bool IsComplex(Rational<T> x) => false;

    public static bool IsFinite(Rational<T> x) => !T.IsZero(x._denominator);

    public static bool IsImaginary(Rational<T> x) => false;

    public static bool IsInfinity(Rational<T> x) => T.IsZero(x._denominator);

    public static bool IsInteger(Rational<T> x) => x._denominator == T.One;

    public static bool IsNaN(Rational<T> x) => T.IsZero(x._numerator) && T.IsZero(x._denominator);

    public static bool IsReal(Rational<T> x) => true;

    public static bool IsZero(Rational<T> x) => T.IsZero(x._numerator) && x._denominator == T.One;

    public static bool IsNegative(Rational<T> x) => T.IsNegative(x._numerator);

    public static bool IsNegativeInfinity(Rational<T> x) => x._numerator == -T.One && T.IsZero(x._denominator);

    public static bool IsPositive(Rational<T> x) => T.IsPositive(x._numerator);

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
                p %= q;
            else
                q %= p;
        }
        return holdP / (p | q) * holdQ;
    }

    public static Rational<T> Lerp(Rational<T> start, Rational<T> end, Rational<T> weight) => (One - weight) * start + weight * end;

    public static Rational<T> Max(Rational<T> x, Rational<T> y)
    {
        var u = x.Reduce();
        var v = y.Reduce();

        return u._numerator * v._denominator >= v._numerator * u._denominator ? x : y;
    }

    public static Rational<T> MaxMagnitude(Rational<T> x, Rational<T> y) => x > y || IsNaN(x) ? x : y;

    static Rational<T> IComplex<Rational<T>>.MaxMagnitudeNumber(Rational<T> x, Rational<T> y) => x > y || IsNaN(y) ? x : y;

    public static Rational<T> Min(Rational<T> x, Rational<T> y)
    {
        var u = x.Reduce();
        var v = y.Reduce();

        return u._numerator * v._denominator <= v._numerator * u._denominator ? x : y;
    }

    public static Rational<T> MinMagnitude(Rational<T> x, Rational<T> y) => x < y || IsNaN(x) ? x : y;

    static Rational<T> IComplex<Rational<T>>.MinMagnitudeNumber(Rational<T> x, Rational<T> y) => x < y || IsNaN(y) ? x : y;

    public static Rational<T> Reciprocate(Rational<T> x)
    {
        if (x._numerator == T.Zero)
            return NaN;
        return new(x._denominator, x._numerator);
    }

    public static Rational<T> Reduce(Rational<T> x)
    {
        var gcd = GCD(x._numerator, x._denominator);
        if (gcd == T.One)
            return x;
        return new(x._numerator / gcd, x._denominator / gcd);
    }

    public static int Sign(Rational<T> x)
    {
        if (x == Zero)
            return 0;
        return x._numerator > T.Zero ? 1 : -1;
    }

    public static Rational<T> ToImproper(T quotient, Rational<T> remainder) => quotient + remainder;

    public static (T Quotient, Rational<T> Remainder) ToMixed(Rational<T> x)
    {
        var (quotient, remainder) = T.DivRem(x._numerator, x._denominator);
        return new(quotient, new(remainder, x._denominator));
    }

    public static Real ToReal(Rational<T> x)
        => double.CreateChecked(x._numerator) / double.CreateChecked(x._denominator);

    public static bool TryConvertFromChecked<U>(U value, out Rational<T> result)
        where U : INumberBase<U>
    {
        if (Real.TryConvertFromChecked(value, out var intermediateResult))
        {
            result = FromReal(intermediateResult);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    /// <summary>Try to convert a <see cref="double"/> into a rational of <see cref="double"/> to within a certain <paramref name="tolerance"/> and with a <paramref name="max"/> denominator size.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="tolerance">A tolerance.</param>
    /// <param name="max">The max denominator size.</param>
    /// <param name="result">The result of the conversion if successful; otherwise <see cref="NaN"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; otherwise <see langword="false"/>.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="value"/> cannot be represented as a rational of <typeparamref name="T"/>.</exception>
    public static bool TryConvertFromDouble(double value, double tolerance, T max, out Rational<T> result)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            result = NaN;
            return false;
        }
        if (tolerance < Precision.DblEpsilonVariant)
            tolerance = Precision.DblEpsilonVariant;

        T num = T.Zero;
        T den = T.One;

        var floor = Math.Floor(value);
        T quotient = T.CreateChecked(floor);
        var remainder = value - floor;

        while (Math.Abs(double.CreateChecked(num) / double.CreateChecked(den) - remainder) > tolerance)
        {
            if (den > max)
            {
                result = NaN;
                return false;
            }
            if (num == den)
            {
                num = T.Zero;
                den++;
            }
            num++;
        }

        // There is no need to reduce our fraction since if it were simpler, it would have been returned already.
        result = new(num + quotient * den, den);
        return true;
    }

    public static bool TryConvertFromSaturating<U>(U value, out Rational<T> result)
        where U : INumberBase<U>
    {
        if (Real.TryConvertFromSaturating(value, out var intermediateResult))
        {
            result = FromReal(intermediateResult);
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
            result = FromReal(intermediateResult);
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
        value = Rational<T>.Reduce(value);
        result = checked(U.CreateChecked(value._numerator) / U.CreateChecked(value._denominator));
        return true;
    }

    public static bool TryConvertToSaturating<U>(Rational<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        value = Rational<T>.Reduce(value);
        result = checked(U.CreateSaturating(value._numerator) / U.CreateSaturating(value._denominator));
        return true;
    }

    public static bool TryConvertToTruncating<U>(Rational<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        value = Rational<T>.Reduce(value);
        result = checked(U.CreateTruncating(value._numerator) / U.CreateTruncating(value._denominator));
        return true;
    }

    //
    // Implicit Operators
    //

    public static implicit operator Rational<T>(T p) => new(p);
}
