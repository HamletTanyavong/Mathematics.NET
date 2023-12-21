// <copyright file="Extensions.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.SourceGenerators;

public static class Extensions
{
    public static string? GetValue(this NameSyntax name)
    {
        return name switch
        {
            SimpleNameSyntax simpleNameSyntax => simpleNameSyntax.Identifier.Text,
            QualifiedNameSyntax qualifiedNameSyntax => qualifiedNameSyntax.Right.Identifier.Text,
            _ => null
        };
    }

    public static MethodDeclarationSyntax RemoveAttribute(this MethodDeclarationSyntax methodDeclarationSyntax, string attributeName)
    {
        if (attributeName.EndsWith("Attribute"))
        {
            attributeName = attributeName.Remove(attributeName.Length - 9);
        }
        var attributeNode = methodDeclarationSyntax
            .DescendantNodes()
            .OfType<AttributeSyntax>()
            .First(x => x.Name.GetValue() == attributeName);

        if (attributeNode.Parent!.ChildNodes().Count() > 1)
        {
            return methodDeclarationSyntax.RemoveNode(attributeNode, SyntaxRemoveOptions.KeepNoTrivia)!;
        }
        else
        {
            return methodDeclarationSyntax.RemoveNode(attributeNode.Parent, SyntaxRemoveOptions.KeepNoTrivia)!;
        }
    }
}
