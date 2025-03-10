// <copyright file="LinAlgTests.cs" company="Mathematics.NET">
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
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Tests.LinearAlgebra;

[TestClass]
[TestCategory("Linear Algebra")]
public sealed class LinAlgTests
{
    // Inputs

    public static IEnumerable<object[]> GetComplexInputData()
    {
        yield return new[]
        {
            new Complex[2, 2]
            {
                { new(1, 2), new(2, 3) },
                { new(1, -2), new(3, 5) }
            }
        };
    }

    public static IEnumerable<object[]> GetRealInputData()
    {
        yield return new[]
        {
            new Real[3, 3]
            {
                { -1, 2.424, 3 },
                { 4, 5, 6.74241 },
                { 7, 8, 9.3 }
            }
        };
    }

    // Tests

    [TestMethod]
    [DynamicData(nameof(GetComplexInputData), DynamicDataSourceType.Method)]
    public void QRGramSchmidt_MatrixOfComplex_ReturnsQRDecompositionOfMatrix(Complex[,] matrix)
        => QRGramSchmidt_Helper_MatrixOfGeneric_ReturnsQRDecompositionOfMatrix(matrix);

    [TestMethod]
    [DynamicData(nameof(GetRealInputData), DynamicDataSourceType.Method)]
    public void QRGramSchmidt_MatrixOfReal_ReturnsQRDecompositionOfMatrix(Real[,] matrix)
        => QRGramSchmidt_Helper_MatrixOfGeneric_ReturnsQRDecompositionOfMatrix(matrix);

    public static void QRGramSchmidt_Helper_MatrixOfGeneric_ReturnsQRDecompositionOfMatrix<T>(T[,] matrix)
        where T : IComplex<T>, IDifferentiableFunctions<T>
    {
        var expected = matrix.AsSpan2D();
        LinAlg.QRGramSchmidt(expected, out var Q, out var R);

        var actual = LinAlg.MatMul(Q, R);

        Assert<T>.AreApproximatelyEqual(expected, actual, 1e-15);
    }
}
