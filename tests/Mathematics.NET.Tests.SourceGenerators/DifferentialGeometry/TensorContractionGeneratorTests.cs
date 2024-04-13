// <copyright file="TensorContractionGeneratorTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.SourceGenerators.DifferentialGeometry;
using Microsoft.CodeAnalysis.CSharp;

namespace Mathematics.NET.Tests.SourceGenerators.DifferentialGeometry;

[TestClass]
[TestCategory("Source Generator"), TestCategory("DifGeo")]
public sealed class TensorContractionGeneratorTests : VerifyBase
{
    [TestMethod]
    public void SourceGenerator_RankThreeTensors_GeneratesContractions()
    {
        var source = """
            namespace TestNamespace;

            [GenerateTensorContractions]
            public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(
                in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a,
                in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> b)
                where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2>
                where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4>
                where TN : IComplex<TN>, IDifferentiableFunctions<TN>
                where TCI : ISymbol
                where TI1 : IIndex
                where TI2 : IIndex
                where TI3 : IIndex
                where TI4 : IIndex
            {
                Array4x4x4x4<TN> array = new();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                for (int m = 0; m < 4; m++)
                                {
                                    array[i, j, k, l] += a[m, i, j] * b[m, k, l];
                                }
                            }
                        }
                    }
                }
                return new(array);
            }
            """;

        SetupAndVerify(source);
    }

    //
    // Helpers
    //

    public Task SetupAndVerify(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [syntaxTree]);

        var generator = new TensorContractionGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

        return Verify(driver)
            .UseDirectory("Snapshots");
    }
}
