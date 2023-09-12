﻿// <copyright file="Real.cs" company="Mathematics.NET">
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

using System.Numerics;

namespace Mathematics.NET.Core;

/// <summary>Represents a real number</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public readonly struct Real<T> : IReal<Real<T>, T>
    where T : IFloatingPointIeee754<T>
{
    private readonly T _real;

    public Real(T real)
    {
        _real = real;
    }

    // Real number properties

    public Real<T> Re => _real;

    // Operators

    public static Real<T> operator -(Real<T> value) => -value._real;
    public static Real<T> operator +(Real<T> left, Real<T> right) => left._real + right._real;
    public static Real<T> operator -(Real<T> left, Real<T> right) => left._real - right._real;
    public static Real<T> operator *(Real<T> left, Real<T> right) => left._real * right._real;
    public static Real<T> operator /(Real<T> left, Real<T> right) => left._real / right._real;

    // Methods

    public static Real<T> Conjugate(Real<T> z) => throw new NotImplementedException();
}
