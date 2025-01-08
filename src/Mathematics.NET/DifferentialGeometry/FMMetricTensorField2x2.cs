// <copyright file="FMMetricTensorField2x2.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a 2x2 metric tensor field.</summary>
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class FMMetricTensorField2x2<TDN, TN, TPI> : FMTensorField2x2<TDN, TN, Lower, Lower, TPI>
    where TDN : IDual<TDN, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TPI : IIndex
{
    /// <summary>Compute the value of the metric tensor at a specific point on the manifold.</summary>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <param name="point">A point on the manifold.</param>
    /// <returns>A metric tensor.</returns>
    public new MetricTensor<Matrix2x2<TN>, TN, Lower, TI1N, TI2N> Compute<TI1N, TI2N>(AutoDiffTensor2<TDN, TN, TPI> point)
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        Matrix2x2<TN> result = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (_buffer[i][j] is Func<AutoDiffTensor2<TDN, TN, TPI>, TDN> function)
                    result[i, j] = function(point).D0;
            }
        }
        return new MetricTensor<Matrix2x2<TN>, TN, Lower, TI1N, TI2N>(result);
    }

    /// <summary>Compute the value of the inverse metric tensor at a specific point on the manifold.</summary>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <param name="point">A point on the manifold.</param>
    /// <returns>An inverse metric tensor.</returns>
    public MetricTensor<Matrix2x2<TN>, TN, Upper, TI1N, TI2N> ComputeInverse<TI1N, TI2N>(AutoDiffTensor2<TDN, TN, TPI> point)
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        var value = Compute<TI1N, TI2N>(point);
        return value.Inverse();
    }
}
