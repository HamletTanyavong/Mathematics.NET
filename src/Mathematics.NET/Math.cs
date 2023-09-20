// <copyright file="Math.cs" company="Mathematics.NET">
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

using System.Numerics;
using Mathematics.NET.Core;

namespace Mathematics.NET;

/// <summary>Provides Mathematics.NET functionality</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/></typeparam>
/// <remarks>
/// This class contains constants and static methods for general mathematical purposes. Complicated mathematical operations may be found in their respective classes, and a list of all available operations can be found in the <see href="https://mathematics.hamlettanyavong.com/api/index.html">documentation</see>.
/// </remarks>
public static class Math<T> where T : IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    //
    // Constants
    //

    /// <summary>Represents the imaginary unit, $ i $</summary>
    public static Complex<T> Im => new(Real<T>.Zero, Real<T>.One);

    /// <inheritdoc cref="Constants.E"/>
    public static Real<T> E => Real<T>.E;

    /// <inheritdoc cref="Constants.Pi"/>
    public static Real<T> Pi => Real<T>.Pi;

    /// <inheritdoc cref="Constants.PiOverTwo"/>
    public static Real<T> PiOverTwo => Real<T>.PiOverTwo;

    /// <inheritdoc cref="Constants.PiSquared"/>
    public static Real<T> PiSquared => Real<T>.PiSquared;

    /// <inheritdoc cref="Constants.Tau"/>
    public static Real<T> Tau => Real<T>.Tau;

    /// <inheritdoc cref="Constants.EulerMascheroni"/>
    public static Real<T> EulerMascheroni => Real<T>.EulerMascheroni;

    /// <inheritdoc cref="Constants.GoldenRatio"/>
    public static Real<T> GoldenRatio => Real<T>.GoldenRatio;

    /// <inheritdoc cref="Constants.Ln2"/>
    public static Real<T> Ln2 => Real<T>.Ln2;

    /// <inheritdoc cref="Constants.Ln10"/>
    public static Real<T> Ln10 => Real<T>.Ln10;

    /// <inheritdoc cref="Constants.Sqrt2"/>
    public static Real<T> Sqrt2 => Real<T>.Sqrt2;

    /// <inheritdoc cref="Constants.Sqrt3"/>
    public static Real<T> Sqrt3 => Real<T>.Sqrt3;

    /// <inheritdoc cref="Constants.Sqrt5"/>
    public static Real<T> Sqrt5 => Real<T>.Sqrt5;

    /// <inheritdoc cref="Constants.ZetaOf2"/>
    public static Real<T> ZetaOf2 => Real<T>.ZetaOf2;

    /// <inheritdoc cref="Constants.ZetaOf3"/>
    public static Real<T> ZetaOf3 => Real<T>.ZetaOf3;

    /// <inheritdoc cref="Constants.ZetaOf4"/>
    public static Real<T> ZetaOf4 => Real<T>.ZetaOf4;
}
