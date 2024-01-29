// <copyright file="TensorSelfContractionBuilder.cs" company="Mathematics.NET">
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
using Mathematics.NET.SourceGenerators.DifferentialGeometry.Models;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Tensor self contractions builder</summary>
internal sealed class TensorSelfContractionBuilder
{
    private static readonly GenericNameSyntax s_leftIndex = GenericName(
        Identifier("Index"))
            .WithTypeArgumentList(
                TypeArgumentList(
                    SeparatedList<TypeSyntax>(
                        new SyntaxNodeOrToken[] {
                            IdentifierName("Lower"),
                            Token(SyntaxKind.CommaToken),
                            IdentifierName("IC") })));

    private static readonly GenericNameSyntax s_rightIndex = GenericName(
        Identifier("Index"))
            .WithTypeArgumentList(
                TypeArgumentList(
                    SeparatedList<TypeSyntax>(
                        new SyntaxNodeOrToken[] {
                            IdentifierName("Upper"),
                            Token(SyntaxKind.CommaToken),
                            IdentifierName("IC") })));

    private readonly SourceProductionContext _context;
    private readonly ImmutableArray<MethodInformation> _methodInformationArray;

    public TensorSelfContractionBuilder(SourceProductionContext context, ImmutableArray<MethodInformation> methodInformationArray)
    {
        _context = context;
        _methodInformationArray = methodInformationArray;
    }

    public CompilationUnitSyntax GenerateSource()
    {
        var members = GenerateMembers();
        return CreateCompilationUnit(members);
    }

    //
    // Compilation unit and members
    //

