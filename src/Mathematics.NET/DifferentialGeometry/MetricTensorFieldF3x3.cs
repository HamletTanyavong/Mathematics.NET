// <copyright file="MetricTensorFieldF3x3.cs" company="Mathematics.NET">
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
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a 3x3 metric tensor field.</summary>
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN, U}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TB">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class MetricTensorFieldF3x3<TDN, TN, TB, TPI> : TensorFieldF3x3<TDN, TN, TB, Lower, Lower, TPI>
    where TDN : IDual<TDN, TN, TB>
    where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN>
    where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB>
    where TPI : IIndex
{
    /// <inheritdoc cref="MetricTensorFieldF2x2{TDN, TN, U, TPI}.Compute{TI1N, TI2N}(AutoDiffTensor2{TDN, TN, U, TPI})"/>
    public new MetricTensor<Matrix3x3<TN, TB>, TN, TB, Lower, TI1N, TI2N> Compute<TI1N, TI2N>(AutoDiffTensor3<TDN, TN, TB, TPI> point)
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        Matrix3x3<TN, TB> result = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_buffer[i][j] is Func<AutoDiffTensor3<TDN, TN, TB, TPI>, TDN> function)
                    result[i, j] = function(point).D0;
            }
        }
        return new MetricTensor<Matrix3x3<TN, TB>, TN, TB, Lower, TI1N, TI2N>(result);
    }

    /// <inheritdoc cref="MetricTensorFieldF2x2{TDN, TN, U, TPI}.ComputeInverse{TI1N, TI2N}(AutoDiffTensor2{TDN, TN, U, TPI})"/>
    public MetricTensor<Matrix3x3<TN, TB>, TN, TB, Upper, TI1N, TI2N> ComputeInverse<TI1N, TI2N>(AutoDiffTensor3<TDN, TN, TB, TPI> point)
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        var value = Compute<TI1N, TI2N>(point);
        return value.Inverse<TI1N, TI2N>();
    }
}
