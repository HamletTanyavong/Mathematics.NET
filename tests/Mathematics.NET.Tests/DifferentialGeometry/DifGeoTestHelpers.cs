// <copyright file="DifGeoTestHelpers.cs" company="Mathematics.NET">
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

using Mathematics.NET.DifferentialGeometry;

namespace Mathematics.NET.Tests.DifferentialGeometry;

public static class DifGeoTestHelpers;

//
// Symbols
//

public readonly struct PIN : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "TestIndex";
    static string IIndexName.DisplayString => DisplayString;
}

public readonly struct Alpha : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "Alpha";
    static string IIndexName.DisplayString => DisplayString;
}

public readonly struct Beta : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "Beta";
    static string IIndexName.DisplayString => DisplayString;
}

public readonly struct Gamma : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "Gamma";
    static string IIndexName.DisplayString => DisplayString;
}

public readonly struct Delta : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "Delta";
    static string IIndexName.DisplayString => DisplayString;
}

public readonly struct Epsilon : IIndexName
{
    /// <inheritdoc cref="IIndexName.DisplayString"/>
    public const string DisplayString = "Epsilon";
    static string IIndexName.DisplayString => DisplayString;
}
