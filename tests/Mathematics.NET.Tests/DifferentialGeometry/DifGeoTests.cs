﻿// <copyright file="DifGeoTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Tests.DifferentialGeometry;

[TestClass]
[TestCategory("DifGeo")]
public sealed class DifGeoTests
{
    private GradientTape<Real> _gradientTape;
    private HessianTape<Real> _hessianTape;

    public DifGeoTests()
    {
        _gradientTape = new();
        _hessianTape = new();
    }

    [TestMethod]
    [DynamicData(nameof(GetMetricTensorDerivativeData), DynamicDataSourceType.Method)]
    public void Derivative_InverseMetric_ReturnsRankThreeTensor(object[] input, object[] values)
    {
        DifGeoTestHelpers.Test4x4MetricTensorFieldNo1<ITape<Real>, Real, Index<Upper, Delta>> metric = new();
        var point = _gradientTape.CreateAutoDiffTensor<Index<Upper, Delta>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.Derivative(_gradientTape, metric, point, out Tensor<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Upper, Beta>, Index<Upper, Gamma>> derivative);
        var actual = derivative.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-13);
    }

    [TestMethod]
    [DynamicData(nameof(GetSecondDerivativeData), DynamicDataSourceType.Method)]
    public void SecondDerivative_RankTwoTensor_ReturnsRankFourTensor(object[] input, object[] values)
    {
        DifGeoTestHelpers.Test4x4MetricTensorFieldNo1<HessianTape<Real>, Real, Index<Upper, Epsilon>> metric = new();
        var point = _hessianTape.CreateAutoDiffTensor<Index<Upper, Epsilon>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,,])values[0];

        DifGeo.SecondDerivative(_hessianTape, metric, point, out Tensor<Array4x4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Beta>, Index<Lower, Gamma>, Index<Lower, Delta>> secondDerivative);
        var actual = secondDerivative.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetMetricTensorDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.46, 3.14, 7.13],
            new object[]
            {
                new Real[,,]
                {
                    {
                        { -2.290881032398611,  0.04970311612889121,  -2.57956167734756,   0.001486381669295939   },
                        { 122.3470549435311,   -2.671189795593617,   137.7841717990172,   -0.0800703668815353    },
                        { -0.0992229855229368, 0.002136384763996471, -0.1117735826697046, 0.00006600657179987652 },
                        { 25.3560164592145,    -0.553257324188447,   28.5556556030038,    -0.01660628062905417   }
                    },
                    {
                        { -10.82434653066666,  0.2198487580749168,  -12.66746323676432,  0.007975737250463516  },
                        { 600.4819658384281,   -12.19898568833379,  703.4775791702065,   -0.4428902526371429   },
                        { -0.4150914591400523, 0.00843166579766857, -0.4860122469444493, 0.0003059696329725992 },
                        { 128.3031985873712,   -2.606176170864468,  150.2193461522867,   -0.0945783956997469   }
                    },
                    {
                        { -3.39510633610317, 0.07663648051813023,  -3.873666685165675,  0.002442221801832182  },
                        { 186.9333294862945, -4.235710578197299,   213.3800859077058,   -0.1345243000761476   },
                        { -0.16578123571062, 0.003663401378359327, -0.1896973978162401, 0.0001195883691042653 },
                        { 38.99467405573273, -0.884303562122364,   44.51297071480505,   -0.02806208158194548  }
                    },
                    {
                        { 2.204904554703536,  -0.05439983485856153, 2.415399499446983,  -0.001951600569672975  },
                        { -99.9083713139095,  2.583393035830481,    -108.4372846947553, 0.0894257725193976     },
                        { 0.0876578722667318, -0.00238146677635786, 0.0960322791258335, -0.0000743715905405725 },
                        { -21.22135210759896, 0.5469516019661675,   -23.0619674179585,  0.01887653616804277    }
                    },
                }
            }
        };
    }

    public static IEnumerable<object[]> GetSecondDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.46, 3.14, 7.13],
            new object[]
            {
                new Real[,,,]
                {
                    {
                        {
                            { -0.942488801931697, 0, 0,  0                 },
                            { 0,                  0, 0,  14.85408845588213 },
                            { 0,                  0, 22, 0                 },
                            { -15301.68323198016, 0, 0,  0                 }
                        },
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 358.2112 }
                        },
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 280.6368 }
                        },
                        {
                            { 0,                 0, 0, 0        },
                            { 0,                 0, 0, 0        },
                            { 0,                 0, 0, 0        },
                            { 5426.483385429979, 0, 0, 123.5904 }
                        },
                    },
                    {
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 358.2112 }
                        },
                        {
                            { 0,                   1.553140567066586, 0, 0                 },
                            { -0.3239719070443792, 0,                 0, 0                 },
                            { 0,                   0,                 0, 23.48462711858732 },
                            { 0,                   13.41041568258237, 0, 0                 }
                        },
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 140.3184 }
                        },
                        {
                            { 0, 0,                  0, 0       },
                            { 0, 0,                  0, 0       },
                            { 0, 0,                  0, 0       },
                            { 0, -2.313437768524027, 0, 61.7952 }
                        },
                    },
                    {
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 280.6368 }
                        },
                        {
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 0        },
                            { 0, 0, 0, 140.3184 }
                        },
                        {
                            { 0,                   0,                    69.31160057616656,  0 },
                            { 0,                   -0.01911190771506838, 0,                  0 },
                            { -0.1338783158199425, 0,                    0,                  0 },
                            { 0,                   0,                    -45.07682430841755, 0 }
                        },
                        {
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 22.53841215420878, 48.41279999999999 }
                        },
                    },
                    {
                        {
                            { 0,                 0, 0, 0        },
                            { 0,                 0, 0, 0        },
                            { 0,                 0, 0, 0        },
                            { 5426.483385429979, 0, 0, 123.5904 }
                        },
                        {
                            { 0, 0,                  0, 0       },
                            { 0, 0,                  0, 0       },
                            { 0, 0,                  0, 0       },
                            { 0, -2.313437768524027, 0, 61.7952 }
                        },
                        {
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 0,                 0                 },
                            { 0, 0, 22.53841215420878, 48.41279999999999 }
                        },
                        {
                            { 0,                 0,                   0,                  -0.07868300388103916 },
                            { 0,                 0,                   4371.066581678536,  0                    },
                            { 0,                 0.05517742207646505, 0,                  0                    },
                            { 15301.68323198016, 0,                   -11.26920607710439, 0                    }
                        },
                    },
                }
            }
        };
    }
}
