// <copyright file="MetricTensorField.cs" company="Mathematics.NET">
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
using System.Runtime.InteropServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a metric tensor field</summary>
/// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
/// <typeparam name="TSquareMatrix">A type that implements <see cref="ISquareMatrix{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <typeparam name="TPointIndex">An index name</typeparam>
public abstract class MetricTensorField<TTape, TSquareMatrix, TNumber, TPointIndex> : TensorField4x4<TTape, TNumber, Lower, Lower, TPointIndex>
    where TTape : ITape<TNumber>
    where TSquareMatrix : ISquareMatrix<TSquareMatrix, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TPointIndex : IIndex
{
    public MetricTensorField() { }

    public new MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1, TIndex2> Compute<TIndex1, TIndex2>(TTape tape, AutoDiffTensor4<TNumber, TPointIndex> x)
        where TIndex1 : ISymbol
        where TIndex2 : ISymbol
    {
        tape.IsTracking = false;

        Matrix4x4<TNumber> result = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_buffer[i][j] is Func<TTape, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> function)
                {
                    result[i, j] = function(tape, x).Value;
                }
            }
        }

        tape.IsTracking = true;
        return new MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1, TIndex2>(result);
    }

    /// <inheritdoc cref="TensorField4x4{TTape, TNumber, TIndex1Position, TIndex2Position, TPointIndex}.Derivative{TIndex1Name, TIndex2Name}(TTape, AutoDiffTensor4{TNumber, TPointIndex})"/>
    public new ReadOnlySpan<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>> Derivative<TIndex1Name, TIndex2Name>(TTape tape, AutoDiffTensor4<TNumber, TPointIndex> point)
        where TIndex1Name : ISymbol
        where TIndex2Name : ISymbol
    {
        Span<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>> result = new MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>[4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (this[i, j] is Func<TTape, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    result[0][i, j] = gradient[0];
                    result[1][i, j] = gradient[1];
                    result[2][i, j] = gradient[2];
                    result[3][i, j] = gradient[3];
                }
            }
        }
        return result;
    }

    // TODO: Optimize
    /// <summary>Compute the derivative of all elements of the inverse tensor.</summary>
    /// <typeparam name="TIndex1Name">The first index of the result tensor</typeparam>
    /// <typeparam name="TIndex2Name">The second index of the result tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="point">A point on the manifold</param>
    /// <returns>A read-only span of derivatives</returns>
    public ReadOnlySpan<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>> DerivativeOfInverse<TIndex1Name, TIndex2Name>(TTape tape, AutoDiffTensor4<TNumber, TPointIndex> point)
        where TIndex1Name : ISymbol
        where TIndex2Name : ISymbol
    {
        Span<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>> result = new MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>[4];

        var derivative = Derivative<TIndex1Name, TIndex2Name>(tape, point);
        ref var start = ref MemoryMarshal.GetReference(derivative);

        // The index names do not matter since they are only placeholders.
        var value = Compute<InternalIndex1, InternalIndex2>(tape, point);
        var inverse = Unsafe.As<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, InternalIndex1, InternalIndex2>, Matrix4x4<TNumber>>(ref value).Inverse();

        for (int i = 0; i < 4; i++)
        {
            var matrix = Unsafe.As<MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>, Matrix4x4<TNumber>>(ref Unsafe.Add(ref start, i));
            var derivativeOfInverse = -inverse * matrix * inverse;
            result[i] = Unsafe.As<Matrix4x4<TNumber>, MetricTensor<Matrix4x4<TNumber>, TNumber, Lower, TIndex1Name, TIndex2Name>>(ref derivativeOfInverse);
        }

        return result;
    }
}
