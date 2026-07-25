// <copyright file="PAdic`1.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0032

using System.Globalization;
using System.Numerics;
using System.Text;

namespace Mathematics.NET.NumberTheory;

/// <summary>Represents a p-adic number.</summary>
/// <typeparam name="T">A type that implements <see cref=" IBinaryInteger{TSelf}"/> and <see cref="ISignedNumber{TSelf}"/>.</typeparam>
public readonly struct PAdic<T>
    : ISpanFormattable
    where T : IBinaryInteger<T>, ISignedNumber<T>
{
    private readonly T _prime;

    private readonly int _valuation;
    private readonly int _period;

    private readonly T[] _digits = [];

    public PAdic(T p, T n)
    {
        _prime = p;
        _valuation = PAdic.Valuation(_prime, n).AsInt();
        _period = 1;

        HashSet<T> coefficients = [];
        List<T> digits = [];
        while (!coefficients.Contains(n))
        {
            _ = coefficients.Add(n);
            digits.Add(Number.ModDivideRemainder(ref n, _prime));
        }
        _digits = [.. digits];
    }

    public PAdic(T p, Rational<T, float> q)
    {
        _prime = p;
        q = q.Reduce();
        _valuation = PAdic.Valuation(_prime, q).AsInt();
        SetFields(q, out _period, out _digits);
    }

    public PAdic(T p, Rational<T, double> q)
    {
        _prime = p;
        q = q.Reduce();
        _valuation = PAdic.Valuation(_prime, q).AsInt();
        SetFields(q, out _period, out _digits);
    }

    public PAdic(T p, T[] digits, int valuation, int period)
    {
        _prime = p;

        ReadOnlySpan<T> span = digits.AsSpan();
        int i = 0;
        while (i + period < span.Length && span[^(i + 1)] == span[^(i + period + 1)])
        {
            i++;
        }

        _valuation = valuation;
        _period = period;
        _digits = digits;
    }

    /// <summary>The p-adic valuation of the number.</summary>
    /// <remarks>This also represents the shift in the p-adic expansion.</remarks>
    public int Valuation => _valuation;

    /// <summary>The length of the preperiodic part.</summary>
    public int PrePeriod => _digits.Length - _period;

    /// <summary>The length of the periodic part.</summary>
    public int Period => _period;

    private void SetFields<U>(Rational<T, U> q, out int period, out T[] digits)
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    {
        var gcd = Number.GCD(_prime, q.Den);
        period = Modular.MultiplicativeOrder(_prime, q.Den / gcd).AsInt();

        if (gcd != T.One)
        {
            var shift = PAdic.Valuation(_prime, q.Den);
            q = new(q.Num, q.Den / IBinaryInteger<T>.Pow(_prime, shift));
        }

        HashSet<Rational<T, U>> coefficients = [];
        List<T> buffer = [];
        bool started = false;
        while (!coefficients.Contains(q))
        {
            _ = coefficients.Add(q);
            var remainder = Number.ModDivideRemainder(ref q, _prime);
            if (!started && remainder == T.Zero)
                continue;
            else
                started = true;
            buffer.Add(remainder);
        }
        digits = [.. buffer];
    }

    //
    // Formatting
    //

    /// <inheritdoc />
    /// <remarks>Make sure the output encoding is set to <see cref="Encoding.Unicode"/> so that the string is displayed properly.</remarks>
    public override readonly string ToString() => ToString(null, null);

    /// <inheritdoc />
    /// <remarks>
    /// <para>P-adic numbers are displayed in quote notation.</para>
    /// <list type="bullet">
    ///   <listheader>
    ///     <term>Formats</term>
    ///     <description>Available formats for p-adic numbers.</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Default</term>
    ///     <description>Display a subscript indicating p at the bottom right of the number. Please make sure the output encoding is set to <see cref="Encoding.Unicode"/> so that the string is displayed properly.</description>
    ///   </item>
    ///   <item>
    ///     <term>Basic</term>
    ///     <description>Display p-adic in parenthesis.</description>
    ///   </item>
    ///   <item>
    ///     <term>Simple</term>
    ///     <description>Display p in parenthesis.</description>
    ///   </item>
    ///   <item>
    ///     <term>Clean</term>
    ///     <description>Display no number indicating p.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "Default";
        formatProvider ??= NumberFormatInfo.InvariantInfo;

        var label = format switch
        {
            var s when string.Equals(s, "Default", StringComparison.OrdinalIgnoreCase) => $"{_prime.ToString(null, formatProvider)}".ToSubscript(),
            var s when string.Equals(s, "Basic", StringComparison.OrdinalIgnoreCase) => $" ({_prime.ToString(null, formatProvider)}-adic)",
            var s when string.Equals(s, "Simple", StringComparison.OrdinalIgnoreCase) => $" ({_prime.ToString(null, formatProvider)})",
            var s when string.Equals(s, "Clean", StringComparison.OrdinalIgnoreCase) => null,
            _ => format
        };

        if (string.Equals(label, format, StringComparison.OrdinalIgnoreCase))
            return format;
        return StringHelper(label);
    }

    private string StringHelper(string? label)
    {
        StringBuilder builder = new();
        for (int i = _digits.Length - 1; i >= 0; i--)
        {
            _ = builder.Append(_digits[i]);
        }

        _ = builder.Insert(_digits.Length - (_digits.Length == _period ? 0 : 1), '\'');

        if (_valuation != 0)
            _ = builder.Append($"E{(_valuation > 0 ? "+" : "")}{_valuation}");
        if (!string.IsNullOrEmpty(label))
            _ = builder.Append(label);

        return builder.ToString();
    }

    /// <inheritdoc />
    /// <remarks>
    /// <para>P-adic numbers are displayed in quote notation.</para>
    /// <list type="bullet">
    ///   <listheader>
    ///     <term>Formats</term>
    ///     <description>Available formats for p-adic numbers.</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Default</term>
    ///     <description>Display a subscript indicating p at the bottom right of the number. Please make sure the output encoding is set to <see cref="Encoding.Unicode"/> so that the string is displayed properly.</description>
    ///   </item>
    ///   <item>
    ///     <term>Basic</term>
    ///     <description>Display p-adic in parenthesis.</description>
    ///   </item>
    ///   <item>
    ///     <term>Simple</term>
    ///     <description>Display p in parenthesis.</description>
    ///   </item>
    ///   <item>
    ///     <term>Clean</term>
    ///     <description>Display no number indicating p.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        format = format.IsEmpty ? "Default" : format;
        provider ??= NumberFormatInfo.InvariantInfo;

        var charsCurrentlyWritten = 0;
        var label = format switch
        {
#pragma warning disable EPS06
            var s when s.Equals("Default", StringComparison.OrdinalIgnoreCase) => $"{_prime.ToString(null, provider)}".ToSubscript(),
            var s when s.Equals("Basic", StringComparison.OrdinalIgnoreCase) => $" ({_prime.ToString(null, provider)}-adic)",
            var s when s.Equals("Simple", StringComparison.OrdinalIgnoreCase) => $" ({_prime.ToString(null, provider)})",
            var s when s.Equals("Clean", StringComparison.OrdinalIgnoreCase) => [],
#pragma warning restore EPS06
            _ => format
        };

