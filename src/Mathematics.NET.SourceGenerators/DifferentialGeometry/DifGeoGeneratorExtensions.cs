// <copyright file="DifGeoSyntaxHelper.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using Mathematics.NET.SourceGenerators.DifferentialGeometry.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>Syntax helper for differential geometry generators</summary>
internal static class DifGeoGeneratorExtensions
{
    /// <summary>Generate the tensor contraction with index positions swapped: "lower" to "upper" and "upper" to "lower."</summary>
    /// <param name="memberDeclaration">A member declaration syntax</param>
    /// <returns>A member declaration syntax</returns>
    internal static MemberDeclarationSyntax GenerateTwinContraction(this MemberDeclarationSyntax memberDeclaration)
    {
        FlipIndexRewriter walker = new();
        return (MemberDeclarationSyntax)walker.Visit(memberDeclaration);
    }

    /// <summary>Get the index structure of a tensor.</summary>
    /// <param name="memberDeclaration">A member declaration syntax</param>
    /// <param name="position">An integer representing the current parameter position—the position of the tensor in question in the parameter list</param>
    /// <returns>An index structure</returns>
    internal static IndexStructure GetIndexStructure(this MemberDeclarationSyntax memberDeclaration, int position)
    {
        if (memberDeclaration.ParameterList() is ParameterListSyntax paramList)
        {
            var args = paramList.Parameters[position].TypeArgumentList()!.Arguments;
            var index = args.IndexOf(args.Last(x => x is GenericNameSyntax name && name.Identifier.Text == "Index"));
            return new(index, args.Count);
        }
        return default;
    }

    /// <summary>Swap the current index with the index immediately to its right.</summary>
    /// <param name="typeArgumentListSyntax">A type argument list syntax</param>
    /// <param name="index">An integer representing the current index position</param>
    /// <returns>A type argument list syntax with the specified indices swapped</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TypeArgumentListSyntax SwapCurrentIndexWithNextIndex(this TypeArgumentListSyntax typeArgumentListSyntax, int index)
    {
        var args = typeArgumentListSyntax.Arguments;
        var currentIndex = args[index];
        var nextIndex = args[index + 1];

        var newArgs = args.Replace(currentIndex, nextIndex);
        nextIndex = newArgs[index + 1];
        newArgs = newArgs.Replace(nextIndex, currentIndex);

        return TypeArgumentList(newArgs);
    }
}
