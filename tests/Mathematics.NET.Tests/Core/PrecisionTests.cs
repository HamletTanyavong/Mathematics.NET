// <copyright file="PrecisionTests.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Tests.Core;

[TestClass]
[TestCategory("Core"), TestCategory("Precision")]
public sealed class PrecisionTests
{
    [TestMethod]
    [DataRow(1e-13, 1.0000000000000002e-13, Precision.DblEpsilonVariant, true)]
    [DataRow(1e-13, 1.0000000000000004e-13, Precision.DblEpsilonVariant, false)]
    [DataRow(1.0, 1.0000000000000002, Precision.DblEpsilonVariant, true)]
    [DataRow(1.0, 1.0000000000000004, Precision.DblEpsilonVariant, false)]
    [DataRow(1.23e27, 1.2300000000000002e27, Precision.DblEpsilonVariant, true)]
    [DataRow(1.23e27, 1.2300000000000004e27, Precision.DblEpsilonVariant, false)]
    public void AreApproximatelyEqual_TwoDoublePrecisionFloatingPointNumbers_ReturnsBool(double left, double right, double epsilon, bool expected)
    {
        var actual = Precision.AreApproximatelyEqual(left, right, epsilon);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1e-13, 1.0000000000000002e-13, Precision.DblEpsilonVariant, true)]
    [DataRow(1e-13, 1.0000000000000004e-13, Precision.DblEpsilonVariant, false)]
    [DataRow(1.0, 1.0000000000000002, Precision.DblEpsilonVariant, true)]
    [DataRow(1.0, 1.0000000000000004, Precision.DblEpsilonVariant, false)]
    [DataRow(1.23e27, 1.2300000000000002e27, Precision.DblEpsilonVariant, true)]
    [DataRow(1.23e27, 1.2300000000000004e27, Precision.DblEpsilonVariant, false)]
    public void AreEssentiallyEqual_TwoDoublePrecisionFloatingPointNumbers_ReturnsBool(double left, double right, double epsilon, bool expected)
    {
        var actual = Precision.AreEssentiallyEqual(left, right, epsilon);

        Assert.AreEqual(expected, actual);
    }
}
