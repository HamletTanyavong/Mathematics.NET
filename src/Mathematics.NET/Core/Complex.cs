// <copyright file="Complex.cs" company="Mathematics.NET">
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

public readonly struct Complex<T> : IComplex<Complex<T>, T>
    where T : INumber<T>
{
    private readonly T _real;
    private readonly T _imaginary;

    public Complex(T real, T imaginary)
    {
        _real = real;
        _imaginary = imaginary;
    }

    public T Real => _real;
    public T Imaginary => _imaginary;

    public static Complex<T> operator +(Complex<T> z, Complex<T> w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static Complex<T> operator -(Complex<T> z, Complex<T> w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static Complex<T> operator *(Complex<T> z, Complex<T> w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    public static Complex<T> operator /(Complex<T> z, Complex<T> w)
        => throw new NotImplementedException();

    public static Complex<T> Conjugate(Complex<T> z) => new(z._real, -z._imaginary);
}
