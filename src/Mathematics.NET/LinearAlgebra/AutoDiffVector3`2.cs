// <copyright file="AutoDiffVector3`2.cs" company="Mathematics.NET">
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.AutoDiff;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector of three dual numbers for use in forward-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IDual{T, U}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public record struct AutoDiffVector3<T, U>
    where T : IDual<T, U>
    where U : IComplex<U>, IDifferentiableFunctions<U>
{
    /// <summary>The first element of the vector.</summary>
    public T X1;

    /// <summary>The second element of the vector.</summary>
    public T X2;

    /// <summary>The third element of the vector.</summary>
    public T X3;

    public AutoDiffVector3(T x1, T x2, T x3)
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
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(AutoDiffVector3<T, U> vector, int index)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref AutoDiffVector3<T, U> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 3);
        return Unsafe.Add(ref Unsafe.As<AutoDiffVector3<T, U>, T>(ref vector), index);
    }

    // Set

    internal static AutoDiffVector3<T, U> WithElement(AutoDiffVector3<T, U> vector, int index, T value)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();
        AutoDiffVector3<T, U> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref AutoDiffVector3<T, U> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 3);
        Unsafe.Add(ref Unsafe.As<AutoDiffVector3<T, U>, T>(ref vector), index) = value;
    }

    //
    // Methods
    //

    //
    // Formatting
    //

    public override readonly string ToString() => $"({X1}, {X2}, {X3})";

    //
    // Three-dimension specific methods
    //

    /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
    public static AutoDiffVector3<T, U> Cross(AutoDiffVector3<T, U> left, AutoDiffVector3<T, U> right)
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
    /// <param name="f1">The first component of the vector field.</param>
    /// <param name="f2">The second component of the vector field.</param>
    /// <param name="f3">The third component of the vector field.</param>
    /// <param name="x">The point at which to compute the curl.</param>
    /// <returns>The curl of the vector field.</returns>
    public static Vector3<U> Curl(
        Func<AutoDiffVector3<T, U>, T> f1,
        Func<AutoDiffVector3<T, U>, T> f2,
        Func<AutoDiffVector3<T, U>, T> f3,
        AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        ReadOnlySpan<U> df1 = [f1(seeds[0]).D1, f1(seeds[1]).D1, f1(seeds[2]).D1];
        ReadOnlySpan<U> df2 = [f2(seeds[0]).D1, f2(seeds[1]).D1, f2(seeds[2]).D1];
        ReadOnlySpan<U> df3 = [f3(seeds[0]).D1, f3(seeds[1]).D1, f3(seeds[2]).D1];

        return new(
            df3[1] - df2[2],
            df1[2] - df3[0],
            df2[0] - df1[1]);
    }

    /// <inheritdoc cref="AutoDiffVector2{T, U}.DirectionalDerivative(Vector2{U}, Func{AutoDiffVector2{T, U}, T}, AutoDiffVector2{T, U})"/>
    public static U DirectionalDerivative(
        Vector3<U> v,
        Func<AutoDiffVector3<T, U>, T> f,
        AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(v.X1), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(v.X2), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(v.X3))];

        return f(seeds[0]).D1 + f(seeds[1]).D1 + f(seeds[2]).D1;
    }

    /// <summary>Compute the divergence of a vector field using forward-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="f1">The first component of the vector field.</param>
    /// <param name="f2">The second component of the vector field.</param>
    /// <param name="f3">The third component of the vector field.</param>
    /// <param name="x">The point at which to compute the divergence.</param>
    /// <returns>The divergence of the vector field.</returns>
    public static U Divergence(
        Func<AutoDiffVector3<T, U>, T> f1,
        Func<AutoDiffVector3<T, U>, T> f2,
        Func<AutoDiffVector3<T, U>, T> f3,
        AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        return f1(seeds[0]).D1 + f2(seeds[1]).D1 + f3(seeds[2]).D1;
    }

    /// <inheritdoc cref="AutoDiffVector2{T, U}.Gradient(Func{AutoDiffVector2{T, U}, T}, AutoDiffVector2{T, U})"/>
    public static Vector3<U> Gradient(Func<AutoDiffVector3<T, U>, T> f, AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        return new(f(seeds[0]).D1, f(seeds[1]).D1, f(seeds[2]).D1);
    }

    /// <inheritdoc cref="AutoDiffVector2{T, U}.Hessian(Func{AutoDiffVector2{HyperDual{U}, U}, HyperDual{U}}, AutoDiffVector2{HyperDual{U}, U})"/>
    public static Matrix3x3<U> Hessian(Func<AutoDiffVector3<HyperDual<U>, U>, HyperDual<U>> f, AutoDiffVector3<HyperDual<U>, U> x)
    {
        Matrix3x3<U> result = new();

        result[0, 0] = f(new(x.X1.WithSeed(U.One, U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero))).D3;
        result[1, 1] = f(new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One, U.One), x.X3.WithSeed(U.Zero))).D3;
        result[2, 2] = f(new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One, U.One))).D3;

        var e12 = f(new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero, U.One), x.X3.WithSeed(U.Zero)));
        result[0, 1] = e12.D3; result[1, 0] = e12.D3;

        var e13 = f(new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero, U.One)));
        result[0, 2] = e13.D3; result[2, 0] = e13.D3;

        var e23 = f(new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero, U.One)));
        result[1, 2] = e23.D3; result[2, 1] = e23.D3;

        return result;
    }

    /// <summary>Compute the Jacobian of a vector function using forward-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="f3">The third function.</param>
    /// <param name="x">The point at which to compute the Jacobian.</param>
    /// <returns>The Jacobian of the vector function.</returns>
    public static Matrix3x3<U> Jacobian(
        Func<AutoDiffVector3<T, U>, T> f1,
        Func<AutoDiffVector3<T, U>, T> f2,
        Func<AutoDiffVector3<T, U>, T> f3,
        AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        Matrix3x3<U> result = new();

        result[0, 0] = f1(seeds[0]).D1; result[0, 1] = f1(seeds[1]).D1; result[0, 2] = f1(seeds[2]).D1;
        result[1, 0] = f2(seeds[0]).D1; result[1, 1] = f2(seeds[1]).D1; result[1, 2] = f2(seeds[2]).D1;
        result[2, 0] = f3(seeds[0]).D1; result[2, 1] = f3(seeds[1]).D1; result[2, 2] = f3(seeds[2]).D1;

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using forward-mode automatic differentiation.</summary>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="f3">The third function.</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product.</param>
    /// <param name="v">A vector.</param>
    /// <returns>The Jacobian-vector product of the vector function and vector.</returns>
    public static Vector3<U> JVP(
        Func<AutoDiffVector3<T, U>, T> f1,
        Func<AutoDiffVector3<T, U>, T> f2,
        Func<AutoDiffVector3<T, U>, T> f3,
        AutoDiffVector3<T, U> x,
        Vector3<U> v)
    {
        AutoDiffVector3<T, U> seed = new(x.X1.WithSeed(v.X1), x.X2.WithSeed(v.X2), x.X3.WithSeed(v.X3));
        return new(f1(seed).D1, f2(seed).D1, f3(seed).D1);
    }

    /// <inheritdoc cref="AutoDiffVector2{T, U}.Laplacian(Func{AutoDiffVector2{HyperDual{U}, U}, HyperDual{U}}, AutoDiffVector2{HyperDual{U}, U})"/>
    public static U Laplacian(Func<AutoDiffVector3<HyperDual<U>, U>, HyperDual<U>> f, AutoDiffVector3<HyperDual<U>, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<HyperDual<U>, U>> seeds = [
            new(x.X1.WithSeed(U.One, U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One, U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One, U.One))];

        return f(seeds[0]).D3 + f(seeds[1]).D3 + f(seeds[2]).D3;
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using forward-mode automatic differentiation.</summary>
    /// <param name="v">A vector.</param>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="f3">The third function.</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product.</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function.</returns>
    public static Vector3<U> VJP(
        Vector3<U> v,
        Func<AutoDiffVector3<T, U>, T> f1,
        Func<AutoDiffVector3<T, U>, T> f2,
        Func<AutoDiffVector3<T, U>, T> f3,
        AutoDiffVector3<T, U> x)
    {
        ReadOnlySpan<AutoDiffVector3<T, U>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One), x.X3.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.Zero), x.X3.WithSeed(U.One))];

        Vector3<U> result = new();

        result[0] = v.X1 * f1(seeds[0]).D1 + v.X2 * f2(seeds[0]).D1 + v.X3 * f3(seeds[0]).D1;
        result[1] = v.X1 * f1(seeds[1]).D1 + v.X2 * f2(seeds[1]).D1 + v.X3 * f3(seeds[1]).D1;
        result[2] = v.X1 * f1(seeds[2]).D1 + v.X2 * f2(seeds[2]).D1 + v.X3 * f3(seeds[2]).D1;

        return result;
    }
}
