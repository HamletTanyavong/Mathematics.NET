// <copyright file="IMultiplicationOperation`3.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core.Operations;

/// <summary>Defines a mechanism for multiplying two values of different types.</summary>
/// <typeparam name="TLeft">An input of the first type.</typeparam>
/// <typeparam name="TRight">An input of the second type.</typeparam>
/// <typeparam name="TOutput">The output type.</typeparam>
public interface IMultiplicationOperation<in TLeft, in TRight, out TOutput>
    where TLeft : IMultiplicationOperation<TLeft, TRight, TOutput>
{
    /// <summary>Multiply the left value by the right value.</summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <returns>The product of the two values.</returns>
    static abstract TOutput operator *(TLeft left, TRight right);

    /// <inheritdoc cref="operator *(TLeft, TRight)"/>
    static abstract TOutput operator *(TRight left, TLeft right);

    /// <inheritdoc cref="operator *(TLeft, TRight)"/>
    static virtual TOutput operator checked *(TLeft left, TRight right) => left * right;

    /// <inheritdoc cref="operator *(TLeft, TRight)"/>
    static virtual TOutput operator checked *(TRight left, TLeft right) => left * right;
}
