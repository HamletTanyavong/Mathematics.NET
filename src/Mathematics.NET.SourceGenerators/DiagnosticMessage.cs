// <copyright file="DiagnosticMessage.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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

using Mathematics.NET.SourceGenerators.DifferentialGeometry;

namespace Mathematics.NET.SourceGenerators;

internal static class DiagnosticMessage
{
    //
    // Differential Geometry
    //

    public static DiagnosticDescriptor CreateInvalidMethodNameDiagnosticDescriptor()
    {
        return new DiagnosticDescriptor(
            id: "ISGDG0001",
            title: "Invalid method name",
            messageFormat: "Methods marked with attributes for generating contractions must be named \"Contract.\"",
            category: "DifGeo",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }

    public static DiagnosticDescriptor CreateIncorrectIndexToContractDiagnosticDescriptor(IndexLocation indexLocation)
    {
        return new DiagnosticDescriptor(
            id: "ISGDG0002",
            title: "Incorrect index to contract",
            messageFormat: $"The {(indexLocation == IndexLocation.First ? "first" : "second")} index of the tensor must be the index to contract.",
            category: "DifGeo",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }

    public static DiagnosticDescriptor CreateIncorrectIndexPositionDiagnosticDescriptor(string indexPosition)
    {
        return new DiagnosticDescriptor(
            id: "ISGDG0003",
            title: "Incorrect index position",
            messageFormat: $"The index position of the index to contract must be \"{indexPosition}.\"",
            category: "DifGeo",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }

    public static DiagnosticDescriptor CreateIncorrectTypeParameterNameDiagnosticDescriptor()
    {
        return new DiagnosticDescriptor(
            id: "ISGDG0004",
            title: "Incorrect type parameter name",
            messageFormat: "The type parameter name of the index to contract must be \"TCI.\"",
            category: "DifGeo",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }

    public static DiagnosticDescriptor CreateMissingSummationComponentsDiagnosticDescriptor()
    {
        return new DiagnosticDescriptor(
            id: "ISGDG0005",
            title: "Missing summation components",
            messageFormat: "Tensor contraction implementations are expected to have summation components—at least one for-loop and an add-assignment expression, as well as a multiplication expression for general contractions.",
            category: "DifGeo",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
