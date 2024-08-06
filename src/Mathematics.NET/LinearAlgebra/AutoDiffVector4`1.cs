// <copyright file="AutoDiffVector4`1.cs" company="Mathematics.NET">
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
using System.Runtime.InteropServices;
using Mathematics.NET.AutoDiff;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a vector of four variables for use in reverse-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public record struct AutoDiffVector4<T>
    where T : IComplex<T>
{
    /// <summary>The first element of the vector.</summary>
    public Variable<T> X1;

    /// <summary>The second element of the vector.</summary>
    public Variable<T> X2;

    /// <summary>The third element of the vector.</summary>
    public Variable<T> X3;

    /// <summary>The fourth element of the vector.</summary>
    public Variable<T> X4;

    public AutoDiffVector4(Variable<T> x1, Variable<T> x2, Variable<T> x3, Variable<T> x4)
    {
        X1 = x1;
        X2 = x2;
        X3 = x3;
        X4 = x4;
    }

    //
    // Indexer
    //

    /// <summary>Get the element at the specified index.</summary>
    /// <param name="index">An index.</param>
    /// <returns>The element at the index.</returns>
    public Variable<T> this[int index]
    {
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static Variable<T> GetElement(AutoDiffVector4<T> vector, int index)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Variable<T> GetElementUnsafe(ref AutoDiffVector4<T> vector, int index)
    {
        Debug.Assert(index is >= 0 and < 4);
        return Unsafe.Add(ref Unsafe.As<AutoDiffVector4<T>, Variable<T>>(ref vector), index);
    }

    // Set

    internal static AutoDiffVector4<T> WithElement(AutoDiffVector4<T> vector, int index, Variable<T> value)
    {
        if ((uint)index >= 4)
        {
            throw new IndexOutOfRangeException();
        }

        AutoDiffVector4<T> result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref AutoDiffVector4<T> vector, int index, Variable<T> value)
    {
        Debug.Assert(index is >= 0 and < 4);
        Unsafe.Add(ref Unsafe.As<AutoDiffVector4<T>, Variable<T>>(ref vector), index) = value;
    }

    //
    // Formatting
    //

    public override readonly string ToString() => $"({X1}, {X2}, {X3}, {X4})";
}
