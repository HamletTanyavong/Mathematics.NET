// <copyright file="Prime.cs" company="Mathematics.NET">
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

// TODO: Implement the Sieve Of Atkin: <see href="https://www.ams.org/journals/mcom/2004-73-246/S0025-5718-03-01501-1/S0025-5718-03-01501-1.pdf"><i>Prime Sieves Using Binary Quadratic Forms</i></see> by A. O. L. Atkin and D. J. Bernstein.

using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Mathematics.NET.NumberTheory;

/// <summary>Provides methods for working with prime numbers.</summary>
public static class Prime
{
    /// <summary>An upper bound to the prime counting function.</summary>
    /// <remarks>
    /// The inequality used is
    /// <code>
    /// pi(x) &lt; 1.25506 * x / ln(x) for x > 1.
    /// </code>
    /// See <see href="https://projecteuclid.org/journalArticle/Download?urlId=10.1215%2Fijm%2F1255631807"><i>Approximate Formulas for Some Functions of Prime Numbers</i></see> by J. Barkley Rosser and Lowell Schoenfeld.
    /// </remarks>
    /// <param name="limit">The limit.</param>
    /// <returns>An upper bound to the number of prime numbers less than <paramref name="limit"/>.</returns>
    public static Real<T> CountingFunctionUpperBound<T>(Real<T> limit)
        where T : IBinaryFloatingPointIeee754<T>, IMinMaxValue<T>
        => limit / Real<T>.Ln(limit) * T.CreateSaturating(1.22506);

    /// <summary>Find the prime factors of a number.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="n">An integer.</param>
    /// <param name="factors">A list of prime factors.</param>
    public static void Factor<T>(T n, out List<T> factors)
        where T : IBinaryInteger<T>
    {
        factors = [];

        while (n % T.CreateSaturating(2) == T.Zero)
        {
            factors.Add(T.CreateSaturating(2));
            n /= T.CreateSaturating(2);
        }
        while (n % T.CreateSaturating(3) == T.Zero)
        {
            factors.Add(T.CreateSaturating(3));
            n /= T.CreateSaturating(3);
        }
        while (n % T.CreateSaturating(5) == T.Zero)
        {
            factors.Add(T.CreateSaturating(5));
            n /= T.CreateSaturating(5);
        }

        ReadOnlySpan<T> increments = [
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(6),
            T.CreateSaturating(2),
            T.CreateSaturating(6)];

        var i = 0;
        var j = T.CreateSaturating(7);
        while (j * j <= n)
        {
            if (n % j == T.Zero)
            {
                factors.Add(j);
                n /= j;
            }
            else
            {
                j += increments[i];
                if (i < 7)
                    i++;
                else
                    i = 0;
            }
        }

        if (n > T.One)
            factors.Add(n);
    }

    /// <summary>Find the prime factors of a number.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="n">An integer.</param>
    /// <param name="factors">A dictionary of prime factors as keys and their count as values.</param>
    public static void Factor<T>(T n, out Dictionary<T, T> factors)
        where T : IBinaryInteger<T>
    {
        factors = [];

        while (n % T.CreateSaturating(2) == T.Zero)
        {
            AddAndIncrement(factors, T.CreateSaturating(2), ref n);
        }
        while (n % T.CreateSaturating(3) == T.Zero)
        {
            AddAndIncrement(factors, T.CreateSaturating(3), ref n);
        }
        while (n % T.CreateSaturating(5) == T.Zero)
        {
            AddAndIncrement(factors, T.CreateSaturating(5), ref n);
        }

        ReadOnlySpan<T> increments = [
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(6),
            T.CreateSaturating(2),
            T.CreateSaturating(6)];

        var i = 0;
        var j = T.CreateSaturating(7);
        while (j * j <= n)
        {
            if (n % j == T.Zero)
            {
                AddAndIncrement(factors, j, ref n);
            }
            else
            {
                j += increments[i];
                if (i < 7)
                    i++;
                else
                    i = 0;
            }
        }

        if (n > T.One)
            AddAndIncrement(factors, n, ref n);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddAndIncrement(Dictionary<T, T> factors, T factor, ref T n)
        {
            if (!factors.TryAdd(factor, T.One))
                factors[factor] += T.One;
            n /= factor;
        }
    }

    /// <summary>Find the distinct prime factors of a number.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="n">An integer.</param>
    /// <param name="factors">A hashset of distinct prime factors.</param>
    public static void Factor<T>(T n, out HashSet<T> factors)
        where T : IBinaryInteger<T>
    {
        factors = [];

        if (n % T.CreateSaturating(2) == T.Zero)
            Add(factors, T.CreateSaturating(2), ref n);
        if (n % T.CreateSaturating(3) == T.Zero)
            Add(factors, T.CreateSaturating(3), ref n);
        if (n % T.CreateSaturating(5) == T.Zero)
            Add(factors, T.CreateSaturating(5), ref n);

        ReadOnlySpan<T> increments = [
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(2),
            T.CreateSaturating(4),
            T.CreateSaturating(6),
            T.CreateSaturating(2),
            T.CreateSaturating(6)];

        var i = 0;
        var j = T.CreateSaturating(7);
        while (j * j <= n)
        {
            if (n % j == T.Zero)
            {
                Add(factors, j, ref n);
            }
            else
            {
                j += increments[i];
                if (i < 7)
                    i++;
                else
                    i = 0;
            }
        }

        if (n > T.One)
            Add(factors, n, ref n);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Add(HashSet<T> factors, T factor, ref T n)
        {
            _ = factors.Add(factor);
            do
            {
                n /= factor;
            } while (n % factor == T.Zero);
        }
    }

    /// <summary>Use the Sieve of Eratosthenses to generate primes below a certain limit.</summary>
    /// <param name="wheel">A wheel generated by the first n primes.</param>
    /// <param name="limit">The limit.</param>
    /// <returns>A list of primes.</returns>
    public static IEnumerable<T> SieveOfEratosthenes<T>(Wheel<T> wheel, T limit)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        if (limit <= T.One)
            yield break;

        foreach (var prime in wheel.Basis)
        {
            if (prime <= limit)
                yield return prime;
        }

        BitArray array = new(limit.AsInt() - 1);
        var bound = IBinaryInteger<T>.FloorSqrt(limit);

        foreach (var candidate in wheel.Spin().TakeWhile(x => x <= limit))
        {
            if (candidate <= bound)
            {
                if (!array[candidate.AsInt() - 2])
                {
                    yield return candidate;
                    var j = candidate * candidate;
                    while (j > T.Zero && j <= limit)
                    {
                        array[j.AsInt() - 2] = true;
                        j += candidate;
                    }
                }
            }
            else
            {
                if (!array[candidate.AsInt() - 2])
                    yield return candidate;
            }
        }
    }
}
