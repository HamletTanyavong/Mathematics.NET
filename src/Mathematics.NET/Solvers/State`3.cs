// <copyright file="State`3.cs" company="Mathematics.NET">
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

/// <summary>Represents the state of a system.</summary>
/// <typeparam name="TSI">A type that implements <see cref="IStateItem{TSC, TA, TN}"/>.</typeparam>
/// <typeparam name="TA">An array-like object that supports addition and multiplication on its elements.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <param name="system">The system.</param>
/// <param name="time">The time.</param>
public sealed class State<TSI, TA, TN>(Memory<TSI> system, TN time)
    where TSI : IStateItem<TSI, TA, TN>
    where TA
    : I1DArrayRepresentable<TA, TN>,
      IAdditionOperation<TA, TA>,
      ISubtractionOperation<TA, TA>,
      IMultiplicationOperation<TA, TN, TA>,
      IUnaryMinusOperation<TA, TA>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
{
    /// <inheritdoc cref="State{T}.System"/>
    public Memory<TSI> System = system;

    /// <inheritdoc cref="State{T}.Time"/>
    public TN Time = time;
}
