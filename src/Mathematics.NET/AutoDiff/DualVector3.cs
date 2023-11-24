// <copyright file="DualVector3.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a vector of three dual numbers for use in forward-mode automatic differentiation</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
[StructLayout(LayoutKind.Sequential)]
public record struct DualVector3<T, U>
    where T : IDual<T, U>
    where U : IComplex<U>, IDifferentiableFunctions<U>
{
    /// <summary>The first element of the vector</summary>
    public T X1;

    /// <summary>The second element of the vector</summary>
    public T X2;

    /// <summary>The third element of the vector</summary>
    public T X3;

    public DualVector3(T x1, T x2, T x3)
    {
        X1 = x1;
        X2 = x2;
        X3 = x3;
    }

    //
    // Indexer
    //

    public T this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(DualVector3<T, U> vector, int index)
    {
        if ((uint)index >= 3)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref DualVector3<T, U> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 3);
        return Unsafe.Add(ref Unsafe.As<DualVector3<T, U>, T>(ref vector), index);
    }

    // Set

    internal static DualVector3<T, U> WithElement(DualVector3<T, U> vector, int index, T value)
    {
        if ((uint)index >= 3)
        {
            throw new IndexOutOfRangeException();
        }

        DualVector3<T, U> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref DualVector3<T, U> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 3);
        Unsafe.Add(ref Unsafe.As<DualVector3<T, U>, T>(ref vector), index) = value;
    }

    //
    // Methods
    //

    /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
    public static DualVector3<T, U> Cross(DualVector3<T, U> left, DualVector3<T, U> right)
    {
        return new(
            left.X2 * right.X3 - left.X3 * right.X2,
            left.X3 * right.X1 - left.X1 * right.X3,
            left.X1 * right.X2 - left.X2 * right.X1);
    }

    //
    // Vector Calculus
    //

    /// <summary>Compute the curl of a vector field using forward-mode automatic differentiation: $ (\nabla\times\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the curl</param>
    /// <returns>The curl of the vector field</returns>
    public static Vector3<U> Curl(
        Func<DualVector3<T, U>, T> fx,
        Func<DualVector3<T, U>, T> fy,
        Func<DualVector3<T, U>, T> fz,
        DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        ReadOnlySpan<U> dfx = [fx(seeds[0]).D1, fx(seeds[1]).D1, fx(seeds[2]).D1];
        ReadOnlySpan<U> dfy = [fy(seeds[0]).D1, fy(seeds[1]).D1, fy(seeds[2]).D1];
        ReadOnlySpan<U> dfz = [fz(seeds[0]).D1, fz(seeds[1]).D1, fz(seeds[2]).D1];

        return new(
            dfz[1] - dfy[2],
            dfx[2] - dfz[0],
            dfy[0] - dfx[1]);
    }

    /// <summary>Compute the derivative of a scalar function along a particular direction using forward-mode automatic differentiation: $ \nabla_{\textbf{v}}f(\textbf{x}) $.</summary>
    /// <param name="v">A direction</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the directional derivative</param>
    /// <returns>A directional derivative</returns>
    public static U DirectionalDerivative(
        Vector3<U> v,
        Func<DualVector3<T, U>, T> f,
        DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(v.X1), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(v.X2), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(v.X3))];

        return f(seeds[0]).D1 + f(seeds[1]).D1 + f(seeds[2]).D1;
    }

    /// <summary>Compute the divergence of a vector field using forward-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static U Divergence(
        Func<DualVector3<T, U>, T> fx,
        Func<DualVector3<T, U>, T> fy,
        Func<DualVector3<T, U>, T> fz,
        DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        return fx(seeds[0]).D1 + fy(seeds[1]).D1 + fz(seeds[2]).D1;
    }

    /// <summary>Compute the gradient of a scalar function using forward-mode automatic differentiation: $ \nabla f(\textbf{x}) $.</summary>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the gradient</param>
    /// <returns>The gradient of the scalar function</returns>
    public static Vector3<U> Gradient(Func<DualVector3<T, U>, T> f, DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        return new(f(seeds[0]).D1, f(seeds[1]).D1, f(seeds[2]).D1);
    }

    /// <summary>Compute the Jacobian of a vector function using forward-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian</param>
    /// <returns>The Jacobian of the vector function</returns>
    public static Matrix3x3<U> Jacobian(
        Func<DualVector3<T, U>, T> fx,
        Func<DualVector3<T, U>, T> fy,
        Func<DualVector3<T, U>, T> fz,
        DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        Matrix3x3<U> result = new();

        result[0, 0] = fx(seeds[0]).D1; result[0, 1] = fx(seeds[1]).D1; result[0, 2] = fx(seeds[2]).D1;
        result[1, 0] = fy(seeds[0]).D1; result[1, 1] = fy(seeds[1]).D1; result[1, 2] = fy(seeds[2]).D1;
        result[2, 0] = fz(seeds[0]).D1; result[2, 1] = fz(seeds[1]).D1; result[2, 2] = fz(seeds[2]).D1;

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using forward-mode automatic differentiation.</summary>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product</param>
    /// <param name="v">A vector</param>
    /// <returns>The Jacobian-vector product of the vector function and vector</returns>
    public static Vector3<U> JVP(
        Func<DualVector3<T, U>, T> fx,
        Func<DualVector3<T, U>, T> fy,
        Func<DualVector3<T, U>, T> fz,
        DualVector3<T, U> x,
        Vector3<U> v)
    {
        DualVector3<T, U> seed = new(x.X1.WithSeed(v.X1), x.X2.WithSeed(v.X2), x.X3.WithSeed(v.X3));
        return new(fx(seed).D1, fy(seed).D1, fz(seed).D1);
    }

    /// <summary>Compute the Laplacian of a scalar function using forward-mode automatic differentiation:  $ \nabla^2f $.</summary>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the gradient</param>
    /// <returns>The gradient of the scalar function</returns>
    public static U Laplacian(Func<DualVector3<HyperDual<U>, U>, HyperDual<U>> f, DualVector3<HyperDual<U>, U> x)
    {
        ReadOnlySpan<DualVector3<HyperDual<U>, U>> seeds = [
            new(x.X1.WithSeed(U.One, U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One, U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One, U.One))];

        return f(seeds[0]).D2 + f(seeds[1]).D2 + f(seeds[2]).D2;
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using forward-mode automatic differentiation.</summary>
    /// <param name="v">A vector</param>
    /// <param name="fx">The first function</param>
    /// <param name="fy">The second function</param>
    /// <param name="fz">The third function</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function</returns>
    public static Vector3<U> VJP(
        Vector3<U> v,
        Func<DualVector3<T, U>, T> fx,
        Func<DualVector3<T, U>, T> fy,
        Func<DualVector3<T, U>, T> fz,
        DualVector3<T, U> x)
    {
        ReadOnlySpan<DualVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        Vector3<U> result = new();

        result[0] = v.X1 * fx(seeds[0]).D1 + v.X2 * fy(seeds[0]).D1 + v.X3 * fz(seeds[0]).D1;
        result[1] = v.X1 * fx(seeds[1]).D1 + v.X2 * fy(seeds[1]).D1 + v.X3 * fz(seeds[1]).D1;
        result[2] = v.X1 * fx(seeds[2]).D1 + v.X2 * fy(seeds[2]).D1 + v.X3 * fz(seeds[2]).D1;

        return result;
    }
}
