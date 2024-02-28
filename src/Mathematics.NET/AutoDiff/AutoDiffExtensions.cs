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
    // Vector calculus
    //

    // TODO: Improve performance; perhaps see if caching is possible for some of these methods

    /// <summary>Compute the curl of a vector field using reverse-mode automatic differentiation: $ (\nabla\times\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first component of the vector field</param>
    /// <param name="f2">The second component of the vector field</param>
    /// <param name="f3">The third component of the vector field</param>
    /// <param name="x">The point at which to compute the curl</param>
    /// <returns>The curl of the vector field</returns>
    public static Vector3<T> Curl<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f1(tape, x);
        tape.ReverseAccumulate(out var df1);

        _ = f2(tape, x);
        tape.ReverseAccumulate(out var df2);

        _ = f3(tape, x);
        tape.ReverseAccumulate(out var df3);

        return new(
            df3[1] - df2[2],
            df1[2] - df3[0],
            df2[0] - df1[1]);
    }

    /// <summary>Compute the derivative of a scalar function along a particular direction using reverse-mode automatic differentiation: $ \nabla_{\textbf{v}}f(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A direction</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the directional derivative</param>
    /// <returns>The directional derivative</returns>
    public static T DirectionalDerivative<T>(this ITape<T> tape, Vector2<T> v, Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f, AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return gradient[0] * v.X1 + gradient[1] * v.X2;
    }

    /// <inheritdoc cref="DirectionalDerivative{T}(ITape{T}, Vector2{T}, Func{ITape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static T DirectionalDerivative<T>(this ITape<T> tape, Vector3<T> v, Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f, AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3;
    }

    /// <inheritdoc cref="DirectionalDerivative{T}(ITape{T}, Vector2{T}, Func{ITape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static T DirectionalDerivative<T>(this ITape<T> tape, Vector4<T> v, Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f, AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3 + gradient[3] * v.X4;
    }

    /// <summary>Compute the divergence of a vector field using reverse-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first component of the vector field</param>
    /// <param name="f2">The second component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static T Divergence<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f2,
        AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        var partialSum = T.Zero;

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        partialSum += gradient[0];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[1];

        return partialSum;
    }

    /// <summary>Compute the divergence of a vector field using reverse-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first component of the vector field</param>
    /// <param name="f2">The second component of the vector field</param>
    /// <param name="f3">The third component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static T Divergence<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        var partialSum = T.Zero;

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        partialSum += gradient[0];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[1];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[2];

        return partialSum;
    }

    /// <summary>Compute the divergence of a vector field using reverse-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first component of the vector field</param>
    /// <param name="f2">The second component of the vector field</param>
    /// <param name="f3">The third component of the vector field</param>
    /// <param name="f4">The fourth component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static T Divergence<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f4,
        AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        var partialSum = T.Zero;

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        partialSum += gradient[0];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[1];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[2];

        _ = f4(tape, x);
        tape.ReverseAccumulate(out gradient);
        partialSum += gradient[3];

        return partialSum;
    }

    /// <summary>Compute the gradient of a scalar function using reverse-mode automatic differentiation: $ \nabla f(\textbf{x}) $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the gradient</param>
    /// <returns>The gradient of the scalar function</returns>
    public static Vector2<T> Gradient<T>(this ITape<T> tape, Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f, AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return new(gradient[0], gradient[1]);
    }

    /// <inheritdoc cref="Gradient{T}(ITape{T}, Func{ITape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static Vector3<T> Gradient<T>(this ITape<T> tape, Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f, AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return new(gradient[0], gradient[1], gradient[2]);
    }

    /// <inheritdoc cref="Gradient{T}(ITape{T}, Func{ITape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static Vector4<T> Gradient<T>(this ITape<T> tape, Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f, AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out var gradient);

        return new(gradient[0], gradient[1], gradient[2], gradient[3]);
    }

    /// <summary>Compute the Hessian of a scaler function using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the gradient</param>
    /// <returns>The Hessian of the scalar function</returns>
    public static Matrix2x2<T> Hessian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector2<T>, Variable<T>> f, AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return new(
            hessian[0, 0], hessian[0, 1],
            hessian[1, 0], hessian[1, 1]);
    }

    /// <inheritdoc cref="Hessian{T}(HessianTape{T}, Func{HessianTape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static Matrix3x3<T> Hessian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector3<T>, Variable<T>> f, AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return new(
            hessian[0, 0], hessian[0, 1], hessian[0, 2],
            hessian[1, 0], hessian[1, 1], hessian[1, 2],
            hessian[2, 0], hessian[2, 1], hessian[2, 2]);
    }

    /// <inheritdoc cref="Hessian{T}(HessianTape{T}, Func{HessianTape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static Matrix4x4<T> Hessian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector4<T>, Variable<T>> f, AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return new(
            hessian[0, 0], hessian[0, 1], hessian[0, 2], hessian[0, 3],
            hessian[1, 0], hessian[1, 1], hessian[1, 2], hessian[1, 3],
            hessian[2, 0], hessian[2, 1], hessian[2, 2], hessian[2, 3],
            hessian[3, 0], hessian[3, 1], hessian[3, 2], hessian[3, 3]);
    }

    /// <summary>Compute the Jacobian of a vector function using reverse-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="x">The point at which to compute the Jacobian</param>
    /// <returns>The Jacobian of the vector function</returns>
    public static Matrix2x2<T> Jacobian<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f2,
        AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Matrix2x2<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0, 0] = gradient[0]; result[0, 1] = gradient[1];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1, 0] = gradient[0]; result[1, 1] = gradient[1];

        return result;
    }

    /// <summary>Compute the Jacobian of a vector function using reverse-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian</param>
    /// <returns>The Jacobian of the vector function</returns>
    public static Matrix3x3<T> Jacobian<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Matrix3x3<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0, 0] = gradient[0]; result[0, 1] = gradient[1]; result[0, 2] = gradient[2];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1, 0] = gradient[0]; result[1, 1] = gradient[1]; result[1, 2] = gradient[2];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[2, 0] = gradient[0]; result[2, 1] = gradient[1]; result[2, 2] = gradient[2];

        return result;
    }

    /// <summary>Compute the Jacobian of a vector function using reverse-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="f4">The fourth function</param>
    /// <param name="x">The point at which to compute the Jacobian</param>
    /// <returns>The Jacobian of the vector function</returns>
    public static Matrix4x4<T> Jacobian<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f3,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f4,
        AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Matrix4x4<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0, 0] = gradient[0]; result[0, 1] = gradient[1]; result[0, 2] = gradient[2]; result[0, 3] = gradient[3];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1, 0] = gradient[0]; result[1, 1] = gradient[1]; result[1, 2] = gradient[2]; result[1, 3] = gradient[3];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[2, 0] = gradient[0]; result[2, 1] = gradient[1]; result[2, 2] = gradient[2]; result[2, 3] = gradient[3];

        _ = f4(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[3, 0] = gradient[0]; result[3, 1] = gradient[1]; result[3, 2] = gradient[2]; result[3, 3] = gradient[3];

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product</param>
    /// <param name="v">A vector</param>
    /// <returns>The Jacobian-vector product of the vector function and vector</returns>
    public static Vector2<T> JVP<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f2,
        AutoDiffVector2<T> x,
        Vector2<T> v)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector2<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0] = gradient[0] * v.X1 + gradient[1] * v.X2;

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1] = gradient[0] * v.X1 + gradient[1] * v.X2;

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product</param>
    /// <param name="v">A vector</param>
    /// <returns>The Jacobian-vector product of the vector function and vector</returns>
    public static Vector3<T> JVP<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        AutoDiffVector3<T> x,
        Vector3<T> v)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector3<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3;

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3;

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[2] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3;

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="f4">The fourth function</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product</param>
    /// <param name="v">A vector</param>
    /// <returns>The Jacobian-vector product of the vector function and vector</returns>
    public static Vector4<T> JVP<T>(
        this ITape<T> tape,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f3,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f4,
        AutoDiffVector4<T> x,
        Vector4<T> v)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector4<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient);
        result[0] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3 + gradient[3] * v.X4;

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[1] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3 + gradient[3] * v.X4;

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[2] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3 + gradient[3] * v.X4;

        _ = f4(tape, x);
        tape.ReverseAccumulate(out gradient);
        result[3] = gradient[0] * v.X1 + gradient[1] * v.X2 + gradient[2] * v.X3 + gradient[3] * v.X4;

        return result;
    }

    /// <summary>Compute the Laplacian of a scalar function using reverse-mode automatic differentiation: $ \nabla^2f $.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the Laplacian</param>
    /// <returns>The Laplacian of the scalar function</returns>
    public static T Laplacian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector2<T>, Variable<T>> f, AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return hessian[0, 0] + hessian[1, 1];
    }

    /// <inheritdoc cref="Laplacian{T}(HessianTape{T}, Func{HessianTape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static T Laplacian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector3<T>, Variable<T>> f, AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return hessian[0, 0] + hessian[1, 1] + hessian[2, 2];
    }

    /// <inheritdoc cref="Laplacian{T}(HessianTape{T}, Func{HessianTape{T}, AutoDiffVector2{T}, Variable{T}}, AutoDiffVector2{T})"/>
    public static T Laplacian<T>(this HessianTape<T> tape, Func<HessianTape<T>, AutoDiffVector4<T>, Variable<T>> f, AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        _ = f(tape, x);

        tape.ReverseAccumulate(out ReadOnlySpan2D<T> hessian);

        return hessian[0, 0] + hessian[1, 1] + hessian[2, 2] + hessian[3, 3];
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A vector</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function</returns>
    public static Vector2<T> VJP<T>(
        this ITape<T> tape,
        Vector2<T> v,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector2<T>, Variable<T>> f2,
        AutoDiffVector2<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector2<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient, v.X1);
        result[0] += gradient[0]; result[1] += gradient[1];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient, v.X2);
        result[0] += gradient[0]; result[1] += gradient[1];

        return new(result[0], result[1]);
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A vector</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function</returns>
    public static Vector3<T> VJP<T>(
        this ITape<T> tape,
        Vector3<T> v,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector3<T>, Variable<T>> f3,
        AutoDiffVector3<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector3<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient, v.X1);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient, v.X2);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient, v.X3);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2];

        return new(result[0], result[1], result[2]);
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using reverse-mode automatic differentiation.</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="v">A vector</param>
    /// <param name="f1">The first function</param>
    /// <param name="f2">The second function</param>
    /// <param name="f3">The third function</param>
    /// <param name="f4">The fourth function</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function</returns>
    public static Vector4<T> VJP<T>(
        this ITape<T> tape,
        Vector4<T> v,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f1,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f2,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f3,
        Func<ITape<T>, AutoDiffVector4<T>, Variable<T>> f4,
        AutoDiffVector4<T> x)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        Vector4<T> result = new();

        _ = f1(tape, x);
        tape.ReverseAccumulate(out var gradient, v.X1);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2]; result[3] += gradient[3];

        _ = f2(tape, x);
        tape.ReverseAccumulate(out gradient, v.X2);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2]; result[3] += gradient[3];

        _ = f3(tape, x);
        tape.ReverseAccumulate(out gradient, v.X3);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2]; result[3] += gradient[3];

        _ = f4(tape, x);
        tape.ReverseAccumulate(out gradient, v.X4);
        result[0] += gradient[0]; result[1] += gradient[1]; result[2] += gradient[2]; result[3] += gradient[3];

        return new(result[0], result[1], result[2], result[3]);
    }
}
