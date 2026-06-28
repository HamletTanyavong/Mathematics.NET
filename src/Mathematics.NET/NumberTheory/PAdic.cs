// <copyright file="PAdic.cs" company="Mathematics.NET">
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

using System.Numerics;

namespace Mathematics.NET.NumberTheory;

/// <summary>Perform p-adic analysis.</summary>
public static class PAdic
{
    /// <summary>Compute the digit sum of the <paramref name="p"/>-adic expansion of <paramref name="n"/></summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="n">An integer.</param>
    /// <returns>The digit sum.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static T DigitSum<T>(T p, T n)
        where T : IBinaryInteger<T>
    {
        if (n < T.Zero)
            throw new NotImplementedException("An implementation for negative integers has not been added.");
        T result = T.Zero;
        while (n > T.Zero)
        {
            T r;
            (n, r) = T.DivRem(n, p);
            result += r;
        }
        return result;
    }

    /// <summary>Perform a <paramref name="p"/>-adic expansion of <paramref name="n"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="n">An integer.</param>
    /// <returns>A list of the digits.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<T> Expand<T>(T p, T n)
        where T : IBinaryInteger<T>
    {
        if (n < T.Zero)
            throw new NotImplementedException("An implementation for negative integers has not been added.");
        List<T> result = [];
        while (n > T.Zero)
        {
            T r;
            (n, r) = T.DivRem(n, p);
            result.Add(r);
        }
        return result;
    }

    /// <summary>Compute the <paramref name="p"/>-adic valuation of a binomial coefficient using Kummer's Theorem.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="n">The <paramref name="n"/> in <paramref name="n"/> choose <paramref name="k"/>.</param>
    /// <param name="k">The <paramref name="k"/> in <paramref name="n"/> choose <paramref name="k"/>.</param>
    /// <returns>The exponent of the largest power of <paramref name="p"/> that divides <paramref name="n"/> choose <paramref name="k"/>.</returns>
    /// <exception cref="MathematicsException"></exception>
    public static T Kummer<T>(T p, T n, T k)
        where T : IBinaryInteger<T>
    {
        if (p <= T.One)
            throw new MathematicsException("p must be a prime number.");
        if (T.IsNegative(n) || T.IsNegative(k) || n < k)
            throw new MathematicsException("Invalid n and/or k.");
        return (DigitSum(p, k) + DigitSum(p, n - k) - DigitSum(p, n)) / (p - T.One);
    }

    /// <summary>Compute the <paramref name="p"/>-adic valuation of a multinomial using the generalized Kummer's Theorem.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="k">The parameters of the multinomial.</param>
    /// <returns>The exponent of the largest power of <paramref name="p"/> that divides the multinomial.</returns>
    /// <exception cref="MathematicsException"></exception>
    public static T Kummer<T>(T p, params ReadOnlySpan<T> k)
        where T : IBinaryInteger<T>
    {
        if (p <= T.One)
            throw new MathematicsException("p must be a prime number.");
        T n = T.Zero;
        T sum = T.Zero;
        for (int i = 0; i < k.Length; i++)
        {
            var j = k[i];
            if (j < T.Zero)
                throw new MathematicsException("Invalid element of k.");
            n += j;
            sum += DigitSum(p, j);
        }
        return (sum - DigitSum(p, n)) / (p - T.One);
    }

    /// <summary>Calculate the <paramref name="p"/>-adic valuation of <paramref name="n"/>! using Legendre's Formula.</summary>
    /// <param name="p">A prime number.</param>
    /// <param name="n">An integer.</param>
    /// <returns>The exponent of the largest power of <paramref name="p"/> that divides <paramref name="n"/>!.</returns>
    public static int Legendre(int p, int n)
    {
        int value = 0;
        for (int i = 1; i <= (int)Math.Floor(Math.Log(n, p)); i++)
        {
            value += (int)Math.Floor(n / Math.Pow(p, i));
        }
        return value;
    }

    /// <summary>Compute the <paramref name="p"/>-adic norm, also known as the <paramref name="p"/>-adic absolute value, of <paramref name="q"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="q">A rational number.</param>
    /// <returns>The <paramref name="p"/>-adic norm of <paramref name="q"/>.</returns>
    public static Rational<int, T> Norm<T>(int p, Rational<int, T> q)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var v = Valuation(p, q);
        return Rational<int, T>.Pow(p, -v);
    }

    /// <summary>Calculate the <paramref name="p"/>-adic valuation of an integer <paramref name="n"/>.</summary>
    /// <param name="p">A prime number.</param>
    /// <param name="n">An integer.</param>
    /// <returns>The exponent of the largest power of <paramref name="p"/> that divides <paramref name="n"/>.</returns>
    public static int Valuation(int p, int n)
    {
        var abs = Math.Abs(n);
        int value = 0;
        for (int i = 1; i <= (int)Math.Floor(Math.Log(abs, p)); i++)
        {
            var pow = Math.Pow(p, i);
            value += (int)Math.Floor(abs / pow) - (int)Math.Floor((abs - 1) / pow);
        }
        return value;
    }

    /// <summary>Calculate the <paramref name="p"/>-adic valuation of a rational number <paramref name="q"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
    /// <param name="p">A prime number.</param>
    /// <param name="q">A rational number.</param>
    /// <returns>The exponent of the largest power of <paramref name="p"/> that divides <paramref name="q"/>.</returns>
    public static int Valuation<T>(int p, Rational<int, T> q)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => Valuation(p, q.Num) - Valuation(p, q.Den);
}
