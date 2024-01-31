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
using Mathematics.NET.SourceGenerators.DifferentialGeometry.Models;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

// The seed tensor contraction should have a lower index for the left argument and an upper
// index for the right argument. This is purely by choice, but it makes the code generation
// easier since it provids a consistent pattern to follow. With the source generator, including
// the self-contraction source generator, we only need to iterate through the index combinations
// from left to right. The first indices of the left and right tensors in the seed contraction
// should also be the indices to contract.
//
// public static Result<...> Contract<...>(
//     TensorA<..., Index<Lower, IC>, ...> a,
//     TensorB<..., Index<Upper, IC>, ...> b)
//     // constraints
// {
//     // code
// }

/// <summary>Tensor contractions builder</summary>
internal sealed class TensorContractionBuilder : TensorContractionBuilderBase
{
    public TensorContractionBuilder(SourceProductionContext context, ImmutableArray<MethodInformation> methodInformationArray)
        : base(context, methodInformationArray) { }

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
                    UsingDirective("Mathematics.NET.DifferentialGeometry.Abstractions".CreateNameSyntaxFromNamespace())
                        .WithUsingKeyword(
                            Token(
                                TriviaList(
                                    Comment("// Auto-generated code")),
                                SyntaxKind.UsingKeyword,
                                TriviaList())),
                    UsingDirective("Mathematics.NET.LinearAlgebra".CreateNameSyntaxFromNamespace()),
                    UsingDirective("Mathematics.NET.LinearAlgebra.Abstractions".CreateNameSyntaxFromNamespace()),
                    UsingDirective("Mathematics.NET.Symbols".CreateNameSyntaxFromNamespace())]))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        FileScopedNamespaceDeclaration(
                            "Mathematics.NET.DifferentialGeometry".CreateNameSyntaxFromNamespace())
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

    private ImmutableArray<MemberDeclarationSyntax> GenerateMembers()
    {
        List<MemberDeclarationSyntax> result = [];
        for (int i = 0; i < _methodInformationArray.Length; i++)
        {
            var method = _methodInformationArray[i].MethodDeclaration;

            // Validate seed contraction.
            if (!IsValidSeedContraction(method) || !HasSummationComponents(method))
            {
                continue;
            }

            method = method.RemoveAttribute("GenerateTensorContractions");

            // Generate the twin of the original contraction.
            result.Add(method.GenerateTwinContraction());

            // Generate contractions with swapped parameters.
            var contractionInformation = GetTensorRankInformation(method);
            MemberDeclarationSyntax member = method;

            for (int j = 0; j < contractionInformation.RightRank - 1; j++)
            {
                member = SwapRightIndices(member);
                result.Add(member);
                result.Add(member.GenerateTwinContraction());
            }

            // Loop through the remaining index combinations and add the contractions.
            for (int j = 0; j < contractionInformation.LeftRank - 1; j++)
            {
                member = ResetIndices(member);

                member = SwapLeftIndices(member);
                result.Add(member);
                result.Add(member.GenerateTwinContraction());

                for (int k = 0; k < contractionInformation.RightRank - 1; k++)
                {
                    member = SwapRightIndices(member);
                    result.Add(member);
                    result.Add(member.GenerateTwinContraction());
                }
            }
        }
        return result.ToImmutableArray();
    }

    //
    // Validation
    //

    private bool HasSummationComponents(MethodDeclarationSyntax methodDeclaration)
    {
        var multiplyExpression = methodDeclaration
            .DescendantNodes()
            .OfType<BinaryExpressionSyntax>()
            .FirstOrDefault(x => x.IsKind(SyntaxKind.MultiplyExpression));
        if (multiplyExpression is null ||
            multiplyExpression.Parent?.Parent?.Parent?.Parent is null) // Check for for loop
        {
            var descriptor = DiagnosticMessage.CreateMissingSummationComponentsDiagnosticDescriptor();
            _context.ReportDiagnostic(Diagnostic.Create(descriptor, methodDeclaration.Identifier.GetLocation()));
            return false;
        }
        return true;
    }

    private bool IsValidSeedContraction(MethodDeclarationSyntax methodDeclaration)
    {
        // Validate method name
        if (!IsValidMethodName(methodDeclaration))
        {
            return false;
        }

        var paramList = methodDeclaration.ParameterList()!;

        // Validate left tensor
        var leftArgs = paramList.Parameters[(int)IndexPosition.Left].TypeArgumentList()!;
        if (!IsValidIndexPositionAndName(IndexLocation.First, leftArgs, "Lower"))
        {
            return false;
        }

        // Validate right tensor
        var rightArgs = paramList.Parameters[(int)IndexPosition.Right].TypeArgumentList()!;
        if (!IsValidIndexPositionAndName(IndexLocation.First, rightArgs, "Upper"))
        {
            return false;
        }

        return true;
    }

    //
    // Helpers
    //

    private static TensorRankInformation GetTensorRankInformation(MemberDeclarationSyntax memberDeclaration)
    {
        var paramList = memberDeclaration.ParameterList()!;

        var leftParam = paramList.Parameters[0];
        var rightParam = paramList.Parameters[1];

        var leftRank = leftParam.TypeArgumentList()!.Arguments.Count - 3;
        var rightRank = rightParam.TypeArgumentList()!.Arguments.Count - 3;

        return new(leftRank, rightRank);
    }

    private static MemberDeclarationSyntax ResetIndices(MemberDeclarationSyntax memberDeclaration)
    {
        memberDeclaration = ResetTypeParameters(memberDeclaration);
        memberDeclaration = ResetTypeParameterConstraints(memberDeclaration);
        memberDeclaration = ResetMultiplyExpressionComponents(memberDeclaration);

        return memberDeclaration;
    }

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
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[2], [s_rightIndex]);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax ResetTypeParameters(MemberDeclarationSyntax memberDeclaration)
    {
        var param = memberDeclaration.ParameterList()!.Parameters[1];
        var args = param.TypeArgumentList()!;

        var newArgs = args.RemoveNode(args.Arguments.Last(), SyntaxRemoveOptions.KeepNoTrivia);
        newArgs = newArgs!.InsertNodesAfter(newArgs!.Arguments[2], [s_rightIndex]);
        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax SwapIndices(MemberDeclarationSyntax memberDeclaration, IndexPosition position)
    {
        memberDeclaration = SwapTypeParameters(memberDeclaration, position);
        memberDeclaration = SwapTypeParameterConstraints(memberDeclaration, position);
        memberDeclaration = SwapMultiplyExpressionComponents(memberDeclaration, position);

        return memberDeclaration;
    }

    private static MemberDeclarationSyntax SwapLeftIndices(MemberDeclarationSyntax memberDeclaration)
        => SwapIndices(memberDeclaration, IndexPosition.Left);

    private static MemberDeclarationSyntax SwapMultiplyExpressionComponents(MemberDeclarationSyntax memberDeclaration, IndexPosition position)
    {
        var multiplyExpression = memberDeclaration
            .DescendantNodes()
            .OfType<BinaryExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.MultiplyExpression));

        var forStatement = (ForStatementSyntax)multiplyExpression.Parent!.Parent!.Parent!.Parent!;
        var variableName = forStatement.Declaration!.Variables[0].Identifier.Text;

        var args = position == IndexPosition.Left
            ? multiplyExpression.Left.DescendantNodes().OfType<BracketedArgumentListSyntax>().First()
            : multiplyExpression.Right.DescendantNodes().OfType<BracketedArgumentListSyntax>().First();
        var indexSwapper = new BracketedArgumentIndexSwapRewriter(args, variableName);
        var newArgs = (BracketedArgumentListSyntax)indexSwapper.Visit(args);

        return memberDeclaration.ReplaceNode(args, newArgs);
    }

    private static MemberDeclarationSyntax SwapRightIndices(MemberDeclarationSyntax memberDeclaration)
        => SwapIndices(memberDeclaration, IndexPosition.Right);

    private static MemberDeclarationSyntax SwapTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration, IndexPosition position)
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

        var newArgs = args.SwapContractIndexWithNextIndex();
        var newConstraints = constraints.ReplaceNode(args, newArgs);

        return memberDeclaration.ReplaceNode(constraints, newConstraints);
    }

    private static MemberDeclarationSyntax SwapTypeParameters(MemberDeclarationSyntax memberDeclaration, IndexPosition position)
    {
        var param = memberDeclaration.ParameterList()!.Parameters[(int)position];
        var args = param.TypeArgumentList()!;
        var newParam = param.ReplaceNode(args, args.SwapContractIndexWithNextIndex());
        return memberDeclaration.ReplaceNode(param, newParam);
    }
}
