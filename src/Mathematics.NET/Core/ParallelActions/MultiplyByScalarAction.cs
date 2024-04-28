// <copyright file="MultiplyByScalarAction.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance.Helpers;

namespace Mathematics.NET.Core.ParallelActions;

/// <summary>An action for multiplying items by a scalar</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/></typeparam>
/// <param name="factor">The factor by which to multiply items</param>
public readonly struct MultiplyByScalarAction<T>(T factor) : IRefAction<T>
    where T : IComplex<T>
{
    private readonly T _factor = factor;

    /// <summary>Executes the action on an item of type <typeparamref name="T"/></summary>
    /// <param name="item">The item</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Invoke(ref T item) => item *= _factor;
}
