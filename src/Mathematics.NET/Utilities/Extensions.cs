// <copyright file="Extensions.cs" company="Mathematics.NET">
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

using System.Text;

namespace Mathematics.NET.Utilities;

/// <summary>A class containing extension methods for external .NET objects.</summary>
public static class Extensions
{
    /// <summary>Remove specified characters from the end of a string being built by a string builder.</summary>
    /// <param name="builder">A string builder instance.</param>
    /// <param name="unwantedChars">An array of characters to trim.</param>
    /// <returns>The same string builder with characters removed.</returns>
    public static StringBuilder TrimEnd(this StringBuilder builder, params char[]? unwantedChars)
    {
        if (unwantedChars == null || builder.Length == 0 || unwantedChars.Length == 0)
            return builder;

        int i = builder.Length - 1;
        while (i >= 0)
        {
            if (!unwantedChars.Contains(builder[i]))
                break;
            i--;
        }
        if (i < builder.Length - 1)
            builder.Length = ++i;

        return builder;
    }
}
