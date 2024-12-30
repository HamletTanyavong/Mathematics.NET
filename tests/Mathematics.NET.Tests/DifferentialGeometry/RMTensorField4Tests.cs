// <copyright file="RMTensorField4Tests.cs" company="Mathematics.NET">
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
public sealed class RMTensorField4Tests
{
    public static RMTensorField4<HessianTape<Real>, Real, Upper, Index<Upper, Alpha>> Tensor { get; set; } = new();

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Tensor[0] = (tape, x) => tape.Sin(tape.Add(x.X0, x.X1));
        Tensor[1] = (tape, x) => tape.Cos(tape.Add(x.X1, x.X2));
        Tensor[2] = (tape, x) => tape.Exp(tape.Add(x.X2, x.X3));
        Tensor[3] = (tape, x) => tape.Sqrt(tape.Add(x.X3, x.X0));
    }

    public RMTensorField4Tests()
    {
        Tape = new();
    }

    public HessianTape<Real> Tape { get; set; }

    //
    // Tests
    //

    [TestMethod]
    [DataRow(1.23, 2.34, 3.45, 4.56, -0.4154226067712459, 0.880829296973609, 3010.9171128823823, 2.406241883103193)]
    public void Compute_Variable_ReturnsValue(double x0, double x1, double x2, double x3, double expectedX0, double expectedX1, double expectedX2, double expectedX3)
    {
        var x = Tape.CreateAutoDiffTensor<Index<Upper, Alpha>>(x0, x1, x2, x3);

        Tensor<Vector4<Real>, Real, Index<Upper, Alpha>> expected = new(new Vector4<Real>(expectedX0, expectedX1, expectedX2, expectedX3));

        var actual = Tensor.Compute<Alpha>(Tape, x);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }
}
