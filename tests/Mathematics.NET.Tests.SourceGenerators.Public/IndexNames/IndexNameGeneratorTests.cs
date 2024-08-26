// <copyright file="IndexNameGeneratorTests.cs" company="Mathematics.NET">
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

using Mathematics.NET.SourceGenerators.Public.IndexNames;
using Microsoft.CodeAnalysis.CSharp;

namespace Mathematics.NET.Tests.SourceGenerators.Public.IndexNames;

[TestClass]
[TestCategory("Source Generator"), TestCategory("DifGeo")]
public sealed class IndexNameGeneratorTests : VerifyBase
{
    [TestMethod]
    public void SourceGenerator_StructWithIndexNameAttribute_AutoImplementsIIndexName()
    {
        var source = """
            namespace A
            {
                [IndexName] public partial struct Alpha;
                [IndexName] public partial struct Beta;
            }

            namespace A
            {
                [IndexName] public partial struct Gamma;
            }

            namespace A.B
            {
                [IndexName] public partial struct Delta;
            }

            namespace B.C
            {
                [IndexName] public partial struct Epsilon;
            }

            namespace B.C.D
            {
                [IndexName] public partial struct Zeta;
                [IndexName] public partial struct Eta;
            }
            """;

        _ = SetupAndVerify(source);
    }

    [TestMethod]
    public void SourceGenerator_IndexNameNotDeclaredInANamespace_ReturnsAnError()
    {
        var source = """
            [IndexName] public partial struct Alpha;
            """;

        _ = SetupAndVerify(source);
    }

    public Task SetupAndVerify(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [syntaxTree]);

        var generator = new IndexNameGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

        return Verify(driver)
            .UseDirectory("Snapshots");
    }
}
