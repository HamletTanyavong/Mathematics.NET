// <copyright file="IRankFourTensor.cs" company="Mathematics.NET">
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

using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry.Abstractions;

/// <summary>Defines support for rank-four tensors</summary>
/// <typeparam name="TRankFourTensor">The type that implements the interface</typeparam>
/// <typeparam name="THyperCubic4DArray">A backing type that implements <see cref="IHyperCubic4DArray{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">An index</typeparam>
/// <typeparam name="TIndex2">An index</typeparam>
/// <typeparam name="TIndex3">An index</typeparam>
/// <typeparam name="TIndex4">An index</typeparam>
public interface IRankFourTensor<TRankFourTensor, THyperCubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4> : IFourDimensionalArrayRepresentable<TRankFourTensor, TNumber>
    where TRankFourTensor : IRankFourTensor<TRankFourTensor, THyperCubic4DArray, TNumber, TIndex1, TIndex2, TIndex3, TIndex4>
    where THyperCubic4DArray : IHyperCubic4DArray<THyperCubic4DArray, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2 : IIndex
    where TIndex3 : IIndex
    where TIndex4 : IIndex
{
    /// <summary>The first index</summary>
    IIndex I1 { get; }

    /// <summary>The second index</summary>
    IIndex I2 { get; }

    /// <summary>The third index</summary>
    IIndex I3 { get; }

    /// <summary>The fourth index</summary>
    IIndex I4 { get; }

    /// <summary>Convert a value that implements <see cref="IHyperCubic4DArray{T, U}"/> to one of type <typeparamref name="TRankFourTensor"/></summary>
    /// <param name="value">The value to convert</param>
    static abstract implicit operator TRankFourTensor(THyperCubic4DArray value);
}
