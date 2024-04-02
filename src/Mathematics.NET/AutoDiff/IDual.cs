// <copyright file="IDual.cs" company="Mathematics.NET">
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

using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for dual numbers</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
public interface IDual<T, U>
    : IAdditionOperation<T, T>,
      IDivisionOperation<T, T>,
      IMultiplicationOperation<T, T>,
      IUnaryMinusOperation<T, T>,
      IUnaryPlusOperation<T, T>,
      ISubtractionOperation<T, T>,
      IFormattable,
      IEqualityRelation<T, bool>,
      IEquatable<T>,
      IDifferentiableFunctions<T>
    where T : IDual<T, U>
    where U : IComplex<U>, IDifferentiableFunctions<U>
{
    /// <summary>Represents the primal part of the dual number</summary>
    U D0 { get; }

    /// <summary>Represents the tangent part of the dual number</summary>
    U D1 { get; }

    /// <summary>Create an instance of the type with a specified value.</summary>
    /// <param name="value">A value</param>
    /// <returns>An instance of the type</returns>
    static abstract T CreateVariable(U value);

    /// <summary>Create an instance of the type with a specified value and seed.</summary>
    /// <param name="value">A value</param>
    /// <param name="seed">A seed</param>
    /// <returns>An instance of the type</returns>
    static abstract T CreateVariable(U value, U seed);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    static abstract T Pow(T x, U y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    static abstract T Pow(U x, T y);

    /// <summary>Create a seeded instance of this type</summary>
    /// <param name="seed">The seed value</param>
    /// <returns>A seeded value</returns>
    T WithSeed(U seed);
}
