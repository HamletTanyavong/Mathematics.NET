// <copyright file="ModularTests.cs" company="Mathematics.NET">
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
[TestCategory("Modular Arithmetic")]
public sealed class ModularTests
{
    [TestMethod]
    [DataRow(2, 1, 0)]
    [DataRow(-128, 11, 3)]
    [DataRow(-34, 16, -1)]
    [DataRow(-23, 66, 43)]
    [DataRow(-13, 44, 27)]
    [DataRow(-4, 9, 2)]
    [DataRow(13, 7, 6)]
    [DataRow(22, 35, 8)]
    [DataRow(56, 23, 7)]
    public void Inverse_Int_ReturnsInverse(int a, int m, int expected)
    {
        var actual = Modular.Inverse(a, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-18, -26, 44, 21, 22)]
    [DataRow(-12, 7, 25, 14, 25)]
    [DataRow(12, 16, 20, 3, 5)]
    [DataRow(14, -12, 32, 6, 16)]
    [DataRow(16, 11, 35, 16, 35)]
    [DataRow(3, 1, 8, 3, 8)]
    [DataRow(2, 3, 7, 5, 7)]
    public void LinearCongruence_Int_ReturnsSolution(int a, int b, int m, int expectedX, int expectedM)
    {
        var (actualX, actualM) = Modular.LinearCongruence(a, b, m);

        Assert.AreEqual(expectedX, actualX);
        Assert.AreEqual(expectedM, actualM);
    }

    [TestMethod]
    [DataRow(2, 3, 4)]
    public void LinearCongruence_NoSolution_ThrowsException(int a, int b, int m)
    {
        var exception = Assert.ThrowsExactly<MathematicsException>(() => Modular.LinearCongruence(a, b, m));
        Assert.AreEqual($"Linear congruence has no solutions.", exception.Message);
    }

    [TestMethod]
    [DataRow(-12, 7, 2)]
    [DataRow(13, 5, 3)]
    public void Mod_Int_ReturnsMod(int x, int m, int expected)
    {
        var actual = Modular.Mod(x, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1, 2, 5, 3)]
    [DataRow(3, 5, 7, 2)]
    public void Mod_RationalOfInt_ReturnsMod(int n, int d, int m, int expected)
    {
        Rational<int, double> p = new(n, d);

        var actual = Modular.Mod(p, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(13, 45, 12)]
    [DataRow(16, 43, 7)]
    [DataRow(29, 3, 2)]
    public void MultiplicativeOrder_Int_ReturnsMultiplicativeOrder(int a, int m, int expected)
    {
        var actual = Modular.MultiplicativeOrder(a, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(3453, 1263, 12, 3)]
    [DataRow(-35234, 231, 67, 39)]
    [DataRow(321, -73, 16, 7)]
    [DataRow(-983, -7321, 23, 4)]
    public void Multiply_Int_ReturnsProduct(int a, int b, int m, int expected)
    {
        var actual = Modular.Multiply(a, b, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(-327, 631, 15, 12)]
    [DataRow(-34, 0, 23, 22)]
    [DataRow(0, 0, 57, 1)]
    [DataRow(0, 17, 13, 0)]
    [DataRow(2, -1, 7, 4)]
    [DataRow(3, -17, 10, 7)]
    [DataRow(8, 3, 4, 0)]
    [DataRow(12, -5, 7, 5)]
    [DataRow(13, 0, 11, 1)]
    [DataRow(3453, 23, 12, 9)]
    public void Pow_Int_ReturnsPower(int a, int x, int m, int expected)
    {
        var actual = Modular.Pow(a, x, m);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(27, -16, 9)]
    public void Pow_NoSolution_ThrowsException(int a, int x, int m)
    {
        var exception = Assert.ThrowsExactly<MathematicsException>(() => Modular.Pow(a, x, m));
        Assert.AreEqual($"Linear congruence has no solutions.", exception.Message);
    }
}
