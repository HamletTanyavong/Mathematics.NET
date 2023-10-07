// <copyright file="Cmplx.cs" company="Mathematics.NET">
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

/// <summary>Represents a complex number</summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct ComplexNumber
    : IComplex<ComplexNumber>,
      IDifferentiableFunctions<ComplexNumber>
{
    private static readonly ComplexNumber s_im = new(Real.Zero, Real.One);
    private static readonly ComplexNumber s_imOverTwo = new(Real.Zero, Real.One / 2.0);

    // For computing Asin and Acos
    private static readonly Real s_asinOverflowThreshold = Real.Sqrt(Real.MaxValue) / 2.0;

    public static readonly ComplexNumber Zero = Real.Zero;
    public static readonly ComplexNumber One = Real.One;

    public static readonly ComplexNumber NaN = new(Real.NaN, Real.NaN);
    public static readonly ComplexNumber Infinity = new(Real.PositiveInfinity, Real.PositiveInfinity);

    private readonly Real _real;
    private readonly Real _imaginary;

    public ComplexNumber(Real real)
    {
        _real = real;
        _imaginary = 0.0;
    }

    public ComplexNumber(Real real, Real imaginary)
    {
        _real = real;
        _imaginary = imaginary;
    }

    //
    // Complex number properties
    //

    public Real Re => _real;
    public Real Im => _imaginary;

    public Real Magnitude => Hypot(_real.Value, _imaginary.Value);
    public Real Phase => Math.Atan2(_imaginary.Value, _real.Value);

    //
    // Constants
    //

    static ComplexNumber IComplex<ComplexNumber>.Zero => Zero;
    static ComplexNumber IComplex<ComplexNumber>.One => One;
    static ComplexNumber IComplex<ComplexNumber>.NaN => NaN;

    //
    // Operators
    //

    public static ComplexNumber operator -(ComplexNumber z) => new(-z._real, -z._imaginary);

    public static ComplexNumber operator +(ComplexNumber z, ComplexNumber w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static ComplexNumber operator -(ComplexNumber z, ComplexNumber w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static ComplexNumber operator *(ComplexNumber z, ComplexNumber w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    // From Michael Baudin and Robert L. Smith
    public static ComplexNumber operator /(ComplexNumber z, ComplexNumber w)
    {
        var zRe = z._real.Value;
        var zIm = z._imaginary.Value;
        var wRe = w._real.Value;
        var wIm = w._imaginary.Value;

        double reResult;
        double imResult;
        if (Math.Abs(wIm) <= Math.Abs(wRe))
        {
            DivisionHelper(zRe, zIm, wRe, wIm, out reResult, out imResult);
        }
        else
        {
            DivisionHelper(zIm, zRe, wIm, wRe, out reResult, out imResult);
            imResult = -imResult;
        }

        return new(reResult, imResult);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DivisionHelper(double x, double y, double maxW, double minW, out double real, out double imaginary)
    {
        var ratio = minW / maxW;
        var scale = 1.0 / (maxW + minW * ratio);
        if (ratio != 0.0)
        {
            real = (x + y * ratio) * scale;
            imaginary = (y - x * ratio) * scale;
        }
        else
        {
            real = (x + minW * (y / maxW)) * scale;
            imaginary = (y - minW * (x / maxW)) * scale;
        }
    }

    //
    // Equality
    //

    public static bool operator ==(ComplexNumber left, ComplexNumber right) => left.Re == right.Re && left.Im == right.Im;

    public static bool operator !=(ComplexNumber left, ComplexNumber right) => left.Re != right.Re || left.Im != right.Im;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ComplexNumber other && Equals(other);

    public bool Equals(ComplexNumber value) => _real.Equals(value.Re) && _imaginary.Equals(value.Im);

    public override int GetHashCode() => HashCode.Combine(_real, _imaginary);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? "ALL" : format.ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "ALL")
        {
            return string.Format(provider, "({0}, {1})", _real.ToString(null, provider), _imaginary.ToString(null, provider));
        }
        else if (format is "RE")
        {
            return string.Format(provider, "{0}", _real.ToString(null, provider));
        }
        else if (format is "IM")
        {
            return string.Format(provider, "{0}", _imaginary.ToString(null, provider));
        }
        else
        {
            throw new FormatException($"The \"{format}\" format is not supported.");
        }
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        format = format.IsEmpty ? "ALL" : format.ToString().ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "ALL")
        {
            // There are a minimum of 6 characters for "(0, 0)".
            int charsCurrentlyWritten = 0;
            if (destination.Length < 6)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = '(';

            bool tryFormatSucceeded = _real.TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
            charsCurrentlyWritten += tryFormatCharsWritten;
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            destination[charsCurrentlyWritten++] = ',';
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

            tryFormatSucceeded = _imaginary.TryFormat(destination[charsCurrentlyWritten..], out tryFormatCharsWritten, null, provider);
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
        else if (format is "RE")
        {
            return _real.TryFormat(destination, out charsWritten, null, provider);
        }
        else if (format is "IM")
        {
            return _imaginary.TryFormat(destination, out charsWritten, null, provider);
        }
        else
        {
            throw new FormatException($"The \"{format}\" format is not supported.");
        }
    }

    //
    // Parsing
    //

    public static ComplexNumber Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static ComplexNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static ComplexNumber Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse((ReadOnlySpan<char>)s, style, provider);
    }

    public static ComplexNumber Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (!TryParse(s, style, provider, out ComplexNumber result))
        {
            return Infinity;
        }
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ComplexNumber result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ComplexNumber result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out ComplexNumber result)
        => TryParse((ReadOnlySpan<char>)s, style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ComplexNumber result)
    {
        s = s.Trim();
        int openParenthesis = s.IndexOf('(');
        int split = s.IndexOf(',');
        int closeParenthesis = s.IndexOf(')');

        // There a minimum of 5 characters for "(0,0)".
        if (s.Length < 5 || openParenthesis == -1 || split == -1 || closeParenthesis == -1 || openParenthesis > split || openParenthesis > closeParenthesis || split > closeParenthesis)
        {
            result = Zero;
            return false;
        }

        if (!Real.TryParse(s.Slice(openParenthesis + 1, split - 1), style, provider, out Real real))
        {
            result = Zero;
            return false;
        }

        if (!Real.TryParse(s.Slice(split + 1, closeParenthesis - split - 1), style, provider, out Real imaginary))
        {
            result = Zero;
            return false;
        }

        result = new(real, imaginary);
        return true;
    }

    //
    // Methods
    //

    public static ComplexNumber Abs(ComplexNumber z) => Hypot(z._real.Value, z._imaginary.Value);

    public static ComplexNumber Conjugate(ComplexNumber z) => new(z._real, -z._imaginary);

    public static ComplexNumber FromPolarForm(Real magnitude, Real phase)
        => new(magnitude * Math.Cos(phase.Value), magnitude * Math.Sin(phase.Value));

    private static double Hypot(double x, double y)
    {
        // Factor out the larger value to avoid possible overflow
        x = Math.Abs(x);
        y = Math.Abs(y);

        double small, large;
        if (x < y)
        {
            small = x;
            large = y;
        }
        else
        {
            small = y;
            large = x;
        }

        if (small == 0.0)
        {
            return large;
        }
        else if (double.IsPositiveInfinity(large) && !double.IsNaN(small))
        {
            return double.PositiveInfinity;
        }
        else
        {
            double ratio = small / large;
            return large * Math.Sqrt(1.0 + ratio * ratio);
        }
    }

    public static bool IsFinite(ComplexNumber z) => Real.IsFinite(z._real) && Real.IsFinite(z._imaginary);

    public static bool IsInfinity(ComplexNumber z) => Real.IsInfinity(z._real) || Real.IsInfinity(z._imaginary);

    public static bool IsNaN(ComplexNumber z) => !IsInfinity(z) && !IsFinite(z);

    public static bool IsZero(ComplexNumber z) => Real.IsZero(z._real) && Real.IsZero(z._imaginary);

    public static ComplexNumber Reciprocate(ComplexNumber z)
    {
        if (z._real == 0.0 && z._imaginary == 0.0)
        {
            return Infinity;
        }

        var re = z._real.Value;
        var im = z._imaginary.Value;

        double reResult;
        double imResult;
        if (Math.Abs(im) <= Math.Abs(re))
        {
            DivisionHelper(1.0, 0.0, re, im, out reResult, out imResult);
        }
        else
        {
            DivisionHelper(0.0, 1.0, im, re, out reResult, out imResult);
            imResult = -imResult;
        }

        return new(reResult, imResult);
    }

    // We will only consider the real part of complex numbers for these conversions.

    public static bool TryConvertFromChecked<V>(V value, out ComplexNumber result)
        where V : INumberBase<V>
    {
        result = double.CreateChecked(value);
        return true;
    }

    public static bool TryConvertFromSaturating<V>(V value, out ComplexNumber result)
        where V : INumberBase<V>
    {
        result = double.CreateSaturating(value);
        return true;
    }

    public static bool TryConvertFromTruncating<V>(V value, out ComplexNumber result)
        where V : INumberBase<V>
    {
        result = double.CreateTruncating(value);
        return true;
    }

    public static bool TryConvertToChecked<V>(ComplexNumber value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        if (value._imaginary == Real.Zero)
        {
            throw new OverflowException();
        }

        result = V.CreateChecked(value._real.Value);
        return true;
    }

    public static bool TryConvertToSaturating<V>(ComplexNumber value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateSaturating(value._real.Value);
        return true;
    }

    public static bool TryConvertToTruncating<V>(ComplexNumber value, [MaybeNullWhen(false)] out V result)
        where V : INumberBase<V>
    {
        result = V.CreateTruncating(value._real.Value);
        return true;
    }

    //
    // IDifferentiableFunctions interface
    //

    // Exponential functions

    public static ComplexNumber Exp(ComplexNumber z)
    {
        Real expReal = Real.Exp(z._real);
        return new(expReal * Real.Cos(z._imaginary), expReal * Real.Sin(z._imaginary));
    }

    public static ComplexNumber Exp2(ComplexNumber z) => Exp(Real.Ln2 * z);

    public static ComplexNumber Exp10(ComplexNumber z) => Exp(Real.Ln10 * z);

    // Hyperbolic functions

    public static ComplexNumber Acosh(ComplexNumber z) => Ln(z + Sqrt(z * z - One));

    public static ComplexNumber Asinh(ComplexNumber z) => Ln(z + Sqrt(z * z + One));

    public static ComplexNumber Atanh(ComplexNumber z) => 0.5 * Ln((One + z) / (One - z));

    public static ComplexNumber Cosh(ComplexNumber z)
        => new(Real.Cosh(z._real) * Real.Cos(z._imaginary), Real.Sinh(z._real) * Real.Sin(z._imaginary));

    public static ComplexNumber Sinh(ComplexNumber z)
        => new(Real.Sinh(z._real) * Real.Cos(z._imaginary), Real.Cosh(z._real) * Real.Sin(z._imaginary));

    public static ComplexNumber Tanh(ComplexNumber z) => Sinh(z) / Cosh(z);

    // Logarithmic functions

    public static ComplexNumber Ln(ComplexNumber z) => new(Real.Ln(Hypot(z._real.Value, z._imaginary.Value)), Real.Atan2(z._imaginary, z._real));

    public static ComplexNumber Log(ComplexNumber z, ComplexNumber b) => Ln(z) / Ln(b);

    public static ComplexNumber Log2(ComplexNumber z) => Ln(z) / Ln(Real.Ln2);

    public static ComplexNumber Log10(ComplexNumber z) => Ln(z) / Ln(Real.Ln10);

    // Power functions

    public static ComplexNumber Pow(ComplexNumber z, ComplexNumber w) => Exp(w * Ln(z));

    // Root functions

    public static ComplexNumber Cbrt(ComplexNumber z) => Exp(Ln(z) / 3.0);

    public static ComplexNumber Root(ComplexNumber z, ComplexNumber w) => Exp(Ln(z) / w);

    public static ComplexNumber Sqrt(ComplexNumber z) => Exp(0.5 * Ln(z));

    // Trigonometric functions

    public static ComplexNumber Acos(ComplexNumber z)
    {
        AsinInternal(Real.Abs(z._real), Real.Abs(z._imaginary), out Real b, out Real bPrime, out Real v);

        Real u;
        if (bPrime < Real.Zero)
        {
            u = Real.Acos(b);
        }
        else
        {
            u = Real.Atan(Real.One / bPrime);
        }

        if (z._real < Real.Zero)
        {
            u = Real.Pi - u;
        }
        if (z._imaginary > Real.Zero)
        {
            v = -v;
        }

        return new(u, v);
    }

    public static ComplexNumber Asin(ComplexNumber z)
    {
        AsinInternal(Real.Abs(z._real), Real.Abs(z._imaginary), out Real b, out Real bPrime, out Real v);

        Real u;
        if (bPrime < Real.Zero)
        {
            u = Real.Asin(b);
        }
        else
        {
            u = Real.Atan(bPrime);
        }

        if (z._real < Real.Zero)
        {
            u = -u;
        }
        if (z._imaginary < Real.Zero)
        {
            v = -v;
        }

        return new(u, v);
    }

    private static void AsinInternal(Real x, Real y, out Real b, out Real bPrime, out Real v)
    {
        // This is the same method described by Hull, Fairgrieve, and Tang in "Implementing the Complex
        // ArcSine and Arccosine Functions Using Exception Handling" that is used in System.Numerics.Complex.
        if (x > s_asinOverflowThreshold || y > s_asinOverflowThreshold)
        {
            b = -Real.One;
            bPrime = x / y;

            Real small, big;
            if (x < y)
            {
                small = x;
                big = y;
            }
            else
            {
                small = y;
                big = x;
            }
            Real ratio = small / big;
            v = Real.Ln2 + Real.Ln(big) + Real.Ln(ratio * ratio + Real.One) / 2.0;
        }
        else
        {
            Real r = Hypot((x + Real.One).Value, y.Value);
            Real s = Hypot((x - Real.One).Value, y.Value);

            Real a = (r + s) / 2.0;
            b = x / a;

            if (b > 0.75)
            {
                if (x <= Real.One)
                {
                    Real amx = (y * y / (r + (x + Real.One)) + (s + (Real.One - x))) / 2.0;
                    bPrime = x / Real.Sqrt((a + x) * amx);
                }
                else
                {
                    Real t = (Real.One / (r + (x + Real.One)) + Real.One / (s + (x - Real.One))) / 2.0;
                    bPrime = x / y / Real.Sqrt((a + x) * t);
                }
            }
            else
            {
                bPrime = -Real.One;
            }

            if (a < 1.5)
            {
                if (x < Real.One)
                {
                    Real t = (Real.One / (r + (x + Real.One)) + Real.One / (s + (Real.One - x))) / 2.0;
                    Real am1 = y * y * t;
                    v = Real.Ln(am1 + y * Real.Sqrt(t * (a + Real.One)) + Real.One);
                }
                else
                {
                    Real am1 = (y * y / (r + (x + Real.One)) + (s + (x - Real.One))) / 2.0;
                    v = Real.Ln(am1 + Real.Sqrt(am1 * (a + Real.One)) + Real.One);
                }
            }
            else
            {
                v = Real.Ln(a + Real.Sqrt((a - Real.One) * (a + Real.One)));
            }
        }
    }

    public static ComplexNumber Atan(ComplexNumber z) => s_imOverTwo * Ln((s_im + z) / (s_im - z));

    public static ComplexNumber Cos(ComplexNumber z)
    {
        Real p = Real.Exp(z._imaginary);
        Real q = Real.One / p;
        Real sinh = (p - q) / 2.0;
        Real cosh = (p + q) / 2.0;
        return new(Real.Cos(z._real) * cosh, -Real.Sin(z._real) * sinh);
    }

    public static ComplexNumber Sin(ComplexNumber z)
    {
        Real p = Real.Exp(z._imaginary);
        Real q = Real.One / p;
        Real sinh = (p - q) / 2.0;
        Real cosh = (p + q) / 2.0;
        return new(Real.Sin(z._real) * cosh, Real.Cos(z._real) * sinh);
    }

    public static ComplexNumber Tan(ComplexNumber z)
    {
        Real x2 = 2.0 * z._real;
        Real y2 = 2.0 * z._imaginary;
        Real p = Real.Exp(y2);
        Real q = Real.One / p;
        Real cosh = (p + q) / 2.0;
        if (Real.Abs(z._imaginary) <= 4.0)
        {
            Real sinh = (p - q) / 2.0;
            Real D = Real.Cos(x2) + cosh;
            return new(Real.Sin(x2) / D, sinh / D);
        }
        else
        {
            Real D = Real.One + Real.Cos(x2) / cosh;
            return new(Real.Sin(x2) / cosh / D, Real.Tanh(y2) / D);
        }
    }

    //
    // Implicit Operators
    //

    public static implicit operator ComplexNumber(double x) => new(x);

    /// <summary>Convert a value of type <see cref="Real"/> to one of type <see cref="ComplexNumber"/></summary>
    /// <param name="x">The value to convert</param>
    public static implicit operator ComplexNumber(Real x) => new(x);
}
