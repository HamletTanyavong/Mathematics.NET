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

using Mathematics.NET.Core.Attributes.GeneratorAttributes;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>A class containing differential geometry operations</summary>
public static partial class DifGeo
{
    //
    // Tensor contractions
    //

    //
    // Rank-one and Rank-one
    //

    [GenerateTensorContractions]
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

    //
    // Rank-one and Rank-two
    //

    [GenerateTensorContractions]
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

    [GenerateTensorContractions]
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

    //
    // Rank-one and Rank-three
    //

    [GenerateTensorContractions]
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

    [GenerateTensorContractions]
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

    //
    // Rank-two and Rank-two
    //

    [GenerateTensorContractions]
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

    //
    // Rank-two and Rank-three
    //

    [GenerateTensorContractions]
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

    [GenerateTensorContractions]
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
    public static RankThreeTensor<Array4x4x4<V>, V, I1, I2, I3> TensorProduct<T, U, V, I1, I2, I3>(
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
