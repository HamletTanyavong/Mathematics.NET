﻿// <copyright file="SymbolicsHelper.cs" company="Mathematics.NET">
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

using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators;

public static class SymbolicsHelper
{
    private static InvocationExpressionSyntax DifSin(MemberAccessExpressionSyntax memberAccessExpressionSyntax, ArgumentListSyntax argumentListSyntax)
    public static IEnumerable<MemberAccessExpressionSyntax> ExtractDerivativeExpressionList(IEnumerable<SyntaxNode> syntaxNodes)
    {
        return syntaxNodes
            .OfType<MemberAccessExpressionSyntax>()
            .Where(x => x.Name.Identifier.ValueText == "Dif");
    }

    public static ExpressionSyntax? TakeDerivative(MemberAccessExpressionSyntax memberAccessExpressionSyntax, ArgumentListSyntax argumentListSyntax)
    {
        return memberAccessExpressionSyntax.Name.Identifier.ValueText switch
        {
            "Cos" => DifCos(memberAccessExpressionSyntax, argumentListSyntax),
            "Sin" => DifSin(memberAccessExpressionSyntax, argumentListSyntax),
            _ => null
        };
    }

    // Mathematical functions

    private static ExpressionSyntax DifCos(MemberAccessExpressionSyntax memberAccessExpressionSyntax, ArgumentListSyntax argumentListSyntax)
    {
        var cosNameSyntax = memberAccessExpressionSyntax
            .DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .First(x => x.Identifier.ValueText == "Cos");
        var newExpression = memberAccessExpressionSyntax.ReplaceNode(cosNameSyntax, IdentifierName("Sin"));
        return BinaryExpression(
            SyntaxKind.MultiplyExpression,
            PrefixUnaryExpression(
                SyntaxKind.UnaryMinusExpression,
                LiteralExpression(
                    SyntaxKind.NumericLiteralExpression,
                    Literal(1))),
            InvocationExpression(
                newExpression,
                argumentListSyntax));
    }

    private static ExpressionSyntax DifSin(MemberAccessExpressionSyntax memberAccessExpressionSyntax, ArgumentListSyntax argumentListSyntax)
    {
        var sinNameSyntax = memberAccessExpressionSyntax
            .DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .First(x => x.Identifier.ValueText == "Sin");
        var newExpression = memberAccessExpressionSyntax.ReplaceNode(sinNameSyntax, IdentifierName("Cos"));
        return InvocationExpression(
            newExpression,
            argumentListSyntax);
    }
}
