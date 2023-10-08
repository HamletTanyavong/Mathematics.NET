// <copyright file="DerivativesBuilder.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.SourceGenerators.SourceBuilders;

/// <summary>Derivatives builder</summary>
public sealed class DerivativesBuilder
{
    private readonly string _assemblyName;
    private readonly ImmutableArray<MethodInformation> _methodInformation;

    public DerivativesBuilder(Compilation compilationUnitSyntax, ImmutableArray<MethodInformation> methodInformation)
    {
        _assemblyName = compilationUnitSyntax.AssemblyName!;
        _methodInformation = methodInformation;
    }

    public CompilationUnitSyntax GenerateSource()
    {
        var members = GenerateMembers();
        return CreateCompilationUnit(members);
    }

    private CompilationUnitSyntax CreateCompilationUnit(MemberDeclarationSyntax[] memberDeclarations)
    {
        return CompilationUnit()
            .WithUsings(
            SingletonList(
                UsingDirective(
                    QualifiedName(
                        QualifiedName(
                            IdentifierName("Mathematics"),
                            IdentifierName("NET")),
                        IdentifierName("Core")))
                .WithUsingKeyword(
                    Token(
                        TriviaList(
                            Comment("// Auto-generated code")),
                        SyntaxKind.UsingKeyword,
                        TriviaList()))))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName(_assemblyName),
                                IdentifierName("Generated")),
                            IdentifierName("Mathematics")))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration("Equations")
                            .WithModifiers(
                                TokenList(new[] {
                                    Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.StaticKeyword) }))
                            .WithMembers(
                                List(memberDeclarations))))))
            .NormalizeWhitespace();
    }

    private MemberDeclarationSyntax[] GenerateMembers()
    {
        var result = new MemberDeclarationSyntax[_methodInformation.Length];
        for (int i = 0; i < _methodInformation.Length; i++)
        {
            var equation = _methodInformation[i].MethodDeclaration.RemoveEquationAttribute();
            var transformedEquation = SymbolicsHelper.TransformEquation(equation);
            result[i] = transformedEquation;
        }
        return result;
    }
}
