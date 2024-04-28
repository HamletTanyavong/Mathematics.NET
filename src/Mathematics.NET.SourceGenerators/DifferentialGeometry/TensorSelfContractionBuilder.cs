// <copyright file="TensorSelfContractionBuilder.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Tensor self contractions builder</summary>
internal sealed class TensorSelfContractionBuilder : TensorContractionBuilderBase
{
    public TensorSelfContractionBuilder(SourceProductionContext context, ImmutableArray<MethodInformation> methodInformationArray)
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
        return result.ToImmutableArray();
    }

    //
    // Validation
    //

    private bool HasSummationComponents(MethodDeclarationSyntax methodDeclaration)
    {
        var addAssignmentExpression = methodDeclaration
            .DescendantNodes()
            .OfType<AssignmentExpressionSyntax>()
            .FirstOrDefault(x => x.IsKind(SyntaxKind.AddAssignmentExpression));
        if (addAssignmentExpression is null ||
            addAssignmentExpression.Parent?.Parent?.Parent is null) // Check for for loop
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

        // Validate tensor
        var args = paramList.Parameters[0].TypeArgumentList()!;
        if (!IsValidIndexPositionAndName(IndexLocation.First, args, "Lower") || !IsValidIndexPositionAndName(IndexLocation.Second, args, "Upper"))
        {
            return false;
        }

        return true;
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
        memberDeclaration = SwapTypeParameters(memberDeclaration);
        memberDeclaration = SwapTypeParameterConstraints(memberDeclaration);
        memberDeclaration = SwapElementAccessComponents(memberDeclaration);

        return memberDeclaration;
    }

    private static MemberDeclarationSyntax SwapElementAccessComponents(MemberDeclarationSyntax memberDeclaration)
    {
        var addAssignmentExpression = memberDeclaration
            .DescendantNodes()
            .OfType<AssignmentExpressionSyntax>()
            .First(x => x.IsKind(SyntaxKind.AddAssignmentExpression));

        var forStatement = (ForStatementSyntax)addAssignmentExpression.Parent!.Parent!.Parent!;
        var iterationIndexName = forStatement.Declaration!.Variables[0].Identifier.Text;
        var args = addAssignmentExpression.Right.ChildNodes().OfType<BracketedArgumentListSyntax>().First();
        return memberDeclaration.ReplaceNode(args, args.SwapIterationIndexWithNextIndex(iterationIndexName));
    }

    private static MemberDeclarationSyntax SwapTypeParameterConstraints(MemberDeclarationSyntax memberDeclaration)
    {
        var constraints = memberDeclaration
            .ChildNodes()
            .OfType<TypeParameterConstraintClauseSyntax>()
            .First();
        var args = constraints
            .DescendantNodes()
            .OfType<TypeArgumentListSyntax>()
            .First();

        var newArgs = args.SwapContractIndexWithNextIndex();
        var newConstraints = constraints.ReplaceNode(args, newArgs);

        return memberDeclaration.ReplaceNode(constraints, newConstraints);
    }

    private static MemberDeclarationSyntax SwapTypeParameters(MemberDeclarationSyntax memberDeclaration)
    {
        var param = memberDeclaration.ParameterList()!.Parameters[0];
        var args = param.TypeArgumentList()!;
        var newParam = param.ReplaceNode(args, args.SwapContractIndexWithNextIndex());
        return memberDeclaration.ReplaceNode(param, newParam);
    }
}
