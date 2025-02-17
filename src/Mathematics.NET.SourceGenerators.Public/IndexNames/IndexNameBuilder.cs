// <copyright file="IndexNameBuilder.cs" company="Mathematics.NET">
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

using System.Collections.Immutable;
using Mathematics.NET.SourceGenerators.Public.Models;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.Public.IndexNames;

/// <summary>Index name builder.</summary>
internal sealed class IndexNameBuilder
{
    private readonly NameSyntax _namespaceNameSyntax;
    private readonly ImmutableArray<StructInformation> _structInformationArray;

    public IndexNameBuilder(NameSyntax namespaceNameSyntax, ImmutableArray<StructInformation> structInformationArray)
    {
        _namespaceNameSyntax = namespaceNameSyntax;
        _structInformationArray = structInformationArray;
    }

    public CompilationUnitSyntax GenerateSource()
    {
        var members = GenerateMembers();
        return CreateCompilationUnit(members);
    }

    //
    // Compilation unit and members
    //

    private CompilationUnitSyntax CreateCompilationUnit(ImmutableArray<MemberDeclarationSyntax> memberDeclarations)
    {
        return CompilationUnit()
            .WithUsings(
                List([
                    UsingDirective("Mathematics.NET.DifferentialGeometry".CreateNameSyntaxFromNamespace())
                        .WithUsingKeyword(
                            Token(
                                TriviaList(
                                    Comment("// Auto-generated code"),
                                    CarriageReturnLineFeed,
                                    CarriageReturnLineFeed),
                                SyntaxKind.UsingKeyword,
                                TriviaList(Space)))
                        .WithSemicolonToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.SemicolonToken,
                                TriviaList(
                                    CarriageReturnLineFeed,
                                    CarriageReturnLineFeed)))]))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(_namespaceNameSyntax)
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList(Space)))
                        .WithSemicolonToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.SemicolonToken,
                                TriviaList(
                                    CarriageReturnLineFeed,
                                    CarriageReturnLineFeed)))
                        .WithMembers(
                            List(memberDeclarations))));
    }

    private ImmutableArray<MemberDeclarationSyntax> GenerateMembers()
    {
        List<MemberDeclarationSyntax> members = [];
        for (int i = 0; i < _structInformationArray.Length; i++)
        {
            members.Add(GenerateIndexName(_structInformationArray[i].StructDeclarationSyntax, i == _structInformationArray.Length - 1));
        }
        return members.ToImmutableArray();
    }

    //
    // Helpers
    //

    private StructDeclarationSyntax GenerateIndexName(StructDeclarationSyntax structDeclaration, bool isLastIndex)
    {
        var indexName = structDeclaration.Identifier.Text;
        return StructDeclaration(
                Identifier(
                    TriviaList(),
                    indexName,
                    TriviaList(Space)))
            .WithKeyword(
                Token(
                    TriviaList(),
                    SyntaxKind.StructKeyword,
                    TriviaList(Space)))
            .WithModifiers(
                TokenList([
                    Token(
                        TriviaList(),
                        SyntaxKind.PublicKeyword,
                        TriviaList(Space)),
                    Token(
                        TriviaList(),
                        SyntaxKind.ReadOnlyKeyword,
                        TriviaList(Space)),
                    Token(
                        TriviaList(),
                        SyntaxKind.PartialKeyword,
                        TriviaList(Space))]))
            .WithBaseList(
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(
                            IdentifierName(
                                Identifier(
                                    TriviaList(),
                                    "IIndexName",
                                    TriviaList(CarriageReturnLineFeed))))))
                    .WithColonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.ColonToken,
                            TriviaList(Space))))
            .WithOpenAndCloseBraceTokens(string.Empty, !isLastIndex)
            .WithMembers(
                List(new MemberDeclarationSyntax[] {
                    GenerateDisplayStringField(indexName),
                    GenerateDisplayStringProperty() }));
    }

    private FieldDeclarationSyntax GenerateDisplayStringField(string indexName)
    {
        return FieldDeclaration(
            VariableDeclaration(
                PredefinedType(
                    Token(
                        TriviaList(),
                        SyntaxKind.StringKeyword,
                        TriviaList(Space))))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier(
                                TriviaList(),
                                "DisplayString",
                                TriviaList(Space)))
                        .WithInitializer(
                            EqualsValueClause(
                                LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    Literal(indexName)))
                            .WithEqualsToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.EqualsToken,
                                    TriviaList(Space)))))))
                .WithModifiers(
                    TokenList(
                        Token(
                            TriviaList(
                                Whitespace("    "),
                                Trivia(GenerateDocumentationComment()),
                                Whitespace("    ")),
                            SyntaxKind.PublicKeyword,
                            TriviaList(Space)),
                        Token(
                            TriviaList(),
                            SyntaxKind.ConstKeyword,
                            TriviaList(Space))))
                .WithSemicolonToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.SemicolonToken,
                        TriviaList(
                            CarriageReturnLineFeed)));
    }

    private PropertyDeclarationSyntax GenerateDisplayStringProperty()
    {
        return PropertyDeclaration(
                PredefinedType(
                    Token(
                        TriviaList(),
                        SyntaxKind.StringKeyword,
                        TriviaList(Space))),
                Identifier(
                    TriviaList(),
                    "DisplayString",
                    TriviaList(Space)))
            .WithModifiers(
                TokenList(
                    Token(
                        TriviaList(
                            Whitespace("    ")),
                        SyntaxKind.StaticKeyword,
                        TriviaList(Space))))
            .WithExplicitInterfaceSpecifier(
                ExplicitInterfaceSpecifier(
                    IdentifierName("IIndexName")))
            .WithExpressionBody(
                ArrowExpressionClause(
                    IdentifierName("DisplayString"))
                .WithArrowToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.EqualsGreaterThanToken,
                        TriviaList(Space))))
            .WithSemicolonToken(
                Token(
                    TriviaList(),
                    SyntaxKind.SemicolonToken,
                    TriviaList(CarriageReturnLineFeed)));
    }

    private DocumentationCommentTriviaSyntax GenerateDocumentationComment()
    {
        return DocumentationCommentTrivia(
            SyntaxKind.SingleLineDocumentationCommentTrivia,
            List(new XmlNodeSyntax[] {
                XmlText()
                    .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior("///")),
                                    " ",
                                    string.Empty,
                                    TriviaList()))),
                XmlNullKeywordElement()
                    .WithName(
                        XmlName(
                            Identifier("inheritdoc")))
                    .WithAttributes(
                        SingletonList<XmlAttributeSyntax>(
                            XmlCrefAttribute(
                                QualifiedCref(
                                    IdentifierName("IIndexName"),
                                    NameMemberCref(
                                        IdentifierName("DisplayString")))))),
                XmlText()
                    .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                "\r\n",
                                "\r\n",
                                TriviaList()))) }));
    }
}
