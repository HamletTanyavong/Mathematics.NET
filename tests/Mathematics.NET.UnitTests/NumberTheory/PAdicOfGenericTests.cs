// <copyright file="PAdicOfGenericTests.cs" company="Mathematics.NET">
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
public sealed class PAdicOfGenericTests
{
    [TestMethod]
    [DataRow(3, new int[] { 2, 0, 1, 2, 1 }, 1, 4, 3, 5)]
    [DataRow(3, new int[] { 2, 0, 0, 1, 1 }, 0, 5, -5, 11)]
    [DataRow(3, new int[] { 2, 0, 1, 2, 1 }, 0, 4, 1, 5)]
    [DataRow(3, new int[] { 2, 0, 1, 2, 1, 0 }, -1, 4, 1, 15)]
    [DataRow(5, new int[] { 4, 0, 4, 1, 1, 2 }, 0, 5, 19, 11)]
    [DataRow(9, new int[] { 3, 1, 5, 2 }, 1, 3, 27, 7)]
    [DataRow(9, new int[] { 3, 1, 5, 2 }, 2, 3, 243, 7)]
    public void ToRational_EventuallyRepeating_ReturnsCorrectRational(int p, int[] digits, int start, int period, int expectedNum, int expectedDen)
    {
        Rational<int, double> expected = new(expectedNum, expectedDen);
        PAdic<int> padic = new(p, digits, start, period);

        var actual = padic.ToRational<double>();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-5, 11, 3, "11002'₃")]
    [DataRow(-3, 14, 5, "101343'₅")]
    [DataRow(1, 5, 3, "1210'2₃")]
    [DataRow(1, 15, 3, "1210'2E-1₃")]
    [DataRow(3, 5, 3, "1210'2E+1₃")]
    [DataRow(19, 11, 5, "21140'4₅")]
    [DataRow(243, 7, 9, "251'3E+2₉")]
    [DataRow(243, 63, 9, "251'3E+1₉")]
    public void ToString_EventuallyRepeating_OutputsCorrectString(int n, int d, int p, string expected)
    {
        Rational<int, double> q = new(n, d);
        PAdic<int> padic = new(p, q);

        var actual = padic.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-5, 11, 3, "11002'₃")]
    [DataRow(-3, 14, 5, "101343'₅")]
    [DataRow(1, 5, 3, "1210'2₃")]
    [DataRow(1, 15, 3, "1210'2E-1₃")]
    [DataRow(3, 5, 3, "1210'2E+1₃")]
    [DataRow(19, 11, 5, "21140'4₅")]
    [DataRow(243, 7, 9, "251'3E+2₉")]
    [DataRow(243, 63, 9, "251'3E+1₉")]
    public void TryFormat_EventuallyRepeating_OutputsCorrectSpan(int n, int d, int p, string expected)
    {
        Rational<int, double> q = new(n, d);
        PAdic<int> padic = new(p, q);

        Span<char> span = new char[expected.Length];
        _ = padic.TryFormat(span, out _, null, null);

        var actual = string.Join("", span.ToArray());

        Assert.AreEqual(expected, actual);
    }
}
