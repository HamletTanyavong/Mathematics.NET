// <copyright file="PrimeTests.cs" company="Mathematics.NET">
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

#pragma warning disable NUM0001

using Mathematics.NET.NumberTheory;

namespace Mathematics.NET.UnitTests.NumberTheory;

[TestClass]
[TestCategory("Prime")]
public sealed class PrimeTests
{
    [TestMethod]
    [DataRow(100, 97)]
    [DataRow(1000, 997)]
    [DataRow(25623, 25621)]
    [DataRow(25621, 25621)]
    [DataRow(1000000, 999983)]
    public void SieveOfEratosthenes_WithLimit_HasCorrectLargestPrimeUnderLimit(int limit, int expected)
    {
        Wheel2357 wheel = new();

        var actual = Prime
            .SieveOfEratosthenes(wheel, limit)
            .ToList()
            .Last();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(100, 97)]
    [DataRow(1000, 997)]
    [DataRow(25623, 25621)]
    [DataRow(25621, 25621)]
    [DataRow(1000000, 999983)]
    public void SieveOfEratosthenes_UsingPriorityQueue_HasCorrectLargestPrimeUnderLimit(int limit, int expected)
    {
        Wheel2357 wheel = new();

        var actual = Prime
            .SieveOfEratosthenes(wheel)
            .TakeWhile(x => x <= limit)
            .LastOrDefault();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(100, 25)]
    [DataRow(1000, 168)]
    [DataRow(25623, 2822)]
    [DataRow(25621, 2822)]
    [DataRow(1000000, 78498)]
    //[DataRow(100000000, 5761455)]
    //[DataRow(1000000000, 50847534)]
    //[DataRow(int.MaxValue, 105097565)]
    public void SieveOfEratosthenes_WithLimit_HasCorrectPrimeCount(int limit, int expected)
    {
        Wheel2357 wheel = new();

        var actual = Prime
            .SieveOfEratosthenes(wheel, limit)
            .Count();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(100, 25)]
    [DataRow(1000, 168)]
    [DataRow(25623, 2822)]
    [DataRow(25621, 2822)]
    [DataRow(1000000, 78498)]
    //[DataRow(100000000, 5761455)]
    //[DataRow(1000000000, 50847534)]
    //[DataRow(int.MaxValue, 105097565)]
    public void SieveOfEratosthenes_UsingPriorityQueue_HasCorrectPrimeCount(int limit, int expected)
    {
        Wheel2357 wheel = new();

        var actual = Prime
            .SieveOfEratosthenes(wheel)
            .TakeWhile(x => x <= limit)
            .Count();

        Assert.AreEqual(expected, actual);
    }
}
