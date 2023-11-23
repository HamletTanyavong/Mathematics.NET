// <copyright file="AutoDiffExtensions.cs" company="Mathematics.NET">
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

using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.AutoDiff;

/// <summary>A class containing AutoDiff extension methods</summary>
public static class AutoDiffExtensions
{
    //
    // Variable creation
    //

    /// <summary>Create a three-element vector from a seed vector of length three.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A three-element vector of seed values</param>
    /// <returns>A variable vector of length three</returns>
    public static VariableVector3<T> CreateVariableVector<T>(this ITape<T> tape, Vector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3));

    /// <summary>Create a three-element vector from seed values.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <param name="x3Seed">The third seed value</param>
    /// <returns>A variable vector of length three</returns>
    public static VariableVector3<T> CreateVariableVector<T>(this ITape<T> tape, T x1Seed, T x2Seed, T x3Seed)
        where T : IComplex<T>, IDifferentiableFunctions<T>
        => new(tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed));

    //
    // Vector calculus
    //

    // TODO: Improve performance; perhaps see if caching is possible for some of these methods

    /// <summary>Compute the curl of a vector field using reverse-mode automatic differentiation: $ (\nabla\times\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the curl</param>
    /// <returns>The curl of the vector field</returns>
    public static Vector3<T> Curl<T>(
        this ITape<T> tape,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fx,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fy,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fz,
        VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = fx(tape, x);
        tape.ReverseAccumulation(out var dfx);

        _ = fy(tape, x);
        tape.ReverseAccumulation(out var dfy);

        _ = fz(tape, x);
        tape.ReverseAccumulation(out var dfz);

        return new(
            dfz[1] - dfy[2],
            dfx[2] - dfz[0],
            dfy[0] - dfx[1]);
    }

    /// <summary>Compute the derivative of a scalar function along a particular direction using reverse-mode automatic differentiation: $ \nabla_{\textbf{v}}f(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A direction</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the directional derivative</param>
    /// <returns>The directional derivative</returns>
    public static T DirectionalDerivative<T>(this ITape<T> tape, Vector3<T> v, Func<ITape<T>, VariableVector3<T>, Variable<T>> f, VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulation(out var gradients);

        return gradients[0] * v.X1 + gradients[1] * v.X2 + gradients[2] * v.X3;
    }

    /// <summary>Compute the divergence of a vector field using reverse-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static T Divergence<T>(
        this ITape<T> tape,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fx,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fy,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fz,
        VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        var partialSum = T.Zero;

        _ = fx(tape, x);
        tape.ReverseAccumulation(out var gradients);
        partialSum += gradients[0];

        _ = fy(tape, x);
        tape.ReverseAccumulation(out gradients);
        partialSum += gradients[1];

        _ = fz(tape, x);
        tape.ReverseAccumulation(out gradients);
        partialSum += gradients[2];

        return partialSum;
    }

    /// <summary>Compute the gradient of a scalar function using reverse-mode automatic differentiation: $ \nabla f(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the gradient</param>
    /// <returns>The gradient of the scalar function</returns>
    public static Vector3<T> Gradient<T>(this ITape<T> tape, Func<ITape<T>, VariableVector3<T>, Variable<T>> f, VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulation(out var gradients);

        return new(gradients[0], gradients[1], gradients[2]);
    }

    /// <summary>Compute the Jacobian of a vector function using reverse-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian</param>
    /// <returns>The Jacobian of the vector function</returns>
    public static Matrix3x3<T> Jacobian<T>(
        this ITape<T> tape,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fx,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fy,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fz,
        VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Matrix3x3<T> result = new();

        _ = fx(tape, x);
        tape.ReverseAccumulation(out var gradients);
        result[0, 0] = gradients[0]; result[0, 1] = gradients[1]; result[0, 2] = gradients[2];

        _ = fy(tape, x);
        tape.ReverseAccumulation(out gradients);
        result[1, 0] = gradients[0]; result[1, 1] = gradients[1]; result[1, 2] = gradients[2];

        _ = fz(tape, x);
        tape.ReverseAccumulation(out gradients);
        result[2, 0] = gradients[0]; result[2, 1] = gradients[1]; result[2, 2] = gradients[2];

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product</param>
    /// <param name="v">A vector</param>
    /// <returns>The Jacobian-vector product of the vector function and vector</returns>
    public static Vector3<T> JVP<T>(
        this ITape<T> tape,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fx,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fy,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fz,
        VariableVector3<T> x,
        Vector3<T> v)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector3<T> result = new();

        _ = fx(tape, x);
        tape.ReverseAccumulation(out var gradients);
        result[0] = gradients[0] * v.X1 + gradients[1] * v.X2 + gradients[2] * v.X3;

        _ = fy(tape, x);
        tape.ReverseAccumulation(out gradients);
        result[1] = gradients[0] * v.X1 + gradients[1] * v.X2 + gradients[2] * v.X3;

        _ = fz(tape, x);
        tape.ReverseAccumulation(out gradients);
        result[2] = gradients[0] * v.X1 + gradients[1] * v.X2 + gradients[2] * v.X3;

        return result;
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A vector</param>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function</returns>
    public static Vector3<T> VJP<T>(
        this ITape<T> tape,
        Vector3<T> v,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fx,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fy,
        Func<ITape<T>, VariableVector3<T>, Variable<T>> fz,
        VariableVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector3<T> result = new();

        _ = fx(tape, x);
        tape.ReverseAccumulation(out var gradients, v.X1);
        result[0] += gradients[0]; result[1] += gradients[1]; result[2] += gradients[2];

        _ = fy(tape, x);
        tape.ReverseAccumulation(out gradients, v.X2);
        result[0] += gradients[0]; result[1] += gradients[1]; result[2] += gradients[2];

        _ = fz(tape, x);
        tape.ReverseAccumulation(out gradients, v.X3);
        result[0] += gradients[0]; result[1] += gradients[1]; result[2] += gradients[2];

        return new(result[0], result[1], result[2]);
    }
}
