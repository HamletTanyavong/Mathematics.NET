// <copyright file="MetricTensorField3x3.cs" company="Mathematics.NET">
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
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a 3x3 metric tensor field.</summary>
/// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public abstract class MetricTensorField3x3<TT, TN, TPI> : TensorField3x3<TT, TN, Lower, Lower, TPI>
    where TT : ITape<TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TPI : IIndex
{
    public MetricTensorField3x3() { }

    /// <inheritdoc cref="MetricTensorField2x2{TT, TN, TPI}.Compute{TI1N, TI2N}(TT, AutoDiffTensor2{TN, TPI})"/>
    public new MetricTensor<Matrix3x3<TN>, TN, Lower, TI1N, TI2N> Compute<TI1N, TI2N>(TT tape, AutoDiffTensor3<TN, TPI> point)
        where TI1N : ISymbol
        where TI2N : ISymbol
    {
        tape.IsTracking = false;

        Matrix3x3<TN> result = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_buffer[i][j] is Func<TT, AutoDiffTensor3<TN, TPI>, Variable<TN>> function)
                    result[i, j] = function(tape, point).Value;
            }
        }

        tape.IsTracking = true;
        return new MetricTensor<Matrix3x3<TN>, TN, Lower, TI1N, TI2N>(result);
    }

    /// <inheritdoc cref="MetricTensorField2x2{TT, TN, TPI}.ComputeInverse{TI1N, TI2N}(TT, AutoDiffTensor2{TN, TPI})"/>
    public MetricTensor<Matrix3x3<TN>, TN, Upper, TI1N, TI2N> ComputeInverse<TI1N, TI2N>(TT tape, AutoDiffTensor3<TN, TPI> point)
        where TI1N : ISymbol
        where TI2N : ISymbol
    {
        var value = Compute<TI1N, TI2N>(tape, point);
        return value.Inverse();
    }
}
