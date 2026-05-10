// <copyright file="Constants.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core;

/// <summary>Common mathematical constants.</summary>
public static class Constants
{
    /// <summary>Represents the imaginary unit, <c>i</c>.</summary>
    public static Complex Im => new(Real.Zero, Real.One);

    /// <summary>Represents the natural logarithmic base, <c>e</c>.</summary>
    public const double E = 2.71828182845904523;
    /// <summary>Represents the ratio of the circumference of a circle to its diameter, <c>π</c>.</summary>
    public const double Pi = 3.14159265358979323;
    /// <summary>Represents <c>π/2</c>.</summary>
    public const double PiOverTwo = 1.57079632679489661;
    /// <summary>Represents <c>π^2</c>.</summary>
    public const double PiSquared = 9.86960440108935861;
    /// <summary>Represents <c>2π</c>.</summary>
    public const double Tau = 6.28318530717958647;
    /// <summary>Represents the Euler-Mascheroni constant, <c>\gamma</c>.</summary>
    public const double EulerMascheroni = 5.77215664901532860e-1;
    /// <summary>Represents the golden ratio, <c>\phi</c>.</summary>
    public const double GoldenRatio = 1.61803398874989484;
    /// <summary>Represents the natural logarithm of 2, <c>\ln(2)</c>.</summary>
    public const double Ln2 = 6.93147180559945309e-1;
    /// <summary>Represents the natural logarithm of 3, <c>\ln(3)</c>.</summary>
    public const double Ln3 = 1.09861228866810969;
    /// <summary>Represents the natural logarithm of 4, <c>\ln(4)</c>.</summary>
    public const double Ln4 = 1.38629436111989061;
    /// <summary>Represents the natural logarithm of 5, <c>\ln(5)</c>.</summary>
    public const double Ln5 = 1.60943791243410037;
    /// <summary>Represents the natural logarithm of 10, <c>\ln(10)</c>.</summary>
    public const double Ln10 = 2.30258509299404568;
    /// <summary>Represents the square root of 2, <c>\sqrt{2}</c>.</summary>
    public const double Sqrt2 = 1.41421356237309504;
    /// <summary>Represents the square root of 3, <c>\sqrt{3}</c>.</summary>
    public const double Sqrt3 = 1.73205080756887729;
    /// <summary>Represents the square root of 5, <c>\sqrt{5}</c>.</summary>
    public const double Sqrt5 = 2.23606797749978969;
    /// <summary>Represents the solution to the Basel problem, <c>\zeta(2) = \pi^2/6</c>.</summary>
    public const double ZetaOf2 = 1.64493406684822643;
    /// <summary>Represents Apéry's constant, <c>\zeta(3)</c>.</summary>
    public const double ZetaOf3 = 1.20205690315959428;
    /// <summary>Represents <c>\zeta(4) = \pi^4/90</c>.</summary>
    public const double ZetaOf4 = 1.08232323371113819;
}
