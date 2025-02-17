// <copyright file="Extensions.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023-present Hamlet Tanyavong
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

using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.Public;

/// <summary>Extension methods for source generators.</summary>
internal static class Extensions
{
    /// <summary>Create a name syntax from a namespace.</summary>
    /// <param name="namespaceName">A string representing a namespace.</param>
    /// <returns>A name syntax.</returns>
    public static NameSyntax CreateNameSyntaxFromNamespace(this string namespaceName)
    {
        Debug.Assert(!namespaceName.Contains(' '), "The namespace string must not contain any spaces.");
        ReadOnlySpan<string> names = namespaceName.Split('.');

        NameSyntax result = IdentifierName(names[0]);
        for (int i = 1; i < names.Length; i++)
        {
            result = QualifiedName(result, IdentifierName(names[i]));
        }

        return result;
    }

    //
    // Syntax helper
    //

    /// <summary>Get the value of the last identifier name in a name syntax.</summary>
    /// <param name="name">A type that derives from name syntax.</param>
    /// <returns>The value of the name syntax.</returns>
    public static string? GetLastIdentifierNameValueOrDefault(this NameSyntax name)
    {
        return name switch
        {
            SimpleNameSyntax simpleNameSyntax => simpleNameSyntax.Identifier.Text,
            QualifiedNameSyntax qualifiedNameSyntax => qualifiedNameSyntax.Right.Identifier.Text,
            _ => default
        };
    }

    /// <summary>Get the namespace name syntax from a struct.</summary>
    /// <param name="structDeclaration">A struct declaration syntax.</param>
    /// <returns>A name syntax.</returns>
    public static NameSyntax? GetNamespaceNameSyntaxFromStructOrDefault(this StructDeclarationSyntax structDeclaration)
    {
        if (structDeclaration.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
            return fileScopedNamespaceDeclaration.Name;

        if (structDeclaration.Parent is NamespaceDeclarationSyntax namespaceDeclaration)
            return namespaceDeclaration.Name.WithoutTrailingTrivia();

        return default;
    }

    /// <summary>Get the value of a name syntax.</summary>
    /// <param name="name">A name syntax.</param>
    /// <returns>A string.</returns>
    public static string? GetNameValueOrDefault(this NameSyntax? name)
    {
        if (name is null)
            return default;

        if (name is SimpleNameSyntax simpleName)
            return simpleName.Identifier.Text;

        if (name is QualifiedNameSyntax qualifiedName)
        {
            Stack<string> stack = new();
            stack.Push(qualifiedName.Right.Identifier.Text);
            while (qualifiedName.Left is QualifiedNameSyntax innerQualifiedName)
            {
                qualifiedName = innerQualifiedName;
                stack.Push(qualifiedName.Right.Identifier.Text);
            }
            stack.Push(((SimpleNameSyntax)qualifiedName.Left).Identifier.Text);

            StringBuilder builder = new();
            while (stack.Count > 0)
            {
                _ = builder.Append(stack.Pop());
                _ = builder.Append('.');
            }
            _ = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        return default;
    }

    /// <summary>Create a struct declaration syntax with both open and close parenthesis.</summary>
    /// <param name="structDeclarationSyntax">A struct declaration syntax.</param>
    /// <param name="whitespace">Indentation whitespace characeters.</param>
    /// <param name="extraNewLine">Whether or not to add an extra new line.</param>
    /// <returns>A struct declaration syntax.</returns>
    public static StructDeclarationSyntax WithOpenAndCloseBraceTokens(this StructDeclarationSyntax structDeclarationSyntax, string whitespace, bool extraNewLine)
    {
        return structDeclarationSyntax
            .WithOpenBraceToken(
                Token(
                    TriviaList(
                        Whitespace(whitespace)),
                    SyntaxKind.OpenBraceToken,
                    TriviaList(CarriageReturnLineFeed)))
            .WithCloseBraceToken(
                Token(
                    TriviaList(
                        Whitespace(whitespace)),
                    SyntaxKind.CloseBraceToken,
                    extraNewLine ? TriviaList(CarriageReturnLineFeed, CarriageReturnLineFeed) : TriviaList(CarriageReturnLineFeed)));
    }
}
