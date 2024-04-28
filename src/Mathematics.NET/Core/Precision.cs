// <copyright file="Precision.cs" company="Mathematics.NET">
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

using System.Numerics;

namespace Mathematics.NET.Core;

/// <summary>
/// A class for working with floating-point numbers.
/// 
/// The methods for computing equalities and inequalies come from <i>The Art of Computer Programming: Seminumerical Algorithms</i>, section 4.2.2, <i>Accuracy of Floating Point Arithmetic</i> by Donald Knuth.
/// </summary>
public static class Precision
{
    /// <summary>Machine epsilon for single-precision floating-point numbers according to the formal definition</summary>
    public const double FltEpsilonFormal = 5.96046447753906250e-8;

    /// <summary>Machine epsilon for double-precision floating-point numbers according to the formal definition</summary>
    public const double DblEpsilonFormal = 1.11022302462515654e-16;

    /// <summary>Machine epsilon for single-precision floating-point numbers according to the variant definition</summary>
    public const double FltEpsilonVariant = 1.19209289550781250e-7;

    /// <summary>Machine epsilon for double-precision floating-point numbers according to the variant definition</summary>
    public const double DblEpsilonVariant = 2.22044604925031308e-16;

    // This is the value given by 2^-149
    /// <summary>The minimum positive value that single-precision numbers can represent</summary>
    /// <remarks>This is equivalent to <see cref="float.Epsilon"/></remarks>
    public const double FltMinPositive = 1.40129846432481707e-45;

    // This is the value given by 2^-1074
    /// <summary>The minimum positive value that double-precision numbers can represent</summary>
    /// <remarks>This is equivalent to <see cref="double.Epsilon"/></remarks>
    public const double DblMinPositive = 4.94065645841246544e-324;

    //
    // Equalities and Inequalities
    //

    /// <summary>Check if two values are approximately equal within a specified threshold</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/></typeparam>
    /// <param name="left">The left value</param>
    /// <param name="right">The right value</param>
    /// <param name="epsilon">A threshold value</param>
    /// <returns><see langword="true"/> if the values are approximately equal; otherwise, <see langword="false"/></returns>
    public static bool AreApproximatelyEqual<T>(T left, T right, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => T.Abs(left - right) <= epsilon * T.Max(T.Abs(left), T.Abs(right));

    /// <summary>
    /// Check if two values are approximately equal within a specified threshold $ \epsilon $.
    /// 
    /// If the values are complex, the threshold represents a disk of radius $ \epsilon $ in which the two values must reside.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="left">The left value</param>
    /// <param name="right">The right value</param>
    /// <param name="epsilon">The radius of threshold disk</param>
    /// <returns><see langword="true"/> if the values are approximately equal; otherwise, <see langword="false"/></returns>
    public static bool AreApproximatelyEqual<T>(T left, T right, Real epsilon)
        where T : IComplex<T>
        => T.Abs(left - right) <= epsilon * Real.Max(T.Abs(left), T.Abs(right));

    /// <summary>Check if two values are essentially equal within a specified threshold</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/></typeparam>
    /// <param name="left">The left value</param>
    /// <param name="right">The right value</param>
    /// <param name="epsilon">A threshold value</param>
    /// <returns><see langword="true"/> if the values are essentially equal; otherwise, <see langword="false"/></returns>
    public static bool AreEssentiallyEqual<T>(T left, T right, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => T.Abs(left - right) <= epsilon * T.Min(T.Abs(left), T.Abs(right));

    /// <summary>
    /// Check if two values are essentially equal within a specified threshold $ \epsilon $.
    /// 
    /// If the values are complex, the threshold represents a disk of radius $ \epsilon $ in which the two values must reside.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
    /// <param name="left">The left value</param>
    /// <param name="right">The right value</param>
    /// <param name="epsilon">The radius of threshold disk</param>
    /// <returns><see langword="true"/> if the values are essentially equal; otherwise, <see langword="false"/></returns>
    public static bool AreEssentiallyEqual<T>(T left, T right, Real epsilon)
        where T : IComplex<T>
        => T.Abs(left - right) <= epsilon * Real.Min(T.Abs(left), T.Abs(right));

    /// <summary>Check if this value is definitely greater than a specified value</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/></typeparam>
    /// <param name="number">This number</param>
    /// <param name="value">A value to which to compare this number</param>
    /// <param name="epsilon">A threshold value</param>
    /// <returns><see langword="true"/> if this number is definitely greater than the specified value; otherwise, <see langword="false"/></returns>
    public static bool IsDefinitelyGreaterThan<T>(this T number, T value, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => number - value > epsilon * T.Max(T.Abs(number), T.Abs(value));

    /// <summary>Check if this value is definitely less than a specified value</summary>
    /// <typeparam name="T">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/></typeparam>
    /// <param name="number">This number</param>
    /// <param name="value">A value to which to compare this number</param>
    /// <param name="epsilon">A threshold value</param>
    /// <returns><see langword="true"/> if this number is definitely less than the specified value; otherwise, <see langword="false"/></returns>
    public static bool IsDefinitelyLessThan<T>(this T number, T value, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => value - number > epsilon * T.Max(T.Abs(number), T.Abs(value));
}
