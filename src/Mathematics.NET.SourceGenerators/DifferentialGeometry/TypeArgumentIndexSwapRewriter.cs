// <copyright file="TypeArgumentIndexSwapRewriter.cs" company="Mathematics.NET">
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

/// <summary>A C# syntax rewriter that swaps indices in a type argument list</summary>
internal sealed class TypeArgumentIndexSwapRewriter : CSharpSyntaxRewriter
{
    private GenericNameSyntax? _indexToContract;
    private IdentifierNameSyntax? _indexToSwap;

    // Since the index to contract should always be to the left of the index to swap, we
    // can look to the immediate right of the index to contract to find the index to swap.
    public TypeArgumentIndexSwapRewriter(TypeArgumentListSyntax typeArgumentList)
    {
        _indexToContract = (GenericNameSyntax)typeArgumentList.Arguments.Last(x => x is GenericNameSyntax name && name.Identifier.Text == "Index");
        _indexToSwap = (IdentifierNameSyntax)typeArgumentList.Arguments.SkipWhile(x => x != _indexToContract).Skip(1).First();
    }

    public override SyntaxNode? VisitGenericName(GenericNameSyntax node)
    {
        if (node == _indexToContract)
        {
            return _indexToSwap;
        }
        return node;
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node == _indexToSwap)
        {
            return _indexToContract;
        }
        return node;
    }
}
