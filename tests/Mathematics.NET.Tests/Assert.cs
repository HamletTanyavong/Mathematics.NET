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

namespace Mathematics.NET.Tests;

public sealed class Assert<T>
    where T : IComplex<T>
{
    public static void AreApproximatelyEqual(T expected, T actual, Real delta)
    {
        if (T.IsNaN(expected) && T.IsNaN(actual) || T.IsInfinity(expected) && T.IsInfinity(actual))
        {
            return;
        }

        if (!Precision.AreApproximatelyEqual(expected, actual, delta))
        {
            Assert.Fail($$"""
                Actual value does not fall within the specifed margin of error, {{delta}}:

                Expected: {{expected}}
                Actual: {{actual}}
                """);
        }
    }

    public static void ElementsAreApproximatelyEqual(Span2D<T> expected, Span2D<T> actual, Real delta)
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

                if (!Precision.AreApproximatelyEqual(expected[i, j], actual[i, j], delta))
                {
                    Assert.Fail($$"""
                        Actual value at row {{i}} and column {{j}} does not fall within the specifed margin of error, {{delta}}:

                        Expected: {{expected[i, j]}}
                        Actual: {{actual[i, j]}}
                        """);
                }
            }
        }
    }
}
