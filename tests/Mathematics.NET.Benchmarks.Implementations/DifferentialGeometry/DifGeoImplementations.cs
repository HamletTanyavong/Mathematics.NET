// <copyright file="DifGeoImplementations.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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

using System.Runtime.CompilerServices;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.Benchmarks.Implementations.DifferentialGeometry;

public static class DifGeoImplementations
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static W ContractRankOneTensorWithNoInKeyword<T, U, V, W, IC>(IRankOneTensor<T, V, W, Index<Lower, IC>> a, IRankOneTensor<U, V, W, Index<Upper, IC>> b)
        where T : IRankOneTensor<T, V, W, Index<Lower, IC>>
        where U : IRankOneTensor<U, V, W, Index<Upper, IC>>
        where V : IVector<V, W>
        where W : IComplex<W>, IDifferentiableFunctions<W>
        where IC : ISymbol
    {
        var result = W.Zero;
        for (int i = 0; i < V.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static W ContractRankOneTensorWithInKeyword<T, U, V, W, IC>(in IRankOneTensor<T, V, W, Index<Lower, IC>> a, in IRankOneTensor<U, V, W, Index<Upper, IC>> b)
        where T : IRankOneTensor<T, V, W, Index<Lower, IC>>
        where U : IRankOneTensor<U, V, W, Index<Upper, IC>>
        where V : IVector<V, W>
        where W : IComplex<W>, IDifferentiableFunctions<W>
        where IC : ISymbol
    {
        var result = W.Zero;
        for (int i = 0; i < V.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> ContractRankThreeTensorWithNoInKeyword<T, U, V, IC, I1, I2, I3, I4>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4>
        where V : IComplex<V>, IDifferentiableFunctions<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
        where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> ContractRankThreeTensorWithInKeyword<T, U, V, IC, I1, I2, I3, I4>(
        in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4>
        where V : IComplex<V>, IDifferentiableFunctions<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
        where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> ContractRankThreeTensorWithRefReadonlyKeyword<V, IC, I1, I2, I3, I4>(
        ref readonly Tensor<Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        ref readonly Tensor<Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where V : IComplex<V>, IDifferentiableFunctions<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
        where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }
}
