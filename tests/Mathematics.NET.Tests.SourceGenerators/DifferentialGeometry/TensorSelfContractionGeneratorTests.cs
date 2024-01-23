// <copyright file="TensorSelfContractionGeneratorTests.cs" company="Mathematics.NET">
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
public sealed class TensorSelfContractionGeneratorTests : VerifyBase
{
    [TestMethod]
    public void SourceGenerator_RankThreeTensor_GeneratesSelfContractions()
    {
        var source = """
            namespace TestNamespace;

            [GenerateTensorSelfContractions]
            public static RankOneTensor<Vector4<U>, U, I> Contract<T, U, IC, I>(IRankThreeTensor<T, Array4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I> a)
                where T : IRankThreeTensor<T, Array4x4x4<U>, U, Index<Lower, IC>, Index<Upper, IC>, I>
                where U : IComplex<U>, IDifferentiableFunctions<U>
                where IC : ISymbol
                where I : IIndex
            {
                Vector4<U> vector = new();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        vector[i] += a[j, j, i];
                    }
                }
                return new(vector);
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

        var generator = new TensorSelfContractionGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

        return Verify(driver)
            .UseDirectory("Snapshots");
    }
}
