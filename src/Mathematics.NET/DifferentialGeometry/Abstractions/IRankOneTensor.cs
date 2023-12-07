// <copyright file="IRankOneTensor.cs" company="Mathematics.NET">
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

/// <summary>Defines support for rank-one tensors</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
/// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <typeparam name="W">An index</typeparam>
public interface IRankOneTensor<T, U, V, W> : IOneDimensionalArrayRepresentable<T, V>
    where T : IRankOneTensor<T, U, V, W>
    where U : IVector<U, V>
    where V : IComplex<V>
    where W : IIndex
{
    /// <summary>Get the index associated with this rank one tensor</summary>
    IIndex I1 { get; }

    /// <summary>Convert a value that implements <see cref="IVector{T, U}"/> to one of type <typeparamref name="T"/></summary>
    /// <param name="input">The value to convert</param>
    static abstract implicit operator T(U input);
}
