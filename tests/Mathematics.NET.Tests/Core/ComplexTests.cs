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
    [DataRow(1.23, 2.34, 1.114564084931578, -1.686112230652994)]
    public void Acos_ComplexOfDouble_ReturnsComplexOfDouble(double inReal, double inImaginary, double expectedRe, double expectedIm)
    {
        Complex<double> input = new(inReal, inImaginary);

        var actualResult = Complex<double>.Acos(input);
        var actualRe = actualResult.Re.Value;
        var actualIm = actualResult.Im.Value;

        Assert.AreEqual(expectedRe, actualRe, 1e-15);
        Assert.AreEqual(expectedIm, actualIm, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 4.562322418633185e-1, 1.686112230652994)]
    public void Asin_ComplexOfDouble_ReturnsComplexOfDouble(double inReal, double inImaginary, double expectedRe, double expectedIm)
    {
        Complex<double> input = new(inReal, inImaginary);

        var actualResult = Complex<double>.Asin(input);
        var actualRe = actualResult.Re.Value;
        var actualIm = actualResult.Im.Value;

        Assert.AreEqual(expectedRe, actualRe, 1e-15);
        Assert.AreEqual(expectedIm, actualIm, 1e-15);
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

    [TestMethod]
    [DataRow(1, 2, 2, 1, 0.8, 0.6)]
    [DataRow(1, 2, 3, 4, 0.44, 0.08)]
    [DataRow(2, 1.5, 5, 3, 0.4264705882352942, 0.04411764705882351)]
    [DataRow(5, 3.5, 7, 0, 0.7142857142857142, 0.5)]
    public void Division_ComplexOfDoubles_ReturnsComplexOfDouble(double dividendRe, double dividendIm, double divisorRe, double divisorIm, double expectedRe, double expectedIm)
    {
        Complex<double> dividend = new(dividendRe, dividendIm);
        Complex<double> divisor = new(divisorRe, divisorIm);

        var actualResult = dividend / divisor;
        var actualRe = actualResult.Re.Value;
        var actualIm = actualResult.Im.Value;

        Assert.AreEqual(expectedRe, actualRe, 1e-15);
        Assert.AreEqual(expectedIm, actualIm, 1e-15);
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
    [DataRow(2, 0, 0.5, 0)]
    [DataRow(1.5, 2.5, 1.76470588235294117e-1, -2.94117647058823529e-1)]
    public void Reciprocate_ComplexOfDouble_ReturnsReciprocal(double inReal, double inImaginary, double expectedRe, double expectedIm)
    {
        Complex<double> z = new(inReal, inImaginary);
        Complex<double> expected = new(expectedRe, expectedIm);

        var actual = Complex<double>.Reciprocate(z);

        Assert.AreEqual(expected, actual);
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
    [DataRow(0, 0, 6)]
    [DataRow(1.23, 2.34, 12)]
    public void TryFormat_ComplexOfDoubleWithAdequateDestinationLength_ReturnsTrue(double inReal, double inImaginary, int length)
    {
        Complex<double> z = new(inReal, inImaginary);

        Span<char> span = new char[length];
        var actual = z.TryFormat(span, out int _, null, null);

        Assert.IsTrue(actual);
    }

    [TestMethod]
    [DataRow(0, 0, 5)]
    [DataRow(1.23, 2.34, 11)]
    public void TryFormat_ComplexOfDoubleWithInadequateDestinationLength_ReturnsFalse(double inReal, double inImaginary, int length)
    {
        Complex<double> z = new(inReal, inImaginary);

        Span<char> span = new char[length];
        var actual = z.TryFormat(span, out int _, null, null);

        Assert.IsFalse(actual);
    }

    [TestMethod]
    [DataRow("(0,0)", 0, 0)]
    [DataRow(" (0, 0) ", 0, 0)]
    [DataRow("(1.23, 3.456)", 1.23, 3.456)]
    [DataRow("( 1.23 , 3.45 )", 1.23, 3.45)]
    public void TryParse_SpanOfChar_ReturnsComplexOfDouble(string s, double expectedRe, double expectedIm)
    {
        Complex<double> expected = new(expectedRe, expectedIm);

        _ = Complex<double>.TryParse(s.AsSpan(), null, out Complex<double> actual);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("(,)")]
    [DataRow("(0,,0)")]
    [DataRow("0,0)")]
    [DataRow("(0,0")]
    public void TryParse_SpanOfChar_ReturnsFalse(string s)
    {
        var actual = Complex<double>.TryParse(s.AsSpan(), null, out _);

        Assert.IsFalse(actual);
    }

    [TestMethod]
    [DataRow(0, 0, 6, 6)]
    [DataRow(1.23, 45.678, 14, 14)]
    [DataRow(1.23, 45.678, 13, 13)]
    [DataRow(1.23, 45.678, 9, 7)]
    [DataRow(1.23, 45.678, 6, 6)]
    [DataRow(1.23, 45.678, 1, 0)]
    [DataRow(0, 1.2345, 6, 4)]
    [DataRow(1.2345, 0, 6, 1)]
    [DataRow(1.2345, 0, 6, 1)]
    [DataRow(1.2345, 0, 7, 7)]
    [DataRow(1.2345, 0, 8, 8)]
    public void TryFormat_ComplexOfDoubleWithInadequateDestinationLength_ReturnsCorrectNumberOfCharsWritten(double inReal, double inImaginary, int length, int expected)
    {
        Complex<double> z = new(inReal, inImaginary);

        Span<char> span = new char[length];
        _ = z.TryFormat(span, out int actual, null, null);

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
