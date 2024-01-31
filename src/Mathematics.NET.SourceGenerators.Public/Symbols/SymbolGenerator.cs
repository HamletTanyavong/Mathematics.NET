// <copyright file="SymbolGenerator.cs" company="Mathematics.NET">
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
using Mathematics.NET.SourceGenerators.Public.Models;

namespace Mathematics.NET.SourceGenerators.Public.Symbols;

/// <summary>A generator for mathematical symbols</summary>
[Generator]
public sealed class SymbolGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(CouldBeSymbolAttribute, GetStructInformationOrNull)
            .Where(x => x is not null);
        var compilation = context.CompilationProvider.Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, (context, source) => GenerateCode(context, source.Right));
    }

    private static bool CouldBeSymbolAttribute(SyntaxNode syntaxNode, CancellationToken token)
        => syntaxNode is AttributeSyntax attributeSyntax && attributeSyntax.Name.GetLastIdentifierNameValueOrDefault() is "Symbol" or "SymbolAttribute";

    private static StructInformation GetStructInformationOrNull(GeneratorSyntaxContext context, CancellationToken token)
    {
        // The struct syntax will not be null if attribute syntax is not null since the attribute can only be applied to structs.
        var attribute = (AttributeSyntax)context.Node;
        var structDeclaration = (StructDeclarationSyntax)attribute.Parent!.Parent!;
        return new(structDeclaration.GetNamespaceNameSyntaxFromStructOrDefault(), attribute, structDeclaration);
    }

    private void GenerateCode(SourceProductionContext context, ImmutableArray<StructInformation> information)
    {
        if (information.Length > 0)
        {
            NameSyntaxComparer comparer = new();
            foreach (var nameSyntax in information.Select(x => x.NamespaceNameSyntax).Distinct(comparer))
            {
                var selectedInformation = information
                    .Where(x => comparer.Equals(x.NamespaceNameSyntax, nameSyntax))
                    .ToImmutableArray();
                if (nameSyntax is null)
                {
                    foreach (var info in selectedInformation)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            DiagnosticMessage.CreateInvalidSymbolDeclarationDiagnosticDescriptor(),
                            info.StructDeclarationSyntax.Identifier.GetLocation()));
                    }
                    continue;
                }
                var symbols = new SymbolBuilder(nameSyntax!, context, selectedInformation);
                context.AddSource($"Symbols.{nameSyntax.GetNameValueOrDefault()}.g.cs", symbols.GenerateSource().GetText(Encoding.UTF8).ToString());
            }
        }
    }
}
