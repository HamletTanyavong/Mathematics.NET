﻿// <copyright file="IMultiplicationOperation`3.cs" company="Mathematics.NET">
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
/// <typeparam name="T">An input of the first type.</typeparam>
/// <typeparam name="U">An input of the second type.</typeparam>
/// <typeparam name="V">The output type.</typeparam>
public interface IMultiplicationOperation<T, U, V>
    where T : IMultiplicationOperation<T, U, V>
{
    /// <summary>Multiply the left value by the right value.</summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <returns>The product of the two values.</returns>
    static abstract V operator *(T left, U right);

    /// <inheritdoc cref="operator *(T, U)"/>
    static abstract V operator *(U left, T right);

    /// <inheritdoc cref="operator *(T, U)"/>
    static virtual V operator checked *(T left, U right) => left * right;

    /// <inheritdoc cref="operator *(T, U)"/>
    static virtual V operator checked *(U left, T right) => left * right;
}
