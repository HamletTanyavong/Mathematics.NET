// <copyright file="Matrix4x4Tests.cs" company="Mathematics.NET">
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

using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Tests.LinearAlgebra;

[TestClass]
[TestCategory("Linear Algebra")]
public sealed class Matrix4x4Tests
{
    // Inputs

    public static IEnumerable<object[]> GetRealInputAndInverseData()
    {
        yield return new[]
        {
            new Real[4, 4]
            {
                { 1, 2, 4, 8 },
                { -1, 7, 3, 5 },
                { 2, -9, 13, 11 },
                { -7, -2, 3, 5 }
            },
            new Real[4, 4]
            {
                { 0.08856088560885609, -0.04551045510455105, 0.01107011070110701, -0.1205412054120541 },
                { -0.05904059040590406, 0.1414514145141451, -0.007380073800738007, -0.03075030750307503 },
                { -0.2832103321033210, 0.2253997539975400, 0.1208487084870849, -0.03813038130381304 },
                { 0.2702952029520295, -0.1423739237392374, -0.05996309963099631, 0.04182041820418204 }
            }
        };
    }

    // Tests

    [TestMethod]
    [DynamicData(nameof(GetRealInputAndInverseData), DynamicDataSourceType.Method)]
    public void Inverse_MatrixOfReal_ReturnsInverseOfMatrix(Real[,] input, Real[,] expected)
        => Inverse_Helper_MatrixOfGeneric_ReturnsInverseOfMatrix(input, expected);

    public static void Inverse_Helper_MatrixOfGeneric_ReturnsInverseOfMatrix<T>(T[,] inputArray, T[,] expectedArray)
        where T : IComplex<T>
    {
        Matrix4x4<T> expected = new(
            expectedArray[0, 0], expectedArray[0, 1], expectedArray[0, 2], expectedArray[0, 3],
            expectedArray[1, 0], expectedArray[1, 1], expectedArray[1, 2], expectedArray[1, 3],
            expectedArray[2, 0], expectedArray[2, 1], expectedArray[2, 2], expectedArray[2, 3],
            expectedArray[3, 0], expectedArray[3, 1], expectedArray[3, 2], expectedArray[3, 3]);

        Matrix4x4<T> input = new(
            inputArray[0, 0], inputArray[0, 1], inputArray[0, 2], inputArray[0, 3],
            inputArray[1, 0], inputArray[1, 1], inputArray[1, 2], inputArray[1, 3],
            inputArray[2, 0], inputArray[2, 1], inputArray[2, 2], inputArray[2, 3],
            inputArray[3, 0], inputArray[3, 1], inputArray[3, 2], inputArray[3, 3]);

        var actual = input.Inverse();

        Assert<T>.AreApproximatelyEqual(expected, actual, 1e-15);
    }
}
