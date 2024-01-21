// <copyright file="DerivativeGenerator.cs" company="Mathematics.NET">
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
using System.Text;

namespace Mathematics.NET.SourceGenerators.Calculus;

// This is experimental. Uncomment the generator attribute to generate code.

/// <summary>A generator for calculating derivatives</summary>
//[Generator]
public sealed class DerivativeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(CouldBeEquationAttribute, GetEquationOrNull)
            .Where(info => info is not null);
        var compilation = context.CompilationProvider.Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, (context, source) => GenerateCode(context, source.Left, source.Right!));
    }

    private static bool CouldBeEquationAttribute(SyntaxNode syntaxNode, CancellationToken cancellationToken)
        => syntaxNode is AttributeSyntax attributeSyntax && attributeSyntax.Name.GetValue() is "Equation" or "EquationAttribute";

    private static MethodInformation? GetEquationOrNull(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        // The method syntax will not be null if attribute syntax is not null since the attribute can only be applied to methods.
        var attribute = (AttributeSyntax)context.Node;
        return new(attribute, (MethodDeclarationSyntax)attribute.Parent!.Parent!);
    }

    private void GenerateCode(SourceProductionContext context, Compilation compilation, ImmutableArray<MethodInformation> methodInformation)
    {
        var derivatives = new DerivativesBuilder(compilation, methodInformation);
        context.AddSource("Equations.g.cs", derivatives.GenerateSource().GetText(Encoding.UTF8).ToString());
    }
}
