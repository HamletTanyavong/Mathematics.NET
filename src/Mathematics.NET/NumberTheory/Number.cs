// <copyright file="Number.cs" company="Mathematics.NET">
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

using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mathematics.NET.NumberTheory;

/// <summary>A class containing methods for Number Theory.</summary>
public static class Number
{
    /// <summary>Convert an integer <paramref name="n"/> from base <paramref name="oldBase"/> to base <paramref name="newBase"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/> and <see cref="ISignedNumber{TSelf}"/>.</typeparam>
    /// <param name="n">An integer.</param>
    /// <param name="oldBase">The old base.</param>
    /// <param name="newBase">The new base.</param>
    /// <returns><paramref name="n"/> in base <paramref name="newBase"/>.</returns>
    public static T ChangeBase<T>(T n, T oldBase, T newBase)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        var m = T.Abs(n);
        var result = T.Zero;
        for (int i = 0; m != T.Zero; i++)
        {
            result += ModDivideRemainder(ref m, newBase) * IBinaryInteger<T>.Pow(oldBase, i);
        }
        return T.CopySign(result, n);
    }

    /// <summary>Compute the digit sum of <paramref name="n"/> in base <paramref name="b"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="b">A base.</param>
    /// <param name="n">An integer.</param>
    /// <returns>The digit sum.</returns>
    /// <exception cref="MathematicsException">Thrown when <paramref name="b"/> is less than or equal to zero.</exception>
    public static T DigitSum<T>(T b, T n)
        where T : IBinaryInteger<T>
    {
        if (b < T.One)
            throw new MathematicsException("The base must be greater than zero.");
        if (b == T.One)
            return n;
        T result = T.Zero;
        while (n != T.Zero)
        {
            (n, var r) = T.DivRem(n, b);
            result += r;
        }
        return result;
    }

    /// <summary>Get the divisor count of <paramref name="n"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="n">An integer.</param>
    /// <returns>The divisor count of <paramref name="n"/>.</returns>
    public static T DivisorCount<T>(T n)
        where T : IBinaryInteger<T>
    {
        Prime.Factor(n, out Dictionary<T, T> factors);
        T count = T.One;
        foreach (var factor in factors)
        {
            count *= factor.Value + T.One;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T DivisorCount<T>(Dictionary<T, T> factors)
        where T : IBinaryInteger<T>
    {
        T count = T.One;
        foreach (var factor in factors)
        {
            count *= factor.Value + T.One;
        }
        return count;
    }

    /// <summary>Find the divisors of <paramref name="n"/>.</summary>
    /// <param name="n">A positive integer.</param>
    /// <returns>The divisors of <paramref name="n"/>.</returns>
    public static ImmutableArray<T> Divisors<T>(T n)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        Prime.Factor(n, out Dictionary<T, T> primes);

        T[] divisors = new T[DivisorCount(primes).AsInt()];
        ref var start = ref MemoryMarshal.GetReference(new Span<T>(divisors));

        var i = start = T.One;
        foreach (var prime in primes)
        {
            var j = T.Zero;
            while (j < i)
            {
                for (T k = T.Zero; k < prime.Value; k++)
                {
                    Debug.Assert((i + j + k).AsInt() < divisors.Length);
                    Unsafe.Add(ref start, (i + j + k).AsInt()) = Unsafe.Add(ref start, j.AsInt()) * IBinaryInteger<T>.Pow(prime.Key, k + T.One);
                }
                j++;
            }
            i += i * prime.Value;
        }

        Array.Sort(divisors);

        return ImmutableCollectionsMarshal.AsImmutableArray(divisors);
    }

    /// <summary>Compute the number of positive integers up to <paramref name="n"/> that are coprime to <paramref name="n"/>.</summary>
    /// <param name="n">An integer.</param>
    /// <returns>The number of integers coprime to <paramref name="n"/>.</returns>
    public static T EulerTotientFunction<T>(T n)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        Prime.Factor(n, out HashSet<T> primes);
        Rational<T, double> product = T.One;
        foreach (var prime in primes)
        {
            product *= T.One - new Rational<T, double>(T.One, prime);
        }
        var result = n * product;
        Debug.Assert(result.Den == T.One, "Euler's Totient Function must return an integer.");
        return result.Num;
    }

    /// <summary>Compute the greatest common divisor of two integers.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="b">An integer.</param>
    /// <returns>The GCD of the two values.</returns>
    public static T GCD<T>(T a, T b)
        where T : IBinaryInteger<T>
    {
        a = T.Abs(a);
        b = T.Abs(b);
        while (a != T.Zero && b != T.Zero)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }
        return a | b;
    }

    /// <summary>Compute the greatest common divisor of two integers using the Extended Euclidean Algorithm.</summary>
    /// <remarks>Solve an equation of the form <paramref name="a"/>x + <paramref name="b"/>y ≡ gcd(<paramref name="a"/>, <paramref name="b"/>).</remarks>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="b">An integer.</param>
    /// <param name="x">A coefficient.</param>
    /// <param name="y">A coefficient.</param>
    /// <returns>The GCD of the two values.</returns>
    public static T GCD<T>(T a, T b, out T x, out T y)
        where T : IBinaryInteger<T>
    {
        x = T.One;
        y = T.Zero;

        T xn = T.Zero;
        T yn = T.One;

        while (b != T.Zero)
        {
            T q = a / b;
            (x, xn) = (xn, x - q * xn);
            (y, yn) = (yn, y - q * yn);
            (a, b) = (b, a - q * b);
        }

        return T.Abs(a);
    }

    /// <summary>Find the least common multiple of two numbers.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="p">An integer.</param>
    /// <param name="q">An integer.</param>
    /// <returns>The LCM of the two values.</returns>
    public static T LCM<T>(T p, T q)
        where T : IBinaryInteger<T>
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

    /// <summary>Compute the mod of <paramref name="dividend"/>, divide it by <paramref name="divisor"/>, and return the remainder.</summary>
    /// <remarks>This method is used for base conversions.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="dividend">The dividend.</param>
    /// <param name="divisor">The divisor.</param>
    /// <returns>The remainder.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ModDivideRemainder<T>(ref T dividend, T divisor)
        where T : IBinaryInteger<T>
    {
        var remainder = Modular.Mod(dividend, divisor);
        dividend = (dividend - remainder) / divisor;
        return remainder;
    }

    /// <inheritdoc cref="ModDivideRemainder{T}(ref T, T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ModDivideRemainder<T, U>(ref Rational<T, U> dividend, T divisor)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    {
        var remainder = Modular.Mod(dividend, divisor);
        dividend = (dividend - remainder) / divisor;
        return remainder;
    }
}
