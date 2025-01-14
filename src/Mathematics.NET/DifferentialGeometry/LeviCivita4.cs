// <copyright file="LeviCivita4.cs" company="Mathematics.NET">
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

/// <summary>Represents a four-dimensional Levi-Civita symbol.</summary>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TI1">The first index.</typeparam>
/// <typeparam name="TI2">The second index.</typeparam>
/// <typeparam name="TI3">The third index.</typeparam>
/// <typeparam name="TI4">The fourth index.</typeparam>
public readonly struct LeviCivita4<TN, TI1, TI2, TI3, TI4>
    : IRankFourTensor<LeviCivita4<TN, TI1, TI2, TI3, TI4>, Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4>,
      IMultiplicationOperation<LeviCivita4<TN, TI1, TI2, TI3, TI4>, TN, Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4>>,
      IUnaryMinusOperation<LeviCivita4<TN, TI1, TI2, TI3, TI4>, Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4>>,
      IUnaryPlusOperation<LeviCivita4<TN, TI1, TI2, TI3, TI4>, LeviCivita4<TN, TI1, TI2, TI3, TI4>>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TI1 : IIndex
    where TI2 : IIndex
    where TI3 : IIndex
    where TI4 : IIndex
{
    private static readonly Array4x4x4x4<TN> s_array;

    static LeviCivita4()
    {
        s_array = new();

        s_array[0, 1, 2, 3] = +1;
        s_array[0, 1, 3, 2] = -1;
        s_array[0, 2, 1, 3] = -1;
        s_array[0, 2, 3, 1] = +1;
        s_array[0, 3, 1, 2] = +1;
        s_array[0, 3, 2, 1] = -1;
        s_array[1, 0, 2, 3] = -1;
        s_array[1, 0, 3, 2] = +1;
        s_array[1, 2, 0, 3] = +1;
        s_array[1, 2, 3, 0] = -1;
        s_array[1, 3, 0, 2] = -1;
        s_array[1, 3, 2, 0] = +1;
        s_array[2, 0, 1, 3] = +1;
        s_array[2, 0, 3, 1] = -1;
        s_array[2, 1, 0, 3] = -1;
        s_array[2, 1, 3, 0] = +1;
        s_array[2, 3, 0, 1] = +1;
        s_array[2, 3, 1, 0] = -1;
        s_array[3, 0, 1, 2] = -1;
        s_array[3, 0, 2, 1] = +1;
        s_array[3, 1, 0, 2] = +1;
        s_array[3, 1, 2, 0] = -1;
        s_array[3, 2, 0, 1] = -1;
        s_array[3, 2, 1, 0] = +1;
    }

    //
    // IRankThreeTensor Interface
    //

    public readonly IIndex I1 => TI1.Instance;

    public readonly IIndex I2 => TI2.Instance;

    public readonly IIndex I3 => TI3.Instance;

    public readonly IIndex I4 => TI4.Instance;

    //
    // IArrayRepresentable & Relevant Interfaces
    //

    public static int Components => Array4x4x4x4<TN>.Components;

    public static int E1Components => Array4x4x4x4<TN>.E1Components;

    public static int E2Components => Array4x4x4x4<TN>.E2Components;

    public static int E3Components => Array4x4x4x4<TN>.E3Components;

    public static int E4Components => Array4x4x4x4<TN>.E4Components;

    //
    // Indexer
    //

    public TN this[int i, int j, int k, int l]
    {
        get => s_array[i, j, k, l];
        set => throw new NotSupportedException();
    }

    //
    // Operators
    //

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> operator -(LeviCivita4<TN, TI1, TI2, TI3, TI4> tensor)
        => new(-s_array);

    public static LeviCivita4<TN, TI1, TI2, TI3, TI4> operator +(LeviCivita4<TN, TI1, TI2, TI3, TI4> tensor)
        => tensor;

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> operator *(TN c, LeviCivita4<TN, TI1, TI2, TI3, TI4> tensor)
        => new(c * s_array);

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> operator *(LeviCivita4<TN, TI1, TI2, TI3, TI4> tensor, TN c)
        => new(s_array * c);

    //
    // Equality
    //

    public static bool operator ==(LeviCivita4<TN, TI1, TI2, TI3, TI4> left, LeviCivita4<TN, TI1, TI2, TI3, TI4> right) => true;

    public static bool operator !=(LeviCivita4<TN, TI1, TI2, TI3, TI4> left, LeviCivita4<TN, TI1, TI2, TI3, TI4> right) => false;

#pragma warning disable EPC11
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LeviCivita4<TN, TI1, TI2, TI3, TI4>;

    public readonly bool Equals(LeviCivita4<TN, TI1, TI2, TI3, TI4> value) => true;
#pragma warning restore EPC11

    public override readonly int GetHashCode() => HashCode.Combine(s_array);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider) => s_array.ToString(format, provider);

    //
    // Methods
    //

    public TN[,,,] ToArray() => s_array.ToArray();

    //
    // Implicit Operators
    //

    public static implicit operator LeviCivita4<TN, TI1, TI2, TI3, TI4>(Array4x4x4x4<TN> input) => throw new NotSupportedException();
}
