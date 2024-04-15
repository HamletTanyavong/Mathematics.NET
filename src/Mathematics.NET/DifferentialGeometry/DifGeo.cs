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
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A metric tensor field</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="derivative">A rank-three tensor</param>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField<TT, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> derivative)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        var metricValueLeft = metric.Compute<TI2N, InternalIndex1>(tape, point);
        ref var metricValueLeftRef = ref metricValueLeft;
        var inverseMetricLeft = metricValueLeftRef.Inverse();
        ref var inverseMetricRight = ref inverseMetricLeft
            .WithIndex1Name<InternalIndex2>()
            .WithIndex2Name<TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, InternalIndex1>, Index<Lower, InternalIndex2>> derivativeOfMetric);

        derivative = -Contract(Contract(derivativeOfMetric, inverseMetricLeft), inverseMetricRight);
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField{TT, Matrix4x4{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField<TT, Matrix4x4<TN>, TN, Index<Lower, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> derivative)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        var metricValueLeft = metric.Compute<TI2N, InternalIndex1>(tape, point);
        ref var metricValueLeftRef = ref metricValueLeft;
        var inverseMetricLeft = metricValueLeftRef.Inverse();
        ref var inverseMetricRight = ref inverseMetricLeft
            .WithIndex1Name<InternalIndex2>()
            .WithIndex2Name<TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, InternalIndex1>, Index<Lower, InternalIndex2>> derivativeOfMetric);

        derivative = -Contract(Contract(derivativeOfMetric, inverseMetricLeft), inverseMetricRight);
    }

    /// <summary>Compute the derivative of rank-two tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="tensor">A rank-two tensor field</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="derivative">A rank-three tensor</param>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        TensorField4x4<TT, TN, TI2P, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> derivative)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        derivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
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

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, TensorField4x4{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        TensorField4x4<TT, TN, TI2P, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> derivative)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        derivative = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
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
    /// <typeparam name="TPIN">The name of the point index</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor</typeparam>
    /// <typeparam name="TI4P">The index position of the fourth idex of the tensor</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor</typeparam>
    /// <typeparam name="TI4N">The name of the fourth index of the tensor</typeparam>
    /// <param name="tape">A Hessian tape</param>
    /// <param name="tensor">A rank-two tensor field</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="secondDerivative">A rank-four tensor</param>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        TensorField4x4<HessianTape<TN>, TN, TI3P, TI4P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> secondDerivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TI4N : ISymbol
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

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, TensorField4x4{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor4{TN, Index{Upper, TPIN}}, out Tensor{Array4x4x4x4{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        TensorField4x4<HessianTape<TN>, TN, TI3P, TI4P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> secondDerivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
        where TI4N : ISymbol
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
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol</typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A metric tensor field</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="christoffel">The result</param>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField<TT, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Lower, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
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
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/></typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol</typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol</typeparam>
    /// <param name="tape">A gradient or Hessian tape</param>
    /// <param name="metric">A metric tensor field</param>
    /// <param name="point">A point on the manifold</param>
    /// <param name="christoffel">The result</param>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField<TT, Matrix4x4<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : ISymbol
        where TI1N : ISymbol
        where TI2N : ISymbol
        where TI3N : ISymbol
    {
        var metricValue = metric.Compute<TI1N, InternalIndex1>(tape, point);
        ref var metricValueRef = ref metricValue;
        var inverseMetric = metricValueRef.Inverse();
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

    // Rank-one and Rank-one

    [GenerateTensorContractions]
    public static TN Contract<TLR1T, TRR1T, TV, TN, TCI>(in IRankOneTensor<TLR1T, TV, TN, Index<Lower, TCI>> a, in IRankOneTensor<TRR1T, TV, TN, Index<Upper, TCI>> b)
        where TLR1T : IRankOneTensor<TLR1T, TV, TN, Index<Lower, TCI>>
        where TRR1T : IRankOneTensor<TRR1T, TV, TN, Index<Upper, TCI>>
        where TV : IVector<TV, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
    {
        var result = TN.Zero;
        for (int i = 0; i < TV.E1Components; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }

    // Rank-one and Rank-two

    [GenerateTensorContractions]
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR1T, TR2T, TN, TCI, TI>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> a,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI> b)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI : IIndex
    {
        Vector4<TN> vector = new();
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
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR2T, TR1T, TN, TCI, TI>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI> a,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> b)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI : IIndex
    {
        Vector4<TN> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += a[j, i] * b[j];
            }
        }
        return new(vector);
    }

    // Rank-one and Rank-three

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR1T, TR3T, TN, TCI, TI1, TI2>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> a,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> b)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix4x4<TN> matrix = new();
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
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR3T, TR1T, TN, TCI, TI1, TI2>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> b)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix4x4<TN> matrix = new();
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

    // Rank-one and Rank-four

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR1T, TR4T, TN, TCI, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> a,
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3> b)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR4T, TR1T, TN, TCI, TI1, TI2, TI3>(
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> a,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> b)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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

    // Rank-two and Rank-two

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TLR2T, TRR3T, TN, TCI, TI1, TI2>(
        in IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> a,
        in IRankTwoTensor<TRR3T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI2> b)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TRR3T : IRankTwoTensor<TRR3T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix4x4<TN> matrix = new();
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

    // Rank-two and Rank-three

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR2T, TR3T, TN, TCI, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> a,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3> b)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR3T, TR2T, TN, TCI, TI1, TI2, TI3>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI3> b)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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

    // Rank-two and Rank-four

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR2T, TR4T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> a,
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4> b)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR4T, TR2T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> a,
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI4> b)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    // Rank-three and Rank-three

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a,
        in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    // Tensor self-contractions

    [GenerateTensorSelfContractions]
    public static TN Contract<TR2T, TSM, TN, TCI>(in IRankTwoTensor<TR2T, TSM, TN, Index<Lower, TCI>, Index<Upper, TCI>> a)
        where TR2T : IRankTwoTensor<TR2T, TSM, TN, Index<Lower, TCI>, Index<Upper, TCI>>
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
    {
        var result = TN.Zero;
        for (int i = 0; i < TSM.E1Components; i++)
        {
            result += a[i, i];
        }
        return result;
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR3T, TN, TCI, TI>(in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI> a)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI : IIndex
    {
        Vector4<TN> vector = new();
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
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR4T, TN, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2> a)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : ISymbol
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix4x4<TN> matrix = new();
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
    /// <typeparam name="TLR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TRR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="TI1">The index of the first tensor</typeparam>
    /// <typeparam name="TI2">The index of the second tensor</typeparam>
    /// <param name="a">The first tensor</param>
    /// <param name="b">The second tensor</param>
    /// <returns>A rank-two tensor</returns>
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> TensorProduct<TLR1T, TRR1T, TN, TI1, TI2>(in IRankOneTensor<TLR1T, Vector4<TN>, TN, TI1> a, in IRankOneTensor<TRR1T, Vector4<TN>, TN, TI2> b)
        where TLR1T : IRankOneTensor<TLR1T, Vector4<TN>, TN, TI1>
        where TRR1T : IRankOneTensor<TRR1T, Vector4<TN>, TN, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix4x4<TN> matrix = new();
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
    /// <typeparam name="TR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TR2T">A rank-two tensor</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="TI1">The index of the first tensor</typeparam>
    /// <typeparam name="TI2">The first index of the second tensor</typeparam>
    /// <typeparam name="TI3">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> TensorProduct<TR1T, TR2T, TN, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI1> a,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI2, TI3> b)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, TI1>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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
    /// <typeparam name="TR2T">A rank-two tensor</typeparam>
    /// <typeparam name="TR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="TI1">The first index of the first tensor</typeparam>
    /// <typeparam name="TI2">The second index of the second tensor</typeparam>
    /// <typeparam name="TI3">The index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-three tensor</returns>
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> TensorProduct<TR2T, TR1T, TN, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI1, TI2> a,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI3> b)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array4x4x4<TN> array = new();
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
    /// <typeparam name="TR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TR3T">A rank-three tensor</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="TI1">The index of the first tensor</typeparam>
    /// <typeparam name="TI2">The first index of the second tensor</typeparam>
    /// <typeparam name="TI3">The second index of the second tensor</typeparam>
    /// <typeparam name="TI4">The third index of the second tensor</typeparam>
    /// <param name="a">A rank-one tensor</param>
    /// <param name="b">A rank-three tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR1T, TR3T, TN, TI1, TI2, TI3, TI4>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI1> a,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI2, TI3, TI4> b)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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
    /// <typeparam name="TR3T">A rank-three tensor</typeparam>
    /// <typeparam name="TR1T">A rank-one tensor</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <typeparam name="TI1">The first index of the first tensor</typeparam>
    /// <typeparam name="TI2">The second index of the first tensor</typeparam>
    /// <typeparam name="TI3">The third index of the first tensor</typeparam>
    /// <typeparam name="TI4">The index of the second tensor</typeparam>
    /// <param name="a">A rank-three tensor</param>
    /// <param name="b">A rank-one tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR3T, TR1T, TN, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI1, TI2, TI3> a,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI4> b)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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
    /// <typeparam name="TLR2T">A rank-two tensors</typeparam>
    /// <typeparam name="TRR2T">A rank-two tensors</typeparam>
    /// <typeparam name="TN"></typeparam>
    /// <typeparam name="TI1">The first index of the first tensor</typeparam>
    /// <typeparam name="TI2">The second index of the first tensor</typeparam>
    /// <typeparam name="TI3">The first index of the second tensor</typeparam>
    /// <typeparam name="TI4">The second index of the second tensor</typeparam>
    /// <param name="a">A rank-two tensor</param>
    /// <param name="b">A rank-two tensor</param>
    /// <returns>A rank-four tensor</returns>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, TI1, TI2> a,
        in IRankTwoTensor<TRR2T, Matrix4x4<TN>, TN, TI1, TI2> b)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, TI1, TI2>
        where TRR2T : IRankTwoTensor<TRR2T, Matrix4x4<TN>, TN, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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
