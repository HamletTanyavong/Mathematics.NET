// <copyright file="Assert.cs" company="Mathematics.NET">
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

using CommunityToolkit.HighPerformance;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.Tests;

/// <summary>Assert helpers for Mathemtics.NET.</summary>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
public sealed class Assert<TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
{
    /// <summary>Assert that two values are approximately equal.</summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual(TN expected, TN actual, Real epsilon)
    {
        if (TN.IsNaN(expected) && TN.IsNaN(actual) || TN.IsInfinity(expected) && TN.IsInfinity(actual))
            return;

        if (!Precision.AreApproximatelyEqual(expected, actual, epsilon))
        {
            Assert.Fail($$"""
                Actual value does not fall within the specifed margin of error, {{epsilon}}:

                Expected: {{expected}}
                Actual: {{actual}}
                """);
        }
    }

    /// <summary>Assert that the elements of two vectors are approximately equal.</summary>
    /// <typeparam name="TV">A vector.</typeparam>
    /// <param name="expected">The expected vector.</param>
    /// <param name="actual">The actual vector.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual<TV>(IVector<TV, TN> expected, IVector<TV, TN> actual, Real epsilon)
        where TV : IVector<TV, TN>
    {
        for (int i = 0; i < TV.E1Components; i++)
        {
            if (!Precision.AreApproximatelyEqual(expected[i], actual[i], epsilon))
            {
                Assert.Fail($$"""
                    Actual value at index {{i}} does not fall within the specified margin of error, {{epsilon}}

                    Expected: {{expected[i]}}
                    Actual: {{actual[i]}}
                    """);
            }
        }
    }

    /// <summary>Assert that the elements of two rank-one tensors are approximately equal.</summary>
    /// <typeparam name="TR1T">A rank-one tensor.</typeparam>
    /// <typeparam name="TV">The backing vector of the tensor.</typeparam>
    /// <typeparam name="TI">An index.</typeparam>
    /// <param name="expected">The expected tensor.</param>
    /// <param name="actual">The actual tensor.</param>
    /// <param name="epsilon">The margin of error.</param>
    public static void AreApproximatelyEqual<TR1T, TV, TI>(
        IRankOneTensor<TR1T, TV, TN, TI> expected,
        IRankOneTensor<TR1T, TV, TN, TI> actual,
        Real epsilon)
        where TR1T : IRankOneTensor<TR1T, TV, TN, TI>
        where TV : IVector<TV, TN>
        where TI : IIndex
    {
        for (int i = 0; i < TR1T.E1Components; i++)
        {
            if (!Precision.AreApproximatelyEqual(expected[i], actual[i], epsilon))
            {
                Assert.Fail($$"""
                    Actual value at index {{i}} does not fall within the specified margin of error, {{epsilon}}

                    Expected: {{expected[i]}}
                    Actual: {{actual[i]}}
                    """);
            }
        }
    }

    /// <summary>Assert that the elements of two read-only spans are approximately equal.</summary>
    /// <param name="expected">A read-only span of expected values.</param>
    /// <param name="actual">A read-only span of actual values.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual(ReadOnlySpan<TN> expected, ReadOnlySpan<TN> actual, Real epsilon)
    {
        if (expected.Length != actual.Length)
            Assert.Fail($"Length of the actual array, {actual.Length}, does not match the length of the expected array, {expected.Length}");

        for (int i = 0; i < expected.Length; i++)
        {
            if (TN.IsNaN(expected[i]) && TN.IsNaN(actual[i]) || TN.IsInfinity(expected[i]) && TN.IsInfinity(actual[i]))
                continue;

            if (!Precision.AreApproximatelyEqual(expected[i], actual[i], epsilon))
            {
                Assert.Fail($$"""
                    Actual value at index {{i}} does not fall within the specified margin of error, {{epsilon}}

                    Expected: {{expected[i]}}
                    Actual: {{actual[i]}}
                    """);
            }
        }
    }

    /// <summary>Assert that the elements of two matrices are approximately equal.</summary>
    /// <typeparam name="TM">A matrix.</typeparam>
    /// <param name="expected">The expected matrix.</param>
    /// <param name="actual">The actual matrix.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual<TM>(IMatrix<TM, TN> expected, IMatrix<TM, TN> actual, Real epsilon)
        where TM : IMatrix<TM, TN>
    {
        for (int i = 0; i < TM.E1Components; i++)
        {
            for (int j = 0; j < TM.E2Components; j++)
            {
                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], epsilon))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specified margin of error, {{epsilon}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }

