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

/// <summary>Defines support for rank-three tensors</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A backing type that implements <see cref="ICubicArray{T, U}"/></typeparam>
/// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="W">An index</typeparam>
/// <typeparam name="X">An index</typeparam>
/// <typeparam name="Y">An index</typeparam>
public interface IRankThreeTensor<T, U, V, W, X, Y> : IThreeDimensionalArrayRepresentable<T, V>
    where T : IRankThreeTensor<T, U, V, W, X, Y>
    where U : ICubicArray<U, V>
    where V : IComplex<V>
    where W : IIndex
    where X : IIndex
    where Y : IIndex
{
    /// <summary>The first index</summary>
    IIndex I1 { get; }

    /// <summary>The second index</summary>
    IIndex I2 { get; }

    /// <summary>The third index</summary>
    IIndex I3 { get; }

    /// <summary>Convert a value that implements <see cref="ICubicArray{T, U}"/></summary>
    /// <param name="value">The value to convert</param>
    static abstract implicit operator T(U value);
}
