// <copyright file="Dual.cs" company="Mathematics.NET">
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

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a dual number.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="d0">The primal part of the dual number.</param>
/// <param name="d1">The tangent part of the dual number.</param>
[Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Dual<T, U>(T d0, T d1) : IDual<Dual<T, U>, T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private readonly T _d0 = d0;
    private readonly T _d1 = d1;

    public Dual(T value) : this(value, T.Zero) { }

    public readonly T D0 => _d0;

    /// <summary>Represents the tangent part of the dual number.</summary>
    public readonly T D1 => _d1;

    //
    // Operators
    //

    public static Dual<T, U> operator +(Dual<T, U> x) => x;

    public static Dual<T, U> operator -(Dual<T, U> x) => new(-x._d0, -x._d1);

    public static Dual<T, U> operator +(Dual<T, U> x, Dual<T, U> y) => new(x._d0 + y._d0, x._d1 + y._d1);

    public static Dual<T, U> operator +(T c, Dual<T, U> x) => new(c + x._d0, x._d1);

    public static Dual<T, U> operator +(Dual<T, U> x, T c) => new(x._d0 + c, x._d1);

    public static Dual<T, U> operator -(Dual<T, U> x, Dual<T, U> y) => new(x._d0 - y._d0, x._d1 - y._d1);

    public static Dual<T, U> operator -(T c, Dual<T, U> x) => new(c - x._d0, -x._d1);

    public static Dual<T, U> operator -(Dual<T, U> x, T c) => new(x._d0 - c, x._d1);

    public static Dual<T, U> operator *(Dual<T, U> x, Dual<T, U> y) => new(x._d0 * y._d0, x._d0 * y._d1 + y._d0 * x._d1);

    public static Dual<T, U> operator *(T c, Dual<T, U> x) => new(c * x._d0, c * x._d1);

    public static Dual<T, U> operator *(Dual<T, U> x, T c) => new(x._d0 * c, c * x._d1);

    public static Dual<T, U> operator /(Dual<T, U> x, Dual<T, U> y)
        => new(x._d0 / y._d0, (x._d1 * y._d0 - y._d1 * x._d0) / (y._d0 * y._d0));

    public static Dual<T, U> operator /(T c, Dual<T, U> x) => new(c / x._d0, -x._d1 * c / (x._d0 * x._d0));

    public static Dual<T, U> operator /(Dual<T, U> x, T c) => new(x._d0 / c, x._d1 / c);

    public static Dual<Real<U>, U> Modulo(in Dual<Real<U>, U> x, in Dual<Real<U>, U> y)
        => new(x._d0 % y._d0, x._d1 - y._d1 * Real<U>.Floor(x._d0 / y._d0));

    public static Dual<Real<U>, U> Modulo(in Dual<Real<U>, U> x, Real<U> c) => new(x._d0 % c, x._d1);

    public static Dual<Real<U>, U> Modulo(Real<U> c, in Dual<Real<U>, U> x) => new(c % x._d0, -x._d1 * Real<U>.Floor(c / x._d0));

    //
    // Equality
    //

    public static bool operator ==(Dual<T, U> left, Dual<T, U> right) => left._d0 == right._d0 && left._d1 == right._d1;

    public static bool operator !=(Dual<T, U> left, Dual<T, U> right) => left._d0 != right._d0 && left._d1 != right._d1;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Dual<T, U> other && Equals(other);

    public bool Equals(Dual<T, U> value) => _d0.Equals(value._d0) && _d1.Equals(value._d1);

    public override int GetHashCode() => HashCode.Combine(_d0, _d0);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? formatProvider) => $"({_d0}, {_d1})";

    //
    // Other operations
    //

    public static Dual<T, U> CreateVariable(T value) => new(value, T.Zero);

    public static Dual<T, U> CreateVariable(T value, T seed) => new(value, seed);

    /// <inheritdoc cref="IReal{T, U, V}.Ceiling(T)"/>
    public static Dual<Real<U>, U> Ceiling(in Dual<Real<U>, U> x) => new(Real<U>.Ceiling(x._d0));

    /// <inheritdoc cref="IReal{T, U, V}.Floor(T)"/>
    public static Dual<Real<U>, U> Floor(in Dual<Real<U>, U> x) => new(Real<U>.Floor(x._d0));

    public Dual<T, U> WithSeed(T seed) => new(_d0, seed);

    // Exponential functions

    public static Dual<T, U> Exp(Dual<T, U> x)
    {
        var exp = T.Exp(x._d0);
        return new(exp, x._d1 * exp);
    }

    public static Dual<T, U> Exp2(Dual<T, U> x)
    {
        var exp2 = T.Exp2(x._d0);
        return new(exp2, (U)Real<U>.Ln2 * x._d1 * exp2);
    }

    public static Dual<T, U> Exp10(Dual<T, U> x)
    {
        var exp10 = T.Exp10(x._d0);
        return new(exp10, (U)Real<U>.Ln10 * x._d1 * exp10);
    }

    // Hyperbolic functions

    public static Dual<T, U> Acosh(Dual<T, U> x)
        => new(T.Acosh(x._d0), x._d1 / (T.Sqrt(x._d0 - T.One) * T.Sqrt(x._d0 + T.One)));

    public static Dual<T, U> Asinh(Dual<T, U> x)
        => new(T.Asinh(x._d0), x._d1 / T.Sqrt(x._d0 * x._d0 + T.One));

    public static Dual<T, U> Atanh(Dual<T, U> x)
        => new(T.Atanh(x._d0), x._d1 / (T.One - x._d0 * x._d0));

    public static Dual<T, U> Cosh(Dual<T, U> x) => new(T.Cosh(x._d0), x._d1 * T.Sinh(x._d0));

    public static Dual<T, U> Sinh(Dual<T, U> x) => new(T.Sinh(x._d0), x._d1 * T.Cosh(x._d0));

    public static Dual<T, U> Tanh(Dual<T, U> x)
    {
        var u = T.One / T.Cosh(x._d0);
        return new(T.Tanh(x._d0), x._d1 * u * u);
    }

    // Logarithmic functions

    public static Dual<T, U> Ln(Dual<T, U> x) => new(T.Ln(x._d0), x._d1 / x._d0);

    public static Dual<T, U> Log(Dual<T, U> x, Dual<T, U> b) => Ln(x) / Ln(b);

    public static Dual<T, U> Log2(Dual<T, U> x) => new(T.Log2(x._d0), x._d1 / ((U)Real<U>.Ln2 * x._d0));

    public static Dual<T, U> Log10(Dual<T, U> x) => new(T.Log10(x._d0), x._d1 / ((U)Real<U>.Ln10 * x._d0));

    // Power functions

    public static Dual<T, U> Pow(Dual<T, U> x, Dual<T, U> y) => Exp(y * Ln(x));

    public static Dual<T, U> Pow(Dual<T, U> x, T y) => new(T.Pow(x._d0, y), x._d1 * y * T.Pow(x._d0, y - T.One));

    public static Dual<T, U> Pow(T x, Dual<T, U> y)
    {
        var pow = T.Pow(x, y._d0);
        return new(pow, y._d1 * T.Ln(x) * pow);
    }

    // Root functions

    public static Dual<T, U> Cbrt(Dual<T, U> x)
    {
        var cbrt = T.Cbrt(x._d0);
        return new(cbrt, x._d1 / (U.CreateSaturating(3) * cbrt * cbrt));
    }

    public static Dual<T, U> Root(Dual<T, U> x, Dual<T, U> n) => Exp(Ln(x) / n);

    public static Dual<T, U> Sqrt(Dual<T, U> x)
    {
        var sqrt = T.Sqrt(x._d0);
        return new(sqrt, U.CreateSaturating(0.5) * x._d1 / sqrt);
    }

    // Trigonometric functions

    public static Dual<T, U> Acos(Dual<T, U> x) => new(T.Acos(x._d0), -x._d1 / T.Sqrt(T.One - x._d0 * x._d0));

    public static Dual<T, U> Asin(Dual<T, U> x) => new(T.Asin(x._d0), x._d1 / T.Sqrt(T.One - x._d0 * x._d0));

    public static Dual<T, U> Atan(Dual<T, U> x) => new(T.Atan(x._d0), x._d1 / (T.One + x._d0 * x._d0));

    /// <inheritdoc cref="IReal{T, U, V}.Atan2(Real{V}, Real{V})"/>
    public static Dual<Real<U>, U> Atan2(in Dual<Real<U>, U> y, in Dual<Real<U>, U> x)
    {
        var u = Real<U>.One / (x._d0 * x._d0 + y._d0 * y._d0);
        return new(Real<U>.Atan2(y._d0, x._d0), (y._d1 * x._d0 - x._d1 * y._d0) * u);
    }

    public static Dual<T, U> Cos(Dual<T, U> x) => new(T.Cos(x._d0), -x._d1 * T.Sin(x._d0));

    public static Dual<T, U> Sin(Dual<T, U> x) => new(T.Sin(x._d0), x._d1 * T.Cos(x._d0));

    public static Dual<T, U> Tan(Dual<T, U> x)
    {
        var sec = T.One / T.Cos(x._d0);
        return new(T.Tan(x._d0), x._d1 * sec * sec);
    }

    //
    // Custom operations
    //

    /// <summary>Perform forward-mode autodiff using a custom unary operation.</summary>
    /// <param name="x">A dual number.</param>
    /// <param name="f">A function.</param>
    /// <param name="df">The derivative of the function.</param>
    /// <returns>A dual number.</returns>
    public static Dual<T, U> Operation(Dual<T, U> x, Func<T, T> f, Func<T, T> df) => new(f(x._d0), x._d1 * df(x._d0));

    /// <summary>Perform forward-mode autodiff using a custom binary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The derivative of the function with respect to the left variable.</param>
    /// <param name="dfy">The derivative of the function with respect to the right variable.</param>
    /// <returns>A dual number.</returns>
    public static Dual<T, U> Operation(Dual<T, U> x, Dual<T, U> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
        => new(f(x._d0, y._d0), dfy(x._d0, y._d0) * x._d1 + dfx(x._d0, y._d1) * y._d1);

    //
    // Implicit Operators
    //

    public static implicit operator Dual<T, U>(T value) => new(value);

    public static implicit operator Dual<T, U>(U value) => new(value);
}
