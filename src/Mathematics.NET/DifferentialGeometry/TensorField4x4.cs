// <copyright file="TensorField4x4.cs" company="Mathematics.NET">
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
using Mathematics.NET.Core.Buffers;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-two tensor field with 16 elements</summary>
/// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <typeparam name="TIndex1Position">An index</typeparam>
/// <typeparam name="TIndex2Position">An index</typeparam>
/// <typeparam name="TPointIndex">An index</typeparam>
public class TensorField4x4<TTape, TNumber, TIndex1Position, TIndex2Position, TPointIndex> : TensorField<TNumber, TPointIndex>
    where TTape : ITape<TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1Position : IIndexPosition
    where TIndex2Position : IIndexPosition
    where TPointIndex : IIndex
{
    private protected AutoDiffTensor4Buffer4x4<TTape, TNumber, TPointIndex> _buffer;

    public TensorField4x4() { }

    public Func<TTape, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[row][column];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[row][column] = value;
    }

    public Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1>, Index<TIndex2Position, TIndex2>> Compute<TIndex1, TIndex2>(TTape tape, AutoDiffTensor4<TNumber, TPointIndex> x)
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
        return new Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1>, Index<TIndex2Position, TIndex2>>(result);
    }

    /// <summary>Compute the derivative of all elements of the tensor.</summary>
    /// <typeparam name="TIndex1Name">The first index of the result tensor</typeparam>
    /// <typeparam name="TIndex2Name">The second index of the result tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="point">A point on the manifold</param>
    /// <returns>A read-only span of derivatives</returns>
    public ReadOnlySpan<Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>> Derivative<TIndex1Name, TIndex2Name>(
        TTape tape,
        AutoDiffTensor4<TNumber, TPointIndex> point)
        where TIndex1Name : ISymbol
        where TIndex2Name : ISymbol
    {
        Span<Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>> result = new Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>[4];

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

    /// <summary>Compute the second derivative of all elements of the tensor.</summary>
    /// <typeparam name="TIndex1Name">The first index of the result tensor</typeparam>
    /// <typeparam name="TIndex2Name">The second index of the result tensor</typeparam>
    /// <param name="tape">A Hessian tape</param>
    /// <param name="point">A point on the manifold</param>
    /// <returns>A 2D read-only spans of derivatives</returns>
    public ReadOnlySpan2D<Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>> SecondDerivative<TIndex1Name, TIndex2Name>(
        HessianTape<TNumber> tape,
        AutoDiffTensor4<TNumber, TPointIndex> point)
        where TIndex1Name : ISymbol
        where TIndex2Name : ISymbol
    {
        Span2D<Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>> result = new Tensor<Matrix4x4<TNumber>, TNumber, Index<TIndex1Position, TIndex1Name>, Index<TIndex2Position, TIndex2Name>>[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (this[i, j] is Func<HessianTape<TNumber>, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TNumber> hessian);

                    result[0, 0][i, j] = hessian[0, 0]; result[0, 1][i, j] = hessian[0, 1]; result[0, 2][i, j] = hessian[0, 2]; result[0, 3][i, j] = hessian[0, 3];
                    result[1, 0][i, j] = hessian[1, 0]; result[1, 1][i, j] = hessian[1, 1]; result[1, 2][i, j] = hessian[1, 2]; result[1, 3][i, j] = hessian[1, 3];
                    result[2, 0][i, j] = hessian[2, 0]; result[2, 1][i, j] = hessian[2, 1]; result[2, 2][i, j] = hessian[2, 2]; result[2, 3][i, j] = hessian[2, 3];
                    result[3, 0][i, j] = hessian[3, 0]; result[3, 1][i, j] = hessian[3, 1]; result[3, 2][i, j] = hessian[3, 2]; result[3, 3][i, j] = hessian[3, 3];
                }
            }
        }
        return result;
    }
}
