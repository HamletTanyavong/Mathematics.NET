// <copyright file="DifGeo.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core.Attributes;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.DifferentialGeometry.IndexNames;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>A class containing differential geometry operations.</summary>
public static partial class DifGeo
{
    //
    // General Calculus
    //

    // TODO: Optimize the following methods.

    // First derivatives.

    /// <summary>Compute the derivative of rank-one tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <param name="tensor">A rank-one tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="dTensor">A rank-two tensor.</param>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField2<TDN, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor2<TDN, TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix2x2<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 2; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor2<TDN, TN, Index<Upper, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TDN, TN, TPIN, TI2P, TI1N, TI2N}(FMTensorField2{TDN, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TDN, TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField3<TDN, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor3<TDN, TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix3x3<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 3; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor3<TDN, TN, Index<Upper, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TDN, TN, TPIN, TI2P, TI1N, TI2N}(FMTensorField2{TDN, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TDN, TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField4<TDN, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TDN, TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 4; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor4<TDN, TN, Index<Upper, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TDN, TN, TPIN, TI2P, TI1N, TI2N}(FMTensorField2{TDN, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TDN, TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField2<TDN, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor2<TDN, TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix2x2<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 2; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor2<TDN, TN, Index<Lower, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TDN, TN, TPIN, TI2P, TI1N, TI2N}(FMTensorField2{TDN, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TDN, TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField3<TDN, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor3<TDN, TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix3x3<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 3; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor3<TDN, TN, Index<Lower, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TDN, TN, TPIN, TI2P, TI1N, TI2N}(FMTensorField2{TDN, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TDN, TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TDN, TN, TPIN, TI2P, TI1N, TI2N>(
        FMTensorField4<TDN, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TDN, TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TDN : IDual<TDN, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            var seed = point;
            seed[i] = TDN.CreateVariable(seed[i].D0, 1);
            for (int k = 0; k < 4; k++)
            {
                if (tensor[k] is Func<AutoDiffTensor4<TDN, TN, Index<Lower, TPIN>>, TDN> function)
                {
                    dTensor[i, k] = function(seed).D1;
                }
            }
        }
    }

    /// <summary>Compute the derivative of rank-one tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="tensor">A rank-one tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="dTensor">A rank-two tensor.</param>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField2<TT, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix2x2<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor2<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 2; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI1N, TI2N}(TT, RMTensorField2{TT, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField3<TT, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix3x3<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor3<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 3; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI1N, TI2N}(TT, RMTensorField2{TT, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField4<TT, TN, TI2P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Matrix4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 4; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI1N, TI2N}(TT, RMTensorField2{TT, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField2<TT, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix2x2<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor2<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 2; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI1N, TI2N}(TT, RMTensorField2{TT, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField3<TT, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix3x3<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor3<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 3; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI1N, TI2N}(TT, RMTensorField2{TT, TN, TI2P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Matrix2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI1N, TI2N>(
        TT tape,
        RMTensorField4<TT, TN, TI2P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Matrix4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            if (tensor[i] is Func<TT, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out var gradient);

                for (int k = 0; k < 4; k++)
                {
                    dTensor[k, i] = gradient[k];
                }
            }
        }
    }

    /// <summary>Compute the derivative of the inverse of a metric tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="dMetric">A rank-three tensor.</param>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField2x2<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField3x3<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField4x4<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <summary>Compute the derivative of a metric tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="dMetric">A rank-three tensor.</param>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField2x2<TT, TN, Index<Lower, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Lower, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Lower, TPIN}}, AutoDiffTensor2{TN, Index{Lower, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Upper, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField3x3<TT, TN, Index<Lower, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Lower, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Lower, TPIN}}, AutoDiffTensor2{TN, Index{Lower, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Upper, TI1N}, Index{Upper, TI2N}, Index{Upper, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField4x4<TT, TN, Index<Lower, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<Upper, TI3N>> dMetric)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI2N, Index1>(tape, point);
        var invMetricL = value.Inverse();
        var invMetricR = invMetricL.WithIndices<Index2, TI3N>();
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, Index1>, Index<Lower, Index2>> dTensor);

        dMetric = -Contract(Contract(dTensor, invMetricL), invMetricR);
    }

    /// <summary>Compute the derivative of rank-two tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="tensor">A rank-two tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="dTensor">A rank-three tensor.</param>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField2x2<TT, TN, TI2P, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor2<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 2; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, RMTensorField2x2{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField3x3<TT, TN, TI2P, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor3<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 3; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, RMTensorField2x2{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField4x4<TT, TN, TI2P, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 4; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, RMTensorField2x2{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField2x2<TT, TN, TI2P, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Lower, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor2<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 2; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, RMTensorField2x2{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField3x3<TT, TN, TI2P, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Lower, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor3<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 3; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Derivative{TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(TT, RMTensorField2x2{TT, TN, TI2P, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void Derivative<TT, TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        TT tape,
        RMTensorField4x4<TT, TN, TI2P, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> dTensor)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        dTensor = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<TT, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out var gradient);

                    for (int k = 0; k < 4; k++)
                    {
                        dTensor[k, i, j] = gradient[k];
                    }
                }
            }
        }
    }

    // Second derivatives.

    /// <summary>Compute the second derivative of rank-one tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI2P">The index position of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor.</typeparam>
    /// <param name="tape">A Hessian tape.</param>
    /// <param name="tensor">A rank-two tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="d2Tensor">A rank-two tensor.</param>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField2<HessianTape<TN>, TN, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 2; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor2<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }


    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(HessianTape{TN}, RMTensorField2{HessianTape{TN}, TN, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField3<HessianTape<TN>, TN, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 3; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor3<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(HessianTape{TN}, RMTensorField2{HessianTape{TN}, TN, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField4<HessianTape<TN>, TN, TI3P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 4; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(HessianTape{TN}, RMTensorField2{HessianTape{TN}, TN, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField2<HessianTape<TN>, TN, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Lower, TPIN>> point,
        out Tensor<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 2; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor2<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(HessianTape{TN}, RMTensorField2{HessianTape{TN}, TN, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField3<HessianTape<TN>, TN, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Lower, TPIN>> point,
        out Tensor<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 3; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor3<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N}(HessianTape{TN}, RMTensorField2{HessianTape{TN}, TN, TI3P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2{TN}, TN, Index{Lower, TI1N}, Index{TI2P, TI2N}, Index{TI3P, TI3N}})"/>
    public static void SecondDerivative<TN, TPIN, TI2P, TI3P, TI1N, TI2N, TI3N>(
        HessianTape<TN> tape,
        RMTensorField4<HessianTape<TN>, TN, TI3P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<TI2P, TI2N>, Index<TI3P, TI3N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI2P : IIndexPosition
        where TI3P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 4; i++)
        {
            if (tensor[i] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
            {
                _ = function(tape, point);
                tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        d2Tensor[k, l, i] = hessian[k, l];
                    }
                }
            }
        }
    }

    /// <summary>Compute the second derivative or a rank-two tensor.</summary>
    /// <remarks>Though the result of this operation returns a tensor object, it may not be a tensor in the mathematical sense.</remarks>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the point index.</typeparam>
    /// <typeparam name="TI3P">The index position of the third index of the tensor.</typeparam>
    /// <typeparam name="TI4P">The index position of the fourth idex of the tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index of the tensor.</typeparam>
    /// <typeparam name="TI2N">The name of the second index of the tensor.</typeparam>
    /// <typeparam name="TI3N">The name of the third index of the tensor.</typeparam>
    /// <typeparam name="TI4N">The name of the fourth index of the tensor.</typeparam>
    /// <param name="tape">A Hessian tape.</param>
    /// <param name="tensor">A rank-two tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="d2Tensor">A rank-four tensor.</param>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField2x2<HessianTape<TN>, TN, TI3P, TI4P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor2<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, RMTensorField2x2{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField3x3<HessianTape<TN>, TN, TI3P, TI4P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor3<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, RMTensorField2x2{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField4x4<HessianTape<TN>, TN, TI3P, TI4P, Index<Upper, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Upper, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, RMTensorField2x2{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField2x2<HessianTape<TN>, TN, TI3P, TI4P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor2<TN, Index<Lower, TPIN>> point,
        out Tensor<Array2x2x2x2<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor2<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, RMTensorField2x2{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField3x3<HessianTape<TN>, TN, TI3P, TI4P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor3<TN, Index<Lower, TPIN>> point,
        out Tensor<Array3x3x3x3<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor3<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="SecondDerivative{TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, RMTensorField2x2{HessianTape{TN}, TN, TI3P, TI4P, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{TI3P, TI3N}, Index{TI4P, TI4N}})"/>
    public static void SecondDerivative<TN, TPIN, TI3P, TI4P, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        RMTensorField4x4<HessianTape<TN>, TN, TI3P, TI4P, Index<Lower, TPIN>> tensor,
        AutoDiffTensor4<TN, Index<Lower, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Upper, TI2N>, Index<TI3P, TI3N>, Index<TI4P, TI4N>> d2Tensor)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI3P : IIndexPosition
        where TI4P : IIndexPosition
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        d2Tensor = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tensor[i, j] is Func<HessianTape<TN>, AutoDiffTensor4<TN, Index<Lower, TPIN>>, Variable<TN>> function)
                {
                    _ = function(tape, point);
                    tape.ReverseAccumulate(out ReadOnlySpan2D<TN> hessian);

                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            d2Tensor[k, l, i, j] = hessian[k, l];
                        }
                    }
                }
            }
        }
    }

    //
    // Christoffel symbols.
    //

    /// <summary>Compute a Christoffel symbol of the first kind given a metric tensor.</summary>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol.</typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol.</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol.</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="christoffel">The result.</param>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField2x2<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array2x2x2<TN>, TN, Index<Lower, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        christoffel = new();
        Derivative(tape, metric, point, out Tensor<Array2x2x2<TN>, TN, Index<Lower, TPIN>, Index<Lower, TI1N>, Index<Lower, TI2N>> derivative);

        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    christoffel[k, i, j] = 0.5 * (derivative[j, k, i] + derivative[i, k, j] - derivative[k, i, j]);
                }
            }
        }
    }

    /// <inheritdoc cref="Christoffel{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Christoffel{Array2x2x2{TN}, TN, Index{Lower, TI1N}, TI2N, TI3N})"/>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField3x3<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array3x3x3<TN>, TN, Index<Lower, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        christoffel = new();
        Derivative(tape, metric, point, out Tensor<Array3x3x3<TN>, TN, Index<Lower, TPIN>, Index<Lower, TI1N>, Index<Lower, TI2N>> derivative);

        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    christoffel[k, i, j] = 0.5 * (derivative[j, k, i] + derivative[i, k, j] - derivative[k, i, j]);
                }
            }
        }
    }

    /// <inheritdoc cref="Christoffel{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Christoffel{Array2x2x2{TN}, TN, Index{Lower, TI1N}, TI2N, TI3N})"/>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField4x4<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Lower, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
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

    /// <summary>Compute a Christoffel symbol of the second kind given a metric tensor.</summary>
    /// <typeparam name="TT">A type that implements <see cref="ITape{T}"/>.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The index of the point at which to compute the Christoffel symbol.</typeparam>
    /// <typeparam name="TI1N">The first index of the Christoffel symbol.</typeparam>
    /// <typeparam name="TI2N">The second index of the Christoffel symbol.</typeparam>
    /// <typeparam name="TI3N">The third index of the Christoffel symbol.</typeparam>
    /// <param name="tape">A gradient or Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="christoffel">The result.</param>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField2x2<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array2x2x2<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI1N, Index1>(tape, point);
        var invMetric = value.Inverse();
        Christoffel(tape, metric, point, out Christoffel<Array2x2x2<TN>, TN, Index<Lower, Index1>, TI2N, TI3N> christoffelFK);

        var result = Contract(invMetric, christoffelFK);

        christoffel = Unsafe.As<
            Tensor<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>>,
            Christoffel<Array2x2x2<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N>
            >(ref result);
    }

    /// <inheritdoc cref="Christoffel{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Christoffel{Array2x2x2{TN}, TN, Index{Upper, TI1N}, TI2N, TI3N})"/>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField3x3<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array3x3x3<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI1N, Index1>(tape, point);
        var invMetric = value.Inverse();
        Christoffel(tape, metric, point, out Christoffel<Array3x3x3<TN>, TN, Index<Lower, Index1>, TI2N, TI3N> christoffelFK);

        var result = Contract(invMetric, christoffelFK);

        christoffel = Unsafe.As<
            Tensor<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>>,
            Christoffel<Array3x3x3<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N>
            >(ref result);
    }

    /// <inheritdoc cref="Christoffel{TT, TN, TPIN, TI1N, TI2N, TI3N}(TT, MetricTensorField2x2{TT, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Christoffel{Array2x2x2{TN}, TN, Index{Upper, TI1N}, TI2N, TI3N})"/>
    public static void Christoffel<TT, TN, TPIN, TI1N, TI2N, TI3N>(
        TT tape,
        MetricTensorField4x4<TT, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N> christoffel)
        where TT : ITape<TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
    {
        var value = metric.Compute<TI1N, Index1>(tape, point);
        var invMetric = value.Inverse();
        Christoffel(tape, metric, point, out Christoffel<Array4x4x4<TN>, TN, Index<Lower, Index1>, TI2N, TI3N> christoffelFK);

        var result = Contract(invMetric, christoffelFK);

        christoffel = Unsafe.As<
            Tensor<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>>,
            Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, TI2N, TI3N>
            >(ref result);
    }

    /// <summary>Compute the derivative of a Christoffel symbol of the first kind given a metric tensor.</summary>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the index of the point at which to compute the Christoffel symbol.</typeparam>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <typeparam name="TI3N">The name of the third index.</typeparam>
    /// <typeparam name="TI4N">The name of the fourth index.</typeparam>
    /// <param name="tape">A Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="derivative">The result.</param>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField2x2<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        SecondDerivative(tape, metric, point, out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> d2Metric);

        derivative = new();
        for (int m = 0; m < 2; m++)
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        derivative[m, k, i, j] = 0.5 * (d2Metric[m, j, k, i] + d2Metric[m, i, k, j] - d2Metric[m, k, i, j]);
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="DerivativeOfChristoffel{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField3x3<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        SecondDerivative(tape, metric, point, out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> d2Metric);

        derivative = new();
        for (int m = 0; m < 3; m++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        derivative[m, k, i, j] = 0.5 * (d2Metric[m, j, k, i] + d2Metric[m, i, k, j] - d2Metric[m, k, i, j]);
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="DerivativeOfChristoffel{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Lower, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField4x4<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        SecondDerivative(tape, metric, point, out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> d2Metric);

        derivative = new();
        for (int m = 0; m < 4; m++)
        {
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        derivative[m, k, i, j] = 0.5 * (d2Metric[m, j, k, i] + d2Metric[m, i, k, j] - d2Metric[m, k, i, j]);
                    }
                }
            }
        }
    }

    /// <summary>Compute the derivative of a Christoffel symbol of the second kind given a metric tensor.</summary>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the index of the point at which to compute the Christoffel symbol.</typeparam>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <typeparam name="TI3N">The name of the third index.</typeparam>
    /// <typeparam name="TI4N">The name of the fourth index.</typeparam>
    /// <param name="tape">A Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="derivative">The result.</param>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField2x2<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        Derivative(tape, metric, point, out Tensor<Array2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, Index1>> dInvMetric);
        Christoffel(tape, metric, point, out Christoffel<Array2x2x2<TN>, TN, Index<Lower, Index1>, TI3N, TI4N> christoffel);
        var invMetric = metric.ComputeInverse<TI2N, Index1>(tape, point);
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, TI3N>, Index<Lower, TI4N>> dChristoffel);

        var firstPart = Contract(dInvMetric, christoffel);
        var secondPart = Contract(invMetric, dChristoffel);

        derivative = new();
        for (int m = 0; m < 2; m++)
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        derivative[m, k, i, j] = firstPart[m, k, i, j] + secondPart[k, m, i, j];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="DerivativeOfChristoffel{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField3x3<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        Derivative(tape, metric, point, out Tensor<Array3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, Index1>> dInvMetric);
        Christoffel(tape, metric, point, out Christoffel<Array3x3x3<TN>, TN, Index<Lower, Index1>, TI3N, TI4N> christoffel);
        var invMetric = metric.ComputeInverse<TI2N, Index1>(tape, point);
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, TI3N>, Index<Lower, TI4N>> dChristoffel);

        var firstPart = Contract(dInvMetric, christoffel);
        var secondPart = Contract(invMetric, dChristoffel);

        derivative = new();
        for (int m = 0; m < 3; m++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        derivative[m, k, i, j] = firstPart[m, k, i, j] + secondPart[k, m, i, j];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="DerivativeOfChristoffel{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Lower, TI1N}, Index{Upper, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void DerivativeOfChristoffel<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField4x4<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> derivative)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        Derivative(tape, metric, point, out Tensor<Array4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Upper, TI2N>, Index<Upper, Index1>> dInvMetric);
        Christoffel(tape, metric, point, out Christoffel<Array4x4x4<TN>, TN, Index<Lower, Index1>, TI3N, TI4N> christoffel);
        var invMetric = metric.ComputeInverse<TI2N, Index1>(tape, point);
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI1N>, Index<Lower, Index1>, Index<Lower, TI3N>, Index<Lower, TI4N>> dChristoffel);

        var firstPart = Contract(dInvMetric, christoffel);
        var secondPart = Contract(invMetric, dChristoffel);

        derivative = new();
        for (int m = 0; m < 4; m++)
        {
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        derivative[m, k, i, j] = firstPart[m, k, i, j] + secondPart[k, m, i, j];
                    }
                }
            }
        }
    }

    //
    // Riemann tensors.
    //

    /// <summary>Compute a Riemann tensor given a metric tensor.</summary>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
    /// <typeparam name="TPIN">The name of the index of the point at which to compute the Riemann tensor.</typeparam>
    /// <typeparam name="TI1N">The name of the first index.</typeparam>
    /// <typeparam name="TI2N">The name of the second index.</typeparam>
    /// <typeparam name="TI3N">The name of the third index.</typeparam>
    /// <typeparam name="TI4N">The name of the fourth index.</typeparam>
    /// <param name="tape">A Hessian tape.</param>
    /// <param name="metric">A metric tensor field.</param>
    /// <param name="point">A point on the manifold.</param>
    /// <param name="riemann">The result.</param>
    public static void Riemann<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField2x2<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor2<TN, Index<Upper, TPIN>> point,
        out Tensor<Array2x2x2x2<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> riemann)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array2x2x2x2<TN>, TN, Index<Lower, TI3N>, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI4N>> dChristoffel);
        Christoffel(tape, metric, point, out Christoffel<Array2x2x2<TN>, TN, Index<Upper, TI1N>, Index1, TI3N> christoffel);
        var cChristoffels = Contract(christoffel, christoffel.WithIndices<Index<Upper, Index1>, TI2N, TI4N>());

        riemann = new();
        for (int r = 0; r < 2; r++)
        {
            for (int s = 0; s < 2; s++)
            {
                for (int m = 0; m < 2; m++)
                {
                    for (int n = 0; n < 2; n++)
                    {
                        riemann[r, s, m, n] = dChristoffel[m, r, s, n] - dChristoffel[n, r, s, m] + cChristoffels[r, m, s, n] - cChristoffels[r, n, s, m];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Riemann{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Upper, TI1N}, Index{Lower, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void Riemann<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField3x3<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor3<TN, Index<Upper, TPIN>> point,
        out Tensor<Array3x3x3x3<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> riemann)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array3x3x3x3<TN>, TN, Index<Lower, TI3N>, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI4N>> dChristoffel);
        Christoffel(tape, metric, point, out Christoffel<Array3x3x3<TN>, TN, Index<Upper, TI1N>, Index1, TI3N> christoffel);
        var cChristoffels = Contract(christoffel, christoffel.WithIndices<Index<Upper, Index1>, TI2N, TI4N>());

        riemann = new();
        for (int r = 0; r < 3; r++)
        {
            for (int s = 0; s < 3; s++)
            {
                for (int m = 0; m < 3; m++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        riemann[r, s, m, n] = dChristoffel[m, r, s, n] - dChristoffel[n, r, s, m] + cChristoffels[r, m, s, n] - cChristoffels[r, n, s, m];
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="Riemann{TN, TPIN, TI1N, TI2N, TI3N, TI4N}(HessianTape{TN}, MetricTensorField2x2{HessianTape{TN}, TN, Index{Upper, TPIN}}, AutoDiffTensor2{TN, Index{Upper, TPIN}}, out Tensor{Array2x2x2x2{TN}, TN, Index{Upper, TI1N}, Index{Lower, TI2N}, Index{Lower, TI3N}, Index{Lower, TI4N}})"/>
    public static void Riemann<TN, TPIN, TI1N, TI2N, TI3N, TI4N>(
        HessianTape<TN> tape,
        MetricTensorField4x4<HessianTape<TN>, TN, Index<Upper, TPIN>> metric,
        AutoDiffTensor4<TN, Index<Upper, TPIN>> point,
        out Tensor<Array4x4x4x4<TN>, TN, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI3N>, Index<Lower, TI4N>> riemann)
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPIN : IIndexName
        where TI1N : IIndexName
        where TI2N : IIndexName
        where TI3N : IIndexName
        where TI4N : IIndexName
    {
        DerivativeOfChristoffel(tape, metric, point, out Tensor<Array4x4x4x4<TN>, TN, Index<Lower, TI3N>, Index<Upper, TI1N>, Index<Lower, TI2N>, Index<Lower, TI4N>> dChristoffel);
        Christoffel(tape, metric, point, out Christoffel<Array4x4x4<TN>, TN, Index<Upper, TI1N>, Index1, TI3N> christoffel);
        var cChristoffels = Contract(christoffel, christoffel.WithIndices<Index<Upper, Index1>, TI2N, TI4N>());

        riemann = new();
        for (int r = 0; r < 4; r++)
        {
            for (int s = 0; s < 4; s++)
            {
                for (int m = 0; m < 4; m++)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        riemann[r, s, m, n] = dChristoffel[m, r, s, n] - dChristoffel[n, r, s, m] + cChristoffels[r, m, s, n] - cChristoffels[r, n, s, m];
                    }
                }
            }
        }
    }

    //
    // Tensor contractions
    //

    // Rank-one and rank-one.

    [GenerateTensorContractions]
    public static TN Contract<TLR1T, TRR1T, TV, TN, TCI>(in IRankOneTensor<TLR1T, TV, TN, Index<Lower, TCI>> left, in IRankOneTensor<TRR1T, TV, TN, Index<Upper, TCI>> right)
        where TLR1T : IRankOneTensor<TLR1T, TV, TN, Index<Lower, TCI>>
        where TRR1T : IRankOneTensor<TRR1T, TV, TN, Index<Upper, TCI>>
        where TV : IVector<TV, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
    {
        var result = TN.Zero;
        for (int i = 0; i < TV.E1Components; i++)
        {
            result += left[i] * right[i];
        }
        return result;
    }

    // Rank-one and rank-two.

    [GenerateTensorContractions]
    public static Tensor<Vector2<TN>, TN, TI> Contract<TR1T, TR2T, TN, TCI, TI>(
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>> left,
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI> right)
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>>
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector2<TN> vector = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                vector[i] += left[j] * right[j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorContractions]
    public static Tensor<Vector3<TN>, TN, TI> Contract<TR1T, TR2T, TN, TCI, TI>(
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>> left,
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI> right)
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>>
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector3<TN> vector = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vector[i] += left[j] * right[j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorContractions]
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR1T, TR2T, TN, TCI, TI>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> left,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI> right)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector4<TN> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += left[j] * right[j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorContractions]
    public static Tensor<Vector2<TN>, TN, TI> Contract<TR2T, TR1T, TN, TCI, TI>(
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI> left,
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI>
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector2<TN> vector = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                vector[i] += left[j, i] * right[j];
            }
        }
        return new(vector);
    }

    [GenerateTensorContractions]
    public static Tensor<Vector3<TN>, TN, TI> Contract<TR2T, TR1T, TN, TCI, TI>(
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI> left,
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI>
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector3<TN> vector = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vector[i] += left[j, i] * right[j];
            }
        }
        return new(vector);
    }

    [GenerateTensorContractions]
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR2T, TR1T, TN, TCI, TI>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI> left,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector4<TN> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += left[j, i] * right[j];
            }
        }
        return new(vector);
    }

    // Rank-one and rank-three.

    [GenerateTensorContractions]
    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> Contract<TR1T, TR3T, TN, TCI, TI1, TI2>(
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>> left,
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI1, TI2> right)
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>>
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix2x2<TN> matrix = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    matrix[i, j] += left[k] * right[k, i, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix3x3<TN>, TN, TI1, TI2> Contract<TR1T, TR3T, TN, TCI, TI1, TI2>(
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>> left,
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI1, TI2> right)
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>>
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix3x3<TN> matrix = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j] += left[k] * right[k, i, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR1T, TR3T, TN, TCI, TI1, TI2>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> left,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> right)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                    matrix[i, j] += left[k] * right[k, i, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> Contract<TR3T, TR1T, TN, TCI, TI1, TI2>(
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>> right)
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix2x2<TN> matrix = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    matrix[i, j] += left[k, i, j] * right[k];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix3x3<TN>, TN, TI1, TI2> Contract<TR3T, TR1T, TN, TCI, TI1, TI2>(
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>> right)
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix3x3<TN> matrix = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j] += left[k, i, j] * right[k];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR3T, TR1T, TN, TCI, TI1, TI2>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> right)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                    matrix[i, j] += left[k, i, j] * right[k];
                }
            }
        }
        return new(matrix);
    }

    // Rank-one and rank-four.

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> Contract<TR1T, TR4T, TN, TCI, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>> left,
        in IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3> right)
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Lower, TCI>>
        where TR4T : IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k] += left[l] * right[l, i, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> Contract<TR1T, TR4T, TN, TCI, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>> left,
        in IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3> right)
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Lower, TCI>>
        where TR4T : IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k] += left[l] * right[l, i, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR1T, TR4T, TN, TCI, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>> left,
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3> right)
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Lower, TCI>>
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                        array[i, j, k] += left[l] * right[l, i, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> Contract<TR4T, TR1T, TN, TCI, TI1, TI2, TI3>(
        in IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>> right)
        where TR4T : IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k] += left[l, i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> Contract<TR4T, TR1T, TN, TCI, TI1, TI2, TI3>(
        in IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>> right)
        where TR4T : IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k] += left[l, i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR4T, TR1T, TN, TCI, TI1, TI2, TI3>(
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>> right)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector4<TN>, TN, Index<Upper, TCI>>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                        array[i, j, k] += left[l, i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    // Rank-two and rank-two.

    [GenerateTensorContractions]
    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> Contract<TLR2T, TRR3T, TN, TCI, TI1, TI2>(
        in IRankTwoTensor<TLR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankTwoTensor<TRR3T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI2> right)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1>
        where TRR3T : IRankTwoTensor<TRR3T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix2x2<TN> matrix = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    matrix[i, j] += left[k, i] * right[k, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix3x3<TN>, TN, TI1, TI2> Contract<TLR2T, TRR3T, TN, TCI, TI1, TI2>(
        in IRankTwoTensor<TLR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankTwoTensor<TRR3T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI2> right)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1>
        where TRR3T : IRankTwoTensor<TRR3T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix3x3<TN> matrix = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j] += left[k, i] * right[k, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TLR2T, TRR3T, TN, TCI, TI1, TI2>(
        in IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankTwoTensor<TRR3T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI2> right)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TRR3T : IRankTwoTensor<TRR3T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                    matrix[i, j] += left[k, i] * right[k, j];
                }
            }
        }
        return new(matrix);
    }

    // Rank-two and rank-three.

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> Contract<TR2T, TR3T, TN, TCI, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI2, TI3> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k] += left[l, i] * right[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> Contract<TR2T, TR3T, TN, TCI, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI2, TI3> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k] += left[l, i] * right[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR2T, TR3T, TN, TCI, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                        array[i, j, k] += left[l, i] * right[l, j, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> Contract<TR3T, TR2T, TN, TCI, TI1, TI2, TI3>(
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI3> right)
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k] += left[l, i, j] * right[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> Contract<TR3T, TR2T, TN, TCI, TI1, TI2, TI3>(
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI3> right)
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k] += left[l, i, j] * right[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> Contract<TR3T, TR2T, TN, TCI, TI1, TI2, TI3>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI3> right)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                        array[i, j, k] += left[l, i, j] * right[l, k];
                    }
                }
            }
        }
        return new(array);
    }

    // Rank-two and rank-four.

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR2T, TR4T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Lower, TCI>, TI1>
        where TR4T : IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            array[i, j, k, l] += left[m, i] * right[m, j, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR2T, TR4T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Lower, TCI>, TI1>
        where TR4T : IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            array[i, j, k, l] += left[m, i] * right[m, j, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR2T, TR4T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1> left,
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Lower, TCI>, TI1>
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Upper, TCI>, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                            array[i, j, k, l] += left[m, i] * right[m, j, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR4T, TR2T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI4> right)
        where TR4T : IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, Index<Upper, TCI>, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            array[i, j, k, l] += left[m, i, j, k] * right[m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR4T, TR2T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI4> right)
        where TR4T : IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, Index<Upper, TCI>, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            array[i, j, k, l] += left[m, i, j, k] * right[m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TR4T, TR2T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3> left,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI4> right)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2, TI3>
        where TR2T : IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, Index<Upper, TCI>, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                            array[i, j, k, l] += left[m, i, j, k] * right[m, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    // Rank-three and rank-three.

    [GenerateTensorContractions]
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TLR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankThreeTensor<TRR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TRR3T : IRankThreeTensor<TRR3T, Array2x2x2<TN>, TN, Index<Upper, TCI>, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            array[i, j, k, l] += left[m, i, j] * right[m, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TLR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankThreeTensor<TRR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TRR3T : IRankThreeTensor<TRR3T, Array3x3x3<TN>, TN, Index<Upper, TCI>, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            array[i, j, k, l] += left[m, i, j] * right[m, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    [GenerateTensorContractions]
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> left,
        in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
        where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                            array[i, j, k, l] += left[m, i, j] * right[m, k, l];
                        }
                    }
                }
            }
        }
        return new(array);
    }

    //
    // Tensor Self-Contractions
    //

    [GenerateTensorSelfContractions]
    public static TN Contract<TR2T, TSM, TN, TCI>(in IRankTwoTensor<TR2T, TSM, TN, Index<Lower, TCI>, Index<Upper, TCI>> tensor)
        where TR2T : IRankTwoTensor<TR2T, TSM, TN, Index<Lower, TCI>, Index<Upper, TCI>>
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
    {
        var result = TN.Zero;
        for (int i = 0; i < TSM.E1Components; i++)
        {
            result += tensor[i, i];
        }
        return result;
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Vector2<TN>, TN, TI> Contract<TR3T, TN, TCI, TI>(in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI> tensor)
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector2<TN> vector = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                vector[i] += tensor[j, j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Vector3<TN>, TN, TI> Contract<TR3T, TN, TCI, TI>(in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI> tensor)
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector3<TN> vector = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vector[i] += tensor[j, j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Vector4<TN>, TN, TI> Contract<TR3T, TN, TCI, TI>(in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI> tensor)
        where TR3T : IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI : IIndex
    {
        Vector4<TN> vector = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                vector[i] += tensor[j, j, i];
            }
        }
        return new(vector);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> Contract<TR4T, TN, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array2x2x2x2<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix2x2<TN> matrix = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    matrix[i, j] += tensor[k, k, i, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Matrix3x3<TN>, TN, TI1, TI2> Contract<TR4T, TN, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array3x3x3x3<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix3x3<TN> matrix = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j] += tensor[k, k, i, j];
                }
            }
        }
        return new(matrix);
    }

    [GenerateTensorSelfContractions]
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> Contract<TR4T, TN, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN>, TN, Index<Lower, TCI>, Index<Upper, TCI>, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TCI : IIndexName
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
                    matrix[i, j] += tensor[k, k, i, j];
                }
            }
        }
        return new(matrix);
    }

    //
    // Tensor Products
    //

    /// <summary>Compute the tensor product of two rank-one tensors.</summary>
    /// <typeparam name="TLR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TRR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The index of the second tensor.</typeparam>
    /// <param name="left">The first tensor.</param>
    /// <param name="right">The second tensor.</param>
    /// <returns>A rank-two tensor.</returns>
    public static Tensor<Matrix2x2<TN>, TN, TI1, TI2> TensorProduct<TLR1T, TRR1T, TN, TI1, TI2>(in IRankOneTensor<TLR1T, Vector2<TN>, TN, TI1> left, in IRankOneTensor<TRR1T, Vector2<TN>, TN, TI2> right)
        where TLR1T : IRankOneTensor<TLR1T, Vector2<TN>, TN, TI1>
        where TRR1T : IRankOneTensor<TRR1T, Vector2<TN>, TN, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix2x2<TN> matrix = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                matrix[i, j] = left[i] * right[j];
            }
        }
        return new(matrix);
    }

    /// <inheritdoc cref="TensorProduct{TLR1T, TRR1T, TN, TI1, TI2}(in IRankOneTensor{TLR1T, Vector2{TN}, TN, TI1}, in IRankOneTensor{TRR1T, Vector2{TN}, TN, TI2})"/>
    public static Tensor<Matrix3x3<TN>, TN, TI1, TI2> TensorProduct<TLR1T, TRR1T, TN, TI1, TI2>(in IRankOneTensor<TLR1T, Vector3<TN>, TN, TI1> left, in IRankOneTensor<TRR1T, Vector3<TN>, TN, TI2> right)
        where TLR1T : IRankOneTensor<TLR1T, Vector3<TN>, TN, TI1>
        where TRR1T : IRankOneTensor<TRR1T, Vector3<TN>, TN, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
    {
        Matrix3x3<TN> matrix = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix[i, j] = left[i] * right[j];
            }
        }
        return new(matrix);
    }

    /// <inheritdoc cref="TensorProduct{TLR1T, TRR1T, TN, TI1, TI2}(in IRankOneTensor{TLR1T, Vector2{TN}, TN, TI1}, in IRankOneTensor{TRR1T, Vector2{TN}, TN, TI2})"/>
    public static Tensor<Matrix4x4<TN>, TN, TI1, TI2> TensorProduct<TLR1T, TRR1T, TN, TI1, TI2>(in IRankOneTensor<TLR1T, Vector4<TN>, TN, TI1> left, in IRankOneTensor<TRR1T, Vector4<TN>, TN, TI2> right)
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
                matrix[i, j] = left[i] * right[j];
            }
        }
        return new(matrix);
    }

    /// <summary>Compute the tensor product of a rank-one tensor and a rank-two tensor.</summary>
    /// <typeparam name="TR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TR2T">A rank-two tensor.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The first index of the second tensor.</typeparam>
    /// <typeparam name="TI3">The second index of the second tensor.</typeparam>
    /// <param name="left">A rank-one tensor.</param>
    /// <param name="right">A rank-two tensor.</param>
    /// <returns>A rank-three tensor.</returns>
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> TensorProduct<TR1T, TR2T, TN, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector2<TN>, TN, TI1> left,
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, TI2, TI3> right)
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, TI1>
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    array[i, j, k] = left[i] * right[j, k];
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR1T, TR2T, TN, TI1, TI2, TI3}(in IRankOneTensor{TR1T, Vector2{TN}, TN, TI1}, in IRankTwoTensor{TR2T, Matrix2x2{TN}, TN, TI2, TI3})"/>
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> TensorProduct<TR1T, TR2T, TN, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector3<TN>, TN, TI1> left,
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, TI2, TI3> right)
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, TI1>
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, TI2, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    array[i, j, k] = left[i] * right[j, k];
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR1T, TR2T, TN, TI1, TI2, TI3}(in IRankOneTensor{TR1T, Vector2{TN}, TN, TI1}, in IRankTwoTensor{TR2T, Matrix2x2{TN}, TN, TI2, TI3})"/>
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> TensorProduct<TR1T, TR2T, TN, TI1, TI2, TI3>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI1> left,
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI2, TI3> right)
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
                    array[i, j, k] = left[i] * right[j, k];
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of a rank-two tensor and a rank-one tensor.</summary>
    /// <typeparam name="TR2T">A rank-two tensor.</typeparam>
    /// <typeparam name="TR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The first index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The second index of the second tensor.</typeparam>
    /// <typeparam name="TI3">The index of the second tensor.</typeparam>
    /// <param name="left">A rank-two tensor.</param>
    /// <param name="right">A rank-one tensor.</param>
    /// <returns>A rank-three tensor.</returns>
    public static Tensor<Array2x2x2<TN>, TN, TI1, TI2, TI3> TensorProduct<TR2T, TR1T, TN, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector2<TN>, TN, TI3> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix2x2<TN>, TN, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    array[i, j, k] = left[i, j] * right[k];
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR2T, TR1T, TN, TI1, TI2, TI3}(in IRankTwoTensor{TR2T, Matrix2x2{TN}, TN, TI1, TI2}, in IRankOneTensor{TR1T, Vector2{TN}, TN, TI3})"/>
    public static Tensor<Array3x3x3<TN>, TN, TI1, TI2, TI3> TensorProduct<TR2T, TR1T, TN, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector3<TN>, TN, TI3> right)
        where TR2T : IRankTwoTensor<TR2T, Matrix3x3<TN>, TN, TI1, TI2>
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, TI3>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
    {
        Array3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    array[i, j, k] = left[i, j] * right[k];
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR2T, TR1T, TN, TI1, TI2, TI3}(in IRankTwoTensor{TR2T, Matrix2x2{TN}, TN, TI1, TI2}, in IRankOneTensor{TR1T, Vector2{TN}, TN, TI3})"/>
    public static Tensor<Array4x4x4<TN>, TN, TI1, TI2, TI3> TensorProduct<TR2T, TR1T, TN, TI1, TI2, TI3>(
        in IRankTwoTensor<TR2T, Matrix4x4<TN>, TN, TI1, TI2> left,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI3> right)
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
                    array[i, j, k] = left[i, j] * right[k];
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of a rank-one tensor and a rank-three tensor.</summary>
    /// <typeparam name="TR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TR3T">A rank-three tensor.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The first index of the second tensor.</typeparam>
    /// <typeparam name="TI3">The second index of the second tensor.</typeparam>
    /// <typeparam name="TI4">The third index of the second tensor.</typeparam>
    /// <param name="left">A rank-one tensor.</param>
    /// <param name="right">A rank-three tensor.</param>
    /// <returns>A rank-four tensor.</returns>
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR1T, TR3T, TN, TI1, TI2, TI3, TI4>(
        in IRankOneTensor<TR1T, Vector2<TN>, TN, TI1> left,
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, TI2, TI3, TI4> right)
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k, l] = left[i] * right[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR1T, TR3T, TN, TI1, TI2, TI3, TI4}(in IRankOneTensor{TR1T, Vector2{TN}, TN, TI1}, in IRankThreeTensor{TR3T, Array2x2x2{TN}, TN, TI2, TI3, TI4})"/>
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR1T, TR3T, TN, TI1, TI2, TI3, TI4>(
        in IRankOneTensor<TR1T, Vector3<TN>, TN, TI1> left,
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, TI2, TI3, TI4> right)
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, TI1>
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, TI2, TI3, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k, l] = left[i] * right[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR1T, TR3T, TN, TI1, TI2, TI3, TI4}(in IRankOneTensor{TR1T, Vector2{TN}, TN, TI1}, in IRankThreeTensor{TR3T, Array2x2x2{TN}, TN, TI2, TI3, TI4})"/>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR1T, TR3T, TN, TI1, TI2, TI3, TI4>(
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI1> left,
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI2, TI3, TI4> right)
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
                        array[i, j, k, l] = left[i] * right[j, k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of a rank-three tensor and a rank-one tensor.</summary>
    /// <typeparam name="TR3T">A rank-three tensor.</typeparam>
    /// <typeparam name="TR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The first index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The second index of the first tensor.</typeparam>
    /// <typeparam name="TI3">The third index of the first tensor.</typeparam>
    /// <typeparam name="TI4">The index of the second tensor.</typeparam>
    /// <param name="left">A rank-three tensor.</param>
    /// <param name="right">A rank-one tensor.</param>
    /// <returns>A rank-four tensor.</returns>
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR3T, TR1T, TN, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector2<TN>, TN, TI4> right)
        where TR3T : IRankThreeTensor<TR3T, Array2x2x2<TN>, TN, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector2<TN>, TN, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        array[i, j, k, l] = left[i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR3T, TR1T, TN, TI1, TI2, TI3, TI4}(in IRankThreeTensor{TR3T, Array2x2x2{TN}, TN, TI1, TI2, TI3}, in IRankOneTensor{TR1T, Vector2{TN}, TN, TI4})"/>
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR3T, TR1T, TN, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector3<TN>, TN, TI4> right)
        where TR3T : IRankThreeTensor<TR3T, Array3x3x3<TN>, TN, TI1, TI2, TI3>
        where TR1T : IRankOneTensor<TR1T, Vector3<TN>, TN, TI4>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k, l] = left[i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TR3T, TR1T, TN, TI1, TI2, TI3, TI4}(in IRankThreeTensor{TR3T, Array2x2x2{TN}, TN, TI1, TI2, TI3}, in IRankOneTensor{TR1T, Vector2{TN}, TN, TI4})"/>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TR3T, TR1T, TN, TI1, TI2, TI3, TI4>(
        in IRankThreeTensor<TR3T, Array4x4x4<TN>, TN, TI1, TI2, TI3> left,
        in IRankOneTensor<TR1T, Vector4<TN>, TN, TI4> right)
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
                        array[i, j, k, l] = left[i, j, k] * right[l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <summary>Compute the tensor product of two rank-two tensors.</summary>
    /// <typeparam name="TLR2T">A rank-two tensors.</typeparam>
    /// <typeparam name="TRR2T">A rank-two tensors.</typeparam>
    /// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
    /// <typeparam name="TI1">The first index of the first tensor.</typeparam>
    /// <typeparam name="TI2">The second index of the first tensor.</typeparam>
    /// <typeparam name="TI3">The first index of the second tensor.</typeparam>
    /// <typeparam name="TI4">The second index of the second tensor.</typeparam>
    /// <param name="left">A rank-two tensor.</param>
    /// <param name="right">A rank-two tensor.</param>
    /// <returns>A rank-four tensor.</returns>
    public static Tensor<Array2x2x2x2<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TLR2T, Matrix2x2<TN>, TN, TI1, TI2> left,
        in IRankTwoTensor<TRR2T, Matrix2x2<TN>, TN, TI1, TI2> right)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix2x2<TN>, TN, TI1, TI2>
        where TRR2T : IRankTwoTensor<TRR2T, Matrix2x2<TN>, TN, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array2x2x2x2<TN> array = new();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        array[i, j, k, l] = left[i, j] * right[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4}(in IRankTwoTensor{TLR2T, Matrix2x2{TN}, TN, TI1, TI2}, in IRankTwoTensor{TRR2T, Matrix2x2{TN}, TN, TI1, TI2})"/>
    public static Tensor<Array3x3x3x3<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TLR2T, Matrix3x3<TN>, TN, TI1, TI2> left,
        in IRankTwoTensor<TRR2T, Matrix3x3<TN>, TN, TI1, TI2> right)
        where TLR2T : IRankTwoTensor<TLR2T, Matrix3x3<TN>, TN, TI1, TI2>
        where TRR2T : IRankTwoTensor<TRR2T, Matrix3x3<TN>, TN, TI1, TI2>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TI1 : IIndex
        where TI2 : IIndex
        where TI3 : IIndex
        where TI4 : IIndex
    {
        Array3x3x3x3<TN> array = new();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        array[i, j, k, l] = left[i, j] * right[k, l];
                    }
                }
            }
        }
        return new(array);
    }

    /// <inheritdoc cref="TensorProduct{TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4}(in IRankTwoTensor{TLR2T, Matrix2x2{TN}, TN, TI1, TI2}, in IRankTwoTensor{TRR2T, Matrix2x2{TN}, TN, TI1, TI2})"/>
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> TensorProduct<TLR2T, TRR2T, TN, TI1, TI2, TI3, TI4>(
        in IRankTwoTensor<TLR2T, Matrix4x4<TN>, TN, TI1, TI2> left,
        in IRankTwoTensor<TRR2T, Matrix4x4<TN>, TN, TI1, TI2> right)
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
                        array[i, j, k, l] = left[i, j] * right[k, l];
                    }
                }
            }
        }
        return new(array);
    }
}
