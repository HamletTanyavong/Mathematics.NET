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

using System.Diagnostics;
using System.Numerics;

namespace Mathematics.NET.Benchmarks.Implementations.NumberTheory;

internal class Combinatorics
{
    public static Real Binomial(uint n, uint k)
    {
        var result = 1.0;
        for (uint i = 0; i < k; i++)
            result *= (double)(n - i) / (i + 1);
        return result;
    }

    public static T Binomial<T>(T n, T k)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        Rational<T> num = new(n);
        Rational<T> result = T.One;
        for (T i = T.Zero; i < k; i++)
            result *= (num - i) / (i + T.One);
        Debug.Assert(result.Den != T.One, "The result must be a whole number.");
        return result.Num;
    }

    public static T Binomial<T>(T n, T k, CancellationToken cancellationToken)
        where T : IBinaryInteger<T>
    {
        var result = T.One;
        T u = T.One;
        for (T i = T.Zero; i < k; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result *= n - i;
            u *= i + T.One;

            var gcd = Algebra.GCD(result, u);
            if (gcd != T.One)
            {
                result /= gcd;
                u /= gcd;
            }
        }
        return result;
    }

    /// <summary>The multinomial coefficient.</summary>
    /// <param name="span">A read-only span of positive integers.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The coefficient associated with a term with powers given by <paramref name="span"/>.</returns>
    /// <exception cref="OverflowException"></exception>
    public static T Multinomial<T>(ReadOnlySpan<T> span, CancellationToken cancellationToken)
        where T : IBinaryInteger<T>
    {
        var result = T.One;
        checked
        {
            var partialSum = span[0];
            for (int i = 1; i < span.Length; i++)
            {
                var element = span[i];
                if (element < T.Zero)
                    throw new ArgumentException("Elements of the span must be positive.");
                partialSum += element;
                result *= Binomial(partialSum, element, cancellationToken);
            }
        }
        return result;
    }
}
