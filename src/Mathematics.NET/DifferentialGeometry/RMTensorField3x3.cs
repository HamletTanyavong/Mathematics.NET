// <copyright file="RMTensorField3x3.cs" company="Mathematics.NET">
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
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core.Buffers;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-two tensor field with 9 elements.</summary>
/// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TI1P">The position of the first index of the tensor.</typeparam>
/// <typeparam name="TI2P">The position of the second index of the tensor.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class RMTensorField3x3<TT, TN, TI1P, TI2P, TPI> : TensorField<TN, TPI>
    where TT : ITape<TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1P : IIndexPosition
    where TI2P : IIndexPosition
    where TPI : IIndex
{
    private protected RMTensor3Buffer3x3<TT, TN, TPI> _buffer;

    public RMTensorField3x3() { }

    public Func<TT, AutoDiffTensor3<TN, TPI>, Variable<TN>> this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[row][column];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[row][column] = value;
    }

    /// <inheritdoc cref="RMTensorField2x2{TT, TN, TI1P, TI2P, TPI}.Compute{TI1N, TI2N}(TT, AutoDiffTensor2{TN, TPI})"/>
    public Tensor<Matrix3x3<TN>, TN, Index<TI1P, TI1N>, Index<TI2P, TI2N>> Compute<TI1N, TI2N>(TT tape, AutoDiffTensor3<TN, TPI> x)
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        tape.IsTracking = false;

        Matrix3x3<TN> result = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_buffer[i][j] is Func<TT, AutoDiffTensor3<TN, TPI>, Variable<TN>> function)
                    result[i, j] = function(tape, x).Value;
            }
        }

        tape.IsTracking = true;
        return new Tensor<Matrix3x3<TN>, TN, Index<TI1P, TI1N>, Index<TI2P, TI2N>>(result);
    }
}
