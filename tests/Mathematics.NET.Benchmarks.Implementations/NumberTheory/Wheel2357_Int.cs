// <copyright file="Wheel2357_Int.cs" company="Mathematics.NET">
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

using Mathematics.NET.NumberTheory;

namespace Mathematics.NET.Benchmarks.Implementations.NumberTheory;

public sealed class Wheel2357_Int : Wheel<int>
{
    public static readonly int[] s_basis = [2, 3, 5, 7];
    public static readonly int[] s_spokes = [11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 121, 127, 131, 137, 139, 143, 149, 151, 157, 163, 167, 169, 173, 179, 181, 187, 191, 193, 197, 199, 209, 211, 221];
    public static readonly int[] s_increments = [2, 4, 2, 4, 6, 2, 6, 4, 2, 4, 6, 6, 2, 6, 4, 2, 6, 4, 6, 8, 4, 2, 4, 2, 4, 8, 6, 4, 6, 2, 4, 6, 2, 6, 6, 4, 2, 4, 6, 2, 6, 4, 2, 4, 2, 10, 2, 10];

    public Wheel2357_Int()
    {
        Size = 1;
        for (int i = 0; i < s_basis.Length; i++)
        {
            Size *= s_basis[i];
        }
    }

    public override int[] Basis => s_basis;

    public override int[] Spokes => s_spokes;

    public override int[] Increments => s_increments;

    public override int Size { get; }

    public override int SpokeCount => s_spokes.Length;

    public override int IncrementCount => s_increments.Length;

    public override IEnumerable<int> Spin()
    {
        var value = s_spokes[0];
        yield return value;

        for (int i = 0; (value += s_increments[i]) > 0; i = i < s_increments.Length - 1 ? i + 1 : 0)
        {
            yield return value;
        }
    }

    public override int this[int i]
    {
        get
        {
            var (quotient, remainder) = int.DivRem(i, IncrementCount);
            return Size * quotient + s_spokes[remainder];
        }
    }
}