#pragma warning disable EPS06
        if (label.Equals(format, StringComparison.OrdinalIgnoreCase))
#pragma warning restore EPS06
        {
            format.CopyTo(destination);
            charsWritten = format.Length;
            return true;
        }

        // Need at least 1 character to write 0.
        if (destination.Length < 1)
        {
            charsWritten = charsCurrentlyWritten;
            return false;
        }

        var isPurelyRepeating = _digits.Length == _period;
        for (int i = _digits.Length - 1; i >= 0; i--)
        {
            if (!isPurelyRepeating && _digits.Length - _period - 1 == i)
                destination[charsCurrentlyWritten++] = '\'';
            bool tryFormatSucceeded = _digits[i].TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
            charsCurrentlyWritten += tryFormatCharsWritten;
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }
        }

        if (isPurelyRepeating)
            destination[charsCurrentlyWritten++] = '\'';

        if (_valuation != 0)
        {
            destination[charsCurrentlyWritten++] = 'E';
            if (_valuation > 0)
                destination[charsCurrentlyWritten++] = '+';
            bool tryFormatSucceeded = _valuation.TryFormat(destination[charsCurrentlyWritten..], out int tryFormatCharsWritten, null, provider);
            charsCurrentlyWritten += tryFormatCharsWritten;
            if (!tryFormatSucceeded || destination.Length < charsCurrentlyWritten + 1)
            {
                charsWritten = charsCurrentlyWritten;
                return false;
            }
        }

        if (!label.IsEmpty)
        {
            label.CopyTo(destination[charsCurrentlyWritten..]);
            charsCurrentlyWritten += label.Length;
        }

        charsWritten = charsCurrentlyWritten;
        return true;
    }

    //
    // Methods
    //

    public Rational<T, U> ToRational<U>()
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    {
        ReadOnlySpan<T> digits = _digits.AsSpan();
        ReadOnlySpan<T> preperiod = digits[..^_period];
        ReadOnlySpan<T> repeating = digits[preperiod.Length..digits.Length];

        var a = T.Zero;
        for (int i = 0; i < preperiod.Length; i++)
        {
            a += preperiod[i] * T.CreateSaturating(IBinaryInteger<T>.Pow(_prime, i));
        }

        var b = T.Zero;
        for (int i = 0; i < _period; i++)
        {
            b += repeating[i] * T.CreateSaturating(IBinaryInteger<T>.Pow(_prime, i));
        }

        Rational<T, U> periodic = new(b, T.One - IBinaryInteger<T>.Pow(_prime, _period));
        periodic = periodic.Reduce();

        return (a + IBinaryInteger<T>.Pow(_prime, preperiod.Length) * periodic) * Rational<T, U>.Pow(_prime, _valuation);
    }
}
