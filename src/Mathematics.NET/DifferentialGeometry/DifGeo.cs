﻿// <copyright file="DifGeo.cs" company="Mathematics.NET">
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

using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>A class containing differential geometry operations</summary>
public static class DifGeo
{
    //
    // Tensor contractions
    //

    // Rank-one and rank-one

    /// <summary>Contract two rank-one tensors.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
    /// <typeparam name="W">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A tensor with a lower index</param>
    /// <param name="b">A tensor with an upper index</param>
    /// <returns>A scalar</returns>
    public static W Contract<T, U, V, W, IC>(IRankOneTensor<T, V, W, Index<Lower, IC>> a, IRankOneTensor<U, V, W, Index<Upper, IC>> b)
        where T : IRankOneTensor<T, V, W, Index<Lower, IC>>
        where U : IRankOneTensor<U, V, W, Index<Upper, IC>>
        where V : IVector<V, W>
        where W : IComplex<W>
        where IC : ISymbol
    {
        var result = W.Zero;
        for (int i = 0; i < V.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    /// <summary>Contract two rank-one tensors.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A backing type that implements <see cref="IVector{T, U}"/></typeparam>
    /// <typeparam name="W">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A tensor with an upper index</param>
    /// <param name="b">A tensor with a lower index</param>
    /// <returns>A scalar</returns>
    public static W Contract<T, U, V, W, IC>(IRankOneTensor<T, V, W, Index<Upper, IC>> a, IRankOneTensor<U, V, W, Index<Lower, IC>> b)
        where T : IRankOneTensor<T, V, W, Index<Upper, IC>>
        where U : IRankOneTensor<U, V, W, Index<Lower, IC>>
        where V : IVector<V, W>
        where W : IComplex<W>
        where IC : ISymbol
    {
        var result = W.Zero;
        for (int i = 0; i < V.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    // Rank-one and rank-two

    /// <summary>Contract a rank-one tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j] * b[j, i];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-one tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j] * b[j, i];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-one tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The first index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I, Index<Upper, IC>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j] * b[i, j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-one tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The first index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I, Index<Lower, IC>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j] * b[i, j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-two tensor with a rank one tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j, i] * b[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-two tensor with a rank one tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j, i] * b[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-two tensor with a rank one tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I">The first index of the rank-two tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, I, IC>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I, Index<Lower, IC>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I, Index<Lower, IC>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>
        where I : IIndex
        where IC : ISymbol
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[i, j] * b[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a rank-two tensor with a rank one tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I">The first index of the rank-two tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, I> Contract<T, U, V, I, IC>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I, Index<Upper, IC>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I, Index<Upper, IC>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>>
        where V : IComplex<V>
        where I : IIndex
        where IC : ISymbol
    {
        Vector4<V> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[i, j] * b[j];
            }
        }
        return new(vector);
    }

    // Rank-one and rank-three

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[k, i, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[k, i, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[i, k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[i, k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The second index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[i, j, k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-one tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The second index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k] * b[i, j, k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, j] * b[k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, j] * b[k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, j] * b[k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, j] * b[k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="U">A rank-one tensor with an upper index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, I2, IC>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, j, k] * b[k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract a rank-three tensor with a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="U">A rank-one tensor with a lower index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="I2">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, I2, IC>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, j, k] * b[k];
                }
            }
        }
        return new(matrix);
    }

    // Rank-two and rank-two

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i] * b[k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I2>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i] * b[k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i] * b[j, k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i] * b[j, k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I2>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k] * b[j, k];
                }
            }
        }
        return new(matrix);
    }

    /// <summary>Contract two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, I1, IC, I2>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k] * b[j, k];
                }
            }
        }
        return new(matrix);
    }

    // Rank-two and rank-three

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I2, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I2, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Upper, IC>, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Upper, IC>, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[j, l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Lower, IC>, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Lower, IC>, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[j, l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the second tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the second tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i] * b[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I2, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I2, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Upper, IC>, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Upper, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[j, l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Lower, IC>, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, Index<Lower, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[j, l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-two tensor with a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Lower, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, Index<Upper, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l] * b[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i, j] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i, j] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i, j] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I1">The second index of the first tensor</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>>
        where V : IComplex<V>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[l, i, j] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l, j] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l, j] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l, j] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I2">The third index of the first tensor</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, IC, I2, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where IC : ISymbol
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, l, j] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, I2, IC, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, j, l] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, I2, IC, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, IC>, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, j, l] * b[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with a lower, third index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, I2, IC, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Upper, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, j, l] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Contract a rank-three tensor with a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-three tensor with an upper, third index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="IC">The name of the index to contract</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, I2, IC, I3>(
        IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I3, Index<Lower, IC>>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where IC : ISymbol
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k] += a[i, j, l] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    //
    // Tensor products
    //

    /// <summary>Compute the tensor product of two rank-one tensors.</summary>
    /// <typeparam name="T">A rank-one tensor</typeparam>
    /// <typeparam name="U">A rank-one tensor</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The index of the first tensor</typeparam>
    /// <typeparam name="I2">The index of the second tensor</typeparam>
    /// <param name="a">The first tensor</param>
    /// <param name="b">The second tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, I1, I2> TensorProduct<T, U, V, I1, I2>(IRankOneTensor<T, Vector4<V>, V, I1> a, IRankOneTensor<U, Vector4<V>, V, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, I1>
        where U : IRankOneTensor<U, Vector4<V>, V, I2>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<V> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] = a[i] * b[j];
            }
        }
        return new(matrix);
    }

    /// <summary>Compute the tensor product of a rank-one tensor and a rank-two tensor.</summary>
    /// <typeparam name="T">A rank-one tensor</typeparam>
    /// <typeparam name="U">A rank-two tensor</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> TensorProduct<T, U, V, I1, I2, I3>(
        IRankOneTensor<T, Vector4<V>, V, I1> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, I2, I3> b)
        where T : IRankOneTensor<T, Vector4<V>, V, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    array[i, j, k] = a[i] * b[j, k];
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of a rank-two tensor and a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-two tensor</typeparam>
    /// <typeparam name="U">A rank-one tensor</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the second tensor</typeparam>
    /// <typeparam name="I3">The index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, I1, I2, I3>(
        IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2> a,
        IRankOneTensor<U, Vector4<V>, V, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, I3>
        where V : IComplex<V>
        where I1 : IIndex
        where I2 : IIndex
        where I3 : IIndex
    {
        Array4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    array[i, j, k] = a[i, j] * b[k];
                }
            }
        }
        return new(array);
    }
}
