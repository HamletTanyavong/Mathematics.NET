// <copyright file="FMTensorField2.cs" company="Mathematics.NET">
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
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN, U, V}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TIP">The position of the index of the tensor.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class FMTensorField2<TDN, TN, U, TIP, TPI> : TensorField<TN, U, TPI>
    where TDN : IDual<TDN, TN, U, U>
    where TN : IComplex<TN, U, U>, IDifferentiableFunctions<TN>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
    where TIP : IIndexPosition
    where TPI : IIndex
{
    private protected FMTensor2Buffer2<TDN, TN, U, TPI> _buffer;

    public FMTensorField2() { }

    public Func<AutoDiffTensor2<TDN, TN, U, TPI>, TDN> this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[i];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[i] = value;
    }

    /// <summary>Compute the value of the tensor at a specified point.</summary>
    /// <typeparam name="TIN">An index name.</typeparam>
    /// <param name="point">A point.</param>
    /// <returns>The value of the tensor at the specified point.</returns>
    public Tensor<Vector2<TN, U>, TN, U, Index<TIP, TIN>> Compute<TIN>(AutoDiffTensor2<TDN, TN, U, TPI> point)
        where TIN : IIndexName
    {
        Vector2<TN, U> result = new();
        for (int i = 0; i < 2; i++)
        {
            if (_buffer[i] is Func<AutoDiffTensor2<TDN, TN, U, TPI>, TDN> function)
                result[i] = function(point).D0;
        }
        return new Tensor<Vector2<TN, U>, TN, U, Index<TIP, TIN>>(result);
    }
}

internal static partial class Buffers
{
    [InlineArray(2)]
    public struct FMTensor2Buffer2<TDN, TN, U, TPI>
        where TDN : IDual<TDN, TN, U, U>
        where TN : IComplex<TN, U, U>, IDifferentiableFunctions<TN>
        where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
        where TPI : IIndex
    {
        private Func<AutoDiffTensor2<TDN, TN, U, TPI>, TDN> _element;
    }
}
