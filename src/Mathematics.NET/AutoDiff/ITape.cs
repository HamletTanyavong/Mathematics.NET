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

using System.Numerics;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for tapes used in reverse-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
public interface ITape<T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    /// <summary>This property indicates whether or not the tape is currently tracking nodes.</summary>
    bool IsTracking { get; set; }

    /// <summary>Get the number of nodes on the gradient tape.</summary>
    int NodeCount { get; }

    /// <summary>Get the number of variables that are being tracked.</summary>
    int VariableCount { get; }

    /// <summary>Create a variable for the gradient tape to track.</summary>
    /// <param name="value">An initial value.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> CreateVariable(T value);

    /// <summary>Log nodes tracked by the tape.</summary>
    /// <param name="logger">A logger.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="limit">The total number of nodes to log.</param>
    void LogNodes(ILogger<ITape<T, U>> logger, CancellationToken cancellationToken, int limit = 100);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the result.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <exception cref="AutoDiffException">The gradient tape does not have any tracked variables.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape and output the result.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="seed">A seed value.</param>
    /// <exception cref="AutoDiffException">The gradient tape does not have any tracked variables.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape starting at a specific node and output the result.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The gradient tape does not have any tracked variables.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient, int index);

    /// <summary>Perform reverse accumulation on the gradient or Hessian tape starting at a specific node and output the result.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="seed">A seed value.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The gradient tape does not have any tracked variables.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed, int index);

    //
    // Basic Operations
    //

    /// <summary>Add two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Add(Variable<T, U> x, Variable<T, U> y);

    /// <summary>Add a constant value and a variable.</summary>
    /// <param name="x">A constant value.</param>
    /// <param name="y">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Add(T x, Variable<T, U> y);

    /// <summary>Add a variable and a constant value.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Add(Variable<T, U> x, T y);

    /// <summary>Divide two variables.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Divide(Variable<T, U> x, Variable<T, U> y);

    /// <summary>Divide a constant value by a variable.</summary>
    /// <param name="x">A constant dividend.</param>
    /// <param name="y">A variable divisor.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Divide(T x, Variable<T, U> y);

    /// <summary>Divide a variable by a constant value.</summary>
    /// <param name="x">A variable dividend.</param>
    /// <param name="y">A constant divisor.</param>
    /// <returns>A variabl.</returns>
    Variable<T, U> Divide(Variable<T, U> x, T y);

    /// <summary>Compute the modulo of a variable given a divisor.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/>.</returns>
    Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, in Variable<Real<U>, U> y);

    /// <summary>Compute the modulo of a real value given a divisor.</summary>
    /// <param name="x">A real dividend.</param>
    /// <param name="y">A variable divisor.</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/>.</returns>
    Variable<Real<U>, U> Modulo(Real<U> x, in Variable<Real<U>, U> y);

    /// <summary>Compute the modulo of a variable given a divisor.</summary>
    /// <param name="x">A variable dividend.</param>
    /// <param name="y">A real divisor.</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/>.</returns>
    Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, Real<U> y);

    /// <summary>Multiply two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Multiply(Variable<T, U> x, Variable<T, U> y);

    /// <summary>Multiply a constant value by a variable.</summary>
    /// <param name="x">A constant value.</param>
    /// <param name="y">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Multiply(T x, Variable<T, U> y);

    /// <summary>Multiply a variable by a constant value.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Multiply(Variable<T, U> x, T y);

    /// <summary>Subract two variables.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Subtract(Variable<T, U> x, Variable<T, U> y);

    /// <summary>Subtract a variable from a constant value.</summary>
    /// <param name="x">A constant value.</param>
    /// <param name="y">A variable.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Subtract(T x, Variable<T, U> y);

    /// <summary>Subtract a constant value from a variable.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A constant value.</param>
    /// <returns>A variable.</returns>
    Variable<T, U> Subtract(Variable<T, U> x, T y);

    //
    // Other Operations
    //

    /// <summary>Negate a variable.</summary>
    /// <param name="x">A variable.</param>
    /// <returns>Minus one times the variable.</returns>
    Variable<T, U> Negate(Variable<T, U> x);

    // Exponential functions.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    Variable<T, U> Exp(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    Variable<T, U> Exp2(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    Variable<T, U> Exp10(Variable<T, U> x);

    // Hyperbolic functions.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    Variable<T, U> Acosh(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    Variable<T, U> Asinh(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    Variable<T, U> Atanh(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    Variable<T, U> Cosh(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    Variable<T, U> Sinh(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    Variable<T, U> Tanh(Variable<T, U> x);

    // Logarithmic functions.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    Variable<T, U> Ln(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    Variable<T, U> Log(Variable<T, U> x, Variable<T, U> b);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    Variable<T, U> Log2(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    Variable<T, U> Log10(Variable<T, U> x);

    // Power functions.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T, U> Pow(Variable<T, U> x, Variable<T, U> n);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T, U> Pow(Variable<T, U> x, T n);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    Variable<T, U> Pow(T x, Variable<T, U> n);

    // Root functions.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    Variable<T, U> Cbrt(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    Variable<T, U> Root(Variable<T, U> x, Variable<T, U> n);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    Variable<T, U> Sqrt(Variable<T, U> x);

    // Trigonometric function.

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    Variable<T, U> Acos(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    Variable<T, U> Asin(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    Variable<T, U> Atan(Variable<T, U> x);

    /// <inheritdoc cref="IReal{T, U, V}.Atan2(Real{V}, Real{V})"/>
    Variable<Real<U>, U> Atan2(in Variable<Real<U>, U> y, in Variable<Real<U>, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    Variable<T, U> Cos(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    Variable<T, U> Sin(Variable<T, U> x);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    Variable<T, U> Tan(Variable<T, U> x);

    //
    // DifGeo
    //

    /// <summary>Create an autodiff, rank-one tensor from a vector of initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <returns>A rank-one tensor of two variables.</returns>
    AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in Vector2<T, U> x)
        where V : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <returns>A rank-one tensor of two variables.</returns>
    AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1)
        where V : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from a vector of initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <returns>A rank-one tensor of three variables.</returns>
    AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in Vector3<T, U> x)
        where V : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <param name="x2">The second value.</param>
    /// <returns>A rank-one tensor of three variables.</returns>
    AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2)
        where V : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from a vector of initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <returns>A rank-one tensor of four variables.</returns>
    AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in Vector4<T, U> x)
        where V : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from initial values.</summary>
    /// <typeparam name="V">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <param name="x2">The second value.</param>
    /// <param name="x3">The third value.</param>
    /// <returns>A rank-one tensor of four variables.</returns>
    AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2, in T x3)
        where V : IIndex;
}
