// <copyright file="IRankOneTensor.cs" company="Mathematics.NET">
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
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Operations;

namespace Mathematics.NET.DifferentialGeometry.Abstractions;

/// <summary>Defines support for rank-one tensors and similar mathematical objects.</summary>
/// <typeparam name="TR1T">The type that implements the interface.</typeparam>
/// <typeparam name="TV">A backing type that implements <see cref="IVector{T, U, V, W}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryNumber{TSelf}"/>.</typeparam>
/// <typeparam name="V">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TI">An index.</typeparam>
public interface IRankOneTensor<TR1T, TV, TN, U, V, TI>
    : I1DArrayRepresentable<TR1T, TN, U, V>,
      IAdditionOperation<TR1T, TR1T>,
      ISubtractionOperation<TR1T, TR1T>
    where TR1T : IRankOneTensor<TR1T, TV, TN, U, V, TI>
    where TV : IVector<TV, TN, U, V>
    where TN : IComplex<TN, U, V>, IDifferentiableFunctions<TN>
    where U : IBinaryNumber<U>
    where V : IBinaryFloatingPointIeee754<V>, IMinMaxValue<V>
    where TI : IIndex
{
    /// <summary>Get the index associated with this rank one tensor.</summary>
    IIndex I1 { get; }

    /// <summary>Convert a value that implements <see cref="IVector{T, U, V, W}"/> to one of type <typeparamref name="TR1T"/>.</summary>
    /// <param name="value">The value to convert.</param>
    static abstract implicit operator TR1T(TV value);
}
