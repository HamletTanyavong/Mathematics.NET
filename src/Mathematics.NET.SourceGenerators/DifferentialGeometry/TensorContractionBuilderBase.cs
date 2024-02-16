// <copyright file="TensorContractionBuilderBase.cs" company="Mathematics.NET">
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
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Mathematics.NET.SourceGenerators.DifferentialGeometry;

/// <summary>A base class for tensor contraction builders</summary>
internal abstract class TensorContractionBuilderBase
{
    private protected static readonly GenericNameSyntax s_leftIndex = GenericName(
        Identifier("Index"))
            .WithTypeArgumentList(
                TypeArgumentList(
                    SeparatedList<TypeSyntax>(new SyntaxNodeOrToken[] {
                            IdentifierName("Lower"),
                            Token(SyntaxKind.CommaToken),
                            IdentifierName("IC") })));

    private protected static readonly GenericNameSyntax s_rightIndex = GenericName(
        Identifier("Index"))
            .WithTypeArgumentList(
                TypeArgumentList(
                    SeparatedList<TypeSyntax>(new SyntaxNodeOrToken[] {
                        IdentifierName("Upper"),
                        Token(SyntaxKind.CommaToken),
                        IdentifierName("IC") })));

    private protected readonly SourceProductionContext _context;
    private protected readonly ImmutableArray<MethodInformation> _methodInformationArray;

    private protected TensorContractionBuilderBase(SourceProductionContext context, ImmutableArray<MethodInformation> methodInformationArray)
    {
        _context = context;
        _methodInformationArray = methodInformationArray;
    }

    private protected bool IsValidIndexPositionAndName(IndexLocation location, TypeArgumentListSyntax typeArgumentList, string indexPosition)
    {
        if (typeArgumentList.Arguments[(int)location] is GenericNameSyntax name)
        {
            var actualPosition = (IdentifierNameSyntax)name.TypeArgumentList.Arguments[0];
            if (actualPosition.Identifier.Text != indexPosition)
            {
                var descriptor = DiagnosticMessage.CreateIncorrectIndexPositionDiagnosticDescriptor(indexPosition);
                _context.ReportDiagnostic(Diagnostic.Create(descriptor, actualPosition.Identifier.GetLocation()));
                return false;
            }

            var typeParameterName = (IdentifierNameSyntax)name.TypeArgumentList.Arguments[1];
            if (typeParameterName.Identifier.Text != "IC")
            {
                var descriptor = DiagnosticMessage.CreateIncorrectTypeParameterNameDiagnosticDescriptor();
                _context.ReportDiagnostic(Diagnostic.Create(descriptor, typeParameterName.Identifier.GetLocation()));
                return false;
            }
        }
        else
        {
            var descriptor = DiagnosticMessage.CreateIncorrectIndexToContractDiagnosticDescriptor(location);
            _context.ReportDiagnostic(Diagnostic.Create(descriptor, typeArgumentList.Arguments[(int)location].GetLocation()));
            return false;
        }

        return true;
    }

    private protected bool IsValidMethodName(MethodDeclarationSyntax methodDeclaration)
    {
        if (methodDeclaration.Identifier.Text != "Contract")
        {
            var descriptor = DiagnosticMessage.CreateInvalidMethodNameDiagnosticDescriptor();
            _context.ReportDiagnostic(Diagnostic.Create(descriptor, methodDeclaration.Identifier.GetLocation()));
        }
        return true;
    }
}
