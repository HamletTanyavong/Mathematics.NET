// <copyright file="Combinatorics.cs" company="Mathematics.NET">
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

/// <summary>A class containing methods for combinatorics.</summary>
public static class Combinatorics
{
    /// <summary>Compute <paramref name="n"/> choose <paramref name="k"/>.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="n">The <paramref name="n"/> in <paramref name="n"/> choose <paramref name="k"/>.</param>
    /// <param name="k">The <paramref name="k"/> in <paramref name="n"/> choose <paramref name="k"/>.</param>
    /// <returns><paramref name="n"/> choose <paramref name="k"/>.</returns>
    public static T Binomial<T>(int n, int k)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        Wheel2357 wheel = new();
        T result = T.One;
        foreach (var prime in Prime.SieveOfEratosthenes(wheel, n))
        {
            var exponent = PAdic.Kummer(prime, n, k);
            result *= IBinaryInteger<T>.Pow(T.CreateTruncating(prime), T.CreateTruncating(exponent));
        }
        return result;
    }

    /// <summary>Compute a multinomial coefficient.</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="k">An array of positive integers.</param>
    /// <returns>The sum of all elements of <paramref name="k"/> choose the elements of <paramref name="k"/>.</returns>
    public static T Multinomial<T>(params int[] k)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        Wheel2357 wheel = new();
        T result = T.One;
        foreach (var prime in Prime.SieveOfEratosthenes(wheel, k.Sum()))
        {
            var exponent = PAdic.Kummer(prime, k);
            result *= IBinaryInteger<T>.Pow(T.CreateTruncating(prime), T.CreateTruncating(exponent));
        }
        return result;
    }
}
