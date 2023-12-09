// <copyright file="DifGeo.cs" company="Mathematics.NET">
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
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <param name="a">A tensor with a lower index</param>
    /// <param name="b">A tensor with an upper index</param>
    /// <returns>A scalar</returns>
    public static W Contract<T, U, V, W, X>(IRankOneTensor<T, V, W, Index<Lower, X>> a, IRankOneTensor<U, V, W, Index<Upper, X>> b)
        where T : IRankOneTensor<T, V, W, Index<Lower, X>>
        where U : IRankOneTensor<U, V, W, Index<Upper, X>>
        where V : IVector<V, W>
        where W : IComplex<W>
        where X : ISymbol
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
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <param name="a">A tensor with an upper index</param>
    /// <param name="b">A tensor with a lower index</param>
    /// <returns>A scalar</returns>
    public static W Contract<T, U, V, W, X>(IRankOneTensor<T, V, W, Index<Upper, X>> a, IRankOneTensor<U, V, W, Index<Lower, X>> b)
        where T : IRankOneTensor<T, V, W, Index<Upper, X>>
        where U : IRankOneTensor<U, V, W, Index<Lower, X>>
        where V : IVector<V, W>
        where W : IComplex<W>
        where X : ISymbol
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, W>, X> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, W>, X>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, W>, X> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, W>, X>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, X, Index<Upper, W>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, X, Index<Upper, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, X, Index<Lower, W>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, X, Index<Lower, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, W>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-two tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, X> Contract<T, U, V, W, X>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, W>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
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
    /// <typeparam name="W">The first index of the rank-two tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, W> Contract<T, U, V, W, X>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, X>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
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
    /// <typeparam name="W">The first index of the rank-two tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<V>, V, W> Contract<T, U, V, W, X>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, X>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, W>, X, Y> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, W>, X, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, W>, X, Y> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, W>, X, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, X, Index<Upper, W>, Y> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, X, Index<Upper, W>, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, X, Index<Lower, W>, Y> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, X, Index<Lower, W>, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The second index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, X, Y, Index<Upper, W>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, X, Y, Index<Upper, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The second index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>> a,
        IRankThreeTensor<U, Array4x4x4<V>, V, X, Y, Index<Lower, W>> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Upper, W>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, X, Y, Index<Lower, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, W>, X, Y> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, W>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, W>, X, Y>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, W>, X, Y> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, W>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, W>, X, Y>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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
    /// <typeparam name="W">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, W, Index<Lower, X>, Y> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, X>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, W, Index<Lower, X>, Y>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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
    /// <typeparam name="W">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The third index of the rank-three tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, W, Index<Upper, X>, Y> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, X>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, W, Index<Upper, X>, Y>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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
    /// <typeparam name="W">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The name of the index to contract</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, X> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, W, X, Index<Lower, Y>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Upper, Y>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, W, X, Index<Lower, Y>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, Y>>
        where V : IComplex<V>
        where W : IIndex
        where X : IIndex
        where Y : ISymbol
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
    /// <typeparam name="W">The first index of the rank-three tensor</typeparam>
    /// <typeparam name="X">The second index of the rank-three tensor</typeparam>
    /// <typeparam name="Y">The name of the index to contract</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, X> Contract<T, U, V, W, X, Y>(
        IRankThreeTensor<T, Array4x4x4<V>, V, W, X, Index<Upper, Y>> a,
        IRankOneTensor<U, Vector4<V>, V, Index<Lower, Y>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, W, X, Index<Upper, Y>>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Lower, Y>>
        where V : IComplex<V>
        where W : IIndex
        where X : IIndex
        where Y : ISymbol
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the first tensor</typeparam>
    /// <typeparam name="Y">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, W>, Y> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, W>, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the first tensor</typeparam>
    /// <typeparam name="Y">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, W>, Y> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, W>, Y>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the first tensor</typeparam>
    /// <typeparam name="Y">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Upper, W>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, W>, X>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Upper, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The name of the index to contract</typeparam>
    /// <typeparam name="X">The second index of the first tensor</typeparam>
    /// <typeparam name="Y">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, X, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Lower, W>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Upper, W>, X>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Lower, W>>
        where V : IComplex<V>
        where W : ISymbol
        where X : IIndex
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The first index of the first tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, X>, Y> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, X>, Y>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, first index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The first index of the first tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, X>, Y> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Lower, X>, Y>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The first index of the first tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Upper, X>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Lower, X>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Upper, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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

    /// <summary>Contract two rank-two tensors</summary>
    /// <typeparam name="T">A rank-two tensor with an upper, second index</typeparam>
    /// <typeparam name="U">A rank-two tensor with a lower, second index</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="W">The first index of the first tensor</typeparam>
    /// <typeparam name="X">The name of the index to contract</typeparam>
    /// <typeparam name="Y">The first index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static RankTwoTensor<Matrix4x4<V>, V, W, Y> Contract<T, U, V, W, X, Y>(
        IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>> a,
        IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Lower, X>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, W, Index<Upper, X>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Y, Index<Lower, X>>
        where V : IComplex<V>
        where W : IIndex
        where X : ISymbol
        where Y : IIndex
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

    //
    // Tensor products
    //

    /// <summary>
    /// Compute the tensor product of two rank-one tensors with four elements.
    ///
    /// This operation is written as $$ a\otimes b $$.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index</typeparam>
    /// <typeparam name="I2">The second index</typeparam>
    /// <param name="a">The first tensor</param>
    /// <param name="b">The second tensor</param>
    /// <returns>A rank two tensor</returns>
    public static RankTwoTensor<Matrix4x4<T>, T, I1, I2> TensorProduct<T, I1, I2>(RankOneTensor<Vector4<T>, T, I1> a, RankOneTensor<Vector4<T>, T, I2> b)
        where T : IComplex<T>
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<T> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] = a[i] * b[j];
            }
        }
        return new(matrix);
    }
}
