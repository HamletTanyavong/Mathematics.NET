// <copyright file="AutoDiffVector2`3.cs" company="Mathematics.NET">
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.AutoDiff;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector of two dual numbers for use in forward-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IDual{T, U, V, W}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="V">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public record struct AutoDiffVector2<T, U, V>
    where T : IDual<T, U, V, V>
    where U : IComplex<U, V, V>, IDifferentiableFunctions<U>
    where V : IBinaryFloatingPointIeee754<V>, IMinMaxValue<V>
{
    /// <summary>The first element of the vector.</summary>
    public T X1;

    /// <summary>The second element of the vector.</summary>
    public T X2;

    public AutoDiffVector2(T x1, T x2)
    {
        X1 = x1;
        X2 = x2;
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

    internal static T GetElement(AutoDiffVector2<T, U, V> vector, int index)
    {
        if ((uint)index >= 2)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref AutoDiffVector2<T, U, V> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 2);
        return Unsafe.Add(ref Unsafe.As<AutoDiffVector2<T, U, V>, T>(ref vector), index);
    }

    // Set

    internal static AutoDiffVector2<T, U, V> WithElement(AutoDiffVector2<T, U, V> vector, int index, T value)
    {
        if ((uint)index >= 2)
            throw new IndexOutOfRangeException();
        AutoDiffVector2<T, U, V> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref AutoDiffVector2<T, U, V> vector, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 2);
        Unsafe.Add(ref Unsafe.As<AutoDiffVector2<T, U, V>, T>(ref vector), index) = value;
    }

    //
    // Methods
    //

    //
    // Formatting
    //

    public override readonly string ToString() => $"({X1}, {X2})";

    //
    // Vector Calculus
    //

    /// <summary>Compute the derivative of a scalar function along a particular direction using forward-mode automatic differentiation: $ \nabla_{\textbf{v}}f(\textbf{x}) $.</summary>
    /// <param name="v">A direction.</param>
    /// <param name="f">A scalar function.</param>
    /// <param name="x">The point at which to compute the directional derivative.</param>
    /// <returns>A directional derivative.</returns>
    public static U DirectionalDerivative(
        Vector2<U, V> v,
        Func<AutoDiffVector2<T, U, V>, T> f,
        AutoDiffVector2<T, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<T, U, V>> seeds = [
            new(x.X1.WithSeed(v.X1), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(v.X2))];

        return f(seeds[0]).D1 + f(seeds[1]).D1;
    }

    /// <summary>Compute the divergence of a vector field using forward-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="f1">The first component of the vector field.</param>
    /// <param name="f2">The second component of the vector field.</param>
    /// <param name="x">The point at which to compute the divergence.</param>
    /// <returns>The divergence of the vector field.</returns>
    public static U Divergence(
        Func<AutoDiffVector2<T, U, V>, T> f1,
        Func<AutoDiffVector2<T, U, V>, T> f2,
        AutoDiffVector2<T, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<T, U, V>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One))];

        return f1(seeds[0]).D1 + f2(seeds[1]).D1;
    }

    /// <summary>Compute the gradient of a scalar function using forward-mode automatic differentiation: $ \nabla f(\textbf{x}) $.</summary>
    /// <param name="f">A scalar function.</param>
    /// <param name="x">The point at which to compute the gradient.</param>
    /// <returns>The gradient of the scalar function.</returns>
    public static Vector2<U, V> Gradient(Func<AutoDiffVector2<T, U, V>, T> f, AutoDiffVector2<T, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<T, U, V>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One))];

        return new(f(seeds[0]).D1, f(seeds[1]).D1);
    }

    /// <summary>Compute the Hessian of a scalar function using forward-mode automatic differentiation.</summary>
    /// <param name="f">A scalar function.</param>
    /// <param name="x">The point at which to compute the gradient.</param>
    /// <returns>The Hessian of the scalar function.</returns>
    public static Matrix2x2<U, V> Hessian(Func<AutoDiffVector2<HyperDual<U, V>, U, V>, HyperDual<U, V>> f, AutoDiffVector2<HyperDual<U, V>, U, V> x)
    {
        Matrix2x2<U, V> result = new();

        result[0, 0] = f(new(x.X1.WithSeed(U.One, U.One), x.X2.WithSeed(U.Zero))).D3;
        result[1, 1] = f(new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One, U.One))).D3;

        var e12 = f(new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero, U.One)));
        result[0, 1] = e12.D3; result[1, 0] = e12.D3;

        return result;
    }

    /// <summary>Compute the Jacobian of a vector function using forward-mode automatic differentiation: $ \nabla^\text{T}f_i(\textbf{x}) $ for $ i=\left\{1,2,3\right\} $.</summary>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="x">The point at which to compute the Jacobian.</param>
    /// <returns>The Jacobian of the vector function.</returns>
    public static Matrix2x2<U, V> Jacobian(
        Func<AutoDiffVector2<T, U, V>, T> f1,
        Func<AutoDiffVector2<T, U, V>, T> f2,
        AutoDiffVector2<T, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<T, U, V>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One))];

        Matrix2x2<U, V> result = new();

        result[0, 0] = f1(seeds[0]).D1; result[0, 1] = f1(seeds[1]).D1;
        result[1, 0] = f2(seeds[0]).D1; result[1, 1] = f2(seeds[1]).D1;

        return result;
    }

    /// <summary>Compute the Jacobian-vector product of a vector function and a vector using forward-mode automatic differentiation.</summary>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="x">The point at which to compute the Jacobian-vector product.</param>
    /// <param name="v">A vector.</param>
    /// <returns>The Jacobian-vector product of the vector function and vector.</returns>
    public static Vector2<U, V> JVP(
        Func<AutoDiffVector2<T, U, V>, T> f1,
        Func<AutoDiffVector2<T, U, V>, T> f2,
        AutoDiffVector2<T, U, V> x,
        Vector2<U, V> v)
    {
        AutoDiffVector2<T, U, V> seed = new(x.X1.WithSeed(v.X1), x.X2.WithSeed(v.X2));
        return new(f1(seed).D1, f2(seed).D1);
    }

    /// <summary>Compute the Laplacian of a scalar function using forward-mode automatic differentiation:  $ \nabla^2f $.</summary>
    /// <param name="f">A scalar function.</param>
    /// <param name="x">The point at which to compute the gradient.</param>
    /// <returns>The gradient of the scalar function.</returns>
    public static U Laplacian(Func<AutoDiffVector2<HyperDual<U, V>, U, V>, HyperDual<U, V>> f, AutoDiffVector2<HyperDual<U, V>, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<HyperDual<U, V>, U, V>> seeds = [
            new(x.X1.WithSeed(U.One, U.One), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One, U.One))];

        return f(seeds[0]).D3 + f(seeds[1]).D3;
    }

    /// <summary>Compute the vector-Jacobian product of a vector and a vector function using forward-mode automatic differentiation.</summary>
    /// <param name="v">A vector.</param>
    /// <param name="f1">The first function.</param>
    /// <param name="f2">The second function.</param>
    /// <param name="x">The point at which to compute the vector-Jacobian product.</param>
    /// <returns>The vector-Jacobian product of the vector and vector-function.</returns>
    public static Vector2<U, V> VJP(
        Vector2<U, V> v,
        Func<AutoDiffVector2<T, U, V>, T> f1,
        Func<AutoDiffVector2<T, U, V>, T> f2,
        AutoDiffVector2<T, U, V> x)
    {
        ReadOnlySpan<AutoDiffVector2<T, U, V>> seeds = [
            new(x.X1.WithSeed(U.One), x.X2.WithSeed(U.Zero)),
            new(x.X1.WithSeed(U.Zero), x.X2.WithSeed(U.One))];

        Vector2<U, V> result = new();

        result[0] = v.X1 * f1(seeds[0]).D1 + v.X2 * f2(seeds[0]).D1;
        result[1] = v.X1 * f1(seeds[1]).D1 + v.X2 * f2(seeds[1]).D1;

        return result;
    }
}
