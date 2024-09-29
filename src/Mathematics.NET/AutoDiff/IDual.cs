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

using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.AutoDiff;

/// <summary>Defines support for dual numbers.</summary>
/// <typeparam name="TDN">The type that implements the interface.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
public interface IDual<TDN, TN>
    : IAdditionOperation<TDN, TDN>,
      IDivisionOperation<TDN, TDN>,
      IMultiplicationOperation<TDN, TDN>,
      IUnaryMinusOperation<TDN, TDN>,
      IUnaryPlusOperation<TDN, TDN>,
      ISubtractionOperation<TDN, TDN>,
      IFormattable,
      IEqualityRelation<TDN, bool>,
      IEquatable<TDN>,
      IDifferentiableFunctions<TDN>
    where TDN : IDual<TDN, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
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
    // DifGeo
    //

    /// <summary>Create an autodiff, rank-one tensor from vectors of initial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <param name="seed">A vector of seed values.</param>
    /// <returns>A rank-one tensor of two initial and seed values.</returns>
    static abstract AutoDiffTensor2<TDN, TN, TI> CreateAutoDiffTensor<TI>(in Vector2<TN> x, in Vector2<TN> seed)
        where TI : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from intial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <param name="seed0">The zeroth seed value.</param>
    /// <param name="seed1">The first seed value.</param>
    /// <returns>A rank-one tensor of two initial and seed values.</returns>
    static abstract AutoDiffTensor2<TDN, TN, TI> CreateAutoDiffTensor<TI>(in TN x0, in TN x1, in TN seed0, in TN seed1)
        where TI : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from vectors of initial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <param name="seed">A vector of seed values.</param>
    /// <returns>A rank-one tensor of three initial and seed values.</returns>
    static abstract AutoDiffTensor3<TDN, TN, TI> CreateAutoDiffTensor<TI>(in Vector3<TN> x, in Vector3<TN> seed)
        where TI : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from intial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <param name="x2">The second value.</param>
    /// <param name="seed0">The zeroth seed value.</param>
    /// <param name="seed1">The first seed value.</param>
    /// <param name="seed2">The second seed value.</param>
    /// <returns>A rank-one tensor of three initial and seed values.</returns>
    static abstract AutoDiffTensor3<TDN, TN, TI> CreateAutoDiffTensor<TI>(in TN x0, in TN x1, in TN x2, in TN seed0, in TN seed1, in TN seed2)
        where TI : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from vectors of initial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x">A vector of initial values.</param>
    /// <param name="seed">A vector of seed values.</param>
    /// <returns>A rank-one tensor of four initial and seed values.</returns>
    static abstract AutoDiffTensor4<TDN, TN, TI> CreateAutoDiffTensor<TI>(in Vector4<TN> x, in Vector4<TN> seed)
        where TI : IIndex;

    /// <summary>Create an autodiff, rank-one tensor from intial and seed values.</summary>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="x0">The zeroth value.</param>
    /// <param name="x1">The first value.</param>
    /// <param name="x2">The second value.</param>
    /// <param name="x3">The third value.</param>
    /// <param name="seed0">The zeroth seed value.</param>
    /// <param name="seed1">The first seed value.</param>
    /// <param name="seed2">The second seed value.</param>
    /// <param name="seed3">The third seed value.</param>
    /// <returns>A rank-one tensor of four initial and seed values.</returns>
    static abstract AutoDiffTensor4<TDN, TN, TI> CreateAutoDiffTensor<TI>(in TN x0, in TN x1, in TN x2, in TN x3, in TN seed0, in TN seed1, in TN seed2, in TN seed3)
        where TI : IIndex;
}
