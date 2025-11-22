// <copyright file="RationalTests.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.UnitTests.Core;

[TestClass]
[TestCategory("Core"), TestCategory("Rational Number")]
public sealed class RationalTests
{
    [TestMethod]
    [DataRow(2, 4, 5, 3, 13, 6)]
    public void Add_TwoRationalsOfInt_ReturnsReducedSum(int xNum, int xDen, int yNum, int yDen, int expectedNum, int expectedDen)
    {
        Rational<int> x = new(xNum, xDen);
        Rational<int> y = new(yNum, yDen);

        Rational<int> expected = new(expectedNum, expectedDen);

        var actual = x + y;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Divide_RationalOfIntByZero_ReturnsNaN()
    {
        var actual = Rational<int>.One / Rational<int>.Zero;

        Assert.AreEqual(Rational<int>.NaN, actual);
    }

    [TestMethod]
    [DataRow(2, 3, 4, 8, 1, 3)]
    public void Multiply_TwoRationalsOfInt_ReturnsReducedProduct(int xNum, int xDen, int yNum, int yDen, int expectedNum, int expectedDen)
    {
        Rational<int> x = new(xNum, xDen);
        Rational<int> y = new(yNum, yDen);

        Rational<int> expected = new(expectedNum, expectedDen);

        var actual = x * y;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(11, 5, 3, 4, 2, 14, 15)]
    public void QuoRem_TwoRationalsOfInt_ReturnsQuotientAndRemainder(int xNum, int xDen, int yNum, int yDen, int expectedQuo, int expectedRemNum, int expectedRemDen)
    {
        Rational<int> x = new(xNum, xDen);
        Rational<int> y = new(yNum, yDen);

        Rational<int> expectedRem = new(expectedRemNum, expectedRemDen);

        var (actualQuo, actualRem) = Rational<int>.QuoRem(x, y);

        Assert.AreEqual(expectedQuo, actualQuo);
        Assert.AreEqual(expectedRem, actualRem);
    }

    [TestMethod]
    [DataRow(6, 8, 3, 4)]
    public void Reduce_RationalOfInt_ReturnsReducedFraction(int inNum, int inDen, int expectedNum, int expectedDen)
    {
        Rational<int> p = new(inNum, inDen);
        Rational<int> expected = new(expectedNum, expectedDen);

        var actual = p.Reduce();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(3, 4, 7)]
    public void TryFormat_RationalOfIntWithAdequateDestinationLength_ReturnsTrue(int inNum, int inDen, int length)
    {
        Rational<int> p = new(inNum, inDen);

        Span<char> span = new char[length];
        var actual = p.TryFormat(span, out int _, null, null);

        Assert.IsTrue(actual);
    }
}
