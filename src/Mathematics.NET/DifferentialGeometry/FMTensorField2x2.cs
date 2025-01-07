﻿// <copyright file="FMTensorField2x2.cs" company="Mathematics.NET">
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
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using static Mathematics.NET.DifferentialGeometry.Buffers;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-two tensor field with four elements.</summary>
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TI1P">The position of the first index of the tensor.</typeparam>
/// <typeparam name="TI2P">The position of the second index of the tensor.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class FMTensorField2x2<TDN, TN, TI1P, TI2P, TPI> : TensorField<TN, TPI>
    where TDN : IDual<TDN, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1P : IIndexPosition
    where TI2P : IIndexPosition
    where TPI : IIndex
{
    private protected FMTensor2Buffer2x2<TDN, TN, TPI> _buffer;

    public FMTensorField2x2() { }

    public Func<AutoDiffTensor2<TDN, TN, TPI>, TDN> this[int i, int j]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[i][j];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[i][j] = value;
    }

    /// <summary>Compute the value of the tensor at a specified point.</summary>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <param name="point">A point.</param>
    /// <returns>The value of the tensor at the specified point.</returns>
    public Tensor<Matrix2x2<TN>, TN, Index<TI1P, TI1N>, Index<TI2P, TI2N>> Compute<TI1N, TI2N>(AutoDiffTensor2<TDN, TN, TPI> point)
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
        return new Tensor<Matrix2x2<TN>, TN, Index<TI1P, TI1N>, Index<TI2P, TI2N>>(result);
    }
}

#pragma warning disable IDE0051

internal static partial class Buffers
{
    [InlineArray(2)]
    public struct FMTensor2Buffer2x2<TDN, TN, TPI>
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPI : IIndex
    {
        private FMTensor2Buffer2<TDN, TN, TPI> _element0;
    }
}