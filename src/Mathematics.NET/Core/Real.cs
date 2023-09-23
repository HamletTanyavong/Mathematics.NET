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
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/></typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Real<T>
    : IReal<Real<T>, T>,
      IConstants<Real<T>, T>,
      IDifferentiableFunctions<Real<T>, T>
    where T : IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    public static readonly Real<T> Zero = T.Zero;
    public static readonly Real<T> One = T.One;
    public static readonly Real<T> Two = T.One + T.One;

    public static readonly Real<T> MaxValue = T.MaxValue;
    public static readonly Real<T> MinValue = T.MinValue;

    public static readonly Real<T> NaN = T.NaN;
    public static readonly Real<T> NegativeInfinity = T.NegativeInfinity;
    public static readonly Real<T> PositiveInfinity = T.PositiveInfinity;

    /// <inheritdoc cref="Constants.E" />
    public static readonly Real<T> E = T.CreateTruncating(Constants.E);

    /// <inheritdoc cref="Constants.Pi" />
    public static readonly Real<T> Pi = T.CreateTruncating(Constants.Pi);

    /// <inheritdoc cref="Constants.PiOverTwo" />
    public static readonly Real<T> PiOverTwo = T.CreateTruncating(Constants.PiOverTwo);

    /// <inheritdoc cref="Constants.PiSquared" />
    public static readonly Real<T> PiSquared = T.CreateTruncating(Constants.PiSquared);

    /// <inheritdoc cref="Constants.Tau" />
    public static readonly Real<T> Tau = T.CreateTruncating(Constants.Tau);

    /// <inheritdoc cref="Constants.EulerMascheroni" />
    public static readonly Real<T> EulerMascheroni = T.CreateTruncating(Constants.EulerMascheroni);

    /// <inheritdoc cref="Constants.GoldenRatio" />
    public static readonly Real<T> GoldenRatio = T.CreateTruncating(Constants.GoldenRatio);

    /// <inheritdoc cref="Constants.Ln2" />
    public static readonly Real<T> Ln2 = T.CreateTruncating(Constants.Ln2);

    /// <inheritdoc cref="Constants.Ln10" />
    public static readonly Real<T> Ln10 = T.CreateTruncating(Constants.Ln10);

    /// <inheritdoc cref="Constants.Sqrt2" />
    public static readonly Real<T> Sqrt2 = T.CreateTruncating(Constants.Sqrt2);

    /// <inheritdoc cref="Constants.Sqrt3" />
    public static readonly Real<T> Sqrt3 = T.CreateTruncating(Constants.Sqrt3);

    /// <inheritdoc cref="Constants.Sqrt5" />
    public static readonly Real<T> Sqrt5 = T.CreateTruncating(Constants.Sqrt5);

    /// <inheritdoc cref="Constants.ZetaOf2" />
    public static readonly Real<T> ZetaOf2 = T.CreateTruncating(Constants.ZetaOf2);

    /// <inheritdoc cref="Constants.ZetaOf3" />
    public static readonly Real<T> ZetaOf3 = T.CreateTruncating(Constants.ZetaOf3);

    /// <inheritdoc cref="Constants.ZetaOf4" />
    public static readonly Real<T> ZetaOf4 = T.CreateTruncating(Constants.ZetaOf4);

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
    static Real<T> IComplex<Real<T>, T>.Two => Two;
    static Real<T> IComplex<Real<T>, T>.NaN => NaN;
    static Real<T> IMinMaxValue<Real<T>>.MaxValue => MaxValue;
    static Real<T> IMinMaxValue<Real<T>>.MinValue => MinValue;

    // IConstants interface

    static Real<T> IConstants<Real<T>, T>.E => E;
    static Real<T> IConstants<Real<T>, T>.Pi => Pi;
    static Real<T> IConstants<Real<T>, T>.PiOverTwo => PiOverTwo;
    static Real<T> IConstants<Real<T>, T>.PiSquared => PiSquared;
    static Real<T> IConstants<Real<T>, T>.Tau => Tau;
    static Real<T> IConstants<Real<T>, T>.EulerMascheroni => EulerMascheroni;
    static Real<T> IConstants<Real<T>, T>.GoldenRatio => GoldenRatio;
    static Real<T> IConstants<Real<T>, T>.Ln2 => Ln2;
    static Real<T> IConstants<Real<T>, T>.Ln10 => Ln10;
    static Real<T> IConstants<Real<T>, T>.Sqrt2 => Sqrt2;
    static Real<T> IConstants<Real<T>, T>.Sqrt3 => Sqrt3;
    static Real<T> IConstants<Real<T>, T>.Sqrt5 => Sqrt5;
    static Real<T> IConstants<Real<T>, T>.ZetaOf2 => ZetaOf2;
    static Real<T> IConstants<Real<T>, T>.ZetaOf3 => ZetaOf3;
    static Real<T> IConstants<Real<T>, T>.ZetaOf4 => ZetaOf4;

    //
    // Operators
    //

    public static Real<T> operator -(Real<T> value) => -value._value;
    public static Real<T> operator --(Real<T> value) => value._value - One;
    public static Real<T> operator ++(Real<T> value) => value._value + One;
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

    public static Real<T> Abs(Real<T> x) => T.Abs(x._value);

    public static Real<T> Conjugate(Real<T> x) => x;

    public static Real<T> Hypot(Real<T> x, Real<T> y) => T.Hypot(x._value, y._value);

    public static bool IsFinite(Real<T> x) => T.IsFinite(x._value);

    public static bool IsInfinity(Real<T> x) => T.IsInfinity(x._value);

    public static bool IsNaN(Real<T> x) => T.IsNaN(x._value);

    public static bool IsZero(Real<T> x) => T.IsZero(x._value);

    public static bool IsNegativeInfinity(Real<T> x) => T.IsNegativeInfinity(x._value);

    public static bool IsPositiveInfinity(Real<T> x) => T.IsPositiveInfinity(x._value);

    public static Real<T> Reciprocate(Real<T> x)
    {
        if (x._value == T.Zero)
        {
            return PositiveInfinity;
        }
        return T.One / x;
    }

    public static bool TryConvertFromChecked<V>(V value, out Real<T> result)
        where V : INumberBase<V>
    {
        result = T.CreateChecked(value);
        return true;
    }

    public static bool TryConvertFromSaturating<V>(V value, out Real<T> result)
        where V : INumberBase<V>
    {
        result = T.CreateSaturating(value);
        return true;
    }

    public static bool TryConvertFromTruncating<V>(V value, out Real<T> result)
        where V : INumberBase<V>
    {
        result = T.CreateTruncating(value);
        return true;
    }

    public static bool TryConvertToChecked<V>(Real<T> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateChecked(value._value);
        return true;
    }

    public static bool TryConvertToSaturating<V>(Real<T> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateSaturating(value._value);
        return true;
    }

    public static bool TryConvertToTruncating<V>(Real<T> value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateTruncating(value._value);
        return true;
    }

    //
    // IDifferentiableFunctions interface
    //

    // Exponential functions

    public static Real<T> Exp(Real<T> x) => T.Exp(x._value);

    public static Real<T> Exp2(Real<T> x) => T.Exp2(x._value);

    public static Real<T> Exp10(Real<T> x) => T.Exp10(x._value);

    // Hyperbolic functions

    public static Real<T> Acosh(Real<T> x) => T.Acosh(x._value);

    public static Real<T> Asinh(Real<T> x) => T.Asinh(x._value);

    public static Real<T> Atanh(Real<T> x) => T.Atanh(x._value);

    public static Real<T> Cosh(Real<T> x) => T.Cosh(x._value);

    public static Real<T> Sinh(Real<T> x) => T.Sinh(x._value);

    public static Real<T> Tanh(Real<T> x) => T.Tanh(x._value);

    // Logarithmic functions

    public static Real<T> Ln(Real<T> x) => T.Log(x._value);

    public static Real<T> Log(Real<T> x, Real<T> b) => T.Log(x._value, b._value);

    public static Real<T> Log2(Real<T> x) => T.Log2(x._value);

    public static Real<T> Log10(Real<T> x) => T.Log10(x._value);

    // Power functions

    public static Real<T> Pow(Real<T> x, Real<T> y) => T.Pow(x._value, y._value);

    // Root functions

    public static Real<T> Cbrt(Real<T> x) => T.Cbrt(x._value);

    public static Real<T> Root(Real<T> x, Real<T> y) => T.Exp(y._value * T.Log(x._value));

    public static Real<T> Sqrt(Real<T> x) => T.Sqrt(x._value);

    // Trigonometric functions

    public static Real<T> Acos(Real<T> x) => T.Acos(x._value);

    public static Real<T> Asin(Real<T> x) => T.Asin(x._value);

    public static Real<T> Atan(Real<T> x) => T.Atan(x._value);

    public static Real<T> Atan2(Real<T> y, Real<T> x) => T.Atan2(y._value, x._value);

    public static Real<T> Cos(Real<T> x) => T.Cos(x._value);

    public static Real<T> Sin(Real<T> x) => T.Sin(x._value);

    public static Real<T> Tan(Real<T> x) => T.Tan(x._value);

    //
    // Implicit operators
    //

    public static implicit operator Real<T>(T x) => new(x);
}
