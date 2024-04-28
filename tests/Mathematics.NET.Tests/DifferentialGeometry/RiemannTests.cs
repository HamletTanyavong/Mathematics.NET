// <copyright file="RiemannTests.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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
[TestCategory("DifGeo"), TestCategory("Riemann")]
public sealed class RiemannTests
{
    private HessianTape<Real> _tape;

    public RiemannTests()
    {
        _tape = new();
    }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetRiemannTensorData), DynamicDataSourceType.Method)]
    public void Riemann_FromMetricTensor_ReturnsRiemannTensor(object[] input, object[] values)
    {
        DifGeoTestHelpers.Test2x2MetricTensorFieldNo1<HessianTape<Real>, Real, Index<Upper, Epsilon>> metric = new();
        var point = _tape.CreateAutoDiffTensor<Index<Upper, Epsilon>>((double)input[0], (double)input[1]);

        var expected = (Real[,,,])values[0];

        DifGeo.Riemann(_tape, metric, point, out Tensor<Array2x2x2x2<Real>, Real, Index<Upper, Alpha>, Index<Lower, Beta>, Index<Lower, Gamma>, Index<Lower, Delta>> result);
        var actual = new Real[2, 2, 2, 2];
        result.CopyTo(ref actual);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetRiemannTensorData()
    {
        yield return new[]
        {
            [1.0, 2.0],
            new object[]
            {
                new Real[,,,]
                {
                    {
                        {
                            { 0,     -0.125 },
                            { 0.125, 0      },
                        },
                        {
                            { 0,    -0.25 },
                            { 0.25, 0     }
                        },
                    },
                    {
                        {
                            { 0,      -0.0625 },
                            { 0.0625, 0       },
                        },
                        {
                            { 0,      0.125 },
                            { -0.125, 0     },
                        }
                    }
                }
            }
        };
    }
}
