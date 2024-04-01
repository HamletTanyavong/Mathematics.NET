// <copyright file="ITape.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for tapes used in reverse-mode automatic differentiation</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
public interface ITape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    /// <summary>This property indicates whether or not the tape is currently tracking nodes.</summary>
    public bool IsTracking { get; set; }

    /// <summary>Get the number of nodes on the gradient tape.</summary>
    public int NodeCount { get; }

    /// <summary>Get the number of variables that are being tracked.</summary>
    public int VariableCount { get; }

    /// <summary>Create a variable for the gradient tape to track.</summary>
    /// <param name="seed">A seed value</param>
    /// <returns>A variable</returns>
    public Variable<T> CreateVariable(T seed);

    /// <summary>Print the nodes of the gradient tape to the console.</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <param name="limit">The total number of nodes to print</param>
    public void PrintNodes(CancellationToken cancellationToken, int limit = 100);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient</param>
    /// <exception cref="Exception">The gradient tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient</param>
    /// <param name="seed">A seed value</param>
    /// <exception cref="Exception">The gradient tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed);

    //
    // Basic operations
    //

    /// <summary>Add two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(Variable<T> x, Variable<T> y);

    /// <summary>Add a constant value and a variable</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(T c, Variable<T> x);

    /// <summary>Add a variable and a constant value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(Variable<T> x, T c);

    /// <summary>Divide two variables</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(Variable<T> x, Variable<T> y);

    /// <summary>Divide a constant value by a variable</summary>
    /// <param name="c">A constant dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(T c, Variable<T> x);

    /// <summary>Divide a variable by a constant value</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A constant divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(Variable<T> x, T c);

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/></returns>
    public Variable<Real> Modulo(Variable<Real> x, Variable<Real> y);

    /// <summary>Compute the modulo of a real value given a divisor</summary>
    /// <param name="c">A real dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns><paramref name="c"/> mod <paramref name="x"/></returns>
    public Variable<Real> Modulo(Real c, Variable<Real> x);

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A real divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="c"/></returns>
    public Variable<Real> Modulo(Variable<Real> x, Real c);

    /// <summary>Multiply two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(Variable<T> x, Variable<T> y);

    /// <summary>Multiply a constant value by a variable</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(T c, Variable<T> x);

    /// <summary>Multiply a variable by a constant value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(Variable<T> x, T c);

    /// <summary>Subract two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(Variable<T> x, Variable<T> y);

    /// <summary>Subtract a variable from a constant value</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(T c, Variable<T> x);

    /// <summary>Subtract a constant value from a variable</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(Variable<T> x, T c);

    //
    // Other operations
    //

    /// <summary>Negate a variable</summary>
    /// <param name="x">A variable</param>
    /// <returns>Minus one times the variable</returns>
    public Variable<T> Negate(Variable<T> x);

    // Exponential functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    public Variable<T> Exp(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    public Variable<T> Exp2(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    public Variable<T> Exp10(Variable<T> x);

    // Hyperbolic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    public Variable<T> Acosh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    public Variable<T> Asinh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    public Variable<T> Atanh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    public Variable<T> Cosh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    public Variable<T> Sinh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    public Variable<T> Tanh(Variable<T> x);

    // Logarithmic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    public Variable<T> Ln(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    public Variable<T> Log(Variable<T> x, Variable<T> b);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    public Variable<T> Log2(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    public Variable<T> Log10(Variable<T> x);

    // Power functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable<T> Pow(Variable<T> x, Variable<T> y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable<T> Pow(Variable<T> x, T y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable<T> Pow(T x, Variable<T> y);

    // Root functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    public Variable<T> Cbrt(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    public Variable<T> Root(Variable<T> x, Variable<T> n);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    public Variable<T> Sqrt(Variable<T> x);

    // Trigonometric function

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    public Variable<T> Acos(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    public Variable<T> Asin(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    public Variable<T> Atan(Variable<T> x);

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public Variable<Real> Atan2(Variable<Real> y, Variable<Real> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    public Variable<T> Cos(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    public Variable<T> Sin(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    public Variable<T> Tan(Variable<T> x);
}
