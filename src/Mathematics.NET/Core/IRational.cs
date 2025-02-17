// <copyright file="IRational.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core;

/// <summary>Defines support for rational numbers.</summary>
/// <typeparam name="T">A type that implements the interface.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
public interface IRational<T, U> : IReal<T>
    where T : IRational<T, U>
    where U : IBinaryInteger<U>
{
    /// <summary>Get the numerator of the rational number.</summary>
    U Num { get; }

    /// <summary>Get the denominator of the rational number.</summary>
    virtual U Den => U.One;

    /// <summary>Compute the absolute value of a number.</summary>
    /// <param name="x">A rational number.</param>
    /// <returns>The absolute value.</returns>
    static new abstract T Abs(T x);

    /// <summary>Divide two rational numbers and get the quotient and remainder.</summary>
    /// <param name="dividend">The dividend.</param>
    /// <param name="divisor">The remainder.</param>
    /// <returns>The quotient as an integer and the remainder as a rational number.</returns>
    static abstract (U Quotient, T Remainder) QuoRem(T dividend, T divisor);

    /// <summary>Reduce a rational number.</summary>
    /// <param name="x">The value to reduce.</param>
    /// <returns>A reduced fraction if the number was reducible; otherwise, itself.</returns>
    static abstract T Reduce(T x);

    /// <summary>Convert a mixed fraction into an improper fraction.</summary>
    /// <param name="quotient">The quotient.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns>An improper fraction.</returns>
    static abstract T ToImproper(U quotient, T remainder);

    /// <summary>Convert a fraction into a mixed fraction if it is improper.</summary>
    /// <param name="x">A fraction.</param>
    /// <returns>A mixed fraction if the input fraction is improper; otherwise, the original fraction as the remainder.</returns>
    static abstract (U Quotient, T Remainder) ToMixed(T x);
}
