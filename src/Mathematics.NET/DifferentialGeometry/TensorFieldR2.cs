// <copyright file="TensorFieldR2.cs" company="Mathematics.NET">
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
using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using static Mathematics.NET.DifferentialGeometry.Buffers;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-one tensor field with two elements.</summary>
/// <typeparam name="TT">A type that implements <see cref="ITape{T, U}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TIP">An index position.</typeparam>
/// <typeparam name="TPI">An index.</typeparam>
public class TensorFieldR2<TT, TN, U, TIP, TPI> : TensorField<TN, U, TPI>
    where TT : ITape<TN, U>
    where TN : IComplex<TN, U, U>, IDifferentiableFunctions<TN>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    where TIP : IIndexPosition
    where TPI : IIndex
{
    private RMTensor2Buffer2<TT, TN, U, TPI> _buffer;

    public TensorFieldR2() { }

    public Func<TT, AutoDiffTensor2<TN, U, TPI>, Variable<TN, U>> this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[i];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[i] = value;
    }

    /// <summary>Compute the value of the tensor at a specified point.</summary>
    /// <typeparam name="TIN">An index name.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="point">A point.</param>
    /// <returns>The value of the tensor at the specified point.</returns>
    public Tensor<Vector2<TN, U>, TN, U, Index<TIP, TIN>> Compute<TIN>(TT tape, AutoDiffTensor2<TN, U, TPI> point)
        where TIN : IIndexName
    {
        Vector2<TN, U> result = new();
        for (int i = 0; i < 2; i++)
        {
            if (_buffer[i] is Func<TT, AutoDiffTensor2<TN, U, TPI>, Variable<TN, U>> function)
                result[i] = function(tape, point).Value;
        }
        return new Tensor<Vector2<TN, U>, TN, U, Index<TIP, TIN>>(result);
    }
}

internal static partial class Buffers
{
    [InlineArray(2)]
    internal struct RMTensor2Buffer2<TT, TN, U, TPI>
        where TT : ITape<TN, U>
        where TN : IComplex<TN, U, U>, IDifferentiableFunctions<TN>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        where TPI : IIndex
    {
        private Func<TT, AutoDiffTensor2<TN, U, TPI>, Variable<TN, U>> _element;
    }
}
