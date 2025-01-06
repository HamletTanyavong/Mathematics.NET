// <copyright file="FMTensorField3.cs" company="Mathematics.NET">
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

/// <summary>Represents a rank-one tensor field with three elements.</summary>
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TIP">The position of the index of the tensor.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
public class FMTensorField3<TDN, TN, TIP, TPI> : TensorField<TN, TPI>
    where TDN : IDual<TDN, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TIP : IIndexPosition
    where TPI : IIndex
{
    private protected FMTensor3Buffer3<TDN, TN, TPI> _buffer;

    public FMTensorField3() { }

    public Func<AutoDiffTensor3<TDN, TN, TPI>, TDN> this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _buffer[i];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[i] = value;
    }

    /// <inheritdoc cref="FMTensorField2{TDN, TN, TIP, TPI}.Compute{TIN}(AutoDiffTensor2{TDN, TN, TPI})"/>
    public Tensor<Vector3<TN>, TN, Index<TIP, TIN>> Compute<TIN>(AutoDiffTensor3<TDN, TN, TPI> point)
        where TIN : IIndexName
    {
        Vector3<TN> result = new();
        for (int i = 0; i < 3; i++)
        {
            if (_buffer[i] is Func<AutoDiffTensor3<TDN, TN, TPI>, TDN> function)
                result[i] = function(point).D0;
        }
        return new Tensor<Vector3<TN>, TN, Index<TIP, TIN>>(result);
    }
}

#pragma warning disable IDE0051

internal static partial class Buffers
{
    [InlineArray(3)]
    internal struct FMTensor3Buffer3<TDN, TN, TPI>
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPI : IIndex
    {
        private Func<AutoDiffTensor3<TDN, TN, TPI>, TDN> _element0;
    }
}
