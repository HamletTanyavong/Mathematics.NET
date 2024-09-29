// <copyright file="ITape.cs" company="Mathematics.NET">
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

using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for tapes used in reverse-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
public interface ITape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    /// <summary>This property indicates whether or not the tape is currently tracking nodes.</summary>
    bool IsTracking { get; set; }

    /// <summary>Get the number of nodes on the gradient tape.</summary>
    int NodeCount { get; }

    /// <summary>Get the number of variables that are being tracked.</summary>
    int VariableCount { get; }

    /// <summary>Create a variable for the gradient tape to track.</summary>
    /// <param name="seed">A seed value.</param>
    /// <returns>A variable.</returns>
    Variable<T> CreateVariable(T seed);

    /// <summary>Log nodes tracked by the tape.</summary>
    /// <param name="logger">A logger.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="limit">The total number of nodes to log.</param>
    void LogNodes(ILogger<ITape<T>> logger, CancellationToken cancellationToken, int limit = 100);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <exception cref="Exception">The gradient tape does not have any tracked variables.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="seed">A seed value.</param>
    /// <exception cref="Exception">The gradient tape does not have any tracked variables.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed);

    //
    // Basic operations
    //

    /// <summary>Add two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Add(Variable<T> x, Variable<T> y);

    /// <summary>Add a constant value and a variable.</summary>
    /// <param name="c">A constant value.</param>
    /// <param name="x">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Add(T c, Variable<T> x);

    /// <summary>Add a variable and a constant value.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="c">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T> Add(Variable<T> x, T c);

    /// <summary>Divide two variables.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns>A variable.</returns>
    Variable<T> Divide(Variable<T> x, Variable<T> y);

    /// <summary>Divide a constant value by a variable.</summary>
    /// <param name="c">A constant dividend.</param>
    /// <param name="x">A variable divisor.</param>
    /// <returns>A variable.</returns>
    Variable<T> Divide(T c, Variable<T> x);

    /// <summary>Divide a variable by a constant value.</summary>
    /// <param name="x">A variable dividend.</param>
    /// <param name="c">A constant divisor.</param>
    /// <returns>A variabl.</returns>
    Variable<T> Divide(Variable<T> x, T c);

    /// <summary>Compute the modulo of a variable given a divisor.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/>.</returns>
    Variable<Real> Modulo(in Variable<Real> x, in Variable<Real> y);

    /// <summary>Compute the modulo of a real value given a divisor.</summary>
    /// <param name="c">A real dividend.</param>
    /// <param name="x">A variable divisor.</param>
    /// <returns><paramref name="c"/> mod <paramref name="x"/>.</returns>
    Variable<Real> Modulo(Real c, in Variable<Real> x);

    /// <summary>Compute the modulo of a variable given a divisor.</summary>
    /// <param name="x">A variable dividend.</param>
    /// <param name="c">A real divisor.</param>
    /// <returns><paramref name="x"/> mod <paramref name="c"/>.</returns>
    Variable<Real> Modulo(in Variable<Real> x, Real c);

    /// <summary>Multiply two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Multiply(Variable<T> x, Variable<T> y);

    /// <summary>Multiply a constant value by a variable.</summary>
    /// <param name="c">A constant value.</param>
    /// <param name="x">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Multiply(T c, Variable<T> x);

    /// <summary>Multiply a variable by a constant value.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="c">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T> Multiply(Variable<T> x, T c);

    /// <summary>Subract two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Subtract(Variable<T> x, Variable<T> y);

    /// <summary>Subtract a variable from a constant value.</summary>
    /// <param name="c">A constant value.</param>
    /// <param name="x">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T> Subtract(T c, Variable<T> x);

    /// <summary>Subtract a constant value from a variable.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="c">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T> Subtract(Variable<T> x, T c);

    //
    // Other operations
    //

    /// <summary>Negate a variable.</summary>
    /// <param name="x">A variable.</param>
    /// <returns>Minus one times the variable.</returns>
    Variable<T> Negate(Variable<T> x);

    // Exponential functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    Variable<T> Exp(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    Variable<T> Exp2(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    Variable<T> Exp10(Variable<T> x);

    // Hyperbolic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    Variable<T> Acosh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    Variable<T> Asinh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    Variable<T> Atanh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    Variable<T> Cosh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    Variable<T> Sinh(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    Variable<T> Tanh(Variable<T> x);

    // Logarithmic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    Variable<T> Ln(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    Variable<T> Log(Variable<T> x, Variable<T> b);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    Variable<T> Log2(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    Variable<T> Log10(Variable<T> x);

    // Power functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T> Pow(Variable<T> x, Variable<T> y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T> Pow(Variable<T> x, T y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T> Pow(T x, Variable<T> y);

    // Root functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    Variable<T> Cbrt(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    Variable<T> Root(Variable<T> x, Variable<T> n);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    Variable<T> Sqrt(Variable<T> x);

    // Trigonometric function

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    Variable<T> Acos(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    Variable<T> Asin(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    Variable<T> Atan(Variable<T> x);

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    Variable<Real> Atan2(in Variable<Real> y, in Variable<Real> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    Variable<T> Cos(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    Variable<T> Sin(Variable<T> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    Variable<T> Tan(Variable<T> x);

    //
    // DifGeo
    //

    /// <summary>Create an autodiff, rank-one tensor from a vector.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A rank-one tensor of two variables.</returns>
    AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in Vector2<T> x)
        where U : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from seed values.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x0Seed">The zeroth seed value.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <returns>A rank-one tensor of two variables.</returns>
    AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed)
        where U : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from a vector.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A rank-one tensor of three variables.</returns>
    AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in Vector3<T> x)
        where U : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from seed values.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x0Seed">The zeroth seed value.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <param name="x2Seed">The second seed value.</param>
    /// <returns>A rank-one tensor of threes variables.</returns>
    AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed, in T x2Seed)
        where U : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from a vector.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x">A vector of seed values.</param>
    /// <returns>A rank-one tensor of four variables.</returns>
    AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in Vector4<T> x)
        where U : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from seed values.</summary>
    /// <typeparam name="U">An index.</typeparam>
    /// <param name="x0Seed">The zeroth seed value.</param>
    /// <param name="x1Seed">The first seed value.</param>
    /// <param name="x2Seed">The second seed value.</param>
    /// <param name="x3Seed">The third seed value.</param>
    /// <returns>A rank-one tensor of four variables.</returns>
    AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed, in T x2Seed, in T x3Seed)
        where U : IIndex;
}
