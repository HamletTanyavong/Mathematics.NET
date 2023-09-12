﻿// <copyright file="ComplexTests.cs" company="Mathematics.NET">
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
    [DataRow(1.2, 2.3, 1.2, -2.3)]
    public void Conjugate_ComplexOfDouble_ReturnsConjugate(double inReal, double inImaginary, double outReal, double outImaginary)
    {
        Complex<double> input = new(inReal, inImaginary);
        Complex<double> expected = new(outReal, outImaginary);

        var actual = Complex<double>.Conjugate(input);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(3, 4, 5)]
    public void Magnitude_ComplexOfDouble_ReturnsMagnitude(double inReal, double inImaginary, double expected)
    {
        Complex<double> z = new(inReal, inImaginary);

        var actual = z.Magnitude.Value;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1, 1, 0, Math.PI / 2)]
    [DataRow(1, -1, -Math.PI / 2, 0)]
    [DataRow(-1, 1, Math.PI / 2, Math.PI)]
    [DataRow(-1, -1, -Math.PI, -Math.PI / 2)]
    public void Phase_ComplexOfDouble_ReturnsAngleInCorrectQuadrant(double inReal, double inImaginary, double expectedMin, double expectedMax)
    {
        Complex<double> z = new(inReal, inImaginary);

        var actual = z.Phase.Value;

        Assert.IsTrue(expectedMin <= actual && actual <= expectedMax);
    }

    [TestMethod]
    [DataRow(1.23, 4.567, null, "(1.23, 4.567)")]
    [DataRow(24.56, 9.23, "ALL", "(24.56, 9.23)")]
    [DataRow(62.151, 27, "RE", "62.151")]
    [DataRow(7.345, 124.841, "IM", "124.841")]
    public void ToString_ComplexOfDouble_ReturnsString(double inReal, double inImaginary, string? format, string expected)
    {
        Complex<double> z = new(inReal, inImaginary);

        var actual = z.ToString(format, null);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1.2, 2.3, 10, null, "(1.2, 2.3)")]
    [DataRow(1.23, 3.4567, 14, "ALL", "(1.23, 3.4567)")]
    [DataRow(4.537, 2.3, 5, "RE", "4.537")]
    [DataRow(1.2, 7, 1, "IM", "7")]
    public void TryFormat_ComplexOfDouble_ReturnsSpanOfCharacters(double inReal, double inImaginary, int length, string? format, string expected)
    {
        Complex<double> z = new(inReal, inImaginary);

        Span<char> actual = new char[length];
        _ = z.TryFormat(actual, out int _, format, null);

        CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());
    }
}
