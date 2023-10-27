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
    /// <summary>Contract two rank-one tensors: $ a_\mu b^\mu $</summary>
    /// <typeparam name="T">The backing type of the tensors</typeparam>
    /// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="V">A symbol</typeparam>
    /// <param name="a">A tensor with a lower index</param>
    /// <param name="b">A tensor with an upper index</param>
    /// <returns>A scalar</returns>
    public static U Contract<T, U, V>(RankOneTensor<T, U, Index<Lower, V>> a, RankOneTensor<T, U, Index<Upper, V>> b)
        where T : IVector<T, U>
        where U : IComplex<U>
        where V : ISymbol
    {
        U result = U.Zero;
        for (int i = 0; i < T.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    /// <summary>Contract two rank-one tensors: $ a^\mu b_\mu $</summary>
    /// <typeparam name="T">The backing type of the tensors</typeparam>
    /// <typeparam name="U">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="V">A symbol</typeparam>
    /// <param name="a">A tensor with an upper index</param>
    /// <param name="b">A tensor with a lower index</param>
    /// <returns>A scalar</returns>
    public static U Contract<T, U, V>(RankOneTensor<T, U, Index<Upper, V>> a, RankOneTensor<T, U, Index<Lower, V>> b)
        where T : IVector<T, U>
        where U : IComplex<U>
        where V : ISymbol
    {
        U result = U.Zero;
        for (int i = 0; i < T.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    /// <summary>Contract a metric tensor and a rank-one tensor: $ g_{\mu\nu}x^\nu $</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index</typeparam>
    /// <typeparam name="I2">The second index</typeparam>
    /// <param name="g">A metric tensor</param>
    /// <param name="x">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<T>, T, Index<Lower, I1>> Contract<T, I1, I2>(
        MetricTensor<Matrix4x4<T>, T, Lower, I1, I2> g,
        RankOneTensor<Vector4<T>, T, Index<Upper, I2>> x)
        where T : IComplex<T>
        where I1 : ISymbol
        where I2 : ISymbol
    {
        Vector4<T> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += g[i, j] * x[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a metric tensor and a rank-one tensor: $ g_{\mu\nu}x^\mu $</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index</typeparam>
    /// <typeparam name="I2">The second index</typeparam>
    /// <param name="g">A metric tensor</param>
    /// <param name="x">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<T>, T, Index<Lower, I2>> Contract<T, I1, I2>(
        MetricTensor<Matrix4x4<T>, T, Lower, I1, I2> g,
        RankOneTensor<Vector4<T>, T, Index<Upper, I1>> x)
        where T : IComplex<T>
        where I1 : ISymbol
        where I2 : ISymbol
    {
        Vector4<T> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += g[j, i] * x[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a metric tensor and a rank-one tensor: $ g^{\mu\nu}x_\nu $</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index</typeparam>
    /// <typeparam name="I2">The second index</typeparam>
    /// <param name="g">A metric tensor</param>
    /// <param name="x">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<T>, T, Index<Upper, I1>> Contract<T, I1, I2>(
        MetricTensor<Matrix4x4<T>, T, Upper, I1, I2> g,
        RankOneTensor<Vector4<T>, T, Index<Lower, I2>> x)
        where T : IComplex<T>
        where I1 : ISymbol
        where I2 : ISymbol
    {
        Vector4<T> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += g[i, j] * x[j];
            }
        }
        return new(vector);
    }

    /// <summary>Contract a metric tensor and a rank-one tensor: $ g^{\mu\nu}x_\mu $</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index</typeparam>
    /// <typeparam name="I2">The second index</typeparam>
    /// <param name="g">A metric tensor</param>
    /// <param name="x">A rank-one tensor</param>
    /// <returns>A rank-one tensor</returns>
    public static RankOneTensor<Vector4<T>, T, Index<Upper, I2>> Contract<T, I1, I2>(
        MetricTensor<Matrix4x4<T>, T, Upper, I1, I2> g,
        RankOneTensor<Vector4<T>, T, Index<Lower, I1>> x)
        where T : IComplex<T>
        where I1 : ISymbol
        where I2 : ISymbol
    {
        Vector4<T> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += g[j, i] * x[j];
            }
        }
        return new(vector);
    }

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

        // TODO: Parallelize
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
