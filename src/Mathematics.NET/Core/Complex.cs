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

#pragma warning disable IDE0032

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Mathematics.NET.Core;

/// <summary>Represents a complex number</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public readonly struct Complex<T> : IComplex<Complex<T>, T>
    where T : IFloatingPointIeee754<T>
{
    public static readonly Complex<T> Zero = new(Real<T>.Zero, Real<T>.Zero);
    public static readonly Complex<T> One = new(Real<T>.One, Real<T>.Zero);
    public static readonly Complex<T> ImaginaryOne = new(Real<T>.Zero, Real<T>.One);
    public static readonly Complex<T> NaN = new(Real<T>.NaN, Real<T>.NaN);
    public static readonly Complex<T> Infinity = new(Real<T>.PositiveInfinity, Real<T>.PositiveInfinity);

    private readonly Real<T> _real;
    private readonly Real<T> _imaginary;

    public Complex(Real<T> real)
    {
        _real = real;
        _imaginary = T.Zero;
    }

    public Complex(Real<T> real, Real<T> imaginary)
    {
        _real = real;
        _imaginary = imaginary;
    }

    // Complex number properties

    public Real<T> Re => _real;
    public Real<T> Im => _imaginary;

    public Real<T> Magnitude => T.Hypot(_real.Value, _imaginary.Value);
    public Real<T> Phase => T.Atan2(_imaginary.Value, _real.Value);

    // Constants

    static Complex<T> IComplex<Complex<T>, T>.Zero => Zero;
    static Complex<T> IComplex<Complex<T>, T>.One => One;
    static Complex<T> IComplex<Complex<T>, T>.NaN => NaN;

    // Operators

    public static Complex<T> operator -(Complex<T> z) => new(-z._real, -z._imaginary);

    public static Complex<T> operator +(Complex<T> z, Complex<T> w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static Complex<T> operator -(Complex<T> z, Complex<T> w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static Complex<T> operator *(Complex<T> z, Complex<T> w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    // From Michael Baudin and Robert L. Smith
    public static Complex<T> operator /(Complex<T> z, Complex<T> w)
    {
        var zRe = z._real.Value;
        var zIm = z._imaginary.Value;
        var wRe = w._real.Value;
        var wIm = w._imaginary.Value;

        T reResult;
        T imResult;
        if (T.Abs(wIm) <= T.Abs(wRe))
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
    private static void DivisionHelper(T x, T y, T maxW, T minW, out T real, out T imaginary)
    {
        var ratio = minW / maxW;
        var scale = T.One / (maxW + minW * ratio);
        if (ratio != T.Zero)
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

    // Equality

    public static bool operator ==(Complex<T> left, Complex<T> right) => left.Re == right.Re && left.Im == right.Im;

    public static bool operator !=(Complex<T> left, Complex<T> right) => left.Re != right.Re || left.Im != right.Im;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Complex<T> other && Equals(other);

    public bool Equals(Complex<T> value) => _real.Equals(value.Re) && _imaginary.Equals(value.Im);

    public override int GetHashCode() => HashCode.Combine(_real, _imaginary);

    // Formatting

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

    // Methods

    public static Complex<T> Conjugate(Complex<T> z) => new(z._real, -z._imaginary);

    public static Complex<T> Reciprocate(Complex<T> w)
    {
        if (w._real == T.Zero && w._imaginary == T.Zero)
        {
            return Infinity;
        }

        var re = w._real.Value;
        var im = w._imaginary.Value;

        T reResult;
        T imResult;
        if (T.Abs(im) <= T.Abs(re))
        {
            DivisionHelper(T.One, T.Zero, re, im, out reResult, out imResult);
        }
        else
        {
            DivisionHelper(T.Zero, T.One, im, re, out reResult, out imResult);
            imResult = -imResult;
        }

        return new(reResult, imResult);
    }

    // Implicit Operators

    public static implicit operator Complex<T>(T x) => new(x);
}
