// <copyright file="DifGeoGeneratorExtensions.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Syntax helper for differential geometry generators</summary>
internal static class DifGeoGeneratorExtensions
{
    /// <summary>Generate the tensor contraction with index positions swapped: "lower" to "upper" and "upper" to "lower."</summary>
    /// <param name="memberDeclarationSyntax">A member declaration syntax</param>
    /// <returns>A member declaration syntax</returns>
    internal static MemberDeclarationSyntax GenerateTwinContraction(this MemberDeclarationSyntax memberDeclarationSyntax)
    {
        FlipIndexPositionRewriter walker = new();
        return (MemberDeclarationSyntax)walker.Visit(memberDeclarationSyntax);
    }

    /// <summary>Swap the index to contract with the index immediately to its right.</summary>
    /// <param name="typeArgumentListSyntax">A type argument list syntax</param>
    /// <returns>A type argument list syntax with the specified indices swapped</returns>
    internal static TypeArgumentListSyntax SwapContractIndexWithNextIndex(this TypeArgumentListSyntax typeArgumentListSyntax)
    {
        TypeArgumentIndexSwapRewriter rewriter = new(typeArgumentListSyntax);
        return (TypeArgumentListSyntax)rewriter.Visit(typeArgumentListSyntax);
    }

    /// <summary>Swap the index to contract with the index immediately to its right.</summary>
    /// <param name="bracketedArgumentListSyntax">A bracketed argument list syntax</param>
    /// <param name="iterationIndexName">The name of the iteration index</param>
    /// <returns>A bracketed argument list syntax with the specified indices swapped</returns>
    internal static BracketedArgumentListSyntax SwapIterationIndexWithNextIndex(this BracketedArgumentListSyntax bracketedArgumentListSyntax, string iterationIndexName)
    {
        BracketedArgumentIndexSwapRewriter rewriter = new(bracketedArgumentListSyntax, iterationIndexName);
        return (BracketedArgumentListSyntax)rewriter.Visit(bracketedArgumentListSyntax);
    }
}