    /// <summary>Assert that the elements of two rank-two tensors are approximately equal.</summary>
    /// <typeparam name="TR2T">A rank-two tensor.</typeparam>
    /// <typeparam name="TSM">The backing matrix of the tensor.</typeparam>
    /// <typeparam name="TI1">The first index.</typeparam>
    /// <typeparam name="TI2">The second index.</typeparam>
    /// <param name="expected">The expected rank-two tensor.</param>
    /// <param name="actual">The actual rank-two tensor.</param>
    /// <param name="epsilon">The margin of error.</param>
    public static void AreApproximatelyEqual<TR2T, TSM, TI1, TI2>(
        IRankTwoTensor<TR2T, TSM, TN, TI1, TI2> expected,
        IRankTwoTensor<TR2T, TSM, TN, TI1, TI2> actual,
        Real epsilon)
        where TR2T : IRankTwoTensor<TR2T, TSM, TN, TI1, TI2>
        where TSM : ISquareMatrix<TSM, TN>
        where TI1 : IIndex
        where TI2 : IIndex
    {
        for (int i = 0; i < TSM.E1Components; i++)
        {
            for (int j = 0; j < TSM.E2Components; j++)
            {
                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], epsilon))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specified margin of error, {{epsilon}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }

    /// <summary>Assert that the elements of two 2D, read-only spans are approximately equal.</summary>
    /// <param name="expected">A 2D, read-only span of expected values.</param>
    /// <param name="actual">A 2D, read-only span of actual values.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual(ReadOnlySpan2D<TN> expected, ReadOnlySpan2D<TN> actual, Real epsilon)
    {
        if (expected.Height != actual.Height || expected.Width != actual.Width)
            Assert.Fail($"Dimensions of the actual matrix, [{actual.Height}, {actual.Width}], does not match the dimensions of the expected matrix, [{expected.Height}, {expected.Width}]");

        for (int i = 0; i < expected.Height; i++)
        {
            for (int j = 0; j < expected.Width; j++)
            {
                if (TN.IsNaN(expected[i, j]) && TN.IsNaN(actual[i, j]) || TN.IsInfinity(expected[i, j]) && TN.IsInfinity(actual[i, j]))
                    continue;

                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], epsilon))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specified margin of error, {{epsilon}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }

    /// <summary>Assert that the elements of two 3D arrays are approximately equal.</summary>
    /// <param name="expected">A 3D array of expected values.</param>
    /// <param name="actual">A 3D array of actual values.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual(TN[,,] expected, TN[,,] actual, Real epsilon)
    {
        if (expected.GetLength(0) != actual.GetLength(0) || expected.GetLength(1) != actual.GetLength(1) || expected.GetLength(2) != actual.GetLength(2))
            Assert.Fail($"Dimensions of the actual array, [{actual.GetLength(0)}, {actual.GetLength(1)}, {actual.GetLength(2)}], does not match the dimensions of the expected matrix, [{expected.GetLength(0)}, {expected.GetLength(1)}, {expected.GetLength(2)}]");

        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                for (int k = 0; k < expected.GetLength(2); k++)
                {
                    if (TN.IsNaN(expected[i, j, k]) && TN.IsNaN(actual[i, j, k]) || TN.IsInfinity(expected[i, j, k]) && TN.IsInfinity(actual[i, j, k]))
                        continue;

                    if (!Precision.AreApproximatelyEqual(expected[i, j, k], actual[i, j, k], epsilon))
                    {
                        Assert.Fail($$"""
                            Actual value at location [{{i}}, {{j}}, {{k}}] does not fall within the specified margin of error, {{epsilon}}:

                            Expected: {{expected[i, j, k]}}
                            Actual: {{actual[i, j, k]}}
                            """);
                    }
                }
            }
        }
    }

    /// <summary>Assert that the elements of two 4D arrays are approximately equal.</summary>
    /// <param name="expected">A 4D array of expected values.</param>
    /// <param name="actual">A 4D array of actual values.</param>
    /// <param name="epsilon">A margin of error.</param>
    public static void AreApproximatelyEqual(TN[,,,] expected, TN[,,,] actual, Real epsilon)
    {
        if (expected.GetLength(0) != actual.GetLength(0) ||
            expected.GetLength(1) != actual.GetLength(1) ||
            expected.GetLength(2) != actual.GetLength(2) ||
            expected.GetLength(3) != actual.GetLength(3))
        {
            Assert.Fail($"Dimensions of the actual array, [{actual.GetLength(0)}, {actual.GetLength(1)}, {actual.GetLength(2)}, {actual.GetLength(3)}], does not match the dimensions of the expected matrix, [{expected.GetLength(0)}, {expected.GetLength(1)}, {expected.GetLength(2)}, {expected.GetLength(3)}]");
        }

        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                for (int k = 0; k < expected.GetLength(2); k++)
                {
                    for (int l = 0; l < expected.GetLength(3); l++)
                    {
                        if (TN.IsNaN(expected[i, j, k, l]) && TN.IsNaN(actual[i, j, k, l]) || TN.IsInfinity(expected[i, j, k, l]) && TN.IsInfinity(actual[i, j, k, l]))
                            continue;

                        if (!Precision.AreApproximatelyEqual(expected[i, j, k, l], actual[i, j, k, l], epsilon))
                        {
                            Assert.Fail($$"""
                                Actual value at location [{{i}}, {{j}}, {{k}}, {{l}}] does not fall within the specified margin of error, {{epsilon}}:

                                Expected: {{expected[i, j, k, l]}}
                                Actual: {{actual[i, j, k, l]}}
                                """);
                        }
                    }
                }
            }
        }
    }
}
