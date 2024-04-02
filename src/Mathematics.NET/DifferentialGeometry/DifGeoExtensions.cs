// <copyright file="DifGeoExtensions.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>A class containing difgeo extension methods</summary>
public static class DifGeoExtensions
{
    //
    // AutoDiff
    //

    /// <summary>Create an autodiff, rank-one tensor from a real vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor2<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Vector2<Real> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2));

    /// <summary>Create an autodiff, rank-one tensor from a complex vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor2<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Vector2<Complex> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2));

    /// <summary>Create an autodiff, rank-one tensor from real seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor2<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Real x0Seed, Real x1Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed));

    /// <summary>Create an autodiff, rank-one tensor from complex seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor2<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Complex x0Seed, Complex x1Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed));

    /// <summary>Create an autodiff, rank-one tensor from a real vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor3<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Vector3<Real> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3));

    /// <summary>Create an autodiff, rank-one tensor from a complex vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor3<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Vector3<Complex> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3));

    /// <summary>Create an autodiff, rank-one tensor from real seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor3<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Real x0Seed, Real x1Seed, Real x2Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed));

    /// <summary>Create an autodiff, rank-one tensor from complex seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor3<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Complex x0Seed, Complex x1Seed, Complex x2Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed));

    /// <summary>Create an autodiff, rank-one tensor from a real vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor4<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Vector4<Real> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3), tape.CreateVariable(x.X4));

    /// <summary>Create an autodiff, rank-one tensor from a complex vector.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x">A vector of seed values</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor4<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Vector4<Complex> x)
        where T : IIndex
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3), tape.CreateVariable(x.X4));

    /// <summary>Create an autodiff, rank-one tensor from real seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <param name="x3Seed">The third seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor4<Real, T> CreateAutoDiffTensor<T>(this ITape<Real> tape, Real x0Seed, Real x1Seed, Real x2Seed, Real x3Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed));

    /// <summary>Create an autodiff, rank-one tensor from complex seed values.</summary>
    /// <typeparam name="T">An index</typeparam>
    /// <param name="tape">A type that implements <see cref="ITape{T}"/></param>
    /// <param name="x0Seed">The zeroth seed value</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <param name="x3Seed">The third seed value</param>
    /// <returns>A rank-one tensor of four variables</returns>
    public static AutoDiffTensor4<Complex, T> CreateAutoDiffTensor<T>(this ITape<Complex> tape, Complex x0Seed, Complex x1Seed, Complex x2Seed, Complex x3Seed)
        where T : IIndex
        => new(tape.CreateVariable(x0Seed), tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed));

    //
    // Calculus
    //

    /// <summary>Compute the inverse of a metric tensor with lower indices.</summary>
    /// <typeparam name="TSM">A type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI1N">The first index of the metric tensor</typeparam>
    /// <typeparam name="TI2N">The second index of the metric tensor</typeparam>
    /// <param name="metric">The metric tensor</param>
    /// <returns>A metric tensor with upper indices</returns>
    public static MetricTensor<TSM, TN, Upper, TI1N, TI2N> Inverse<TSM, TN, TI1N, TI2N>(this MetricTensor<TSM, TN, Lower, TI1N, TI2N> metric)
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : ISymbol
        where TI2N : ISymbol
    {
        var inverse = Unsafe.As<MetricTensor<TSM, TN, Lower, TI1N, TI2N>, TSM>(ref metric).Inverse();
        return Unsafe.As<TSM, MetricTensor<TSM, TN, Upper, TI1N, TI2N>>(ref inverse);
    }

    /// <summary>Compute the inverse of a metric tensor with upper indices.</summary>
    /// <typeparam name="TSM">A type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI1N">The first index of the metric tensor</typeparam>
    /// <typeparam name="TI2N">The second index of the metric tensor</typeparam>
    /// <param name="metric">The metric tensor</param>
    /// <returns>A metric tensor with lower indices</returns>
    public static MetricTensor<TSM, TN, Lower, TI1N, TI2N> Inverse<TSM, TN, TI1N, TI2N>(this MetricTensor<TSM, TN, Upper, TI1N, TI2N> metric)
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : ISymbol
        where TI2N : ISymbol
    {
        var inverse = Unsafe.As<MetricTensor<TSM, TN, Upper, TI1N, TI2N>, TSM>(ref metric).Inverse();
        return Unsafe.As<TSM, MetricTensor<TSM, TN, Lower, TI1N, TI2N>>(ref inverse);
    }
}
