// <copyright file="ComplexTests.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Tests.Core;

[TestClass]
[TestCategory("Core"), TestCategory("Complex Number")]
public sealed class ComplexTests
{
    [TestMethod]
    [DataRow(1, 2, 1, -2)]
    public void Conjugate_ComplexOfInt32_ReturnsConjugate(int inReal, int inImaginary, int outReal, int outImaginary)
    {
        Complex<int> input = new(inReal, inImaginary);
        Complex<int> expected = new(outReal, outImaginary);

        var actual = Complex<int>.Conjugate(input);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1.2, 2.3, 1.2, -2.3)]
    public void Conjugate_ComplexOfDouble_ReturnsConjugate(double inReal, double inImaginary, double outReal, double outImaginary)
    {
        Complex<double> input = new(inReal, inImaginary);
        Complex<double> expected = new(outReal, outImaginary);

        var actual = Complex<double>.Conjugate(input);

        Assert.AreEqual(expected, actual);
    }
}
