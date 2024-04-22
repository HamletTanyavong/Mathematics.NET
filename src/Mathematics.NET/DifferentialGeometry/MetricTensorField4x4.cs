// <copyright file="MetricTensorField4x4.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a 4x4 metric tensor field</summary>
/// <typeparam name="TT">A type that implements <see cref="ITape{T}"/></typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <typeparam name="TPI">An index name</typeparam>
public abstract class MetricTensorField4x4<TT, TN, TPI> : TensorField4x4<TT, TN, Lower, Lower, TPI>
    where TT : ITape<TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TPI : IIndex
{
    public MetricTensorField4x4() { }

    public new MetricTensor<Matrix4x4<TN>, TN, Lower, TI1, TI2> Compute<TI1, TI2>(TT tape, AutoDiffTensor4<TN, TPI> point)
        where TI1 : ISymbol
        where TI2 : ISymbol
    {
        tape.IsTracking = false;

        Matrix4x4<TN> result = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_buffer[i][j] is Func<TT, AutoDiffTensor4<TN, TPI>, Variable<TN>> function)
                {
                    result[i, j] = function(tape, point).Value;
                }
            }
        }

        tape.IsTracking = true;
        return new MetricTensor<Matrix4x4<TN>, TN, Lower, TI1, TI2>(result);
    }

    public MetricTensor<Matrix4x4<TN>, TN, Upper, TI1, TI2> ComputeInverse<TI1, TI2>(TT tape, AutoDiffTensor4<TN, TPI> point)
        where TI1 : ISymbol
        where TI2 : ISymbol
    {
        var value = Compute<TI1, TI2>(tape, point);
        return value.Inverse();
    }
}
