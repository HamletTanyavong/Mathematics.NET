// <copyright file="LeviCivita3.cs" company="Mathematics.NET">
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

/// <summary>Represents a three-dimensional Levi-Civita symbol.</summary>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TI1">The first index.</typeparam>
/// <typeparam name="TI2">The second index.</typeparam>
/// <typeparam name="TI3">The third index.</typeparam>
public readonly struct LeviCivita3<TN, TI1, TI2, TI3>
    : IRankThreeTensor<LeviCivita3<TN, TI1, TI2, TI3>, Array3x3x3<TN>, TN, TI1, TI2, TI3>,
      IMultiplicationOperation<LeviCivita3<TN, TI1, TI2, TI3>, TN, Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3>>,
      IUnaryMinusOperation<LeviCivita3<TN, TI1, TI2, TI3>, Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3>>,
      IUnaryPlusOperation<LeviCivita3<TN, TI1, TI2, TI3>, LeviCivita3<TN, TI1, TI2, TI3>>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2 : IIndex
    where TI3 : IIndex
{
    private static Array3x3x3<TN> s_array;

    static LeviCivita3()
    {
        s_array = new();

        s_array[0, 1, 2] = +1;
        s_array[0, 2, 1] = -1;
        s_array[1, 0, 2] = -1;
        s_array[1, 2, 0] = +1;
        s_array[2, 0, 1] = +1;
        s_array[2, 1, 0] = -1;
    }

    //
    // IRankThreeTensor Interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => TI2.Instance;

    public readonly IIndex I3 => TI3.Instance;

    //
    // IArrayRepresentable & Relevant Interfaces
    //

    public static int Components => Array3x3x3<TN>.Components;

    public static int E1Components => Array3x3x3<TN>.E1Components;

    public static int E2Components => Array3x3x3<TN>.E2Components;

    public static int E3Components => Array3x3x3<TN>.E3Components;

    //
    // Indexer
    //

    public TN this[int i, int j, int k]
    {
        get => s_array[i, j, k];
        set => throw new NotSupportedException();
    }

    //
    // Operators
    //

    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> operator -(LeviCivita3<TN, TI1, TI2, TI3> tensor)
        => new(-s_array);

    public static LeviCivita3<TN, TI1, TI2, TI3> operator +(LeviCivita3<TN, TI1, TI2, TI3> tensor)
        => tensor;

    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> operator *(TN c, LeviCivita3<TN, TI1, TI2, TI3> tensor)
        => new(c * s_array);

    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> operator *(LeviCivita3<TN, TI1, TI2, TI3> tensor, TN c)
        => new(s_array * c);

    //
    // Equality
    //

    public static bool operator ==(LeviCivita3<TN, TI1, TI2, TI3> left, LeviCivita3<TN, TI1, TI2, TI3> right) => true;

    public static bool operator !=(LeviCivita3<TN, TI1, TI2, TI3> left, LeviCivita3<TN, TI1, TI2, TI3> right) => false;

#pragma warning disable EPC11
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LeviCivita3<TN, TI1, TI2, TI3>;

    public readonly bool Equals(LeviCivita3<TN, TI1, TI2, TI3> value) => true;
#pragma warning restore EPC11

    public override readonly int GetHashCode() => HashCode.Combine(s_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => s_array.ToString(format, provider);

    //
    // Methods
    //

    public TN[,,] ToArray() => s_array.ToArray();

    //
    // Implicit Operators
    //

    public static implicit operator LeviCivita3<TN, TI1, TI2, TI3>(Array3x3x3<TN> input) => throw new NotSupportedException();
}
