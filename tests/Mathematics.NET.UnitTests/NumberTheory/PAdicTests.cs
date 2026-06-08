// <copyright file="PAdicTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.NumberTheory;

namespace Mathematics.NET.UnitTests.NumberTheory;

[TestClass]
[TestCategory("PAdic")]
public sealed class PAdicTests
{
    [TestMethod]
    [DataRow(2, 8, 1)]
    [DataRow(3, 57, 3)]
    [DataRow(5, 18, 6)]
    public void DigitSum_PositiveInteger_ReturnsDigitSum(int p, int n, int expected)
    {
        var actual = PAdic.DigitSum(p, n);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(2, 10, 3, 3)]
    [DataRow(3, 230, 16, 2)]
    [DataRow(5, 312, 13, 2)]
    public void Kummer_Binomial_ReturnsValuation(int p, int n, int k, int expected)
    {
        var actual = PAdic.Kummer(p, n, k);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(GetMultinomialData))]
    public void Kummer_Multinomial_ReturnsValuation(int p, int[] k, int expected)
    {
        var actual = PAdic.Kummer(p, k);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(2, 6, 4)]
    [DataRow(3, 6, 2)]
    [DataRow(5, 6, 1)]
    public void Legendre_Factorial_ReturnsValuation(int p, int input, int expected)
    {
        var actual = PAdic.Legendre(p, input);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(3, 140, 297, 27, 1)]
    [DataRow(5, 140, 297, 1, 5)]
    public void Norm_RationalNumber_ReturnsPAdicNorm(int p, int num, int den, int expectedNum, int expectedDen)
    {
        Rational<int, double> input = new(num, den);

        Rational<int, double> expected = new(expectedNum, expectedDen);

        var actual = PAdic.Norm(p, input);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(2, -12, 2)]
    [DataRow(3, -12, 1)]
    [DataRow(5, -12, 0)]
    public void Valuation_Integer_ReturnsValuation(int p, int input, int expected)
    {
        var actual = PAdic.Valuation(p, input);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(2, 9, 8, -3)]
    [DataRow(3, 9, 8, 2)]
    public void Valuation_Rational_ReturnsValuation(int p, int num, int den, int expected)
    {
        Rational<int, double> input = new(num, den);

        var actual = PAdic.Valuation(p, input);

        Assert.AreEqual(expected, actual);
    }

    //
    // Helpers
    //

    public static IEnumerable<(int Prime, int[] K, int Expected)> GetMultinomialData()
    {
        yield return new(5, [3, 7, 4, 2], 2);
    }
}
