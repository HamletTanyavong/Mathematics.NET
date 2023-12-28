// <copyright file="IndexSwapRewriter.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>A C# syntax rewriter that swaps tensor index orders</summary>
public sealed class IndexSwapRewriter : CSharpSyntaxRewriter
{
    private readonly string _indexToContract;
    private readonly string _indexToSwap;

    public IndexSwapRewriter(BracketedArgumentListSyntax bracketedArgumentList, string indexName)
    {
        _indexToContract = indexName;
        _indexToSwap = GetIndexToSwapName(bracketedArgumentList, indexName);
    }

    private string GetIndexToSwapName(BracketedArgumentListSyntax bracketedArgumentList, string indexName)
    {
        var argument = bracketedArgumentList.Arguments.First(x => x.Expression is IdentifierNameSyntax name && name.Identifier.Text == indexName);
        var index = bracketedArgumentList.Arguments.IndexOf(argument);
        return ((IdentifierNameSyntax)bracketedArgumentList.Arguments[index + 1].Expression).Identifier.Text;
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Identifier.Text == _indexToContract)
        {
            return SyntaxFactory.IdentifierName(_indexToSwap);
        }
        else if (node.Identifier.Text == _indexToSwap)
        {
            return SyntaxFactory.IdentifierName(_indexToContract);
        }
        return node;
    }
}
