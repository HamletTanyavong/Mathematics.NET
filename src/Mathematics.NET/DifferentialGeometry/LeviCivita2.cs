// <copyright file="LeviCivita2.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a two-dimensional Levi-Civita symbol.</summary>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TI1">The first index.</typeparam>
/// <typeparam name="TI2">The second index.</typeparam>
public readonly struct LeviCivita2<TN, TI1, TI2>
    : IRankTwoTensor<LeviCivita2<TN, TI1, TI2>, Matrix2x2<TN>, TN, TI1, TI2>,
      IMultiplicationOperation<LeviCivita2<TN, TI1, TI2>, TN, Tensor<Matrix2x2<TN>, TN, TI1, TI2>>,
      IUnaryMinusOperation<LeviCivita2<TN, TI1, TI2>, Tensor<Matrix2x2<TN>, TN, TI1, TI2>>,
      IUnaryPlusOperation<LeviCivita2<TN, TI1, TI2>, LeviCivita2<TN, TI1, TI2>>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2 : IIndex
{
    private static Matrix2x2<TN> s_matrix;

    static LeviCivita2()
    {
        s_matrix = new();

        s_matrix[0, 1] = +1;
        s_matrix[1, 0] = -1;
    }

    //
    // IRankTwoTensor Interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => TI2.Instance;

    //
    // IArrayRepresentable & Relevant Interfaces
    //

    public static int Components => Matrix2x2<TN>.Components;

    public static int E1Components => Matrix2x2<TN>.E1Components;

    public static int E2Components => Matrix2x2<TN>.E2Components;

    //
    // Indexer
    //

    public TN this[int i, int j]
    {
        get => s_matrix[i, j];
        set => throw new NotSupportedException();
    }

    //
    // Operators
    //

    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> operator -(LeviCivita2<TN, TI1, TI2> tensor)
        => new(-s_matrix);

    public static LeviCivita2<TN, TI1, TI2> operator +(LeviCivita2<TN, TI1, TI2> tensor)
        => tensor;

    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> operator *(TN c, LeviCivita2<TN, TI1, TI2> tensor)
        => new(c * s_matrix);

    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> operator *(LeviCivita2<TN, TI1, TI2> tensor, TN c)
        => new(s_matrix * c);

    //
    // Equality
    //

    public static bool operator ==(LeviCivita2<TN, TI1, TI2> left, LeviCivita2<TN, TI1, TI2> right) => true;

    public static bool operator !=(LeviCivita2<TN, TI1, TI2> left, LeviCivita2<TN, TI1, TI2> right) => false;

#pragma warning disable EPC11
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LeviCivita2<TN, TI1, TI2>;

    public readonly bool Equals(LeviCivita2<TN, TI1, TI2> value) => true;
#pragma warning restore EPC11

    public override readonly int GetHashCode() => HashCode.Combine(s_matrix);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => s_matrix.ToString(format, provider);

    //
    // Methods
    //

    public TN[,] ToArray() => s_matrix.ToArray();

    //
    // Implicit Operators
    //

    public static implicit operator LeviCivita2<TN, TI1, TI2>(Matrix2x2<TN> input) => throw new NotSupportedException();
}
