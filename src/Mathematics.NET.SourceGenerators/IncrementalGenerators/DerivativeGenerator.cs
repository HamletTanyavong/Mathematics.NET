﻿// <copyright file="DerivativeGenerator.cs" company="Mathematics.NET">
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

using Mathematics.NET.SourceGenerators.Models;

namespace Mathematics.NET.SourceGenerators.IncrementalGenerators;

/// <summary>A generator for calculating derivatives</summary>
[Generator]
public sealed class DerivativeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(CouldBeFunctionAttribute, GetFunctionOrNull)
            .Where(syntax => syntax is not null);
    }

    private static bool CouldBeFunctionAttribute(SyntaxNode syntaxNode, CancellationToken cancellationToken)
        => syntaxNode is AttributeSyntax attributeSyntax && attributeSyntax.Name.GetValue() is "Function" or "FunctionAttribute";

    public static MethodInformation? GetFunctionOrNull(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        // The method syntax will not be null if attribute syntax is not null since the attribute can only be applied to methods.
        var attribute = (AttributeSyntax)context.Node;
        return new(attribute, (MethodDeclarationSyntax)attribute.Parent!.Parent!);
    }
}