    private CompilationUnitSyntax CreateCompilationUnit(MemberDeclarationSyntax[] memberDeclarations)
    {
        return CompilationUnit()
            .WithUsings(
                List([
                    UsingDirective(
                        QualifiedName(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("Mathematics"),
                                    IdentifierName("NET")),
                                IdentifierName("DifferentialGeometry")),
                            IdentifierName("Abstractions")))
                        .WithUsingKeyword(
                            Token(
                                TriviaList(
                                    Comment("// Auto-generated code")),
                                SyntaxKind.UsingKeyword,
                                TriviaList())),
                    UsingDirective(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Mathematics"),
                                IdentifierName("NET")),
                            IdentifierName("LinearAlgebra"))),
                    UsingDirective(
                        QualifiedName(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("Mathematics"),
                                    IdentifierName("NET")),
                                IdentifierName("LinearAlgebra")),
                            IdentifierName("Abstractions"))),
                    UsingDirective(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Mathematics"),
                                IdentifierName("NET")),
                            IdentifierName("Symbols")))]))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        FileScopedNamespaceDeclaration(
                            IdentifierName("Mathematics.NET.DifferentialGeometry"))
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
            var method = _methodInformationArray[i].MethodDeclaration;
            method = method.RemoveAttribute("GenerateTensorSelfContractions");

            // Generate twin of the original contraction.
            result.Add(method.GenerateTwinContraction());

            // Generate contractions with swapped parameters.
            var rank = GetTensorRank(method);
            MemberDeclarationSyntax member = method;

            for (int j = rank - 2; j > 0; j--)
            {
                member = SwapIndices(member);
                result.Add(member);
                result.Add(member.GenerateTwinContraction());
            }

            // Generate the remaining contractions.
            for (int j = rank - 2; j > 0; j--)
            {
                member = ResetIndices(member);
                result.Add(member);
                result.Add(member.GenerateTwinContraction());

                for (int k = j - 1; k > 0; k--)
                {
                    member = SwapIndices(member);
                    result.Add(member);
                    result.Add(member.GenerateTwinContraction());
                }
            }
        }
        return result.ToArray();
    }

    //
    // Helpers
    //

    private static int GetTensorRank(MemberDeclarationSyntax memberDeclaration)
    {
        var paramList = memberDeclaration.ParameterList()!;
        return paramList.Parameters[0].TypeArgumentList()!.Arguments.Count - 3;
    }

    private static MemberDeclarationSyntax ResetIndices(MemberDeclarationSyntax memberDeclaration)
    {
        memberDeclaration = ResetTypeParameters(memberDeclaration);
        memberDeclaration = ResetTypeParameterConstraints(memberDeclaration);
        memberDeclaration = ResetElementAccessComponents(memberDeclaration);

        return memberDeclaration;
    }

    private static MemberDeclarationSyntax ResetElementAccessComponents(MemberDeclarationSyntax memberDeclaration)
    {
        var addAssignmentExpression = memberDeclaration
            .DescendantNodes()
            .OfType<AssignmentExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.AddAssignmentExpression));

        // This gets the first enclosing for loop.
        var forStatement = (ForStatementSyntax)addAssignmentExpression.Parent!.Parent!.Parent!;
        var variableName = forStatement.Declaration!.Variables[0].Identifier.Text;

        var args = addAssignmentExpression.Right.ChildNodes().OfType<BracketedArgumentListSyntax>().First();

        var leftIndex = args.Arguments.First(x => x.Expression is IdentifierNameSyntax name && name.Identifier.Text == variableName);
        var index = args.Arguments.IndexOf(leftIndex);

        var newArgs = args.RemoveNode(args.Arguments.ElementAt(index), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index], [Argument(IdentifierName(variableName))]);

        newArgs = newArgs.RemoveNode(newArgs.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index + 1], [Argument(IdentifierName(variableName))]);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax ResetTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration)
    {
        var constraints = memberDeclaration
            .ChildNodes()
            .OfType<TypeParameterConstraintClauseSyntax>()
            .First();
        var args = constraints
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();

        var leftIndex = args.Arguments.First(x => x is GenericNameSyntax name && name.Identifier.Text == "Index");
        var index = args.Arguments.IndexOf(leftIndex);

        var newArgs = args.RemoveNode(args.Arguments.ElementAt(index), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index], [s_leftIndex]);

        newArgs = newArgs.RemoveNode(newArgs.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index + 1], [s_rightIndex]);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax ResetTypeParameters(MemberDeclarationSyntax memberDeclaration)
    {
        var param = memberDeclaration.ParameterList()!.Parameters[0];
        var args = param.TypeArgumentList()!;

        var leftIndex = args.Arguments.First(x => x is GenericNameSyntax name && name.Identifier.Text == "Index");
        var index = args.Arguments.IndexOf(leftIndex);

        var newArgs = args.RemoveNode(args.Arguments.ElementAt(index), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index], [s_leftIndex]);

        newArgs = newArgs.RemoveNode(newArgs.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[index + 1], [s_rightIndex]);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax SwapIndices(MemberDeclarationSyntax memberDeclaration)
    {
        var indexStructure = memberDeclaration.GetIndexStructure(0);

        memberDeclaration = SwapTypeParameters(memberDeclaration, indexStructure);
        memberDeclaration = SwapTypeParameterConstraints(memberDeclaration, indexStructure);
        memberDeclaration = SwapElementAccessComponents(memberDeclaration);

        return memberDeclaration;
    }

    private static MemberDeclarationSyntax SwapElementAccessComponents(MemberDeclarationSyntax memberDeclaration)
    {
        var addAssignmentExpression = memberDeclaration
            .DescendantNodes()
            .OfType<AssignmentExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.AddAssignmentExpression));

        // This gets the first enclosing for loop.
        var forStatement = (ForStatementSyntax)addAssignmentExpression.Parent!.Parent!.Parent!;
        var variableName = forStatement.Declaration!.Variables[0].Identifier.Text;

        var args = addAssignmentExpression.Right.ChildNodes().OfType<BracketedArgumentListSyntax>().First();
        var indexSwapper = new IndexSwapRewriter(args, variableName);
        var newArgs = (BracketedArgumentListSyntax)indexSwapper.Visit(args);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax SwapTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration, IndexStructure indexStructure)
    {
        var constraints = memberDeclaration
            .ChildNodes()
            .OfType<TypeParameterConstraintClauseSyntax>()
            .First();
        var args = constraints
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();

        var newArgs = args.SwapCurrentIndexWithNextIndex(indexStructure.ContractPosition);
        var newConstraints = constraints.ReplaceNode(args, newArgs);

        return memberDeclaration.ReplaceNode(constraints, newConstraints);
    }

    private static MemberDeclarationSyntax SwapTypeParameters(MemberDeclarationSyntax memberDeclaration, IndexStructure indexStructure)
    {
        var param = memberDeclaration.ParameterList()!.Parameters[0];
        var args = param.TypeArgumentList()!;
        var newParam = param.ReplaceNode(args, args.SwapCurrentIndexWithNextIndex(indexStructure.ContractPosition));
        return memberDeclaration.ReplaceNode(param, newParam);
    }
}
