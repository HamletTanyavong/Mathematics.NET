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
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Real
    : IReal<Real>,
      IConstants<Real>,
      IDifferentiableFunctions<Real>
{
    public static readonly Real Zero = 0.0;
    public static readonly Real One = 1.0;

    public static readonly Real MaxValue = double.MaxValue;
    public static readonly Real MinValue = double.MinValue;

    public static readonly Real NaN = double.NaN;
    public static readonly Real NegativeInfinity = double.NegativeInfinity;
    public static readonly Real PositiveInfinity = double.PositiveInfinity;

    /// <inheritdoc cref="Constants.E" />
    public static readonly Real E = Constants.E;

    /// <inheritdoc cref="Constants.Pi" />
    public static readonly Real Pi = Constants.Pi;

    /// <inheritdoc cref="Constants.PiOverTwo" />
    public static readonly Real PiOverTwo = Constants.PiOverTwo;

    /// <inheritdoc cref="Constants.PiSquared" />
    public static readonly Real PiSquared = Constants.PiSquared;

    /// <inheritdoc cref="Constants.Tau" />
    public static readonly Real Tau = Constants.Tau;

    /// <inheritdoc cref="Constants.EulerMascheroni" />
    public static readonly Real EulerMascheroni = Constants.EulerMascheroni;

    /// <inheritdoc cref="Constants.GoldenRatio" />
    public static readonly Real GoldenRatio = Constants.GoldenRatio;

    /// <inheritdoc cref="Constants.Ln2" />
    public static readonly Real Ln2 = Constants.Ln2;

    /// <inheritdoc cref="Constants.Ln10" />
    public static readonly Real Ln10 = Constants.Ln10;

    /// <inheritdoc cref="Constants.Sqrt2" />
    public static readonly Real Sqrt2 = Constants.Sqrt2;

    /// <inheritdoc cref="Constants.Sqrt3" />
    public static readonly Real Sqrt3 = Constants.Sqrt3;

    /// <inheritdoc cref="Constants.Sqrt5" />
    public static readonly Real Sqrt5 = Constants.Sqrt5;

    /// <inheritdoc cref="Constants.ZetaOf2" />
    public static readonly Real ZetaOf2 = Constants.ZetaOf2;

    /// <inheritdoc cref="Constants.ZetaOf3" />
    public static readonly Real ZetaOf3 = Constants.ZetaOf3;

    /// <inheritdoc cref="Constants.ZetaOf4" />
    public static readonly Real ZetaOf4 = Constants.ZetaOf4;

    private readonly double _value;

    public Real(double real)
    {
        _value = real;
    }

    //
    // Real number properties
    //

    public Real Re => _value;
    public double Value => _value;

    //
    // Constants
    //

    static Real IComplex<Real>.Zero => Zero;
    static Real IComplex<Real>.One => One;
    static Real IComplex<Real>.NaN => NaN;
    static Real IMinMaxValue<Real>.MaxValue => MaxValue;
    static Real IMinMaxValue<Real>.MinValue => MinValue;

    // IConstants interface

    static Real IConstants<Real>.E => E;
    static Real IConstants<Real>.Pi => Pi;
    static Real IConstants<Real>.PiOverTwo => PiOverTwo;
    static Real IConstants<Real>.PiSquared => PiSquared;
    static Real IConstants<Real>.Tau => Tau;
    static Real IConstants<Real>.EulerMascheroni => EulerMascheroni;
    static Real IConstants<Real>.GoldenRatio => GoldenRatio;
    static Real IConstants<Real>.Ln2 => Ln2;
    static Real IConstants<Real>.Ln10 => Ln10;
    static Real IConstants<Real>.Sqrt2 => Sqrt2;
    static Real IConstants<Real>.Sqrt3 => Sqrt3;
    static Real IConstants<Real>.Sqrt5 => Sqrt5;
    static Real IConstants<Real>.ZetaOf2 => ZetaOf2;
    static Real IConstants<Real>.ZetaOf3 => ZetaOf3;
    static Real IConstants<Real>.ZetaOf4 => ZetaOf4;

    //
    // Operators
    //

    public static Real operator -(Real value) => -value._value;
    public static Real operator --(Real value) => value._value - One;
    public static Real operator ++(Real value) => value._value + One;
    public static Real operator +(Real left, Real right) => left._value + right._value;
    public static Real operator -(Real left, Real right) => left._value - right._value;
    public static Real operator *(Real left, Real right) => left._value * right._value;
    public static Real operator /(Real left, Real right) => left._value / right._value;

    //
    // Equality
    //

    public static bool operator ==(Real left, Real right) => left._value == right._value;

    public static bool operator !=(Real left, Real right) => left._value != right._value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Real other && Equals(other);

    public bool Equals(Real value) => _value.Equals(value._value);

    public override int GetHashCode() => HashCode.Combine(_value);

    //
    // Comparison
    //

    public static bool operator <(Real left, Real right) => left._value < right._value;
    public static bool operator >(Real left, Real right) => left._value > right._value;
    public static bool operator <=(Real left, Real right) => left._value <= right._value;
    public static bool operator >=(Real left, Real right) => left._value >= right._value;

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is Real value)
        {
            return _value.CompareTo(value._value);
        }

        throw new ArgumentException("Argument is not a real number");
    }

    public int CompareTo(Real value) => _value.CompareTo(value._value);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider) => string.Format(provider, "{0}", _value);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => _value.TryFormat(destination, out charsWritten, null, provider);

    //
    // Parsing
    //

    public static Real Parse(string s, IFormatProvider? provider) => double.Parse(s, provider);

    public static Real Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => double.Parse(s, provider);

    public static Real Parse(string s, NumberStyles style, IFormatProvider? provider)
        => double.Parse(s, style, provider);

    public static Real Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
        => double.Parse(s, style, provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Real result)
    {
        var succeeded = double.TryParse(s, provider, out double number);
        result = number;
        return succeeded;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Real result)
    {
        var succeeded = double.TryParse(s, provider, out double number);
        result = number;
        return succeeded;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Real result)
    {
        if (s is null)
        {
            result = 0.0;
            return false;
        }

        var succeeded = double.TryParse((ReadOnlySpan<char>)s, style, NumberFormatInfo.GetInstance(provider), out double number);
        result = number;
        return succeeded;
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Real result)
    {
        if (s.IsEmpty)
        {
            result = 0.0;
            return false;
        }

        var succeeded = double.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out double number);
        result = number;
        return succeeded;
    }

    //
    // Methods
    //

    public static Real Abs(Real x) => Math.Abs(x._value);

    public static Real Conjugate(Real x) => x;

    public static Real Hypot(Real x, Real y) => double.Hypot(x._value, y._value);

    public static bool IsFinite(Real x) => double.IsFinite(x._value);

    public static bool IsInfinity(Real x) => double.IsInfinity(x._value);

    public static bool IsNaN(Real x) => double.IsNaN(x._value);

    public static bool IsZero(Real x) => x._value == 0.0;

    public static bool IsNegativeInfinity(Real x) => double.IsNegativeInfinity(x._value);

    public static bool IsPositiveInfinity(Real x) => double.IsPositiveInfinity(x._value);

    public static Real Reciprocate(Real x)
    {
        if (x._value == 0.0)
        {
            return PositiveInfinity;
        }
        return 1.0 / x;
    }

    public static bool TryConvertFromChecked<U>(U value, out Real result)
        where U : INumberBase<U>
    {
        result = double.CreateChecked(value);
        return true;
    }

    public static bool TryConvertFromSaturating<U>(U value, out Real result)
        where U : INumberBase<U>
    {
        result = double.CreateSaturating(value);
        return true;
    }

    public static bool TryConvertFromTruncating<U>(U value, out Real result)
        where U : INumberBase<U>
    {
        result = double.CreateTruncating(value);
        return true;
    }

    public static bool TryConvertToChecked<U>(Real value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateChecked(value._value);
        return true;
    }

    public static bool TryConvertToSaturating<U>(Real value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateSaturating(value._value);
        return true;
    }

    public static bool TryConvertToTruncating<U>(Real value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateTruncating(value._value);
        return true;
    }

    //
    // IDifferentiableFunctions interface
    //

    // Exponential functions

    public static Real Exp(Real x) => Math.Exp(x._value);

    public static Real Exp2(Real x) => double.Exp2(x._value);

    public static Real Exp10(Real x) => double.Exp10(x._value);

    // Hyperbolic functions

    public static Real Acosh(Real x) => Math.Acosh(x._value);

    public static Real Asinh(Real x) => Math.Asinh(x._value);

    public static Real Atanh(Real x) => Math.Atanh(x._value);

    public static Real Cosh(Real x) => Math.Cosh(x._value);

    public static Real Sinh(Real x) => Math.Sinh(x._value);

    public static Real Tanh(Real x) => Math.Tanh(x._value);

    // Logarithmic functions

    public static Real Ln(Real x) => Math.Log(x._value);

    public static Real Log(Real x, Real b) => Math.Log(x._value, b._value);

    public static Real Log2(Real x) => Math.Log2(x._value);

    public static Real Log10(Real x) => Math.Log10(x._value);

    // Power functions

    public static Real Pow(Real x, Real y) => Math.Pow(x._value, y._value);

    // Root functions

    public static Real Cbrt(Real x) => Math.Cbrt(x._value);

    public static Real Root(Real x, Real y) => Math.Exp(Math.Log(x._value) / y._value);

    public static Real Sqrt(Real x) => Math.Sqrt(x._value);

    // Trigonometric functions

    public static Real Acos(Real x) => Math.Acos(x._value);

    public static Real Asin(Real x) => Math.Asin(x._value);

    public static Real Atan(Real x) => Math.Atan(x._value);

    public static Real Atan2(Real y, Real x) => Math.Atan2(y._value, x._value);

    public static Real Cos(Real x) => Math.Cos(x._value);

    public static Real Sin(Real x) => Math.Sin(x._value);

    public static Real Tan(Real x) => Math.Tan(x._value);

    //
    // Implicit operators
    //

    public static implicit operator Real(double x) => new(x);
}
