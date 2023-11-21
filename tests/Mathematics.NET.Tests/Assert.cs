// <copyright file="Assert.cs" company="Mathematics.NET">
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

using CommunityToolkit.HighPerformance;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.Tests;

/// <summary>Assert helpers for Mathemtics.NET</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
public sealed class Assert<T>
    where T : IComplex<T>
{
    /// <summary>Assert that two values are approximately equal.</summary>
    /// <param name="expected">The expected value</param>
    /// <param name="actual">The actual value</param>
    /// <param name="epsilon">A margin of error</param>
    public static void AreApproximatelyEqual(T expected, T actual, Real epsilon)
    {
        if (T.IsNaN(expected) && T.IsNaN(actual) || T.IsInfinity(expected) && T.IsInfinity(actual))
        {
            return;
        }

        if (!Precision.AreApproximatelyEqual(expected, actual, epsilon))
        {
            Assert.Fail($$"""
                Actual value does not fall within the specifed margin of error, {{epsilon}}:

                Expected: {{expected}}
                Actual: {{actual}}
                """);
        }
    }

    /// <summary>Assert that the elements in vector are approximately equal.</summary>
    /// <typeparam name="S">A vector</typeparam>
    /// <param name="expected">The expected vector</param>
    /// <param name="actual">The actual vector</param>
    /// <param name="epsilon">A margin of error</param>
    public static void AreApproximatelyEqual<S>(IVector<S, T> expected, IVector<S, T> actual, Real epsilon)
        where S : IVector<S, T>
    {
        for (int i = 0; i < S.E1Components; i++)
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

    /// <summary>Assert that the elements in two read-only spans are approximately equal.</summary>
    /// <param name="expected">A read-only span of expected values</param>
    /// <param name="actual">A read-only span of actual values</param>
    /// <param name="epsilon">A margin of error</param>
    public static void AreApproximatelyEqual(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual, Real epsilon)
    {
        if (expected.Length != actual.Length)
        {
            Assert.Fail($"Length of the actual array, {actual.Length}, does not match the length of the expected array, {expected.Length}");
        }

        for (int i = 0; i < expected.Length; i++)
        {
            if (T.IsNaN(expected[i]) && T.IsNaN(actual[i]) || T.IsInfinity(expected[i]) && T.IsInfinity(actual[i]))
            {
                continue;
            }

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

    /// <summary>Assert that the elements in a matrix are approximately equal.</summary>
    /// <typeparam name="S">A matrix</typeparam>
    /// <param name="expected">The expected matrix</param>
    /// <param name="actual">The actual matrix</param>
    /// <param name="epsilon">A margin of error</param>
    public static void AreApproximatelyEqual<S>(IMatrix<S, T> expected, IMatrix<S, T> actual, Real epsilon)
        where S : IMatrix<S, T>
    {
        for (int i = 0; i < S.E1Components; i++)
        {
            for (int j = 0; j < S.E2Components; j++)
            {
                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], epsilon))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specifed margin of error, {{epsilon}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }

    /// <summary>Assert that the elements in two 2D, read-only spans are approximately equal.</summary>
    /// <param name="expected">A 2D, read-only span of expected values</param>
    /// <param name="actual">A 2D, read-only span of actual values</param>
    /// <param name="epsilon">A margin of error</param>
    public static void AreApproximatelyEqual(ReadOnlySpan2D<T> expected, ReadOnlySpan2D<T> actual, Real epsilon)
    {
        if (expected.Height != actual.Height || expected.Width != actual.Width)
        {
            Assert.Fail($"Dimensions of the actual matrix, [{actual.Height}, {actual.Width}], does not match the dimensions of the expected matrix, [{expected.Height}, {expected.Width}]");
        }

        for (int i = 0; i < expected.Height; i++)
        {
            for (int j = 0; j < expected.Width; j++)
            {
                if (T.IsNaN(expected[i, j]) && T.IsNaN(actual[i, j]) || T.IsInfinity(expected[i, j]) && T.IsInfinity(actual[i, j]))
                {
                    continue;
                }

                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], epsilon))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specifed margin of error, {{epsilon}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }
}
