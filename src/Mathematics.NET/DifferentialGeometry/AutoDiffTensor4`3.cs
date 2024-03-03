// <copyright file="AutoDiffTensor4`3.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;

/// <summary>Represents a rank-one tensor of four variables for use in forward-mode automatic differentiation</summary>
/// <typeparam name="T">A type that implements <see cref="IDual{T, U}"/></typeparam>
/// <typeparam name="U">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <typeparam name="V">An index</typeparam>
public record struct AutoDiffTensor4<T, U, V>
    where T : IDual<T, U>
    where U : IComplex<U>, IDifferentiableFunctions<U>
    where V : IIndex
{
    /// <summary>The zeroth element of the rank-one tensor</summary>
    public T X0;

    /// <summary>The first element of the rank-one tensor</summary>
    public T X1;

    /// <summary>The second element of the rank-one tensor</summary>
    public T X2;

    /// <summary>The third element of the rank-one tensor</summary>
    public T X3;

    public AutoDiffTensor4(T x0, T x1, T x2, T x3)
    {
        X0 = x0;
        X1 = x1;
        X2 = x2;
        X3 = x3;
    }

    //
    // Indexer
    //

    public T this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static T GetElement(AutoDiffTensor4<T, U, V> tensor, int index)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref tensor, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe(ref AutoDiffTensor4<T, U, V> tensor, int index)
    {
        Debug.Assert(index is >= 0 and < 4);
        return Unsafe.Add(ref Unsafe.As<AutoDiffTensor4<T, U, V>, T>(ref tensor), index);
    }

    // Set

    internal static AutoDiffTensor4<T, U, V> WithElement(AutoDiffTensor4<T, U, V> tensor, int index, T value)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        AutoDiffTensor4<T, U, V> result = tensor;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref AutoDiffTensor4<T, U, V> tensor, int index, T value)
    {
        Debug.Assert(index is >= 0 and < 4);
        Unsafe.Add(ref Unsafe.As<AutoDiffTensor4<T, U, V>, T>(ref tensor), index) = value;
    }

    //
    // Methods
    //

    //
    // Formatting
    //

    public override readonly string ToString() => $"({X0}, {X1}, {X2}, {X3})";
}
