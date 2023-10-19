// <copyright file="IDifferentiableFunctions.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core;

/// <summary>Defines support for common differentiable functions</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
public interface IDifferentiableFunctions<T> : IComplex<T>
    where T : IDifferentiableFunctions<T>
{
    //
    // Exponential functions
    //

    /// <summary>Compute e raised to the power of <paramref name="x"/>: $ e^x $</summary>
    /// <param name="x">A value</param>
    /// <returns>e raised to the power of <paramref name="x"/></returns>
    static abstract T Exp(T x);

    /// <summary>Compute 2 raised to the power of <paramref name="x"/>: $ 2^x $</summary>
    /// <param name="x">A value</param>
    /// <returns>2 raised to the power of <paramref name="x"/></returns>
    static abstract T Exp2(T x);

    /// <summary>Compute 10 raised to the power of <paramref name="x"/>: $ 10^x $</summary>
    /// <param name="x">A value</param>
    /// <returns>10 raised to the power of <paramref name="x"/></returns>
    static abstract T Exp10(T x);

    //
    // Hyperbolic functions
    //

    /// <summary>Compute the hyperbolic arccosine of <paramref name="x"/>: $ \cosh^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic arccosine of <paramref name="x"/></returns>
    static abstract T Acosh(T x);

    /// <summary>Compute the hyperbolic arcsine of <paramref name="x"/>: $ \sinh^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic arcsine of <paramref name="x"/></returns>
    static abstract T Asinh(T x);

    /// <summary>Compute the hyperbolic arctangent of <paramref name="x"/>: $ \tanh^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic arctangent of <paramref name="x"/></returns>
    static abstract T Atanh(T x);

    /// <summary>Compute the hyperbolic cosine of <paramref name="x"/>: $ \cosh(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic cosine of <paramref name="x"/></returns>
    static abstract T Cosh(T x);

    /// <summary>Compute the hyperbolic sine of <paramref name="x"/>: $ \sinh(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic sine of <paramref name="x"/></returns>
    static abstract T Sinh(T x);

    /// <summary>Compute the hyperbolic tangent of <paramref name="x"/>: $ \tanh(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The hyperbolic tangent of <paramref name="x"/></returns>
    static abstract T Tanh(T x);

    //
    // Logarithmic functions
    //

    /// <summary>Compute the natural logarithm of <paramref name="x"/>: $ \ln(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The natural logarithm of<paramref name="x"/></returns>
    static abstract T Ln(T x);

    /// <summary>Compute the logarithm of <paramref name="x"/> to base <paramref name="b"/>: $ \log_b(x) $</summary>
    /// <param name="x">A value</param>
    /// <param name="b">A base</param>
    /// <returns>The natural logarithm of <paramref name="x"/> to base b</returns>
    static abstract T Log(T x, T b);

    /// <summary>Compute the logarithm of <paramref name="x"/> to base 2: $ \log_2(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The natural logarithm of <paramref name="x"/> to base 2</returns>
    static abstract T Log2(T x);

    /// <summary>Compute the logarithm of <paramref name="x"/> to base 10: $ \log_{10}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The natural logarithm of <paramref name="x"/> to base 10</returns>
    static abstract T Log10(T x);

    //
    // Power functions
    //

    /// <summary>Compute <paramref name="x"/> raised to the power of <paramref name="y"/>: $ x^y $ </summary>
    /// <param name="x">A value</param>
    /// <param name="y">A power</param>
    /// <returns><paramref name="x"/> raised to the power of <paramref name="y"/></returns>
    static abstract T Pow(T x, T y);

    //
    // Root functions
    //

    /// <summary>Compute the cuberoot of <paramref name="x"/>: $ \sqrt[3]{x} $</summary>
    /// <param name="x">A value</param>
    /// <returns>The cuberoot of <paramref name="x"/></returns>
    static abstract T Cbrt(T x);

    /// <summary>Compute the <paramref name="n"/>th root of <paramref name="x"/>: $ \sqrt[n]{x} $</summary>
    /// <param name="x">A value</param>
    /// <returns>The <paramref name="n"/>th root of <paramref name="x"/></returns>
    static abstract T Root(T x, T n);

    /// <summary>Compute the square root of <paramref name="x"/>: $ \sqrt{x} $</summary>
    /// <param name="x">A value</param>
    /// <returns>The square root of <paramref name="x"/></returns>
    static abstract T Sqrt(T x);

    //
    // Trigonometric functions
    //

    /// <summary>Compute the arccosine of <paramref name="x"/>: $ \cos^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The arccosine of <paramref name="x"/></returns>
    static abstract T Acos(T x);

    /// <summary>Compute the arcsine of <paramref name="x"/>: $ \sin^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The arcsine of <paramref name="x"/></returns>
    static abstract T Asin(T x);

    /// <summary>Compute the arctangent of <paramref name="x"/>: $ \tan^{-1}(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The arctangent of <paramref name="x"/></returns>
    static abstract T Atan(T x);

    /// <summary>Compute the cosine of <paramref name="x"/>: $ \cos(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The cosine of <paramref name="x"/></returns>
    static abstract T Cos(T x);

    /// <summary>Compute the sine of <paramref name="x"/>: $ \sin(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The sine of <paramref name="x"/></returns>
    static abstract T Sin(T x);

    /// <summary>Compute the tangent of <paramref name="x"/>: $ \tan(x) $</summary>
    /// <param name="x">A value</param>
    /// <returns>The tangent of <paramref name="x"/></returns>
    static abstract T Tan(T x);
}
