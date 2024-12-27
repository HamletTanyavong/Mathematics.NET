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

namespace Mathematics.NET.Tests.DifferentialGeometry;

[TestClass]
[TestCategory("DifGeo")]
public sealed class DifGeoForwardModeTests
{
    public static FMTensorField4<HyperDual<Real>, Real, Upper, Index<Upper, PIN>> R1Tensor { get; set; } = new();

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        R1Tensor[0] = x => HyperDual<Real>.Sin(x.X0 + x.X1);
        R1Tensor[1] = x => HyperDual<Real>.Cos(x.X1 + x.X2);
        R1Tensor[2] = x => HyperDual<Real>.Exp(x.X2 + x.X3);
        R1Tensor[3] = x => HyperDual<Real>.Sqrt(x.X3 + x.X0);
    }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetR1TenserDerivativeData), DynamicDataSourceType.Method)]
    public void Derivative_RankOneTensor_ReturnsRankTwoTensor(object[] input, object[] values)
    {
        var point = HyperDual<Real>.CreateAutoDiffTensor<Index<Upper, PIN>>(1.23, 2.34, 3.45, 4.56);

        var expected = (Real[,])values[0];

        DifGeo.Derivative(R1Tensor, point, out Tensor<Matrix4x4<Real>, Real, Index<Lower, Alpha>, Index<Upper, Beta>> derivative);
        var actual = derivative.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetR1TenserDerivativeData()
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
}
