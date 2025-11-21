// <copyright file="TensorCalculusTests.cs" company="Mathematics.NET">
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
[TestCategory("DifGeo"), TestCategory("Tensor Calculus")]
public sealed class TensorCalculusTests
{
    public static RMMetricTensorField4x4<GradientTape<Real>, Real, Index<Upper, PIN>> Metric { get; set; } = new();
    public static RMTensorField4<GradientTape<Real>, Real, Upper, Index<Upper, PIN>> Tensor { get; set; } = new();

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Metric[0, 0] = (tape, x) => tape.Sin(x.X0);
        Metric[0, 1] = (tape, x) => tape.Multiply(2, tape.Cos(x.X1));
        Metric[0, 2] = (tape, x) => tape.Multiply(3, tape.Exp(x.X2));
        Metric[0, 3] = (tape, x) => tape.Multiply(4, tape.Ln(x.X3));

        Metric[1, 0] = (tape, x) => tape.Multiply(5, tape.Sqrt(x.X1));
        Metric[1, 1] = (tape, x) => tape.Multiply(6, tape.Tan(x.X2));
        Metric[1, 2] = (tape, x) => tape.Multiply(7, tape.Sinh(x.X3));
        Metric[1, 3] = (tape, x) => tape.Multiply(8, tape.Cosh(x.X0));

        Metric[2, 0] = (tape, x) => tape.Multiply(9, tape.Tanh(x.X2));
        Metric[2, 1] = (tape, x) => tape.Divide(10, x.X3);
        Metric[2, 2] = (tape, x) => tape.Multiply(11, tape.Pow(x.X0, 2));
        Metric[2, 3] = (tape, x) => tape.Multiply(12, tape.Multiply(tape.Sin(x.X1), tape.Cos(x.X1)));

        Metric[3, 0] = (tape, x) => tape.Multiply(13, tape.Multiply(tape.Exp(x.X3), tape.Sin(x.X0)));
        Metric[3, 1] = (tape, x) => tape.Multiply(14, tape.Divide(x.X3, x.X1));
        Metric[3, 2] = (tape, x) => tape.Multiply(15, tape.Sin(tape.Subtract(x.X3, tape.Multiply(2, x.X2))));
        Metric[3, 3] = (tape, x) => tape.Multiply(16, tape.Multiply(x.X0, tape.Multiply(x.X1, tape.Multiply(x.X2, x.X3))));

        Tensor[0] = (tape, x) => tape.Multiply(x.X0, tape.Add(tape.Sin(x.X1), x.X2));
        Tensor[1] = (tape, x) => tape.Add(tape.Cos(tape.Multiply(x.X2, x.X3)), x.X1);
        Tensor[2] = (tape, x) => tape.Divide(tape.Subtract(x.X1, x.X0), tape.Add(x.X2, x.X3));
        Tensor[3] = (tape, x) => tape.Ln(tape.Multiply(x.X1, x.X3));
    }

    public TensorCalculusTests()
    {
        Tape = new();
    }

    public GradientTape<Real> Tape { get; set; }

    //
    // Tests
    //

    [TestMethod]
    [DynamicData(nameof(GetCovariantDerivativeData))]
    public void CovariantDerivative_WithMetricTensorAndRankOneTensor_ReturnsCovariantDerivative(object[] input, object[] values)
    {
        var point = Tape.CreateAutoDiffTensor<Index<Upper, PIN>>((double)input[0], (double)input[1], (double)input[2], (double)input[3]);

        var expected = (Real[,])values[0];

        DifGeo.CovariantDerivative(Tape, Metric, Tensor, point, out Tensor<Matrix4x4<Real>, Real, Index<Lower, PIN>, Index<Upper, Alpha>> derivative);
        var actual = derivative.ToArray();

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-14);
    }

    //
    // Helpers
    //

    public static IEnumerable<object[]> GetCovariantDerivativeData()
    {
        yield return new[]
        {
            [1.23, 2.34, 3.45, 4.56],
            new object[]
            {
                new Real[,]
                {
                    { 5.289263063818893,   5.013308424588511,  -0.29788865000129444, 3.8105919037189384 },
                    { -2.4563414493568,    13.25416522566854,  -0.02204745753009399, 3.0134829061941315 },
                    { -3.522275180531237,  61.491449747524356, 0.6855961853286481,   5.711550823511512  },
                    { -151.09436038088194, 1159.2560019770624, -13.109910390357598,  203.26824800200063 }
                }
            }
        };
    }
}
