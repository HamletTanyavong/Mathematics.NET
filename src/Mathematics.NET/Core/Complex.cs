// <copyright file="Complex.cs" company="Mathematics.NET">
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
/// <param name="real">The real part of the complex number</param>
/// <param name="imaginary">The imaginary part of the complex number</param>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Complex(Real real, Real imaginary)
    : IComplex<Complex>,
      IDifferentiableFunctions<Complex>
{
    private static readonly Complex s_im = new(Real.Zero, Real.One);
    private static readonly Complex s_imOverTwo = new(Real.Zero, Real.One / 2.0);

    // For division and reciprocal

    // double.MaxValue / 2.0;
    private const double s_upper = 8.98846567431157854e307;
    // 2.0 * Precision.DblMinPositive / (Precision.DblEpsilonVariant * Precision.DblEpsilonVariant)
    private const double s_lower = 2.00416836000897277e-292;
    // 2.0 / (Precision.DblEpsilonVariant * Precision.DblEpsilonVariant)
    private const double s_scale = 4.05648192073033408e31;

    public static readonly Complex Zero = Real.Zero;
    public static readonly Complex One = Real.One;

    public static readonly Complex NaN = new(Real.NaN, Real.NaN);
    public static readonly Complex Infinity = new(Real.PositiveInfinity, Real.PositiveInfinity);

    private readonly Real _real = real;
    private readonly Real _imaginary = imaginary;

    public Complex(Real real) : this(real, 0.0) { }

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

    static Complex IComplex<Complex>.Zero => Zero;
    static Complex IComplex<Complex>.One => One;
    static Complex IComplex<Complex>.NaN => NaN;

    //
    // Operators
    //

    public static Complex operator +(Complex z) => z;

    public static Complex operator -(Complex z) => new(-z._real, -z._imaginary);

    public static Complex operator +(Complex z, Complex w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static Complex operator -(Complex z, Complex w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static Complex operator *(Complex z, Complex w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    // From Michael Baudin and Robert L. Smith
    public static Complex operator /(Complex z, Complex w)
    {
        var a = z._real.Value;
        var b = z._imaginary.Value;
        var c = w._real.Value;
        var d = w._imaginary.Value;

        Max(Math.Abs(a), Math.Abs(b), out var maxAB);
        Max(Math.Abs(c), Math.Abs(d), out var maxCD);
        var scale = 1.0;

        if (maxAB > s_upper)
        {
            a /= 2.0;
            b /= 2.0;
            scale *= 2.0;
        }
        if (maxCD > s_upper)
        {
            c /= 2.0;
            d /= 2.0;
            scale /= 2.0;
        }

        if (maxAB < s_lower)
        {
            a *= s_scale;
            b *= s_scale;
            scale /= s_scale;
        }
        if (maxCD < s_lower)
        {
            c *= s_scale;
            d *= s_scale;
            scale *= s_scale;
        }

        double re, im;
        if (Math.Abs(d) <= Math.Abs(c))
        {
            DivisionInternal(a, b, c, d, out re, out im);
        }
        else
        {
            DivisionInternal(b, a, d, c, out re, out im);
            im = -im;
        }

        return new(scale * re, scale * im);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Max(double a, double b, out double max) => max = a < b ? b : a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void MinMax(double a, double b, out double minab, out double maxab)
    {
        if (a < b)
        {
            minab = a;
            maxab = b;
        }
        else
        {
            minab = b;
            maxab = a;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DivisionInternal(double a, double b, double c, double d, out double re, out double im)
    {
        var u = d / c;
        var v = 1.0 / (c + d * u);
        ComputeComponent(a, b, c, d, u, v, out re);
        a = -a;
        ComputeComponent(b, a, c, d, u, v, out im);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ComputeComponent(double a, double b, double c, double d, double u, double v, out double component)
    {
        if (u != 0.0)
        {
            var bu = b * u;
            if (bu != 0.0)
            {
                component = (a + bu) * v;
            }
            else
            {
                component = a * v + b * v * u;
            }
        }
        else
        {
            component = (a + d * (b / c)) * v;
        }
    }

    //
    // Equality
    //

    public static bool operator ==(Complex left, Complex right) => left.Re == right.Re && left.Im == right.Im;

    public static bool operator !=(Complex left, Complex right) => left.Re != right.Re || left.Im != right.Im;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Complex other && Equals(other);

    public bool Equals(Complex value) => _real.Equals(value.Re) && _imaginary.Equals(value.Im);

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

    public static Complex Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Complex Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Complex Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse((ReadOnlySpan<char>)s, style, provider);
    }

    public static Complex Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (!TryParse(s, style, provider, out Complex result))
        {
            return Infinity;
        }
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
        => TryParse((ReadOnlySpan<char>)s, style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
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

    public static Complex Abs(Complex z) => Hypot(z._real.Value, z._imaginary.Value);

    public static Complex Conjugate(Complex z) => new(z._real, -z._imaginary);

    public static Complex FromDouble(double x) => new(x);

    public static Complex FromPolarForm(Real magnitude, Real phase)
        => new(magnitude * Math.Cos(phase.Value), magnitude * Math.Sin(phase.Value));

    public static Complex FromReal(Real x) => new(x);

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

    public static bool IsFinite(Complex z) => Real.IsFinite(z._real) && Real.IsFinite(z._imaginary);

    public static bool IsInfinity(Complex z) => Real.IsInfinity(z._real) || Real.IsInfinity(z._imaginary);

    public static bool IsNaN(Complex z) => !IsInfinity(z) && !IsFinite(z);

    public static bool IsZero(Complex z) => Real.IsZero(z._real) && Real.IsZero(z._imaginary);

    public static Complex Reciprocate(Complex z)
    {
        if (z._real == 0.0 && z._imaginary == 0.0)
        {
            return Infinity;
        }

        var scale = 1.0;

        var re = z._real.Value;
        var im = z._imaginary.Value;

        Max(Math.Abs(re), Math.Abs(im), out var max);

        if (max > s_upper)
        {
            re /= 2.0;
            im /= 2.0;
            scale /= 2.0;
        }

        if (max < s_lower)
        {
            re *= s_scale;
            im *= s_scale;
            scale *= s_scale;
        }

        double outRe, outIm;
        if (Math.Abs(im) <= Math.Abs(re))
        {
            DivisionInternal(1.0, 0.0, re, im, out outRe, out outIm);
        }
        else
        {
            DivisionInternal(0.0, 1.0, im, re, out outRe, out outIm);
            outIm = -outIm;
        }

        return new(scale * outRe, scale * outIm);
    }

    // We will only consider the real part of complex numbers for these conversions.

    public static bool TryConvertFromChecked<U>(U value, out Complex result)
        where U : INumberBase<U>
    {
        result = double.CreateChecked(value);
        return true;
    }

    public static bool TryConvertFromSaturating<U>(U value, out Complex result)
        where U : INumberBase<U>
    {
        result = double.CreateSaturating(value);
        return true;
    }

    public static bool TryConvertFromTruncating<U>(U value, out Complex result)
        where U : INumberBase<U>
    {
        result = double.CreateTruncating(value);
        return true;
    }

    public static bool TryConvertToChecked<U>(Complex value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        if (value._imaginary == Real.Zero)
        {
            throw new OverflowException();
        }

        result = U.CreateChecked(value._real.Value);
        return true;
    }

    public static bool TryConvertToSaturating<U>(Complex value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateSaturating(value._real.Value);
        return true;
    }

    public static bool TryConvertToTruncating<U>(Complex value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateTruncating(value._real.Value);
        return true;
    }

    //
    // IDifferentiableFunctions interface
    //

    // Exponential functions

    public static Complex Exp(Complex z)
    {
        Real expReal = Real.Exp(z._real);
        return new(expReal * Real.Cos(z._imaginary), expReal * Real.Sin(z._imaginary));
    }

    public static Complex Exp2(Complex z) => Exp(Real.Ln2 * z);

    public static Complex Exp10(Complex z) => Exp(Real.Ln10 * z);

    // Hyperbolic functions

    public static Complex Acosh(Complex z) => Ln(z + Sqrt(z * z - One));

    public static Complex Asinh(Complex z) => Ln(z + Sqrt(z * z + One));

    public static Complex Atanh(Complex z) => 0.5 * Ln((One + z) / (One - z));

    public static Complex Cosh(Complex z)
        => new(Real.Cosh(z._real) * Real.Cos(z._imaginary), Real.Sinh(z._real) * Real.Sin(z._imaginary));

    public static Complex Sinh(Complex z)
        => new(Real.Sinh(z._real) * Real.Cos(z._imaginary), Real.Cosh(z._real) * Real.Sin(z._imaginary));

    public static Complex Tanh(Complex z) => Sinh(z) / Cosh(z);

    // Logarithmic functions

    public static Complex Ln(Complex z) => new(Real.Ln(Hypot(z._real.Value, z._imaginary.Value)), Real.Atan2(z._imaginary, z._real));

    public static Complex Log(Complex z, Complex b) => Ln(z) / Ln(b);

    public static Complex Log2(Complex z) => Ln(z) / Ln(Real.Ln2);

    public static Complex Log10(Complex z) => Ln(z) / Ln(Real.Ln10);

    // Power functions

    public static Complex Pow(Complex z, Complex w) => Exp(w * Ln(z));

    // Root functions

    public static Complex Cbrt(Complex z) => Exp(Ln(z) / 3.0);

    public static Complex Root(Complex z, Complex w) => Exp(Ln(z) / w);

    public static Complex Sqrt(Complex z) => Exp(0.5 * Ln(z));

    // Trigonometric functions

    public static Complex Acos(Complex z)
    {
        AsinInternal(Math.Abs(z._real.Value), Math.Abs(z._imaginary.Value), out double b, out double bPrime, out double v);

        double u;
        if (bPrime < 0.0)
        {
            u = Math.Acos(b);
        }
        else
        {
            u = Math.Atan(1.0 / bPrime);
        }

        if (z._real < 0.0)
        {
            u = Constants.Pi - u;
        }
        if (z._imaginary > 0.0)
        {
            v = -v;
        }

        return new(u, v);
    }

    public static Complex Asin(Complex z)
    {
        AsinInternal(Math.Abs(z._real.Value), Math.Abs(z._imaginary.Value), out double b, out double bPrime, out double v);

        double u;
        if (bPrime < 0.0)
        {
            u = Math.Asin(b);
        }
        else
        {
            u = Math.Atan(bPrime);
        }

        if (z._real < 0.0)
        {
            u = -u;
        }
        if (z._imaginary < 0.0)
        {
            v = -v;
        }

        return new(u, v);
    }

    private static void AsinInternal(double x, double y, out double b, out double bPrime, out double v)
    {
        // This is the same method described by Hull, Fairgrieve, and Tang in "Implementing the Complex
        // ArcSine and Arccosine Functions Using Exception Handling" that is used in System.Numerics.Complex.
        //
        // https://www.researchgate.net/profile/Ping_Tang3/publication/220493330_Implementing_the_Complex_Arcsine_and_Arccosine_Functions_Using_Exception_Handling/links/55b244b208ae9289a085245d.pdf

        const double overflowThreshold = 6.70390396497129854e153;

        if (x > overflowThreshold || y > overflowThreshold)
        {
            b = -1.0;
            bPrime = x / y;

            double small, big;
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
            double ratio = small / big;
            v = Constants.Ln2 + Math.Log(big) + 0.5 * Log1P(ratio * ratio);
        }
        else
        {
            double r = Hypot(x + 1.0, y);
            double s = Hypot(x - 1.0, y);

            double a = (r + s) * 0.5;
            b = x / a;

            if (b > 0.75)
            {
                if (x <= 1.0)
                {
                    double amx = (y * y / (r + (x + 1.0)) + (s + (1.0 - x))) * 0.5;
                    bPrime = x / Math.Sqrt((a + x) * amx);
                }
                else
                {
                    double t = (1.0 / (r + (x + 1.0)) + 1.0 / (s + (x - 1.0))) * 0.5;
                    bPrime = x / y / Math.Sqrt((a + x) * t);
                }
            }
            else
            {
                bPrime = -1.0;
            }

            if (a < 1.5)
            {
                if (x < 1.0)
                {
                    double t = (1.0 / (r + (x + 1.0)) + 1.0 / (s + (1.0 - x))) * 0.5;
                    double am1 = y * y * t;
                    v = Log1P(am1 + y * Math.Sqrt(t * (a + 1.0)));
                }
                else
                {
                    double am1 = (y * y / (r + (x + 1.0)) + (s + (x - 1.0))) * 0.5;
                    v = Log1P(am1 + Math.Sqrt(am1 * (a + 1.0)));
                }
            }
            else
            {
                v = Math.Log(a + Math.Sqrt((a - 1.0) * (a + 1.0)));
            }
        }
    }

    private static double Log1P(double x)
    {
        double xp1 = 1.0 + x;
        if (xp1 == 1.0)
        {
            return x;
        }
        else if (x < 0.75)
        {
            return x * Math.Log(xp1) / (xp1 - 1.0);
        }
        else
        {
            return Math.Log(xp1);
        }
    }

    public static Complex Atan(Complex z) => s_imOverTwo * Ln((s_im + z) / (s_im - z));

    public static Complex Cos(Complex z)
    {
        Real p = Real.Exp(z._imaginary);
        Real q = Real.One / p;
        Real sinh = (p - q) / 2.0;
        Real cosh = (p + q) / 2.0;
        return new(Real.Cos(z._real) * cosh, -Real.Sin(z._real) * sinh);
    }

    public static Complex Sin(Complex z)
    {
        Real p = Real.Exp(z._imaginary);
        Real q = Real.One / p;
        Real sinh = (p - q) / 2.0;
        Real cosh = (p + q) / 2.0;
        return new(Real.Sin(z._real) * cosh, Real.Cos(z._real) * sinh);
    }

    public static Complex Tan(Complex z)
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
    // Implicit operators
    //

    public static implicit operator Complex(double x) => new(x);

    /// <summary>Convert a value of type <see cref="Real"/> to one of type <see cref="Complex"/></summary>
    /// <param name="x">The value to convert</param>
    public static implicit operator Complex(Real x) => new(x);
}
