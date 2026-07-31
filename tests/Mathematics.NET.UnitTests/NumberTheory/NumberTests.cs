// <copyright file="NumberTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.Exceptions;
using Mathematics.NET.NumberTheory;

namespace Mathematics.NET.UnitTests.NumberTheory;

[TestClass]
[TestCategory("Number Theory")]
public sealed class NumberTests
{
    [TestMethod]
    [DataRow(1, 3, 3)]
    [DataRow(2, 8, 1)]
    [DataRow(2, 5244, 7)]
    [DataRow(3, -57, -3)]
    [DataRow(5, 18, 6)]
    [DataRow(10, -1263387, -30)]
    public void DigitSum_Int_ReturnsDigitSum(int b, int n, int expected)
    {
        var actual = Number.DigitSum(b, n);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-2, 13)]
    [DataRow(0, 1)]
    public void DigitSum_NegativeBase_ThrowsMathematicsException(int b, int n)
    {
        var exception = Assert.ThrowsExactly<MathematicsException>(() => Number.DigitSum(b, n), "The base must be greater than zero.");
        Assert.AreEqual("The base must be greater than zero.", exception.Message);
    }

    [TestMethod]
    [DynamicData(nameof(GetDivisorData))]
    public void Divisor_Int_ReturnsDivisors(int n, int[] expected)
    {
        var actual = Number.Divisors(n);

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(21654, 16)]
    [DataRow(126843214, 8)]
    [DataRow(185714347800, 288)]
    public void DivisorCount_Long_ReturnsNumberOfDivisors(long n, long expected)
    {
        var actual = Number.DivisorCount(n);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(9, 6)]
    [DataRow(20, 8)]
    [DataRow(24, 8)]
    [DataRow(29, 28)]
    public void EulerTotientFunction_Int_ReturnsNumberofPositiveIntegersRelativelyPrimeToInput(int n, int expected)
    {
        var actual = Number.EulerTotientFunction(n);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-14, -286, 2)]
    [DataRow(-28, 612, 4)]
    [DataRow(124, -48, 4)]
    [DataRow(38, 26, 2)]
    public void GCD_ExtendedOfInt_ReturnsGCD(int a, int b, int expected)
    {
        var actual = Number.GCD(a, b, out var _, out var _);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-132, -58, 11, -25)]
    [DataRow(-56, 133, 7, 3)]
    [DataRow(60, -14, 3, 13)]
    [DataRow(44, 360, 41, -5)]
    public void GCD_ExtendedOfInt_ReturnsCoefficients(int a, int b, int expectedX, int expectedY)
    {
        _ = Number.GCD(a, b, out var actualX, out var actualY);

        Assert.AreEqual(expectedX, actualX);
        Assert.AreEqual(expectedY, actualY);
    }

    //
    // Helpers
    //

    public static IEnumerable<(int N, int[] Divisors)> GetDivisorData()
    {
        yield return new(136, [1, 2, 4, 8, 17, 34, 68, 136]);
        yield return new(245068, [1, 2, 4, 197, 311, 394, 622, 788, 1244, 61267, 122534, 245068]);
    }
}
