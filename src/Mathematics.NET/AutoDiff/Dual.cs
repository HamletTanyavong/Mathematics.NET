// <copyright file="Dual.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a dual number</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
/// <param name="d0">The primal part of the dual number</param>
/// <param name="d1">The tangent part of the dual number</param>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Dual<T>(T d0, T d1) : IDual<Dual<T>, T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    private readonly T _d0 = d0;
    private readonly T _d1 = d1;

    public Dual(T value) : this(value, T.Zero) { }

    public readonly T D0 => _d0;

    /// <summary>Represents the tangent part of the dual number</summary>
    public readonly T D1 => _d1;

    //
    // Operators
    //

    public static Dual<T> operator +(Dual<T> x) => x;

    public static Dual<T> operator -(Dual<T> x) => new(-x._d0, -x._d1);

    public static Dual<T> operator +(Dual<T> x, Dual<T> y) => new(x._d0 + y._d0, x._d1 + y._d1);

    public static Dual<T> operator +(T c, Dual<T> x) => new(c + x._d0, x._d1);

    public static Dual<T> operator +(Dual<T> x, T c) => new(x._d0 + c, x._d1);

    public static Dual<T> operator -(Dual<T> x, Dual<T> y) => new(x._d0 - y._d0, x._d1 - y._d1);

    public static Dual<T> operator -(T c, Dual<T> x) => new(c - x._d0, -x._d1);

    public static Dual<T> operator -(Dual<T> x, T c) => new(x._d0 - c, x._d1);

    public static Dual<T> operator *(Dual<T> x, Dual<T> y) => new(x._d0 * y._d0, x._d0 * y._d1 + y._d0 * x._d1);

    public static Dual<T> operator *(T c, Dual<T> x) => new(c * x._d0, c * x._d1);

    public static Dual<T> operator *(Dual<T> x, T c) => new(x._d0 * c, c * x._d1);

    public static Dual<T> operator /(Dual<T> x, Dual<T> y)
        => new(x._d0 / y._d0, (x._d1 * y._d0 - y._d1 * x._d0) / (y._d0 * y._d0));

    public static Dual<T> operator /(T c, Dual<T> x) => new(c / x._d0, -x._d1 * c / (x._d0 * x._d0));

    public static Dual<T> operator /(Dual<T> x, T c) => new(x._d0 / c, x._d1 / c);

    public static Dual<Real> Modulo(in Dual<Real> x, in Dual<Real> y)
    {
        return new(x._d0 % y._d0, x._d1 - y._d1 * Real.Floor(x._d0 / y._d0));
    }

    public static Dual<Real> Modulo(in Dual<Real> x, Real c) => new(x._d0 % c, x._d1);

    public static Dual<Real> Modulo(Real c, in Dual<Real> x) => new(c % x._d0, -x._d1 * Real.Floor(c / x._d0));

    //
    // Equality
    //

    public static bool operator ==(Dual<T> left, Dual<T> right) => left._d0 == right._d0 && left._d1 == right._d1;

    public static bool operator !=(Dual<T> left, Dual<T> right) => left._d0 != right._d0 && left._d1 != right._d1;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Dual<T> other && Equals(other);

    public bool Equals(Dual<T> value) => _d0.Equals(value._d0) && _d1.Equals(value._d1);

    public override int GetHashCode() => HashCode.Combine(_d0, _d0);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? formatProvider) => $"({_d0}, {_d1})";

    //
    // Other operations
    //

    public static Dual<T> CreateVariable(T value) => new(value, T.Zero);

    public static Dual<T> CreateVariable(T value, T seed) => new(value, seed);

    public Dual<T> WithSeed(T seed) => new(_d0, seed);

    // Exponential functions

    public static Dual<T> Exp(Dual<T> x)
    {
        var exp = T.Exp(x._d0);
        return new(exp, x._d1 * exp);
    }

    public static Dual<T> Exp2(Dual<T> x)
    {
        var exp2 = T.Exp2(x._d0);
        return new(exp2, Real.Ln2 * x._d1 * exp2);
    }

    public static Dual<T> Exp10(Dual<T> x)
    {
        var exp10 = T.Exp10(x._d0);
        return new(exp10, Real.Ln10 * x._d1 * exp10);
    }

    // Hyperbolic functions

    public static Dual<T> Acosh(Dual<T> x)
        => new(T.Acosh(x._d0), x._d1 / (T.Sqrt(x._d0 - T.One) * T.Sqrt(x._d0 + T.One)));

    public static Dual<T> Asinh(Dual<T> x)
        => new(T.Asinh(x._d0), x._d1 / T.Sqrt(x._d0 * x._d0 + T.One));

    public static Dual<T> Atanh(Dual<T> x)
        => new(T.Atanh(x._d0), x._d1 / (T.One - x._d0 * x._d0));

    public static Dual<T> Cosh(Dual<T> x) => new(T.Cosh(x._d0), x._d1 * T.Sinh(x._d0));

    public static Dual<T> Sinh(Dual<T> x) => new(T.Sinh(x._d0), x._d1 * T.Cosh(x._d0));

    public static Dual<T> Tanh(Dual<T> x)
    {
        var u = T.One / T.Cosh(x._d0);
        return new(T.Tanh(x._d0), x._d1 * u * u);
    }

    // Logarithmic functions

    public static Dual<T> Ln(Dual<T> x) => new(T.Ln(x._d0), x._d1 / x._d0);

    public static Dual<T> Log(Dual<T> x, Dual<T> b) => Ln(x) / Ln(b);

    public static Dual<T> Log2(Dual<T> x) => new(T.Log2(x._d0), x._d1 / (Real.Ln2 * x._d0));

    public static Dual<T> Log10(Dual<T> x) => new(T.Log10(x._d0), x._d1 / (Real.Ln10 * x._d0));

    // Power functions

    public static Dual<T> Pow(Dual<T> x, Dual<T> y) => Exp(y * Ln(x));

    public static Dual<T> Pow(Dual<T> x, T y) => new(T.Pow(x._d0, y), x._d1 * y * T.Pow(x._d0, y - T.One));

    public static Dual<T> Pow(T x, Dual<T> y)
    {
        var pow = T.Pow(x, y._d0);
        return new(pow, y._d1 * T.Ln(x) * pow);
    }

    // Root functions

    public static Dual<T> Cbrt(Dual<T> x)
    {
        var cbrt = T.Cbrt(x._d0);
        return new(cbrt, x._d1 / (3.0 * cbrt * cbrt));
    }

    public static Dual<T> Root(Dual<T> x, Dual<T> n) => Exp(Ln(x) / n);

    public static Dual<T> Sqrt(Dual<T> x)
    {
        var sqrt = T.Sqrt(x._d0);
        return new(sqrt, 0.5 * x._d1 / sqrt);
    }

    // Trigonometric functions

    public static Dual<T> Acos(Dual<T> x) => new(T.Acos(x._d0), -x._d1 / T.Sqrt(T.One - x._d0 * x._d0));

    public static Dual<T> Asin(Dual<T> x) => new(T.Asin(x._d0), x._d1 / T.Sqrt(T.One - x._d0 * x._d0));

    public static Dual<T> Atan(Dual<T> x) => new(T.Atan(x._d0), x._d1 / (T.One + x._d0 * x._d0));

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public static Dual<Real> Atan2(in Dual<Real> y, in Dual<Real> x)
    {
        var u = Real.One / (x._d0 * x._d0 + y._d0 * y._d0);
        return new(Real.Atan2(y._d0, x._d0), (y._d1 * x._d0 - x._d1 * y._d0) * u);
    }

    public static Dual<T> Cos(Dual<T> x) => new(T.Cos(x._d0), -x._d1 * T.Sin(x._d0));

    public static Dual<T> Sin(Dual<T> x) => new(T.Sin(x._d0), x._d1 * T.Cos(x._d0));

    public static Dual<T> Tan(Dual<T> x)
    {
        var sec = T.One / T.Cos(x._d0);
        return new(T.Tan(x._d0), x._d1 * sec * sec);
    }

    //
    // Custom operations
    //

    /// <summary>Perform forward-mode autodiff using a custom unary operation.</summary>
    /// <param name="x">A dual number</param>
    /// <param name="f">A function</param>
    /// <param name="df">The derivative of the function</param>
    /// <returns>A dual number</returns>
    public static Dual<T> CustomOperation(Dual<T> x, Func<T, T> f, Func<T, T> df) => new(f(x._d0), x._d1 * df(x._d0));

    /// <summary>Perform forward-mode autodiff using a custom binary operation.</summary>
    /// <param name="x">A variable</param>
    /// <param name="y">A variable</param>
    /// <param name="f">A function</param>
    /// <param name="dfx">The derivative of the function with respect to the left variable</param>
    /// <param name="dfy">The derivative of the function with respect to the right variable</param>
    /// <returns>A dual number</returns>
    public static Dual<T> CustomOperation(Dual<T> x, Dual<T> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
        => new(f(x._d0, y._d0), dfy(x._d0, y._d0) * x._d1 + dfx(x._d0, y._d1) * y._d1);
}
