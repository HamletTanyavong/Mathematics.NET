// <copyright file="Wheel2357_Generic.cs" company="Mathematics.NET">
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
using Mathematics.NET.NumberTheory;

namespace Mathematics.NET.Benchmarks.Implementations.NumberTheory;

public sealed class Wheel2357_Generic<T> : Wheel<T>
    where T : IBinaryInteger<T>
{
    public static readonly T[] s_basis = [.. new int[] { 2, 3, 5, 7 }.Select(x => T.CreateSaturating(x))];
    public static readonly T[] s_spokes = [.. new int[] { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 121, 127, 131, 137, 139, 143, 149, 151, 157, 163, 167, 169, 173, 179, 181, 187, 191, 193, 197, 199, 209, 211, 221 }.Select(x => T.CreateSaturating(x))];
    public static readonly T[] s_increments = [.. new int[] { 2, 4, 2, 4, 6, 2, 6, 4, 2, 4, 6, 6, 2, 6, 4, 2, 6, 4, 6, 8, 4, 2, 4, 2, 4, 8, 6, 4, 6, 2, 4, 6, 2, 6, 6, 4, 2, 4, 6, 2, 6, 4, 2, 4, 2, 10, 2, 10 }.Select(x => T.CreateSaturating(x))];

    public Wheel2357_Generic()
    {
        Size = T.One;
        for (int i = 0; i < s_basis.Length; i++)
        {
            Size *= s_basis[i];
        }
    }

    public override T[] Basis => s_basis;

    public override T[] Spokes => s_spokes;

    public override T[] Increments => s_increments;

    public override T Size { get; }

    public override int SpokeCount => s_spokes.Length;

    public override int IncrementCount => s_increments.Length;

    public override IEnumerable<T> Spin()
    {
        var value = s_spokes[0];
        yield return value;

        for (int i = 0; (value += s_increments[i]) > T.Zero; i = i < s_increments.Length - 1 ? i + 1 : 0)
        {
            yield return value;
        }
    }

    public override T this[int i]
    {
        get
        {
            var (quotient, remainder) = int.DivRem(i, IncrementCount);
            return Size * T.CreateSaturating(quotient) + s_spokes[remainder];
        }
    }
}
