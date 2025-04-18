// <copyright file="Variable.cs" company="Mathematics.NET">
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

using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a variable used in reverse-mode automatic differentiation.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe record struct Variable<T>
    where T : IComplex<T>
{
    /// <summary>The index of the variable.</summary>
    public readonly int Index;

    /// <summary>The value of the variable.</summary>
    public readonly T Value;

    internal Variable(int index, T value)
    {
        Index = index;
        Value = value;
    }

    public override string? ToString() => Value.ToString();
}
