// <copyright file="Complex.cs" company="Mathematics.NET">
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

namespace Mathematics.NET;

/// <summary>Represents a complex number.</summary>
/// <param name="real">The real part of the complex number.</param>
/// <param name="imaginary">The imaginary part of the complex number.</param>
/// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential), Serializable]
public readonly struct Complex<T>(Real<T> real, Real<T> imaginary)
    : IComplex<Complex<T>, T, T>,
      IDifferentiableFunctions<Complex<T>>
    where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
{
    private static readonly Complex<T> s_im = new(Real<T>.Zero, Real<T>.One);
    private static readonly Complex<T> s_imOverTwo = new(Real<T>.Zero, Real<T>.One / IBinaryFloatingPointIeee754<T>.Two);

    public static readonly Complex<T> Zero = T.Zero;
    public static readonly Complex<T> One = T.One;

    public static readonly Complex<T> NaN = new(Real<T>.NaN, Real<T>.NaN);
    public static readonly Complex<T> Infinity = new(Real<T>.PositiveInfinity, Real<T>.PositiveInfinity);

    private readonly Real<T> _real = real;
    private readonly Real<T> _imaginary = imaginary;

    public Complex(Real<T> real) : this(real, T.Zero) { }

    //
    // Complex Number Properties
    //

    public Real<T> Re => _real;
    public Real<T> Im => _imaginary;

    public Real<T> Magnitude => Hypot(_real.AsBackingType(), _imaginary.AsBackingType());
    public Real<T> Phase => T.Atan2(_imaginary.AsBackingType(), _real.AsBackingType());

    //
    // Constants
    //

    /// <summary>Represents the imaginary unit, <c>i</c>.</summary>
    public static Complex<T> ImUnit => s_im;

    static Complex<T> IComplex<Complex<T>, T, T>.Zero => Zero;
    static Complex<T> IComplex<Complex<T>, T, T>.One => One;
    static Complex<T> IComplex<Complex<T>, T, T>.NaN => NaN;
    static int IComplex<Complex<T>, T, T>.Radix => 2;

    //
    // Operators
    //

    public static Complex<T> operator +(Complex<T> z) => z;

    public static Complex<T> operator -(Complex<T> z) => new(-z._real, -z._imaginary);

    public static Complex<T> operator +(Complex<T> z, Complex<T> w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static Complex<T> operator -(Complex<T> z, Complex<T> w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static Complex<T> operator *(Complex<T> z, Complex<T> w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    // Algorithm 116: Complex Division by Robert L. Smith: https://doi.org/10.1145/368637.368661
    public static Complex<T> operator /(Complex<T> z, Complex<T> w)
    {
        var a = z._real.AsBackingType();
        var b = z._imaginary.AsBackingType();
        var c = w._real.AsBackingType();
        var d = w._imaginary.AsBackingType();

        if (T.Abs(d) < T.Abs(c))
        {
            var u = d / c;
            return new((a + b * u) / (c + d * u), (b - a * u) / (c + d * u));
        }
        else
        {
            var u = c / d;
            return new((b + a * u) / (d + c * u), (b * u - a) / (d + c * u));
        }
    }

    //
    // Equality
    //

    public static bool operator ==(Complex<T> left, Complex<T> right) => left.Re == right.Re && left.Im == right.Im;

    public static bool operator !=(Complex<T> left, Complex<T> right) => left.Re != right.Re || left.Im != right.Im;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Complex<T> other && Equals(other);

    public bool Equals(Complex<T> value) => _real.Equals(value.Re) && _imaginary.Equals(value.Im);

    public override int GetHashCode() => HashCode.Combine(_real, _imaginary);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? string.Empty : format.ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        if (format is "RE")
            return _real.ToString(null, provider);
        else if (format is "IM")
            return _imaginary.ToString(null, provider);
        else
            return $"({_real.ToString(null, provider)}, {_imaginary.ToString(null, provider)})";
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        format = format.IsEmpty ? string.Empty : format.ToString().ToUpperInvariant();
        provider ??= NumberFormatInfo.InvariantInfo;

        var charsCurrentlyWritten = 0;
        if (format is "RE")
        {
            if (destination.Length < 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            var tryFormatSucceeded = _real.TryFormat(destination, out charsCurrentlyWritten, null, provider);
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = charsCurrentlyWritten;
            return true;
        }
        else if (format is "IM")
        {
            if (destination.Length < 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }

            var tryFormatSucceeded = _imaginary.TryFormat(destination, out charsCurrentlyWritten, null, provider);
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
            // There are a minimum of 6 characters for "(0, 0)".
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
    }

    //
    // Parsing
    //

    public static Complex<T> Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Complex<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    public static Complex<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse((ReadOnlySpan<char>)s, style, provider);
    }

    public static Complex<T> Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (!TryParse(s, style, provider, out Complex<T> result))
            return Infinity;
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex<T> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex<T> result)
        => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex<T> result)
        => TryParse((ReadOnlySpan<char>)s, style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex<T> result)
    {
#pragma warning disable EPS06
        s = s.Trim();
        int openParenthesis = s.IndexOf('(');
        int split = s.IndexOf(',');
        int closeParenthesis = s.IndexOf(')');
#pragma warning restore EPS06

        // There a minimum of 5 characters for "(0,0)".
        if (s.Length < 5 || openParenthesis == -1 || split == -1 || closeParenthesis == -1 || openParenthesis > split || openParenthesis > closeParenthesis || split > closeParenthesis)
        {
            result = Zero;
            return false;
        }

        if (!Real<T>.TryParse(s.Slice(openParenthesis + 1, split - 1), style, provider, out Real<T> real))
        {
            result = Zero;
            return false;
        }

        if (!Real<T>.TryParse(s.Slice(split + 1, closeParenthesis - split - 1), style, provider, out Real<T> imaginary))
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

    public static Complex<T> Abs(in Complex<T> z) => Hypot(z._real.AsBackingType(), z._imaginary.AsBackingType());

    public static Complex<T> Conjugate(Complex<T> z) => new(z._real, -z._imaginary);

    public static Complex<T> FromFloat(T x) => new(x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Complex<T> FromPolarForm(Real<T> magnitude, Real<T> phase)
        => new(magnitude * T.Cos(phase.AsBackingType()), magnitude * T.Sin(phase.AsBackingType()));

    public static Complex<T> FromReal(Real<T> x) => new(x);

    private static T Hypot(T x, T y)
    {
        // Factor out the larger value to avoid possible overflow.
        x = T.Abs(x);
        y = T.Abs(y);

        T small, large;
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

        if (small == T.Zero)
        {
            return large;
        }
        else if (T.IsPositiveInfinity(large) && !T.IsNaN(small))
        {
            return T.PositiveInfinity;
        }
        else
        {
            T ratio = small / large;
            return large * T.Sqrt(T.One + ratio * ratio);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Complex<T> InverseLerp(in Complex<T> start, in Complex<T> end, Real<T> weight)
        => new(Real<T>.InverseLerp(start._real, end._real, weight), Real<T>.InverseLerp(start._imaginary, end._imaginary, weight));

    public static bool IsCanonical(Complex<T> z) => true;

    public static bool IsComplex(Complex<T> z) => !IsReal(z) && !IsImaginary(z);

    public static bool IsFinite(Complex<T> z) => Real<T>.IsFinite(z._real) && Real<T>.IsFinite(z._imaginary);

    public static bool IsImaginary(Complex<T> z) => Real<T>.IsZero(z._real) && !Real<T>.IsZero(z._imaginary);

    public static bool IsInfinity(Complex<T> z) => Real<T>.IsInfinity(z._real) || Real<T>.IsInfinity(z._imaginary);

    public static bool IsInteger(Complex<T> z) => Real<T>.IsZero(z._imaginary) && Real<T>.IsInteger(z._real);

    public static bool IsNaN(Complex<T> z) => !IsInfinity(z) && !IsFinite(z);

    public static bool IsReal(Complex<T> z) => !Real<T>.IsZero(z._real) && Real<T>.IsZero(z._imaginary);

    public static bool IsZero(Complex<T> z) => Real<T>.IsZero(z._real) && Real<T>.IsZero(z._imaginary);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Complex<T> Lerp(in Complex<T> start, in Complex<T> end, Real<T> weight)
        => new(Real<T>.Lerp(start._real, end._real, weight), Real<T>.Lerp(start._imaginary, end._imaginary, weight));

    public static Complex<T> MaxMagnitude(Complex<T> z, Complex<T> w)
    {
        var mz = z.Magnitude;
        var mw = w.Magnitude;

        if (mz > mw || Real<T>.IsNaN(mz))
            return z;

        if (mz == mw)
        {
            if (Real<T>.IsNegative(w._real))
            {
                if (Real<T>.IsNegative(w._imaginary))
                    return z;
                else
                    return Real<T>.IsNegative(z._real) ? w : z;
            }
            else if (Real<T>.IsNegative(w._imaginary))
            {
                return Real<T>.IsNegative(z._real) ? w : z;
            }
        }

        return w;
    }

    static Complex<T> IComplex<Complex<T>, T, T>.MaxMagnitudeNumber(Complex<T> z, Complex<T> w)
    {
        var mz = z.Magnitude;
        var mw = w.Magnitude;

        if (mz > mw || Real<T>.IsNaN(mw))
            return z;

        if (mz == mw)
        {
            if (Real<T>.IsNegative(w._real))
            {
                if (Real<T>.IsNegative(w._imaginary))
                    return z;
                else
                    return Real<T>.IsNegative(z._real) ? w : z;
            }
            else if (Real<T>.IsNegative(w._imaginary))
            {
                return Real<T>.IsNegative(z._real) ? w : z;
            }
        }

        return w;
    }

    public static Complex<T> MinMagnitude(Complex<T> z, Complex<T> w)
    {
        var mz = z.Magnitude;
        var mw = w.Magnitude;

        if (mz < mw || Real<T>.IsNaN(mz))
            return z;

        if (mz == mw)
        {
            if (Real<T>.IsNegative(w._real))
            {
                if (Real<T>.IsNegative(w._imaginary))
                    return w;
                else
                    return Real<T>.IsNegative(z._real) ? z : w;
            }
            else if (Real<T>.IsNegative(w._imaginary))
            {
                return Real<T>.IsNegative(z._real) ? z : w;
            }
            else
            {
                return z;
            }
        }

        return w;
    }

    static Complex<T> IComplex<Complex<T>, T, T>.MinMagnitudeNumber(Complex<T> z, Complex<T> w)
    {
        var mz = z.Magnitude;
        var mw = w.Magnitude;

        if (mz < mw || Real<T>.IsNaN(mw))
            return z;

        if (mz == mw)
        {
            if (Real<T>.IsNegative(w._real))
            {
                if (Real<T>.IsNegative(w._imaginary))
                    return w;
                else
                    return Real<T>.IsNegative(z._real) ? z : w;
            }
            else if (Real<T>.IsNegative(w._imaginary))
            {
                return Real<T>.IsNegative(z._real) ? z : w;
            }
            else
            {
                return z;
            }
        }

        return w;
    }

    public static Complex<T> Reciprocate(Complex<T> z)
    {
        if (z._real == T.Zero && z._imaginary == T.Zero)
            return Infinity;

        var u = z._real * z._real + z._imaginary * z._imaginary;
        return new(z._real / u, -z._imaginary / u);
    }

    // We will only consider the real part of complex numbers for these conversions.

    public static bool TryConvertFromChecked<U>(U value, out Complex<T> result)
        where U : INumberBase<U>
    {
        result = T.CreateChecked(value);
        return true;
    }

    public static bool TryConvertFromSaturating<U>(U value, out Complex<T> result)
        where U : INumberBase<U>
    {
        result = T.CreateSaturating(value);
        return true;
    }

    public static bool TryConvertFromTruncating<U>(U value, out Complex<T> result)
        where U : INumberBase<U>
    {
        result = T.CreateTruncating(value);
        return true;
    }

    public static bool TryConvertToChecked<U>(Complex<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        if (value._imaginary == Real<T>.Zero)
            throw new OverflowException();

        result = U.CreateChecked(value._real.AsBackingType());
        return true;
    }

    public static bool TryConvertToSaturating<U>(Complex<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateSaturating(value._real.AsBackingType());
        return true;
    }

    public static bool TryConvertToTruncating<U>(Complex<T> value, [MaybeNullWhen(false)] out U result)
        where U : INumberBase<U>
    {
        result = U.CreateTruncating(value._real.AsBackingType());
        return true;
    }

    //
    // IDifferentiableFunctions Interface
    //

    // Exponential functions.

    public static Complex<T> Exp(Complex<T> z)
    {
        Real<T> expReal = Real<T>.Exp(z._real);
        return new(expReal * Real<T>.Cos(z._imaginary), expReal * Real<T>.Sin(z._imaginary));
    }

    public static Complex<T> Exp2(Complex<T> z) => Exp(Real<T>.Ln2 * z);

    public static Complex<T> Exp10(Complex<T> z) => Exp(Real<T>.Ln10 * z);

    // Hyperbolic functions.

    public static Complex<T> Acosh(Complex<T> z) => Ln(z + Sqrt(z * z - One));

    public static Complex<T> Asinh(Complex<T> z) => Ln(z + Sqrt(z * z + One));

    public static Complex<T> Atanh(Complex<T> z) => T.CreateChecked(0.5) * Ln((One + z) / (One - z));

    public static Complex<T> Cosh(Complex<T> z)
        => new(Real<T>.Cosh(z._real) * Real<T>.Cos(z._imaginary), Real<T>.Sinh(z._real) * Real<T>.Sin(z._imaginary));

    public static Complex<T> Sinh(Complex<T> z)
        => new(Real<T>.Sinh(z._real) * Real<T>.Cos(z._imaginary), Real<T>.Cosh(z._real) * Real<T>.Sin(z._imaginary));

    public static Complex<T> Tanh(Complex<T> z) => Sinh(z) / Cosh(z);

    // Logarithmic functions.

    public static Complex<T> Ln(Complex<T> z) => new(Real<T>.Ln(Hypot(z._real.AsBackingType(), z._imaginary.AsBackingType())), Real<T>.Atan2(z._imaginary, z._real));

    public static Complex<T> Log(Complex<T> z, Complex<T> b) => Ln(z) / Ln(b);

    public static Complex<T> Log2(Complex<T> z) => Ln(z) / Real<T>.Ln2;

    public static Complex<T> Log10(Complex<T> z) => Ln(z) / Real<T>.Ln10;

    // Power functions.

    public static Complex<T> Pow(Complex<T> z, Complex<T> w) => Exp(w * Ln(z));

    // Root functions.

    public static Complex<T> Cbrt(Complex<T> z) => Exp(Ln(z) / IBinaryFloatingPointIeee754<T>.Three);

    public static Complex<T> Root(Complex<T> z, Complex<T> w) => Exp(Ln(z) / w);

    public static Complex<T> Sqrt(Complex<T> z) => Exp(IBinaryFloatingPointIeee754<T>.Half * Ln(z));

    // Trigonometric functions.

    public static Complex<T> Acos(Complex<T> z)
    {
        AsinInternal(T.Abs(z._real.AsBackingType()), T.Abs(z._imaginary.AsBackingType()), out T b, out T bPrime, out T v);

        T u;
        if (bPrime < T.Zero)
            u = T.Acos(b);
        else
            u = T.Atan(T.One / bPrime);

        if (z._real < T.Zero)
            u = T.CreateChecked(Constants.Pi) - u;
        if (z._imaginary > T.Zero)
            v = -v;

        return new(u, v);
    }

    public static Complex<T> Asin(Complex<T> z)
    {
        AsinInternal(T.Abs(z._real.AsBackingType()), T.Abs(z._imaginary.AsBackingType()), out T b, out T bPrime, out T v);

        T u;
        if (bPrime < T.Zero)
            u = T.Asin(b);
        else
            u = T.Atan(bPrime);

        if (z._real < T.Zero)
            u = -u;
        if (z._imaginary < T.Zero)
            v = -v;

        return new(u, v);
    }

    private static void AsinInternal(T x, T y, out T b, out T bPrime, out T v)
    {
        // This is the same method described by Hull, Fairgrieve, and Tang in "Implementing the Complex
        // ArcSine and Arccosine Functions Using Exception Handling" that is used in System.Numerics.Complex.
        //
        // https://www.researchgate.net/profile/Ping_Tang3/publication/220493330_Implementing_the_Complex_Arcsine_and_Arccosine_Functions_Using_Exception_Handling/links/55b244b208ae9289a085245d.pdf

        T overflowThreshold = T.CreateChecked(6.70390396497129854e153);

        if (x > overflowThreshold || y > overflowThreshold)
        {
            b = -T.One;
            bPrime = x / y;

            T small, big;
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
            var ratio = small / big;
            v = T.CreateChecked(Constants.Ln2) + T.Log(big) + T.CreateChecked(0.5) * Log1P(ratio * ratio);
        }
        else
        {
            var r = Hypot(x + T.One, y);
            var s = Hypot(x - T.One, y);

            var a = (r + s) * T.CreateChecked(0.5);
            b = x / a;

            if (b > T.CreateChecked(0.75))
            {
                if (x <= T.One)
                {
                    var amx = (y * y / (r + (x + T.One)) + (s + (T.One - x))) * T.CreateChecked(0.5);
                    bPrime = x / T.Sqrt((a + x) * amx);
                }
                else
                {
                    var t = (T.One / (r + (x + T.One)) + T.One / (s + (x - T.One))) * T.CreateChecked(0.5);
                    bPrime = x / y / T.Sqrt((a + x) * t);
                }
            }
            else
            {
                bPrime = -T.One;
            }

            if (a < T.CreateChecked(1.5))
            {
                if (x < T.One)
                {
                    var t = (T.One / (r + (x + T.One)) + T.One / (s + (T.One - x))) * T.CreateChecked(0.5);
                    var am1 = y * y * t;
                    v = Log1P(am1 + y * T.Sqrt(t * (a + T.One)));
                }
                else
                {
                    var am1 = (y * y / (r + (x + T.One)) + (s + (x - T.One))) * T.CreateChecked(0.5);
                    v = Log1P(am1 + T.Sqrt(am1 * (a + T.One)));
                }
            }
            else
            {
                v = T.Log(a + T.Sqrt((a - T.One) * (a + T.One)));
            }
        }
    }

    private static T Log1P(T x)
    {
        var xp1 = T.One + x;
        if (xp1 == T.One)
            return x;
        else if (x < T.CreateChecked(0.75))
            return x * T.Log(xp1) / (xp1 - T.One);
        else
            return T.Log(xp1);
    }

    public static Complex<T> Atan(Complex<T> z) => s_imOverTwo * Ln((s_im + z) / (s_im - z));

    public static Complex<T> Cos(Complex<T> z)
    {
        Real<T> p = Real<T>.Exp(z._imaginary);
        Real<T> q = Real<T>.One / p;
        Real<T> sinh = (p - q) / IBinaryFloatingPointIeee754<T>.Two;
        Real<T> cosh = (p + q) / IBinaryFloatingPointIeee754<T>.Two;
        return new(Real<T>.Cos(z._real) * cosh, -Real<T>.Sin(z._real) * sinh);
    }

    public static Complex<T> Sin(Complex<T> z)
    {
        Real<T> p = Real<T>.Exp(z._imaginary);
        Real<T> q = Real<T>.One / p;
        Real<T> sinh = (p - q) / IBinaryFloatingPointIeee754<T>.Two;
        Real<T> cosh = (p + q) / IBinaryFloatingPointIeee754<T>.Two;
        return new(Real<T>.Sin(z._real) * cosh, Real<T>.Cos(z._real) * sinh);
    }

    public static Complex<T> Tan(Complex<T> z)
    {
        Real<T> x2 = IBinaryFloatingPointIeee754<T>.Two * z._real;
        Real<T> y2 = IBinaryFloatingPointIeee754<T>.Two * z._imaginary;
        Real<T> p = Real<T>.Exp(y2);
        Real<T> q = Real<T>.One / p;
        Real<T> cosh = (p + q) / IBinaryFloatingPointIeee754<T>.Two;
        if (Real<T>.Abs(z._imaginary) <= IBinaryFloatingPointIeee754<T>.Four)
        {
            Real<T> sinh = (p - q) / IBinaryFloatingPointIeee754<T>.Two;
            Real<T> u = Real<T>.Cos(x2) + cosh;
            return new(Real<T>.Sin(x2) / u, sinh / u);
        }
        else
        {
            Real<T> u = Real<T>.One + Real<T>.Cos(x2) / cosh;
            return new(Real<T>.Sin(x2) / cosh / u, Real<T>.Tanh(y2) / u);
        }
    }

    //
    // Implicit Operators
    //

    public static implicit operator Complex<T>(T x) => new(x);

    public static implicit operator Complex<T>(Real<T> x) => new(x);

    //
    // Explicit Operators
    //

    public static explicit operator T(Complex<T> x) => Unsafe.As<Complex<T>, T>(ref x);
}
