// <copyright file="TensorContractionBuilder.cs" company="Mathematics.NET">
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

using System.Collections.Immutable;
using Mathematics.NET.SourceGenerators.Models;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Tensor contractions builder</summary>
public sealed class TensorContractionBuilder
{
    private readonly ImmutableArray<MethodInformation> _methodInformationArray;

    public TensorContractionBuilder(ImmutableArray<MethodInformation> methodInformationArray)
    {
        _methodInformationArray = methodInformationArray;
    }

    public CompilationUnitSyntax GenerateSource()
    {
        var members = GenerateMembers();
        return CreateCompilationUnit(members);
    }

    //
    // Helpers
    //

    public CompilationUnitSyntax CreateCompilationUnit(MemberDeclarationSyntax[] memberDeclarations)
    {
        return CompilationUnit()
            .WithUsings(
            List([
                UsingDirective(
                    IdentifierName("Mathematics.NET.Core"))
                .WithUsingKeyword(
                    Token(
                        TriviaList(
                            Comment("// Auto-generated code")),
                        SyntaxKind.UsingKeyword,
                        TriviaList())),
                UsingDirective(
                    IdentifierName("Mathematics.NET.DifferentialGeometry.Abstractions")),
                UsingDirective(
                    IdentifierName("Mathematics.NET.LinearAlgebra")),
                UsingDirective(
                    IdentifierName("Mathematics.NET.LinearAlgebra.Abstractions")),
                UsingDirective(
                    IdentifierName("Mathematics.NET.Symbols"))]))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Mathematics"),
                                IdentifierName("NET")),
                            IdentifierName("DifferentialGeometry")))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration("DifGeo")
                            .WithModifiers(
                                TokenList([
                                    Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.StaticKeyword),
                                    Token(SyntaxKind.PartialKeyword)]))
                            .WithMembers(
                                List(memberDeclarations))))))
            .NormalizeWhitespace();
    }

    private MemberDeclarationSyntax[] GenerateMembers()
    {
        List<MemberDeclarationSyntax> result = [];
        for (int i = 0; i < _methodInformationArray.Length; i++)
        {
            var method = _methodInformationArray[i].MethodDeclaration.RemoveAttribute("GenerateTensorContractions");
            result.Add(GenerateTwinContraction(method));
        }
        return result.ToArray();
    }

    // Generate the tensor contraction with index positions swapped: lower => upper and upper => lower.
    private MemberDeclarationSyntax GenerateTwinContraction(MemberDeclarationSyntax member)
    {
        FlipIndexRewriter walker = new();
        return (MemberDeclarationSyntax)walker.Visit(member);
    }
}
