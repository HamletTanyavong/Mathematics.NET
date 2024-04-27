// <copyright file="LinAlg.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance.Enumerables;
using CommunityToolkit.HighPerformance.Helpers;
using Mathematics.NET.Core.ParallelActions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>A class containing linear algebra operations</summary>
public static class LinAlg
{
    /// <summary>Compute the eigenvalues of a matrix using a QR decomposition method</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">A matrix</param>
    /// <param name="method">A QR decomposition method</param>
    /// <returns>The eigenvalues of the given matrix</returns>
    /// <example>
    /// Suppose we want to find the eigenvalues of a 2x2 matrix. We can write the following to get them:
    /// <code language="cs">
    /// Span2D&lt;Complex&gt; matrix = new Complex[2, 2]
    /// {
    ///     { new(1, 2), new(2, 3) },
    ///     { new(1, -2), new(3, 5) }
    /// }
    /// var eigvals = LinAlg.EigVals(matrix, LinAlg.QRGramSchmidt);
    /// </code>
    /// This method returns a span of the eigenvalues which we can use for other computations. We can also specify the number of
    /// iterations as well if the values do not converge quickly enough. To display the result in the console, write
    /// <code language="cs">
    /// Console.WriteLine(eigvals.ToDisplayString());
    /// </code>
    /// </example>
    public static Span<T> EigVals<T>(Span2D<T> matrix, QRDecompositionMethod<T> method, int iterations = 10)
        where T : IComplex<T>
    {
        // TODO: Implement shifted QR algorithm for faster convergence and dynamically determine how many iterations should be run
        // TODO: Check to make sure new arrays are not being created every iteration
        Span2D<T> rq = matrix;
        for (int i = 0; i < iterations; i++)
        {
            method(rq, out Span2D<T> Q, out Span2D<T> R);
            rq = MatMul(R, Q);
        }

        Span<T> result = new T[matrix.Height];
        for (int i = 0; i < matrix.Height; i++)
        {
            result[i] = rq[i, i];
        }
        return result;
    }

    /// <summary>Multiply two matrices</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="left">The left matrix</param>
    /// <param name="right">The right matrix</param>
    /// <returns>The result of the operation if the dimensions of the matrices are valid; otherwise, throw an exception</returns>
    /// <exception cref="ArgumentException">Cannot multiply matrices of incompatible dimensions</exception>
    public static Span2D<T> MatMul<T>(Span2D<T> left, Span2D<T> right)
        where T : IComplex<T>
    {
        if (left.Width != right.Height)
        {
            throw new ArgumentException("Cannot multiply matrices of incompatible dimensions");
        }

        Span2D<T> result = new T[left.Height, right.Width];
        for (int i = 0; i < left.Height; i++)
        {
            for (int j = 0; j < right.Width; j++)
            {
                for (int k = 0; k < left.Width; k++)
                {
                    result[i, j] += left[i, k] * right[k, j];
                }
            }
        }
        return result;
    }

    /// <summary>Multiply a matrix by a scalar in parallel</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="scalar">A scalar</param>
    /// <param name="matrix">A matrix</param>
    /// <remarks>This process overwrites the original matrix.</remarks>
    public static void MatMulByScalarParallel<T>(T scalar, Memory2D<T> matrix)
        where T : IComplex<T>
        => ParallelHelper.ForEach(matrix, new MultiplyByScalarAction<T>(scalar));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Real Norm<T>(RefEnumerable<T> vector)
        where T : IComplex<T>
    {
        Span<Real> components = new Real[vector.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            components[i] = (vector[i] * T.Conjugate(vector[i])).Re;
            Debug.Assert(components[i] >= Real.Zero, "Components must be greater than zero.");
        }

        var max = components[0];
        for (int i = 1; i < vector.Length; i++)
        {
            if (components[i] > max)
            {
                max = components[i];
            }
        }

        if (max == Real.Zero)
        {
            return Real.Zero;
        }

        var partialSum = Real.Zero;
        var scale = Real.One / (max * max);
        foreach (ref var component in components)
        {
            partialSum += scale * component;
        }

        return max * Real.Sqrt(partialSum);
    }

    /// <summary>Encapsulates a QR decomposition method with one input and two out parameters</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">An input matrix</param>
    /// <param name="Q">The resulting orthogonal matrix</param>
    /// <param name="R">The resulting upper triangular matrix</param>
    public delegate void QRDecompositionMethod<T>(Span2D<T> matrix, out Span2D<T> Q, out Span2D<T> R)
        where T : IComplex<T>;

    /// <summary>Perform QR decomposition on a matrix using the modified Gram-Schmidt process</summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="matrix">An input matrix</param>
    /// <param name="Q">The resulting orthogonal matrix</param>
    /// <param name="R">The resulting upper triangular matrix</param>
    /// <exception cref="DivideByZeroException">One of the columns is not linearly independent from the others</exception>
    public static void QRGramSchmidt<T>(Span2D<T> matrix, out Span2D<T> Q, out Span2D<T> R)
        where T : IComplex<T>
    {
        var height = matrix.Height;
        var width = matrix.Width;

        Q = new T[height, width];
        R = new T[height, width];

        for (int i = 0; i < width; i++)
        {
            var column = Q.GetColumn(i);
            matrix.GetColumn(i).CopyTo(column);
            for (int j = 0; j < i; j++)
            {
                var proj = T.Zero;
                for (int k = 0; k < height; k++)
                {
                    proj += T.Conjugate(Q[k, j]) * Q[k, i];
                }
                R[j, i] = proj;

                for (int k = 0; k < height; k++)
                {
                    Q[k, i] -= proj * Q[k, j];
                }
            }

            var norm = Norm(column);
            if (norm != Real.Zero)
            {
                R[i, i] = norm;
                foreach (ref var element in column)
                {
                    element /= norm;
                }
            }
            else
            {
                // TODO: Consider not throwing an exception
                throw new DivideByZeroException(string.Format("Column {0} is not linearly independent from the others", i));
            }
        }
    }
}
