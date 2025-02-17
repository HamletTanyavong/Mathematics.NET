// <copyright file="AutoDiffTensor3`2.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-one tensor of three variables for use in reverse-mode automatic differentiation.</summary>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/>.</typeparam>
/// <typeparam name="TI">An index.</typeparam>
public record struct AutoDiffTensor3<TN, TI>
    where TN : IComplex<TN>
    where TI : IIndex
{
    /// <summary>The zeroth element of the rank-one tensor.</summary>
    public Variable<TN> X0;

    /// <summary>The first element of the rank-one tensor.</summary>
    public Variable<TN> X1;

    /// <summary>The second element of the rank-one tensor.</summary>
    public Variable<TN> X2;

    public AutoDiffTensor3(Variable<TN> x0, Variable<TN> x1, Variable<TN> x2)
    {
        X0 = x0;
        X1 = x1;
        X2 = x2;
    }

    //
    // Indexer
    //

    /// <summary>Get the element at the specified index.</summary>
    /// <param name="index">An index.</param>
    /// <returns>The element at the index.</returns>
    public Variable<TN> this[int index]
    {
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static Variable<TN> GetElement(AutoDiffTensor3<TN, TI> tensor, int index)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref tensor, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Variable<TN> GetElementUnsafe(ref AutoDiffTensor3<TN, TI> tensor, int index)
    {
        Debug.Assert(index is >= 0 and < 3);
        return Unsafe.Add(ref Unsafe.As<AutoDiffTensor3<TN, TI>, Variable<TN>>(ref tensor), index);
    }

    // Set

    internal static AutoDiffTensor3<TN, TI> WithElement(AutoDiffTensor3<TN, TI> tensor, int index, Variable<TN> value)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();
        AutoDiffTensor3<TN, TI> result = tensor;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref AutoDiffTensor3<TN, TI> tensor, int index, Variable<TN> value)
    {
        Debug.Assert(index is >= 0 and < 3);
        Unsafe.Add(ref Unsafe.As<AutoDiffTensor3<TN, TI>, Variable<TN>>(ref tensor), index) = value;
    }

    //
    // Formatting
    //

    public override readonly string ToString() => $"({X0}, {X1}, {X2})";
}
