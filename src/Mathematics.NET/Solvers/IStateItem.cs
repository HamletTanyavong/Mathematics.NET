// <copyright file="IStateItem.cs" company="Mathematics.NET">
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

using Mathematics.NET.Core.Operations;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.Solvers;

/// <summary>Defines support for state items.</summary>
/// <typeparam name="TSC">The type that implements the interface.</typeparam>
/// <typeparam name="TA">An array-like object that supports addition and multiplication on its elements.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
public interface IStateItem<TSC, TA, TN>
    : IAdditionOperation<TSC, TSC>,
      ISubtractionOperation<TSC, TSC>,
      IMultiplicationOperation<TSC, TN, TSC>,
      IUnaryPlusOperation<TSC, TSC>,
      IUnaryMinusOperation<TSC, TSC>
    where TSC : IStateItem<TSC, TA, TN>
    where TA
    : I1DArrayRepresentable<TA, TN>,
      IAdditionOperation<TA, TA>,
      ISubtractionOperation<TA, TA>,
      IMultiplicationOperation<TA, TN, TA>,
      IUnaryMinusOperation<TA, TA>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
{
    /// <summary>The number of <typeparamref name="TA"/> items.</summary>
    static abstract int E1Components { get; }

    /// <summary>The number of components in <typeparamref name="TA"/>.</summary>
    static abstract int E2Components { get; }

    /// <summary>Get the element at a certain index.</summary>
    /// <param name="i">The index.</param>
    /// <returns>A <typeparamref name="TA"/>.</returns>
    TA this[int i] { get; set; }
}
