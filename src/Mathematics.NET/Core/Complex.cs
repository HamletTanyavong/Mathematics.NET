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

#pragma warning disable IDE0032

using System.Numerics;

namespace Mathematics.NET.Core;

/// <summary>Represents a complex number</summary>
/// <typeparam name="T">A type that implements <see cref="IFloatingPointIeee754{TSelf}"/></typeparam>
public readonly struct Complex<T> : IComplex<Complex<T>, T>
    where T : IFloatingPointIeee754<T>
{
    private readonly Real<T> _real;
    private readonly Real<T> _imaginary;

    public Complex(Real<T> real)
    {
        _real = real;
        _imaginary = T.Zero;
    }

    public Complex(Real<T> real, Real<T> imaginary)
    {
        _real = real;
        _imaginary = imaginary;
    }

    // Complex number properties

    public Real<T> Re => _real;
    public Real<T> Im => _imaginary;

    // Constants

    public static Complex<T> Zero => new(Real<T>.Zero, Real<T>.Zero);
    public static Complex<T> One => new(Real<T>.One, Real<T>.Zero);

    // Operators

    public static Complex<T> operator -(Complex<T> z) => new(-z._real, -z._imaginary);

    public static Complex<T> operator +(Complex<T> z, Complex<T> w) => new(z._real + w._real, z._imaginary + w._imaginary);

    public static Complex<T> operator -(Complex<T> z, Complex<T> w) => new(z._real - w._real, z._imaginary - w._imaginary);

    public static Complex<T> operator *(Complex<T> z, Complex<T> w)
        => new(z._real * w._real - z._imaginary * w._imaginary, z._real * w._imaginary + w._real * z._imaginary);

    public static Complex<T> operator /(Complex<T> z, Complex<T> w)
        => throw new NotImplementedException();

    // Methods

    public static Complex<T> Conjugate(Complex<T> z) => new(z._real, -z._imaginary);

    // Implicit Operators

    public static implicit operator Complex<T>(T x) => new(x);
}
