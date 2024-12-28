// <copyright file="ChristoffelSymbolTests.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Tests.DifferentialGeometry;

[TestClass]
[TestCategory("DifGeo"), TestCategory("Christoffel")]
public sealed class ChristoffelSymbolTests
{
    public static MetricTensorField4x4<GradientTape<Real>, Real, Index<Upper, PIN>> Tensor { get; set; } = new();

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Tensor[0, 0] = (tape, x) => tape.Sin(x.X0);
        Tensor[0, 1] = (tape, x) => tape.Multiply(2, tape.Cos(x.X1));
        Tensor[0, 2] = (tape, x) => tape.Multiply(3, tape.Exp(x.X2));
        Tensor[0, 3] = (tape, x) => tape.Multiply(4, tape.Ln(x.X3));

        Tensor[1, 0] = (tape, x) => tape.Multiply(5, tape.Sqrt(x.X1));
        Tensor[1, 1] = (tape, x) => tape.Multiply(6, tape.Tan(x.X2));
        Tensor[1, 2] = (tape, x) => tape.Multiply(7, tape.Sinh(x.X3));
        Tensor[1, 3] = (tape, x) => tape.Multiply(8, tape.Cosh(x.X0));

        Tensor[2, 0] = (tape, x) => tape.Multiply(9, tape.Tanh(x.X2));
        Tensor[2, 1] = (tape, x) => tape.Divide(10, x.X3);
        Tensor[2, 2] = (tape, x) => tape.Multiply(11, tape.Pow(x.X0, 2));
        Tensor[2, 3] = (tape, x) => tape.Multiply(12, tape.Multiply(tape.Sin(x.X1), tape.Cos(x.X1)));

        Tensor[3, 0] = (tape, x) => tape.Multiply(13, tape.Multiply(tape.Exp(x.X3), tape.Sin(x.X0)));
        Tensor[3, 1] = (tape, x) => tape.Multiply(14, tape.Divide(x.X3, x.X1));
        Tensor[3, 2] = (tape, x) => tape.Multiply(15, tape.Sin(tape.Subtract(x.X3, tape.Multiply(2, x.X2))));
        Tensor[3, 3] = (tape, x) => tape.Multiply(16, tape.Multiply(x.X0, tape.Multiply(x.X1, tape.Multiply(x.X2, x.X3))));
    }

    public ChristoffelSymbolTests()
    {
        Tape = new();
    }

    public GradientTape<Real> Tape { get; set; }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetChristoffelSymbolOfFirstKindData), DynamicDataSourceType.Method)]
    public void Christoffel_FromMetricTensor_ReturnsChristoffelSymbolOfTheFirstKind(object[] input, object[] values)
    {
        var point = Tape.CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.Christoffel(Tape, Tensor, point, out Christoffel<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Beta, Gamma> result);
        var actual = result.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DynamicData(nameof(GetChristoffelSymbolOfSecondKindData), DynamicDataSourceType.Method)]
    public void Christoffel_FromMetricTensor_ReturnsChristoffelSymbolOfTheSecondKind(object[] input, object[] values)
    {
        var point = Tape.CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.Christoffel(Tape, Tensor, point, out Christoffel<Array4x4x4<Real>, Real, Index<Upper, Alpha>, Beta, Gamma> result);
        var actual = result.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-14);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetChristoffelSymbolOfFirstKindData()
    {
        yield return new[]
        {
            [1.23, 2.46, 3.14, 7.13],
            new object[]
            {
                new Real[,,]
                {
                    {
                        { 0.1671188635622513, 0,                  0,                 0                  },
                        { 0,                  -1.260061259991784, 0,                 -6.257873917217628 },
                        { 0,                  0,                  55.78160057616655, 0                  },
                        { -2713.241692714989, 0,                  0,                 -440.0387661823283 }
                    },
                    {
                        { 0,                 1.427001521325065, 0,                 6.257873917217628 },
                        { 0,                 0,                 3.00000760964924,  0                 },
                        { 0,                 3.00000760964924,  0,                 2184.299356493921 },
                        { 6.257873917217628, 8.24740564478816,  2185.536093357125, -220.299888       }
                    },
                    {
                        { 0,     0,                 -21.09220506989748, 0                 },
                        { 0,     -3.00000760964924, 0,                  1.138383108352451 },
                        { 13.53, 0,                 0,                  0                 },
                        { 0,     1.138383108352451, 9.89974718827474,   -172.591632       }
                    },
                    {
                        { 5426.483385429979, 0,                  0,                  8091.16088708124  },
                        { 0,                 -16.49481128957631, -2185.536093357125, 223.1454164552846 },
                        { 0,                 0.0983537548512989, -19.79949437654947, 177.5415055941374 },
                        { 440.599776,        220.299888,         172.591632,         76.008096         }
                    }
                }
            }
        };
    }

    public static IEnumerable<object[]> GetChristoffelSymbolOfSecondKindData()
    {
        yield return new[]
        {
            [1.23, 2.46, 3.14, 7.13],
            new object[]
            {
                new Real[,,]
                {
                    {
                        { 2.239350609791927,  0.01461934253211735, 12.12758273782715,  3.529703641587461 },
                        { 0,                  2.358106360643505,   -0.90537082358634,  2.620615431196252 },
                        { -7.779470847122719, 0.03077659910243831, -28.35074813512594, 22.45375027707856 },
                        { 1378.833245794903,  -0.4756953011488042, 16.77214989444621,  320.5930840087684 }
                    },
                    {
                        { -109.4128102983294, -0.836915783423558, -704.4688660629324, -174.0290625011717 },
                        { 0,                  -136.3557010019774, 44.25687093771166,  -147.9690124918006 },
                        { 451.8950828633208,  -1.761531969227613, 1616.423265670495,  -1284.79815262318  },
                        { -78616.21153791852, 28.54602852524207,  -954.772766741069,  -18384.8988352843  }
                    },
                    {
                        { 0.07224604305626464, 0.00089333296529076 , 0.4689197542974098,   0.1165529750046903 },
                        { 0,                   0.0913097548918665,   -0.02854633808253575, 0.1011789045933849 },
                        { -0.3007975816003572, 0.001879437023045884, -1.100072057551851,   1.36988885711727   },
                        { 53.5046353708834,    -0.01707861770862742, 1.150504003285304,    12.37604007775708  }
                    },
                    {
                        { -22.51226842522944, -0.1750670584484963, -144.8435008425763, -35.85185118427089 },
                        { 0,                  -28.20211764966779,  9.10866804307758,   -31.25234409183736 },
                        { 92.9126452120913,   -0.3684726863946806, 339.7224073964993,  -268.7435284973888 },
                        { -16522.75026428793, 5.850406741691683,   -200.8906641281141, -3837.77389479284  }
                    }
                }
            }
        };
    }
}
