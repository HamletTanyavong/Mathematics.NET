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
using System.Runtime.CompilerServices;
using Mathematics.NET.SourceGenerators.DifferentialGeometry.Models;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Tensor contractions builder</summary>
public sealed class TensorContractionBuilder
{
    private static readonly GenericNameSyntax s_indexToContract = GenericName(
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

    public TensorContractionBuilder(SourceProductionContext context, ImmutableArray<MethodInformation> methodInformationArray)
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
    // Helpers
    //

    public CompilationUnitSyntax CreateCompilationUnit(MemberDeclarationSyntax[] memberDeclarations)
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
            ValidateSeedContraction(method);
            method = method.RemoveAttribute("GenerateTensorContractions");

            // Generate the twin of the original contraction.
            result.Add(GenerateTwinContraction(method));

            // Generate contractions with swapped parameters.
            var contractionInformation = GetContractionInformation(method);
            MemberDeclarationSyntax member = method;

            for (int j = 0; j < contractionInformation.RightRank - 1; j++)
            {
                member = SwapRightIndices(member);
                result.Add(member);
                result.Add(GenerateTwinContraction(member));
            }
            member = ResetIndices(member);

            // Loop through the remaining index combinations and add the contractions.
            for (int j = 0; j < contractionInformation.LeftRank - 1; j++)
            {
                member = SwapLeftIndices(member);
                result.Add(member);
                result.Add(GenerateTwinContraction(member));
                for (int k = 0; k < contractionInformation.RightRank - 1; k++)
                {
                    member = SwapRightIndices(member);
                    result.Add(member);
                    result.Add(GenerateTwinContraction(member));
                }
                member = ResetIndices(member);
            }
        }
        return result.ToArray();
    }

    //
    // Validation
    //

    private void ValidateSeedContraction(MemberDeclarationSyntax memberDeclaration)
    {
        var paramList = GetParameterList(memberDeclaration);

        var leftParam = paramList.Parameters[0];
        var leftArgs = GetTypeArgumentList(leftParam);
        if (leftArgs.Arguments[3] is GenericNameSyntax leftName)
        {
            if (((IdentifierNameSyntax)leftName.TypeArgumentList.Arguments[0]).Identifier.Text != "Lower")
            {
                var descriptor = DifGeoDiagnostics.CreateIncorrectIndexPositionDescriptor("The index position of the first parameter must be \"Lower.\"");
                _context.ReportDiagnostic(Diagnostic.Create(descriptor, leftArgs.Arguments[3].GetLocation()));
            }
        }
        else
        {
            var descriptor = DifGeoDiagnostics.CreateIncorrectIndexDescriptor("The first index of the first parameter must be of type \"Index.\"");
            _context.ReportDiagnostic(Diagnostic.Create(descriptor, leftArgs.Arguments[3].GetLocation()));
        }

        var rightParam = paramList.Parameters[1];
        var rightArgs = GetTypeArgumentList(rightParam);
        if (rightArgs.Arguments[3] is GenericNameSyntax rightName)
        {
            if (((IdentifierNameSyntax)rightName.TypeArgumentList.Arguments[0]).Identifier.Text != "Upper")
            {
                var descriptor = DifGeoDiagnostics.CreateIncorrectIndexPositionDescriptor("The index position of the second parameter must be \"Upper.\"");
                _context.ReportDiagnostic(Diagnostic.Create(descriptor, rightArgs.Arguments[3].GetLocation()));
            }
        }
        else
        {
            var descriptor = DifGeoDiagnostics.CreateIncorrectIndexDescriptor("The first index of the second parameter must be of type \"Index.\"");
            _context.ReportDiagnostic(Diagnostic.Create(descriptor, rightArgs.Arguments[3].GetLocation()));
        }
    }

    //
    // Helpers
    //

    // Generate the tensor contraction with index positions swapped: lower => upper and upper => lower.
    private static MemberDeclarationSyntax GenerateTwinContraction(MemberDeclarationSyntax member)
    {
        FlipIndexRewriter walker = new();
        return (MemberDeclarationSyntax)walker.Visit(member);
    }

    private static ContractionInformation GetContractionInformation(MemberDeclarationSyntax memberDeclaration)
    {
        var paramList = GetParameterList(memberDeclaration);

        var leftParam = paramList.Parameters[0];
        var rightParam = paramList.Parameters[1];

        var leftRank = GetTypeArgumentList(leftParam).Arguments.Count - 3;
        var rightRank = GetTypeArgumentList(rightParam).Arguments.Count - 3;

        return new(leftRank, rightRank);
    }

    private static IndexStructure GetIndexStructure(MemberDeclarationSyntax memberDeclaration, Position position)
    {
        var paramList = GetParameterList(memberDeclaration).Parameters[(int)position];
        var args = GetTypeArgumentList(paramList).Arguments;
        var count = args.Count;
        var index = args.IndexOf(args.First(x => x is GenericNameSyntax name && name.Identifier.Text == "Index"));
        return new(index, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ParameterListSyntax GetParameterList(MemberDeclarationSyntax memberDeclaration)
    {
        return memberDeclaration
            .DescendantNodes()
            .OfType<ParameterListSyntax>()
            .First();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TypeArgumentListSyntax GetTypeArgumentList(ParameterSyntax parameter)
    {
        return parameter
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();
    }

    private static MemberDeclarationSyntax ResetIndices(MemberDeclarationSyntax memberDeclaration)
    {
        memberDeclaration = ResetTypeParameters(memberDeclaration);
        memberDeclaration = ResetTypeParameterConstraints(memberDeclaration);
        memberDeclaration = ResetMultiplyExpressionComponents(memberDeclaration);

        return memberDeclaration;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax ResetMultiplyExpressionComponents(MemberDeclarationSyntax memberDeclaration)
    {
        var multiplyExpression = memberDeclaration
            .DescendantNodes()
            .OfType<BinaryExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.MultiplyExpression));
        var args = multiplyExpression.Right
            .DescendantNodes()
            .OfType<BracketedArgumentListSyntax>()
            .First();

        var indexName = ((IdentifierNameSyntax)args.Arguments.Last().ChildNodes().First()).Identifier.Text;
        var newArgs = args!.InsertNodesBefore(args!.Arguments[0], [Argument(IdentifierName(indexName))]);
        newArgs = newArgs.RemoveNode(newArgs.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia)!;

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax ResetTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration)
    {
        var constraints = memberDeclaration
            .ChildNodes()
            .OfType<TypeParameterConstraintClauseSyntax>()
            .Skip(1)
            .First();
        var args = constraints
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();

        var newArgs = args.RemoveNode(args.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[2], [s_indexToContract]);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax ResetTypeParameters(MemberDeclarationSyntax memberDeclaration)
    {
        var param = GetParameterList(memberDeclaration).Parameters[1];
        var args = GetTypeArgumentList(param);

        var newArgs = args.RemoveNode(args.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[2], [s_indexToContract]);
        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TypeArgumentListSyntax SwapCurrentIndexWithNextIndex(TypeArgumentListSyntax typeArgumentListSyntax, int index)
    {
        var args = typeArgumentListSyntax.Arguments;
        var currentIndex = args[index];
        var nextIndex = args[index + 1];

        var newArgs = args.Replace(currentIndex, nextIndex);
        nextIndex = newArgs[index + 1];
        newArgs = newArgs.Replace(nextIndex, currentIndex);

        return TypeArgumentList(newArgs);
    }

    private static MemberDeclarationSyntax SwapIndices(MemberDeclarationSyntax memberDeclaration, Position position)
    {
        var indexStructure = GetIndexStructure(memberDeclaration, position);

        memberDeclaration = SwapTypeParameters(memberDeclaration, position, indexStructure);
        memberDeclaration = SwapTypeParameterConstraints(memberDeclaration, position, indexStructure);
        memberDeclaration = SwapMultiplyExpressionComponents(memberDeclaration, position);

        return memberDeclaration;
    }

    private static MemberDeclarationSyntax SwapLeftIndices(MemberDeclarationSyntax memberDeclaration)
        => SwapIndices(memberDeclaration, Position.Left);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax SwapMultiplyExpressionComponents(MemberDeclarationSyntax memberDeclaration, Position position)
    {
        var multiplyExpression = memberDeclaration
            .DescendantNodes()
            .OfType<BinaryExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.MultiplyExpression));

        // This gets the first enclosing for loop.
        var forStatement = (ForStatementSyntax)multiplyExpression.Parent!.Parent!.Parent!.Parent!;
        var variableName = forStatement.Declaration!.Variables[0].Identifier.Text;

        var args = position == Position.Left
            ? multiplyExpression.Left.DescendantNodes().OfType<BracketedArgumentListSyntax>().First()
            : multiplyExpression.Right.DescendantNodes().OfType<BracketedArgumentListSyntax>().First();
        var indexSwapper = new IndexSwapRewriter(args, variableName);
        var newArgs = (BracketedArgumentListSyntax)indexSwapper.Visit(args);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax SwapRightIndices(MemberDeclarationSyntax memberDeclaration)
        => SwapIndices(memberDeclaration, Position.Right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax SwapTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration, Position position, IndexStructure indexStructure)
    {
        var constraints = memberDeclaration
            .ChildNodes()
            .OfType<TypeParameterConstraintClauseSyntax>()
            .Skip((int)position)
            .First();
        var args = constraints
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();

        var newArgs = SwapCurrentIndexWithNextIndex(args, indexStructure.ContractPosition);
        var newConstraints = constraints.ReplaceNode(args, newArgs);

        return memberDeclaration.ReplaceNode(constraints, newConstraints);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MemberDeclarationSyntax SwapTypeParameters(MemberDeclarationSyntax memberDeclaration, Position position, IndexStructure indexStructure)
    {
        var param = GetParameterList(memberDeclaration).Parameters[(int)position];
        var args = GetTypeArgumentList(param);

        var newArgs = SwapCurrentIndexWithNextIndex(args, indexStructure.ContractPosition);
        var newParam = param.ReplaceNode(args, newArgs);

        return memberDeclaration.ReplaceNode(param, newParam);
    }
}
