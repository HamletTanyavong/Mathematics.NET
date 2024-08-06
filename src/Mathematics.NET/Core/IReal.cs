// <copyright file="IReal.cs" company="Mathematics.NET">
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

using System.Numerics;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.Core.Relations;

namespace Mathematics.NET.Core;

/// <summary>Defines support for real numbers.</summary>
/// <typeparam name="T">A type that implements the interface.</typeparam>
public interface IReal<T>
    : IComplex<T>,
      IModuloOperation<T, T>,
      IInequalityRelations<T, bool>,
      IDecrementOperation<T>,
      IIncrementOperation<T>,
      IComparable,
      IComparable<T>,
      IMinMaxValue<T>
    where T : IReal<T>
{
    /// <summary>Compute a quadrant-aware arctangent given two values.</summary>
    /// <param name="y">The first value.</param>
    /// <param name="x">The second value.</param>
    /// <returns>An angle.</returns>
    static virtual Real Atan2(T y, T x) => Math.Atan2(T.ToReal(y).AsDouble(), T.ToReal(x).AsDouble());

    /// <summary>Compute the ceiling function of a value.</summary>
    /// <param name="x">A value.</param>
    /// <returns>The smallest integer greater than or equal to the value.</returns>
    static abstract T Ceiling(T x);

    /// <summary>Clamp a value to an inclusive range.</summary>
    /// <param name="value">A value.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <returns>The clamped value.</returns>
    static abstract T Clamp(T value, T min, T max);

    /// <summary>Compute the floor function of a value.</summary>
    /// <param name="x">A value.</param>
    /// <returns>The largest integer less than or equal to the value.</returns>
    static abstract T Floor(T x);

    /// <summary>Perform inverse linear interpolation given two points and a weight.</summary>
    /// <param name="start">The start point.</param>
    /// <param name="end">The end point.</param>
    /// <param name="weight">The weight.</param>
    /// <returns>The inversely interpolated value.</returns>
    static abstract T InverseLerp(T start, T end, T weight);

    /// <summary>Check if a value is negative infinity.</summary>
    /// <param name="x">The value to check.</param>
    /// <returns><c>true</c> if the value is negative infinity; otherwise, <c>false</c></returns>
    static abstract bool IsNegativeInfinity(T x);

    /// <summary>Check if a valule is positive infinity.</summary>
    /// <param name="x">The value to check.</param>
    /// <returns><c>true</c> if the value is positive infinity; otherwise, <c>false</c></returns>
    static abstract bool IsPositiveInfinity(T x);

    /// <summary>Linearly interpolate between two points.</summary>
    /// <param name="start">The start point.</param>
    /// <param name="end">The end point.</param>
    /// <param name="weight">The weight.</param>
    /// <returns>The interpolated value.</returns>
    static abstract T Lerp(T start, T end, T weight);

    /// <summary>Find which of two numbers is greater than the other.</summary>
    /// <remarks>If any of the values are <c>NaN</c>, then <c>NaN</c> is returned.</remarks>
    /// <param name="x">The first value.</param>
    /// <param name="y">The second value.</param>
    /// <returns>The greater of the two values.</returns>
    static abstract T Max(T x, T y);

    /// <summary>Find which of two numbers is less than the other.</summary>
    /// <remarks>If any of the values are <c>NaN</c>, then <c>NaN</c> is returned.</remarks>
    /// <param name="x">The first value.</param>
    /// <param name="y">The second value.</param>
    /// <returns>The lesser of the two values.</returns>
    static abstract T Min(T x, T y);

    /// <summary>Find the sign of a real number.</summary>
    /// <param name="x">The value to check.</param>
    /// <returns>
    /// A number indicating the sign of the value:<br/><br/>
    /// <b>1</b> if the value is greater than zero<br/>
    /// <b>0</b> if the value is greater than zero<br/>
    /// <b>-1</b> if the value is less than zero
    /// </returns>
    /// <exception cref="ArithmeticException">An overflow or underflow has occurred.</exception>
    static abstract int Sign(T x);

    /// <summary>Create a real number from a type that implements <typeparamref name="T"/>.</summary>
    /// <param name="x">A type that implements <typeparamref name="T"/>.</param>
    /// <returns>A real value.</returns>
    /// <exception cref="OverflowException">Thrown when the value cannot be converted to the type <see cref="Real"/>.</exception>
    static abstract Real ToReal(T x);
}
