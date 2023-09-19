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

using System.Numerics;

namespace Mathematics.NET.Core;

/// <summary>Defines support for common differentiable functions</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public interface IDifferentiableFunctions<T, U>
    : IComplex<T, U>
    where T : IDifferentiableFunctions<T, U>
    where U : IFloatingPointIeee754<U>
{
    //
    // Exponential functions
    //

    static abstract T Exp(T x);

    static abstract T Exp2(T x);

    static abstract T Exp10(T x);

    //
    // Hyperbolic functions
    //

    static abstract T Acosh(T x);

    static abstract T Asinh(T x);

    static abstract T Atanh(T x);

    static abstract T Cosh(T x);

    static abstract T Sinh(T x);

    static abstract T Tanh(T x);

    //
    // Logarithmic functions
    //

    static abstract T Ln(T x);

    static abstract T Log(T x, T b);

    static abstract T Log2(T x);

    static abstract T Log10(T x);

    //
    // Power functions
    //

    static abstract T Pow(T x, T y);

    //
    // Root functions
    //

    static abstract T Cbrt(T x);

    static abstract T NthRoot(T x, int n);

    static abstract T Root(T x, T r);

    static abstract T Sqrt(T x);

    //
    // Trigonometric functions
    //

    static abstract T Acos(T x);

    static abstract T Asin(T x);

    static abstract T Atan(T x);

    static abstract T Cos(T x);

    static abstract T Sin(T x);

    static abstract T Tan(T x);
}
