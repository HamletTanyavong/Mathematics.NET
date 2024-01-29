// <copyright file="DiagnosticMessage.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.SourceGenerators.Public;

/// <summary>A class for creating diagnostic messages for use in Mathematics.NET source generators</summary>
public static class DiagnosticMessage
{
    public static DiagnosticDescriptor CreateInvalidSymbolDeclarationDiagnosticDescriptor()
    {
        return new DiagnosticDescriptor(
            id: "SGS0001",
            title: "Invalid symbol declaration",
            messageFormat: "Symbols must be declared in namespaces.",
            category: "Symbol",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: "https://mathematics.hamlettanyavong.com/guide/diagnostic_messages/symbols/sgs0001.html");
    }
}
