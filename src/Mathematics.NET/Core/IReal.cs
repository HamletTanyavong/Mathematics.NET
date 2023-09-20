﻿// <copyright file="IReal.cs" company="Mathematics.NET">
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

using System.Numerics;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for real numbers</summary>
/// <typeparam name="T">A type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/></typeparam>
public interface IReal<T, U>
    : IComplex<T, U>,
      IInequalityRelations<T, bool>,
      IComparable,
      IComparable<T>,
      IMinMaxValue<T>
    where T : IReal<T, U>
    where U : IFloatingPointIeee754<U>, IMinMaxValue<U>
{
    /// <summary>The backing value of the type</summary>
    U Value { get; }

    /// <summary>Represents negative infinty for the type</summary>
    static abstract T NegativeInfinity { get; }

    /// <summary>Represents positive infinity for the type</summary>
    static abstract T PositiveInfinity { get; }

    /// <summary>Check if a value is negative infinity</summary>
    /// <param name="x">The value to check</param>
    /// <returns><c>true</c> if the value is negative infinity; otherwise, <c>false</c></returns>
    static abstract bool IsNegativeInfinity(T x);

    /// <summary>Check if a valule is positive infinity</summary>
    /// <param name="x">The value to check</param>
    /// <returns><c>true</c> if the value is positive infinity; otherwise, <c>false</c></returns>
    static abstract bool IsPositiveInfinity(T x);
}
