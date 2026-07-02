// <copyright file="Modular.cs" company="Mathematics.NET">
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
using System.Runtime.CompilerServices;

namespace Mathematics.NET.NumberTheory;

/// <summary>A class containing methods for modular arithmetic.</summary>
public static class Modular
{
    /// <summary>Compute the multiplicative inverse of <paramref name="a"/> in the congruence <paramref name="a"/>x ≡ 1 (mod <paramref name="m"/>).</summary>
    /// <remarks><paramref name="a"/> and <paramref name="m"/> must be coprime.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="m">A modulus.</param>
    /// <returns>The multiplicative inverse of <paramref name="a"/> if it exists; otherwise, <c>-1</c>.</returns>
    public static T Inverse<T>(T a, T m)
        where T : IBinaryInteger<T>
    {
        if (m == T.One)
            return T.Zero;
        // Turn a positive if it is negative.
        if (a < T.Zero)
            a = Mod(a, m);

        var x = T.One;
        var xn = T.Zero;
        var mod = m;

        while (m != T.Zero)
        {
            T q = a / m;
            (x, xn) = (xn, x - q * xn);
            (a, m) = (m, a - q * m);
        }

        if (a > T.One)
            return -T.One;

        return x < T.Zero ? x + mod : x;
    }

    /// <summary>Solve a congruence of the form <paramref name="a"/>x ≡ <paramref name="b"/> (mod <paramref name="m"/>).</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="b">An integer.</param>
    /// <param name="m">A modulus.</param>
    /// <returns>
    /// If a solution exists, a value tuple with the first element as the least, non-negative solution to <paramref name="a"/>x ≡ <paramref name="b"/> (mod <paramref name="m"/>) and the second element as the new modulus. If no solution exists, a value tuple of zeroes.
    /// </returns>
    /// <exception cref="MathematicsException">Thrown when there are no solutions.</exception>
    public static (T X, T Modulus) LinearCongruence<T>(T a, T b, T m)
        where T : IBinaryInteger<T>
    {
        var gcd = Number.GCD(a, m);

        (b, var r) = T.DivRem(b, gcd);
        if (r != T.Zero)
            throw new MathematicsException($"Linear congruence has no solutions.");
        a /= gcd;
        m /= gcd;

        var inverse = Inverse(a, m);

        return (Mod(inverse * b, m), m);
    }

    /// <summary>Compute <paramref name="x"/> (mod <paramref name="m"/>).</summary>
    /// <remarks>This method works for negative integers.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="x">An integer.</param>
    /// <param name="m">A modulus.</param>
    /// <returns><paramref name="x"/> (mod <paramref name="m"/>).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Mod<T>(T x, T m)
        where T : IBinaryInteger<T>
    {
        T r = x % m;
        return r < T.Zero ? r + m : r;
    }

    /// <summary>Compute <paramref name="p"/> (mod <paramref name="m"/>).</summary>
    /// <remarks>The numerator and denominator of <paramref name="p"/> must be coprime.</remarks>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/> and <see cref="ISignedNumber{TSelf}"/>.</typeparam>
    /// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
    /// <param name="p">A rational number.</param>
    /// <param name="m">A modulus.</param>
    /// <returns><paramref name="p"/> (mod <paramref name="m"/>).</returns>
    public static T Mod<T, U>(Rational<T, U> p, T m)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        => LinearCongruence(p.Den, p.Num, m).X;

    /// <summary>Compute the mutiplicative order of <paramref name="a"/> (mod <paramref name="m"/>).</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="m">A modulus.</param>
    /// <returns>The multiplicative order of <paramref name="a"/> (mod <paramref name="m"/>).</returns>
    public static T MultiplicativeOrder<T>(T a, T m)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        var etf = Number.EulerTotientFunction(m);
        var divisors = Number.Divisors(etf);
        for (int i = 0; i < divisors.Length; i++)
        {
            if (Pow(a, divisors[i], m) == T.One)
                return divisors[i];
        }
        return T.Zero;
    }

    /// <summary>Perform modular multiplication, <paramref name="a"/> * <paramref name="b"/> (mod <paramref name="m"/>).</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="b">An integer.</param>
    /// <param name="m">A modulus.</param>
    /// <returns><paramref name="a"/> * <paramref name="b"/> (mod <paramref name="m"/>).</returns>
    public static T Multiply<T>(T a, T b, T m)
        where T : IBinaryInteger<T>
    {
        if (T.Abs(a) > T.Abs(b))
            (a, b) = (b, a);
        var sign = T.Sign(a);
        var result = T.Zero;
        while (a != T.Zero)
        {
            if ((a & T.One) == T.One)
                result = (sign > 0 ? result + b : result - b) % m;
            (a, b) = (a / T.CreateSaturating(2), T.CreateSaturating(2) * b % m);
        }
        return result < T.Zero ? result + m : result;
    }

    /// <summary>Perform modular exponentiation, <paramref name="a"/>^<paramref name="x"/> (mod <paramref name="m"/>).</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="a">An integer.</param>
    /// <param name="x">An integer exponent.</param>
    /// <param name="m">A modulus.</param>
    /// <returns><paramref name="a"/>^<paramref name="x"/> (mod <paramref name="m"/>).</returns>
    /// <exception cref="MathematicsException">Thrown when there are no solutions.</exception>
    public static T Pow<T>(T a, T x, T m)
        where T : IBinaryInteger<T>
    {
        if (m == T.One)
            return T.Zero;
        if (x == T.Zero)
            return a >= T.Zero ? T.One : m - T.One;

        if (x < T.Zero)
        {
            a = LinearCongruence(a, T.One, m).X;
            x = T.Abs(x);
        }
        else
        {
            a %= m;
        }

        var result = T.One;
        while (x > T.Zero)
        {
            if ((x & T.One) == T.One)
                result = Multiply(result, a, m);
            a = Multiply(a, a, m);
            x >>= 1;
        }
        return result;
    }
}
