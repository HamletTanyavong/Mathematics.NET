// <copyright file="DifGeoExtensions.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>A class containing difgeo extension methods.</summary>
public static class DifGeoExtensions
{
    //
    // Calculus
    //

    /// <summary>Compute the inverse of a metric tensor with lower indices.</summary>
    /// <typeparam name="TSM">A type that implements <see cref="ISquareMatrix{T, U}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TI1N">The first index of the metric tensor.</typeparam>
    /// <typeparam name="TI2N">The second index of the metric tensor.</typeparam>
    /// <param name="metric">The metric tensor.</param>
    /// <returns>A metric tensor with upper indices.</returns>
    public static MetricTensor<TSM, TN, Upper, TI1N, TI2N> Inverse<TSM, TN, TI1N, TI2N>(this ref readonly MetricTensor<TSM, TN, Lower, TI1N, TI2N> metric)
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        var inverse = Unsafe.As<MetricTensor<TSM, TN, Lower, TI1N, TI2N>, TSM>(ref Unsafe.AsRef(in metric)).Inverse();
        return Unsafe.As<TSM, MetricTensor<TSM, TN, Upper, TI1N, TI2N>>(ref inverse);
    }

    /// <summary>Compute the inverse of a metric tensor with upper indices.</summary>
    /// <typeparam name="TSM">A type that implements <see cref="ISquareMatrix{T, U}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TI1N">The first index of the metric tensor.</typeparam>
    /// <typeparam name="TI2N">The second index of the metric tensor.</typeparam>
    /// <param name="metric">The metric tensor.</param>
    /// <returns>A metric tensor with lower indices.</returns>
    public static MetricTensor<TSM, TN, Lower, TI1N, TI2N> Inverse<TSM, TN, TI1N, TI2N>(this ref readonly MetricTensor<TSM, TN, Upper, TI1N, TI2N> metric)
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        var inverse = Unsafe.As<MetricTensor<TSM, TN, Upper, TI1N, TI2N>, TSM>(ref Unsafe.AsRef(in metric)).Inverse();
        return Unsafe.As<TSM, MetricTensor<TSM, TN, Lower, TI1N, TI2N>>(ref inverse);
    }
}
