// <copyright file="ChristoffelSymbolTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Tests.DifferentialGeometry;

[TestClass]
[TestCategory("DifGeo"), TestCategory("Christoffel")]
public sealed class ChristoffelSymbolTests
{
    private GradientTape<Real> _tape;

    public ChristoffelSymbolTests()
    {
        _tape = new();
    }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetChristoffelSymbolData), DynamicDataSourceType.Method)]
    public void Christoffel_FromMetricTensor_ReturnsChristoffelSymbolOfTheFirstKind(object[] input, object[] values)
    {
        DifGeoTestHelpers.Test4x4MetricTensorFieldNo1<ITape<Real>, Matrix4x4<Real>, Real, Index<Upper, Delta>> metric = new();
        var position = _tape.CreateAutoDiffTensor<Index<Upper, Delta>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,,])values[0];

        DifGeo.Christoffel(_tape, metric, position, out Christoffel<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Beta, Gamma> result);
        var actual = new Real[4, 4, 4];
        result.CopyTo(ref actual);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetChristoffelSymbolData()
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
}
