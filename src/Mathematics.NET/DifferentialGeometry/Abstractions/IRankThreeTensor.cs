// <copyright file="IRankThreeTensor.cs" company="Mathematics.NET">
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

/// <summary>Defines support for rank-three tensors and similar mathematical objects</summary>
/// <typeparam name="TRankThreeTensor">The type that implements the interface</typeparam>
/// <typeparam name="TCubicArray">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="TNumber">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="TIndex1">An index</typeparam>
/// <typeparam name="TIndex2">An index</typeparam>
/// <typeparam name="TIndex3">An index</typeparam>
public interface IRankThreeTensor<TRankThreeTensor, TCubicArray, TNumber, TIndex1, TIndex2, TIndex3> : IThreeDimensionalArrayRepresentable<TRankThreeTensor, TNumber>
    where TRankThreeTensor : IRankThreeTensor<TRankThreeTensor, TCubicArray, TNumber, TIndex1, TIndex2, TIndex3>
    where TCubicArray : ICubicArray<TCubicArray, TNumber>
    where TNumber : IComplex<TNumber>, IDifferentiableFunctions<TNumber>
    where TIndex1 : IIndex
    where TIndex2 : IIndex
    where TIndex3 : IIndex
{
    /// <summary>The first index</summary>
    IIndex I1 { get; }

    /// <summary>The second index</summary>
    IIndex I2 { get; }

    /// <summary>The third index</summary>
    IIndex I3 { get; }

    /// <summary>Convert a value that implements <see cref="ICubicArray{T, U}"/></summary>
    /// <param name="value">The value to convert</param>
    static abstract implicit operator TRankThreeTensor(TCubicArray value);
}
