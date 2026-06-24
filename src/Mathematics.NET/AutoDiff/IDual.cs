// <copyright file="IDual.cs" company="Mathematics.NET">
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
using Mathematics.NET.Operations;
using Mathematics.NET.Relations;

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for dual numbers.</summary>
/// <typeparam name="TDN">The type that implements the interface.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryNumber{TSelf}"/>.</typeparam>
/// <typeparam name="V">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
public interface IDual<TDN, TN, U, V>
    : IAdditionOperation<TDN, TDN>,
      IDivisionOperation<TDN, TDN>,
      IMultiplicationOperation<TDN, TDN>,
      IUnaryMinusOperation<TDN, TDN>,
      IUnaryPlusOperation<TDN, TDN>,
      ISubtractionOperation<TDN, TDN>,
      IEqualityRelation<TDN, bool>,
      IEquatable<TDN>,
      IFormattable,
      IDifferentiableFunctions<TDN>
    where TDN : IDual<TDN, TN, U, V>
    where TN : IComplex<TN, U, V>, IDifferentiableFunctions<TN>
    where U : IBinaryNumber<U>
    where V : IBinaryFloatingPointIeee754<V>, IMinMaxValue<V>
{
    /// <summary>Represents the primal part of the dual number.</summary>
    TN D0 { get; }

    /// <summary>Represents the tangent part of the dual number.</summary>
    TN D1 { get; }

    /// <summary>Create an instance of the type with a specified value.</summary>
    /// <param name="value">A value.</param>
    /// <returns>An instance of the type.</returns>
    static abstract TDN CreateVariable(TN value);

    /// <summary>Create an instance of the type with a specified value and seed.</summary>
    /// <param name="value">A value.</param>
    /// <param name="seed">A seed.</param>
    /// <returns>An instance of the type.</returns>
    static abstract TDN CreateVariable(TN value, TN seed);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    static abstract TDN Pow(TDN x, TN y);

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    static abstract TDN Pow(TN x, TDN y);

    /// <summary>Create a seeded instance of this type.</summary>
    /// <param name="seed">The seed value.</param>
    /// <returns>A seeded value.</returns>
    TDN WithSeed(TN seed);

    //
    // Implicit Operators
    //

    /// <summary>Convert a number into a dual number.</summary>
    /// <remarks>The tangent parts of the dual number will be zero.</remarks>
    /// <param name="value">A number.</param>
    static abstract implicit operator TDN(TN value);

    /// <summary>Convert a backing number into a dual number.</summary>
    /// <remarks>The tangent parts of the dual number will be zero.</remarks>
    /// <param name="value">A number.</param>
    static abstract implicit operator TDN(V value);
}
