// <copyright file="IConstants.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Core;

/// <summary>Defines support for common mathematical constants</summary>
/// <typeparam name="T">The type that implements the interface</typeparam>
public interface IConstants<T> : IComplex<T>
    where T : IConstants<T>
{
    /// <inheritdoc cref="Constants.E" />
    static abstract T E { get; }

    /// <inheritdoc cref="Constants.Pi" />
    static abstract T Pi { get; }

    /// <inheritdoc cref="Constants.PiOverTwo" />
    static abstract T PiOverTwo { get; }

    /// <inheritdoc cref="Constants.PiSquared" />
    static abstract T PiSquared { get; }

    /// <inheritdoc cref="Constants.Tau" />
    static abstract T Tau { get; }

    /// <inheritdoc cref="Constants.EulerMascheroni" />
    static abstract T EulerMascheroni { get; }

    /// <inheritdoc cref="Constants.GoldenRatio" />
    static abstract T GoldenRatio { get; }

    /// <inheritdoc cref="Constants.Ln2" />
    static abstract T Ln2 { get; }

    /// <inheritdoc cref="Constants.Ln10" />
    static abstract T Ln10 { get; }

    /// <inheritdoc cref="Constants.Sqrt2" />
    static abstract T Sqrt2 { get; }

    /// <inheritdoc cref="Constants.Sqrt3" />
    static abstract T Sqrt3 { get; }

    /// <inheritdoc cref="Constants.Sqrt5" />
    static abstract T Sqrt5 { get; }

    /// <inheritdoc cref="Constants.ZetaOf2" />
    static abstract T ZetaOf2 { get; }

    /// <inheritdoc cref="Constants.ZetaOf3" />
    static abstract T ZetaOf3 { get; }

    /// <inheritdoc cref="Constants.ZetaOf4" />
    static abstract T ZetaOf4 { get; }
}
