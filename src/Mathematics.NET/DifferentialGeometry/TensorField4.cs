// <copyright file="TensorField4.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-one tensor field with four elements</summary>
/// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <typeparam name="TIndexPosition">An index position</typeparam>
/// <typeparam name="TPointIndex">An index</typeparam>
public class TensorField4<TTape, TNumber, TIndexPosition, TPointIndex> : TensorField<TNumber, TPointIndex>
    where TTape : ITape<TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndexPosition : IIndexPosition
    where TPointIndex : IIndex
{
    private AutoDiffTensor4Buffer4<TTape, TNumber, TPointIndex> _buffer;

    public TensorField4() { }

    public Func<TTape, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[index] = value;
    }

    /// <summary>Compute the derivative of all elements of the tensor.</summary>
    /// <typeparam name="TIndexName">The first index of the result tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="point">A point on the manifold</param>
    /// <returns>A read-only span of derivatives</returns>
    public ReadOnlySpan<Tensor<Vector4<TNumber>, TNumber, Index<TIndexPosition, TIndexName>>> Derivative<TIndexName>(
        TTape tape,
        AutoDiffTensor4<TNumber, TPointIndex> point)
        where TIndexName : ISymbol
    {
        Span<Tensor<Vector4<TNumber>, TNumber, Index<TIndexPosition, TIndexName>>> result = new Tensor<Vector4<TNumber>, TNumber, Index<TIndexPosition, TIndexName>>[4];

        for (int i = 0; i < 4; i++)
        {
            if (this[i] is Func<TTape, AutoDiffTensor4<TNumber, TPointIndex>, Variable<TNumber>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                result[0][i] = gradient[0];
                result[1][i] = gradient[1];
                result[2][i] = gradient[2];
                result[3][i] = gradient[3];
            }
        }
        return result;
    }
}
