// <copyright file="DifGeoForwardModeTests.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0060

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.LinearAlgebra;
using static Mathematics.NET.AutoDiff.HyperDual<Mathematics.NET.Core.Real>;

namespace Mathematics.NET.Tests.DifferentialGeometry;

[TestClass]
[TestCategory("DifGeo")]
public sealed class DifGeoForwardModeTests
{
    public static FMTensorField4<HyperDual<Real>, Real, Upper, Index<Upper, PIN>> R1Tensor { get; set; } = new();

    public static FMTensorField4x4<HyperDual<Real>, Real, Upper, Upper, Index<Upper, PIN>> R2Tensor { get; set; } = new();

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        R1Tensor[0] = x => Sin(x.X0 + x.X1);
        R1Tensor[1] = x => Cos(x.X1 + x.X2);
        R1Tensor[2] = x => Exp(x.X2 + x.X3);
        R1Tensor[3] = x => Sqrt(x.X3 + x.X0);

        R2Tensor[0, 0] = x => Sin(x.X0);
        R2Tensor[0, 1] = x => 2 * Cos(x.X1);
        R2Tensor[0, 2] = x => 3 * Exp(x.X2);
        R2Tensor[0, 3] = x => 4 * Ln(x.X3);

        R2Tensor[1, 0] = x => 5 * Sqrt(x.X1);
        R2Tensor[1, 1] = x => 6 * Tan(x.X2);
        R2Tensor[1, 2] = x => 7 * Sinh(x.X3);
        R2Tensor[1, 3] = x => 8 * Cosh(x.X0);

        R2Tensor[2, 0] = x => 9 * Tanh(x.X2);
        R2Tensor[2, 1] = x => 10 / x.X3;
        R2Tensor[2, 2] = x => 11 * Pow(x.X0, 2);
        R2Tensor[2, 3] = x => 12 * Sin(x.X1) * Cos(x.X1);

        R2Tensor[3, 0] = x => 13 * Exp(x.X3) * Sin(x.X0);
        R2Tensor[3, 1] = x => 14 * x.X3 / x.X1;
        R2Tensor[3, 2] = x => 15 * Sin(x.X3 - 2 * x.X2);
        R2Tensor[3, 3] = x => 16 * x.X0 * x.X1 * x.X2 * x.X3;
    }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetR1TensorDerivativeData), DynamicDataSourceType.Method)]
    public void Derivative_RankOneTensor_ReturnsRankTwoTensor(object[] input, object[] values)
    {
        var point = CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,])values[0];

        DifGeo.Derivative(R1Tensor, point, out Tensor<Matrix4x4<Real>, Real, Index<Lower, Alpha>, Index<Upper, Beta>> dTensor);
        var actual = dTensor.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DynamicData(nameof(GetR2TensorDerivativeData), DynamicDataSourceType.Method)]
    public void Derivative_RankTwoTensor_ReturnsRankThreeTensor(object[] input, object[] values)
    {
        var point = CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.Derivative(R2Tensor, point, out Tensor<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Upper, Beta>, Index<Upper, Gamma>> dTensor);
        var actual = dTensor.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DynamicData(nameof(GetR1TensorSecondDerivativeData), DynamicDataSourceType.Method)]
    public void SecondDerivative_RankOneTensor_ReturnsRankTwoTensor(object[] input, object[] values)
    {
        var point = CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.SecondDerivative(R1Tensor, point, out Tensor<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Upper, Beta>, Index<Upper, Gamma>> d2Tensor);
        var actual = d2Tensor.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DynamicData(nameof(GetR2TensorSecondDerivativeData), DynamicDataSourceType.Method)]
    public void SecondDerivative_RankTwoTensor_ReturnsRankFourTensor(object[] input, object[] values)
    {
        var point = CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,,])values[0];

        DifGeo.SecondDerivative(R2Tensor, point, out Tensor<Array4x4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Beta>, Index<Upper, Gamma>, Index<Upper, Delta>> d2Tensor);
        var actual = d2Tensor.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetR1TensorDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.34, 3.45, 4.56],
            new object[]
            {
                new Real[,]
                {
                    { -0.9096285273579445, 0,                   0,                  0.2077929087308457 },
                    { -0.9096285273579445, 0.47343399708193507, 0,                  0                  },
                    { 0,                   0.47343399708193507, 3010.9171128823823, 0                  },
                    { 0,                   0,                   3010.9171128823823, 0.2077929087308457 }
                }
            }
        };
    }

    public static IEnumerable<object[]> GetR2TensorDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.46, 3.14, 7.13],
            new object[]
            {
                new Real[,,]
                {
                    {
                        { 0.3342377271245026, 0, 0,     0                  },
                        { 0,                  0, 0,     12.515747834435256 },
                        { 0,                  0, 27.06, 0                  },
                        { 5426.483385429979,  0, 0,     881.199552         }
                    },
                    {
                        { 0,                  -1.2600612599917844, 0, 0                 },
                        { 1.5939417826583457, 0,                   0, 0                 },
                        { 0,                  0,                   0, 2.473473726407499 },
                        { 0,                  -16.494811289576308, 0, 440.599776        }
                    },
                    {
                        { 0,                   0,                69.31160057616655,  0                  },
                        { 0,                   6.00001521929848, 0,                  0                  },
                        { 0.06719043637160116, 0,                0,                  0                  },
                        { 0,                   0,                -19.79949437654947, 345.18326399999995 }
                    },
                    {
                        { 0,                 0,                   0,                 0.5610098176718092 },
                        { 0,                 0,                   4371.072186714249, 0                  },
                        { 0,                 -0.1967075097025979, 0,                 0                  },
                        { 15301.68323198016, 5.691056910569106,   9.899747188274736, 152.016192         }
                    },
                }
            }
        };
    }

    public static IEnumerable<object[]> GetR1TensorSecondDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.34, 3.45, 4.56],
            new object[]
            {
                new Real[,,]
                {
                    {
                        { 0.4154226067712459, 0, 0, -0.017944119924943498 },
                        { 0.4154226067712459, 0, 0, 0                     },
                        { 0,                  0, 0, 0                     },
                        { 0,                  0, 0, -0.017944119924943498 }
                    },
                    {
                        { 0.4154226067712459, 0,                  0, 0 },
                        { 0.4154226067712459, -0.880829296973609, 0, 0 },
                        { 0,                  -0.880829296973609, 0, 0 },
                        { 0,                  0,                  0, 0 }
                    },
                    {
                        { 0, 0,                  0,                  0 },
                        { 0, -0.880829296973609, 0,                  0 },
                        { 0, -0.880829296973609, 3010.9171128823823, 0 },
                        { 0, 0,                  3010.9171128823823, 0 }
                    },
                    {
                        { 0, 0, 0,                  -0.017944119924943498 },
                        { 0, 0, 0,                  0                     },
                        { 0, 0, 3010.9171128823823, 0                     },
                        { 0, 0, 3010.9171128823823, -0.017944119924943498 }
                    },
                }
            }
        };
    }

    public static IEnumerable<object[]> GetR2TensorSecondDerivativeData()
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
