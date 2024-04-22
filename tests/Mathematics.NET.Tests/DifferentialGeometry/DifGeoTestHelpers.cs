// <copyright file="DifGeoTestHelpers.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.Tests.DifferentialGeometry;

public static class DifGeoTestHelpers
{
    public sealed class Test4x4MetricTensorFieldNo1<TT, TSM, TN, TPI> : MetricTensorField4x4<TT, TSM, TN, TPI>
        where TT : ITape<TN>
        where TSM : ISquareMatrix<TSM, TN>
        where TN : IComplex<TN>, IDifferentiableFunctions<TN>
        where TPI : IIndex
    {
        public Test4x4MetricTensorFieldNo1()
        {
            Initialize();
        }

        private void Initialize()
        {
            base[0, 0] = (tape, x) => tape.Sin(x.X0);
            base[0, 1] = (tape, x) => tape.Multiply(2, tape.Cos(x.X1));
            base[0, 2] = (tape, x) => tape.Multiply(3, tape.Exp(x.X2));
            base[0, 3] = (tape, x) => tape.Multiply(4, tape.Ln(x.X3));

            base[1, 0] = (tape, x) => tape.Multiply(5, tape.Sqrt(x.X1));
            base[1, 1] = (tape, x) => tape.Multiply(6, tape.Tan(x.X2));
            base[1, 2] = (tape, x) => tape.Multiply(7, tape.Sinh(x.X3));
            base[1, 3] = (tape, x) => tape.Multiply(8, tape.Cosh(x.X0));

            base[2, 0] = (tape, x) => tape.Multiply(9, tape.Tanh(x.X2));
            base[2, 1] = (tape, x) => tape.Divide(10, x.X3);
            base[2, 2] = (tape, x) => tape.Multiply(11, tape.Pow(x.X0, 2));
            base[2, 3] = (tape, x) => tape.Multiply(12, tape.Multiply(tape.Sin(x.X1), tape.Cos(x.X1)));

            base[3, 0] = (tape, x) => tape.Multiply(13, tape.Multiply(tape.Exp(x.X3), tape.Sin(x.X0)));
            base[3, 1] = (tape, x) => tape.Multiply(14, tape.Divide(x.X3, x.X1));
            base[3, 2] = (tape, x) => tape.Multiply(15, tape.Sin(tape.Subtract(x.X3, tape.Multiply(2, x.X2))));
            base[3, 3] = (tape, x) => tape.Multiply(16, tape.Multiply(x.X0, tape.Multiply(x.X1, tape.Multiply(x.X2, x.X3))));
        }
    }
}

//
// Symbols
//

public readonly struct Alpha : ISymbol
{
    /// <inheritdoc cref="ISymbol.DisplayString"/>
    public const string DisplayString = "Alpha";

    static string ISymbol.DisplayString => DisplayString;
}

public readonly struct Beta : ISymbol
{
    /// <inheritdoc cref="ISymbol.DisplayString"/>
    public const string DisplayString = "Beta";

    static string ISymbol.DisplayString => DisplayString;
}

public readonly struct Gamma : ISymbol
{
    /// <inheritdoc cref="ISymbol.DisplayString"/>
    public const string DisplayString = "Gamma";

    static string ISymbol.DisplayString => DisplayString;
}

public readonly struct Delta : ISymbol
{
    /// <inheritdoc cref="ISymbol.DisplayString"/>
    public const string DisplayString = "Delta";

    static string ISymbol.DisplayString => DisplayString;
}

public readonly struct Epsilon : ISymbol
{
    /// <inheritdoc cref="ISymbol.DisplayString"/>
    public const string DisplayString = "Epsilon";

    static string ISymbol.DisplayString => DisplayString;
}
