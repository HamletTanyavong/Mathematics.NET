﻿// <copyright file="Precision.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core;

/// <summary>A class for working with floating-point numbers</summary>
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

    public static bool AreApproximatelyEqual<T>(T left, T right, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => T.Abs(left - right) <= epsilon * T.Max(T.Abs(left), T.Abs(right));

    public static bool AreEssentiallyEqual<T>(T left, T right, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => T.Abs(left - right) <= epsilon * T.Min(T.Abs(left), T.Abs(right));

    public static bool IsDefinitelyGreaterThan<T>(this T number, T value, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => number - value > epsilon * T.Max(T.Abs(number), T.Abs(value));

    public static bool IsDefinitelyLessThan<T>(this T number, T value, T epsilon)
        where T : IBinaryFloatingPointIeee754<T>
        => value - number > epsilon * T.Max(T.Abs(number), T.Abs(value));
}
