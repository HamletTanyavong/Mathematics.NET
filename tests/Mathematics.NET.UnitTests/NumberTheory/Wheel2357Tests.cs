// <copyright file="Wheel2357Tests.cs" company="Mathematics.NET">
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
[TestCategory("Prime")]
public sealed class Wheel2357Tests
{
    public Wheel2357 Wheel { get; } = new();

    [TestMethod]
    public void Basis_Property_HasCorrectBasis()
    {
        List<int> expected = [2, 3, 5, 7];

        var actual = Wheel.Basis;

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(GetSequenceData))]
    public void Indexer_LimitGreaterThanWheelSize_ReturnsCorrectSequence(int _, int count, int[] expected)
    {
        Wheel2357 wheel = new();

        List<int> actual = [];
        for (int i = 0; i < count; i++)
        {
            actual.Add(wheel[i]);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(GetSequenceData))]
    public void Spin_LimitGreaterThanWheelSize_ReturnsCorrectSequence(int limit, int _, int[] expected)
    {
        Wheel2357 wheel = new();

        var actual = wheel.Spin().TakeWhile(x => x < limit).ToArray();

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Increments_Property_HasCorrectIncrements()
    {
        List<int> expected = [2, 4, 2, 4, 6, 2, 6, 4, 2, 4, 6, 6, 2, 6, 4, 2, 6, 4, 6, 8, 4, 2, 4, 2, 4, 8, 6, 4, 6, 2, 4, 6, 2, 6, 6, 4, 2, 4, 6, 2, 6, 4, 2, 4, 2, 10, 2, 10];

        var actual = Wheel.Increments;

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Spokes_Property_HasCorrectSpokes()
    {
        List<int> expected = [11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 121, 127, 131, 137, 139, 143, 149, 151, 157, 163, 167, 169, 173, 179, 181, 187, 191, 193, 197, 199, 209, 211, 221];

        var actual = Wheel.Spokes;

        CollectionAssert.AreEqual(expected, actual);
    }

    //
    // Helpers
    //

    public static IEnumerable<(int Limit, int Count, int[] Values)> GetSequenceData()
    {
        yield return new(512, 116,
        [
            11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 121, 127, 131, 137, 139, 143, 149, 151, 157, 163, 167, 169, 173, 179, 181, 187, 191, 193, 197, 199, 209, 211, 221, 223, 227, 229, 233, 239, 241, 247, 251, 253, 257, 263, 269, 271, 277, 281, 283, 289, 293, 299, 307, 311, 313, 317, 319, 323, 331, 337, 341, 347, 349, 353, 359, 361, 367, 373, 377, 379, 383, 389, 391, 397, 401, 403, 407, 409, 419, 421, 431, 433, 437, 439, 443, 449, 451, 457, 461, 463, 467, 473, 479, 481, 487, 491, 493, 499, 503, 509
        ]);
    }
}
