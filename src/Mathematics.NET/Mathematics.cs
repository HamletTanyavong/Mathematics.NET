// <copyright file="Mathematics.cs" company="Mathematics.NET">
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

namespace Mathematics.NET;

/// <summary>Provides Mathematics.NET functionality</summary>
/// <remarks>
/// This class contains constants and static methods for general mathematical purposes. Complicated mathematical operations may be found in their respective classes, and a list of all available operations can be found in the <see href="https://mathematics.hamlettanyavong.com/api/index.html">documentation</see>.
/// </remarks>
public static class Mathematics
{
    //
    // Constants
    //

    /// <summary>Represents the imaginary unit, $ i $</summary>
    public static ComplexNumber Im => new(Real.Zero, Real.One);

    /// <inheritdoc cref="Constants.E"/>
    public static Real E => Real.E;

    /// <inheritdoc cref="Constants.Pi"/>
    public static Real Pi => Real.Pi;

    /// <inheritdoc cref="Constants.PiOverTwo"/>
    public static Real PiOverTwo => Real.PiOverTwo;

    /// <inheritdoc cref="Constants.PiSquared"/>
    public static Real PiSquared => Real.PiSquared;

    /// <inheritdoc cref="Constants.Tau"/>
    public static Real Tau => Real.Tau;

    /// <inheritdoc cref="Constants.EulerMascheroni"/>
    public static Real EulerMascheroni => Real.EulerMascheroni;

    /// <inheritdoc cref="Constants.GoldenRatio"/>
    public static Real GoldenRatio => Real.GoldenRatio;

    /// <inheritdoc cref="Constants.Ln2"/>
    public static Real Ln2 => Real.Ln2;

    /// <inheritdoc cref="Constants.Ln10"/>
    public static Real Ln10 => Real.Ln10;

    /// <inheritdoc cref="Constants.Sqrt2"/>
    public static Real Sqrt2 => Real.Sqrt2;

    /// <inheritdoc cref="Constants.Sqrt3"/>
    public static Real Sqrt3 => Real.Sqrt3;

    /// <inheritdoc cref="Constants.Sqrt5"/>
    public static Real Sqrt5 => Real.Sqrt5;

    /// <inheritdoc cref="Constants.ZetaOf2"/>
    public static Real ZetaOf2 => Real.ZetaOf2;

    /// <inheritdoc cref="Constants.ZetaOf3"/>
    public static Real ZetaOf3 => Real.ZetaOf3;

    /// <inheritdoc cref="Constants.ZetaOf4"/>
    public static Real ZetaOf4 => Real.ZetaOf4;

    //
    // Methods
    //

    // TODO: Account for fractional derivatives
    public static Real Dif(Func<Real, Real> function, (Real X, int N) args) => throw new NotImplementedException();
}
