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

using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
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
    // General calculus
    //

    // TODO: Optimize the following methods

    /// <summary>Compute the derivative of the inverse of a metric tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A rank-two tensor</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="derivative">A rank-three tensor</param>
    public static void Derivative<TTape, TN, TPIN, TI1N, TI2N, TI3N>(
        TTape tape,
        MetricTensorField<TTape, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> derivative)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        var inverseMetricLeft = metric
            .Compute<TI2N, InternalIndex1>(tape, point)
            .Inverse();
        var inverseMetricRight = inverseMetricLeft
            .WithIndexOne<InternalIndex2>()
            .WithIndexTwo<TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, InternalIndex1>, Index<Lower, InternalIndex2>> derivativeOfMetric);

        derivative = -Contract(Contract(derivativeOfMetric, inverseMetricLeft), inverseMetricRight);
    }

    /// <inheritdoc cref="Derivative{TTape, TN, TPIN, TI1N, TI2N, TI3N}(TTape, MetricTensorField{TTape, Matrix4x4{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TTape, TN, TPIN, TI1N, TI2N, TI3N>(
        TTape tape,
        MetricTensorField<TTape, Matrix4x4<TN>, TN, Index<Lower, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> derivative)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        var inverseMetricLeft = metric
            .Compute<TI2N, InternalIndex1>(tape, point)
            .Inverse();
        var inverseMetricRight = inverseMetricLeft
            .WithIndexOne<InternalIndex2>()
            .WithIndexTwo<TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, InternalIndex1>, Index<Lower, InternalIndex2>> derivativeOfMetric);

        derivative = -Contract(Contract(derivativeOfMetric, inverseMetricLeft), inverseMetricRight);
    }

    /// <summary>Compute the derivative of rank-two tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor</typeparam>
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="tensor">A rank-two tensor</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="derivative">A rank-three tensor</param>
    public static void Derivative<TTape, TN, TI2P, TI3P, TPIN, TI1N, TI2N, TI3N>(
        TTape tape,
        TensorField4x4<TTape, TN, TI2P, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> derivative)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        derivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TTape, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    derivative[0, i, j] = gradient[0];
                    derivative[1, i, j] = gradient[1];
                    derivative[2, i, j] = gradient[2];
                    derivative[3, i, j] = gradient[3];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TTape, TN, TI2P, TI3P, TPIN, TI1N, TI2N, TI3N}(TTape, TensorField4x4{TTape, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TTape, TN, TI2P, TI3P, TPIN, TI1N, TI2N, TI3N>(
        TTape tape,
        TensorField4x4<TTape, TN, TI2P, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> derivative)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        derivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TTape, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    derivative[0, i, j] = gradient[0];
                    derivative[1, i, j] = gradient[1];
                    derivative[2, i, j] = gradient[2];
                    derivative[3, i, j] = gradient[3];
                }
            }
        }
    }

    /// <summary>Compute the second derivative or a rank-two tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor</typeparam>
    /// <typeparam name="TIndex4Position">The index position of the fourth idex of the tensor</typeparam>
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <typeparam name="TIndex4Name">The name of the fourth index of the tensor</typeparam>
    /// <param name="tape">A Hessian tape</param>
    /// <param name="tensor">A rank-two tensor</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="secondDerivative">A rank-four tensor</param>
    public static void SecondDerivative<TN, TI3P, TIndex4Position, TPIN, TI1N, TI2N, TI3N, TIndex4Name>(
        HessianTape<TN> tape,
        TensorField4x4<HessianTape<TN>, TN, TI3P, TIndex4Position, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<TI3P, TI3N>, Index<TIndex4Position, TIndex4Name>> secondDerivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI3P : IIndexPosition
        where TIndex4Position : IIndexPosition
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TIndex4Name : ISymbol
    {
        secondDerivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    secondDerivative[0, 0, i, j] = hessian[0, 0];
                    secondDerivative[0, 1, i, j] = hessian[0, 1];
                    secondDerivative[0, 2, i, j] = hessian[0, 2];
                    secondDerivative[0, 3, i, j] = hessian[0, 3];

                    secondDerivative[1, 0, i, j] = hessian[1, 0];
                    secondDerivative[1, 1, i, j] = hessian[1, 1];
                    secondDerivative[1, 2, i, j] = hessian[1, 2];
                    secondDerivative[1, 3, i, j] = hessian[1, 3];

                    secondDerivative[2, 0, i, j] = hessian[2, 0];
                    secondDerivative[2, 1, i, j] = hessian[2, 1];
                    secondDerivative[2, 2, i, j] = hessian[2, 2];
                    secondDerivative[2, 3, i, j] = hessian[2, 3];

                    secondDerivative[3, 0, i, j] = hessian[3, 0];
                    secondDerivative[3, 1, i, j] = hessian[3, 1];
                    secondDerivative[3, 2, i, j] = hessian[3, 2];
                    secondDerivative[3, 3, i, j] = hessian[3, 3];
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TI3P, TIndex4Position, TPIN, TI1N, TI2N, TI3N, TIndex4Name}(HessianTape{TN}, TensorField4x4{HessianTape{TN}, TN, TI3P, TIndex4Position, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4x4{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TIndex4Position, TIndex4Name}})"/>
    public static void SecondDerivative<TN, TI3P, TIndex4Position, TPIN, TI1N, TI2N, TI3N, TIndex4Name>(
        HessianTape<TN> tape,
        TensorField4x4<HessianTape<TN>, TN, TI3P, TIndex4Position, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<TI3P, TI3N>, Index<TIndex4Position, TIndex4Name>> secondDerivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI3P : IIndexPosition
        where TIndex4Position : IIndexPosition
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TIndex4Name : ISymbol
    {
        secondDerivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    secondDerivative[0, 0, i, j] = hessian[0, 0];
                    secondDerivative[0, 1, i, j] = hessian[0, 1];
                    secondDerivative[0, 2, i, j] = hessian[0, 2];
                    secondDerivative[0, 3, i, j] = hessian[0, 3];

                    secondDerivative[1, 0, i, j] = hessian[1, 0];
                    secondDerivative[1, 1, i, j] = hessian[1, 1];
                    secondDerivative[1, 2, i, j] = hessian[1, 2];
                    secondDerivative[1, 3, i, j] = hessian[1, 3];

                    secondDerivative[2, 0, i, j] = hessian[2, 0];
                    secondDerivative[2, 1, i, j] = hessian[2, 1];
                    secondDerivative[2, 2, i, j] = hessian[2, 2];
                    secondDerivative[2, 3, i, j] = hessian[2, 3];

                    secondDerivative[3, 0, i, j] = hessian[3, 0];
                    secondDerivative[3, 1, i, j] = hessian[3, 1];
                    secondDerivative[3, 2, i, j] = hessian[3, 2];
                    secondDerivative[3, 3, i, j] = hessian[3, 3];
                }
            }
        }
    }

    //
    // Christoffel symbols
    //

    /// <summary>Compute the Christoffel symbol of the first kind given a metric tensor.</summary>
    /// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol</typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A metric tensor</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="christoffel">The result</param>
    public static void Christoffel<TTape, TN, TI1N, TI2N, TI3N, TPIN>(
        TTape tape,
        MetricTensorField<TTape, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Lower, TI1N>, TI2N, TI3N> christoffel)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TPIN : ISymbol
    {
        christoffel = new();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Lower, TPIN>, Index<Lower, TI1N>, Index<Lower, TI2N>> derivative);

        for (int k = 0; k < 4; k++)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    christoffel[k, i, j] = 0.5 * (derivative[j, k, i] + derivative[i, k, j] - derivative[k, i, j]);
                }
            }
        }
    }

    /// <summary>Compute the Christoffel symbol of the second kind given a metric tensor.</summary>
    /// <typeparam name="TTape">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol</typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A metric tensor</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="christoffel">The result</param>
    public static void Christoffel<TTape, TN, TI1N, TI2N, TI3N, TPIN>(
        TTape tape,
        MetricTensorField<TTape, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N> christoffel)
        where TTape : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TPIN : ISymbol
    {
        var inverseMetric = metric
            .Compute<TI1N, InternalIndex1>(tape, point)
            .Inverse();
        Christoffel(tape, metric, point, out Christoffel<Array4x4x4<TN>, TN, Index<Lower, InternalIndex1>, TI2N, TI3N> christoffelFirstKind);

        var result = Contract(inverseMetric, christoffelFirstKind);

        christoffel = Unsafe.As<
            Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>>,
            Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N>
            >(ref result);
    }

    //
    // Tensor contractions
    //

    //
    // Rank-one and Rank-one
    //

    [GenerateTensorContractions]
    public static W Contract<T, U, V, W, IC>(in IRankOneTensor<T, V, W, Index<Lower, IC>> a, in IRankOneTensor<U, V, W, Index<Upper, IC>> b)
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

    //
    // Rank-one and Rank-two
    //

    [GenerateTensorContractions]
    public static Tensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        in IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        in IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Vector4<V>, V, I> Contract<T, U, V, IC, I>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I> a,
        in IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        in IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        in IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    // Rank-one and Rank-four
    //

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        in IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>> a,
        in IRankFourTensor<U, Array4x4x4x4<V>, V, Index<Upper, IC>, I1, I2, I3> b)
        where T : IRankOneTensor<T, Vector4<V>, V, Index<Lower, IC>>
        where U : IRankFourTensor<U, Array4x4x4x4<V>, V, Index<Upper, IC>, I1, I2, I3>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
                        array[i, j, k] += a[l] * b[l, i, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        in IRankFourTensor<T, Array4x4x4x4<V>, V, Index<Lower, IC>, I1, I2, I3> a,
        in IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>> b)
        where T : IRankFourTensor<T, Array4x4x4x4<V>, V, Index<Lower, IC>, I1, I2, I3>
        where U : IRankOneTensor<U, Vector4<V>, V, Index<Upper, IC>>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
                        array[i, j, k] += a[l, i, j, k] * b[l];
                    }
                }
            }
        }
        return new(array);
    }

    //
    // Rank-two and Rank-two
    //

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<V>, V, I1, I2> Contract<T, U, V, IC, I1, I2>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        in IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I2>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I2, I3>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> Contract<T, U, V, IC, I1, I2, I3>(
        in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        in IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, Index<Upper, IC>, I3>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    // Rank-two and Rank-four
    //

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1> a,
        in IRankFourTensor<U, Array4x4x4x4<V>, V, Index<Upper, IC>, I2, I3, I4> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, Index<Lower, IC>, I1>
        where U : IRankFourTensor<U, Array4x4x4x4<V>, V, Index<Upper, IC>, I2, I3, I4>
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
                            array[i, j, k, l] += a[m, i] * b[m, j, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(
        in IRankFourTensor<T, Array4x4x4x4<V>, V, Index<Lower, IC>, I1, I2, I3> a,
        in IRankTwoTensor<U, Matrix2x2<V>, V, Index<Upper, IC>, I4> b)
        where T : IRankFourTensor<T, Array4x4x4x4<V>, V, Index<Lower, IC>, I1, I2, I3>
        where U : IRankTwoTensor<U, Matrix2x2<V>, V, Index<Upper, IC>, I4>
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
                            array[i, j, k, l] += a[m, i, j, k] * b[m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    //
    // Rank-three and Rank-three
    //

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(
        in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a,
        in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4>
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
                            array[i, j, k, l] += a[m, i, j] * b[m, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    //
    // Tensor self-contractions
    //

    [GenerateTensorSelfContractions]
    public static V Contract<T, U, V, IC>(in IRankTwoTensor<T, U, V, Index<Lower, IC>, Index<Upper, IC>> a)
        where T : IRankTwoTensor<T, U, V, Index<Lower, IC>, Index<Upper, IC>>
        where U : ISquareMatrix<U, V>
        where V : IComplex<V>, IDifferentiableFunctions<V>
        where IC : ISymbol
    {
        var result = V.Zero;
        for (int i = 0; i < U.E1Components; i++)
        {
            result += a[i, i];
        }
        return result;
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Vector4<U>, U, I> Contract<T, U, IC, I>(in IRankThreeTensor<T, Array4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I> a)
        where T : IRankThreeTensor<T, Array4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I>
        where U : IComplex<U>, IDifferentiableFunctions<U>
        where IC : ISymbol
        where I : IIndex
    {
        Vector4<U> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j, j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I1, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I1, I2>
        where U : IComplex<U>, IDifferentiableFunctions<U>
        where IC : ISymbol
        where I1 : IIndex
        where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, k, i, j];
                }
            }
        }
        return new(matrix);
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
    public static Tensor<Matrix4x4<V>, V, I1, I2> TensorProduct<T, U, V, I1, I2>(in IRankOneTensor<T, Vector4<V>, V, I1> a, in IRankOneTensor<U, Vector4<V>, V, I2> b)
        where T : IRankOneTensor<T, Vector4<V>, V, I1>
        where U : IRankOneTensor<U, Vector4<V>, V, I2>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> TensorProduct<T, U, V, I1, I2, I3>(
        in IRankOneTensor<T, Vector4<V>, V, I1> a,
        in IRankTwoTensor<U, Matrix4x4<V>, V, I2, I3> b)
        where T : IRankOneTensor<T, Vector4<V>, V, I1>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I2, I3>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
    public static Tensor<Array4x4x4<V>, V, I1, I2, I3> TensorProduct<T, U, V, I1, I2, I3>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2> a,
        in IRankOneTensor<U, Vector4<V>, V, I3> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2>
        where U : IRankOneTensor<U, Vector4<V>, V, I3>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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

    /// <summary>Compute the tensor product of a rank-one tensor and a rank-three tensor.</summary>
    /// <typeparam name="T">A rank-one tensor</typeparam>
    /// <typeparam name="U">A rank-three tensor</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The index of the first tensor</typeparam>
    /// <typeparam name="I2">The first index of the second tensor</typeparam>
    /// <typeparam name="I3">The second index of the second tensor</typeparam>
    /// <typeparam name="I4">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> TensorProduct<T, U, V, I1, I2, I3, I4>(
        in IRankOneTensor<T, Vector4<V>, V, I1> a,
        in IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, I4> b)
        where T : IRankOneTensor<T, Vector4<V>, V, I1>
        where U : IRankThreeTensor<U, Array4x4x4<V>, V, I2, I3, I4>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
                        array[i, j, k, l] = a[i] * b[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of a rank-three tensor and a rank-one tensor.</summary>
    /// <typeparam name="T">A rank-three tensor</typeparam>
    /// <typeparam name="U">A rank-one tensor</typeparam>
    /// <typeparam name="V">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="I3">The third index of the first tensor</typeparam>
    /// <typeparam name="I4">The index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> TensorProduct<T, U, V, I1, I2, I3, I4>(
        in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, I3> a,
        in IRankOneTensor<U, Vector4<V>, V, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, I3>
        where U : IRankOneTensor<U, Vector4<V>, V, I4>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
                        array[i, j, k, l] = a[i, j, k] * b[l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of two rank-two tensors.</summary>
    /// <typeparam name="T">A rank-two tensors</typeparam>
    /// <typeparam name="U">A rank-two tensors</typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="I1">The first index of the first tensor</typeparam>
    /// <typeparam name="I2">The second index of the first tensor</typeparam>
    /// <typeparam name="I3">The first index of the second tensor</typeparam>
    /// <typeparam name="I4">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> TensorProduct<T, U, V, I1, I2, I3, I4>(
        in IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2> a,
        in IRankTwoTensor<U, Matrix4x4<V>, V, I1, I2> b)
        where T : IRankTwoTensor<T, Matrix4x4<V>, V, I1, I2>
        where U : IRankTwoTensor<U, Matrix4x4<V>, V, I1, I2>
        where V : IComplex<V>, IDifferentiableFunctions<V>
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
                        array[i, j, k, l] = a[i, j] * b[k, l];
                    }
                }
            }
        }
        return new(array);
    }
}
