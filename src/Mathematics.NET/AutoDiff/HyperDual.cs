// <copyright file="HyperDual.cs" company="Mathematics.NET">
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

/// <summary>Represents a hyper-dual number.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="d0">The primal part of a hyper-dual number.</param>
/// <param name="d1">The tangent part of a hyper-dual number.</param>
[Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct HyperDual<T, U>(Dual<T, U> d0, Dual<T, U> d1) : IDual<HyperDual<T, U>, T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private readonly Dual<T, U> _d0 = d0;
    private readonly Dual<T, U> _d1 = d1;

    public HyperDual(Dual<T, U> value) : this(value, new(T.Zero)) { }

    /// <summary>Represents the primal part of the hyper-dual number.</summary>
    public T D0 => _d0.D0;

    /// <summary>Represents the first tangent part of the hyper-dual number.</summary>
    public T D1 => _d0.D1;

    /// <summary>Represents the second tangent part of the hyper-dual number.</summary>
    public T D2 => _d1.D0;

    /// <summary>Represents the third tangent part, the cross term, of a hyper-dual number.</summary>
    public T D3 => _d1.D1;

    internal Dual<T, U> Primal => _d0;

    internal Dual<T, U> Tangent => _d1;

    //
    // Operators
    //

    public static HyperDual<T, U> operator +(HyperDual<T, U> x) => x;

    public static HyperDual<T, U> operator -(HyperDual<T, U> x) => new(-x._d0, -x._d1);

    public static HyperDual<T, U> operator +(HyperDual<T, U> x, HyperDual<T, U> y) => new(x._d0 + y._d0, x._d1 + y._d1);

    public static HyperDual<T, U> operator +(T c, HyperDual<T, U> x) => new(c + x._d0, x._d1);

    public static HyperDual<T, U> operator +(HyperDual<T, U> x, T c) => new(x._d0 + c, x._d1);

    public static HyperDual<T, U> operator -(HyperDual<T, U> x, HyperDual<T, U> y) => new(x._d0 - y._d0, x._d1 - y._d1);

    public static HyperDual<T, U> operator -(T c, HyperDual<T, U> x) => new(c - x._d0, -x._d1);

    public static HyperDual<T, U> operator -(HyperDual<T, U> x, T c) => new(x._d0 - c, x._d1);

    public static HyperDual<T, U> operator *(HyperDual<T, U> x, HyperDual<T, U> y) => new(x._d0 * y._d0, x._d0 * y._d1 + y._d0 * x._d1);

    public static HyperDual<T, U> operator *(T c, HyperDual<T, U> x) => new(c * x._d0, c * x._d1);

    public static HyperDual<T, U> operator *(HyperDual<T, U> x, T c) => new(x._d0 * c, c * x._d1);

    public static HyperDual<T, U> operator /(HyperDual<T, U> x, HyperDual<T, U> y)
        => new(x._d0 / y._d0, (x._d1 * y._d0 - y._d1 * x._d0) / (y._d0 * y._d0));

    public static HyperDual<T, U> operator /(T c, HyperDual<T, U> x) => new(c / x._d0, -x._d1 * c / (x._d0 * x._d0));

    public static HyperDual<T, U> operator /(HyperDual<T, U> x, T c) => new(x._d0 / c, x._d1 / c);

    public static HyperDual<Real<U>, U> Modulo(in HyperDual<Real<U>, U> x, in HyperDual<Real<U>, U> y)
        => new(Dual<Real<U>, U>.Modulo(x._d0, y._d0), x._d1 - y._d1 * Dual<Real<U>, U>.Floor(x._d0 / y._d0));

    public static HyperDual<Real<U>, U> Modulo(in HyperDual<Real<U>, U> x, in Dual<Real<U>, U> c) => new(Dual<Real<U>, U>.Modulo(x._d0, c), x._d1);

    public static HyperDual<Real<U>, U> Modulo(in Dual<Real<U>, U> c, in HyperDual<Real<U>, U> x) => new(Dual<Real<U>, U>.Modulo(c, x._d0), -x._d1 * Dual<Real<U>, U>.Floor(c / x._d0));

    //
    // Equality
    //

    public static bool operator ==(HyperDual<T, U> left, HyperDual<T, U> right) => left._d0 == right._d0 && left._d1 == right._d1;

    public static bool operator !=(HyperDual<T, U> left, HyperDual<T, U> right) => left._d0 != right._d0 && left._d1 != right._d1;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is HyperDual<T, U> other && Equals(other);

    public bool Equals(HyperDual<T, U> value) => _d0.Equals(value._d0) && _d1.Equals(value._d1);

    public override int GetHashCode() => HashCode.Combine(_d0, _d0);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? formatProvider) => $"({_d0.D0}, {_d0.D1}, {_d1.D0} {_d1.D1})";

    //
    // Other operations
    //

    public static HyperDual<T, U> CreateVariable(T value) => new(new(value, T.Zero));

    public static HyperDual<T, U> CreateVariable(T value, T seed) => new(new(value, seed));

    internal static HyperDual<T, U> CreateVariable(Dual<T, U> value, T seed) => new(value, seed);

    /// <summary>Create an instance of the type with a specified value and two seed values.</summary>
    /// <remarks>Use this to compute mixed second derivatives.</remarks>
    /// <param name="value">A value.</param>
    /// <param name="e1Seed">A seed value for the first variable of interest.</param>
    /// <param name="e2Seed">A seec value for the second variable of interest.</param>
    /// <returns>An instance of the type.</returns>
    public static HyperDual<T, U> CreateVariable(T value, T e1Seed, T e2Seed) => new(new(value, e1Seed), new(e2Seed));

    /// <inheritdoc cref="IReal{T, U, V}.Ceiling(T)"/>
    public static HyperDual<Real<U>, U> Ceiling(in HyperDual<Real<U>, U> x) => new(Dual<Real<U>, U>.Ceiling(x._d0));

    /// <inheritdoc cref="IReal{T, U, V}.Floor(T)"/>
    public static HyperDual<Real<U>, U> Floor(in HyperDual<Real<U>, U> x) => new(Dual<Real<U>, U>.Floor(x._d0));

    public HyperDual<T, U> WithSeed(T seed) => new(new(_d0.D0, seed));

    /// <summary>Create a seeded instance of this type.</summary>
    /// <param name="e1Seed">The first seed.</param>
    /// <param name="e2Seed">The second seed.</param>
    /// <returns>A seeded value.</returns>
    public HyperDual<T, U> WithSeed(T e1Seed, T e2Seed) => new(new(_d0.D0, e1Seed), new(e2Seed));

    // Exponential functions

    public static HyperDual<T, U> Exp(HyperDual<T, U> x)
    {
        var exp = Dual<T, U>.Exp(x._d0);
        return new(exp, x._d1 * exp);
    }

    public static HyperDual<T, U> Exp2(HyperDual<T, U> x)
    {
        var exp2 = Dual<T, U>.Exp2(x._d0);
        return new(exp2, (U)Real<U>.Ln2 * x._d1 * exp2);
    }

    public static HyperDual<T, U> Exp10(HyperDual<T, U> x)
    {
        var exp10 = Dual<T, U>.Exp10(x._d0);
        return new(exp10, (U)Real<U>.Ln10 * x._d1 * exp10);
    }

    // Hyperbolic functions

    public static HyperDual<T, U> Acosh(HyperDual<T, U> x)
        => new(Dual<T, U>.Acosh(x._d0), x._d1 / (Dual<T, U>.Sqrt(x._d0 - T.One) * Dual<T, U>.Sqrt(x._d0 + T.One)));

    public static HyperDual<T, U> Asinh(HyperDual<T, U> x)
        => new(Dual<T, U>.Asinh(x._d0), x._d1 / Dual<T, U>.Sqrt(x._d0 * x._d0 + T.One));

    public static HyperDual<T, U> Atanh(HyperDual<T, U> x)
        => new(Dual<T, U>.Atanh(x._d0), x._d1 / (T.One - x._d0 * x._d0));

    public static HyperDual<T, U> Cosh(HyperDual<T, U> x) => new(Dual<T, U>.Cosh(x._d0), x._d1 * Dual<T, U>.Sinh(x._d0));

    public static HyperDual<T, U> Sinh(HyperDual<T, U> x) => new(Dual<T, U>.Sinh(x._d0), x._d1 * Dual<T, U>.Cosh(x._d0));

    public static HyperDual<T, U> Tanh(HyperDual<T, U> x)
    {
        var u = T.One / Dual<T, U>.Cosh(x._d0);
        return new(Dual<T, U>.Tanh(x._d0), x._d1 * u * u);
    }

    // Logarithmic functions

    public static HyperDual<T, U> Ln(HyperDual<T, U> x) => new(Dual<T, U>.Ln(x._d0), x._d1 / x._d0);

    public static HyperDual<T, U> Log(HyperDual<T, U> x, HyperDual<T, U> b) => Ln(x) / Ln(b);

    public static HyperDual<T, U> Log2(HyperDual<T, U> x) => new(Dual<T, U>.Log2(x._d0), x._d1 / ((U)Real<U>.Ln2 * x._d0));

    public static HyperDual<T, U> Log10(HyperDual<T, U> x) => new(Dual<T, U>.Log10(x._d0), x._d1 / ((U)Real<U>.Ln10 * x._d0));

    // Power functions

    public static HyperDual<T, U> Pow(HyperDual<T, U> x, HyperDual<T, U> y) => Exp(y * Ln(x));

    public static HyperDual<T, U> Pow(HyperDual<T, U> x, T y) => new(Dual<T, U>.Pow(x._d0, y), x._d1 * y * Dual<T, U>.Pow(x._d0, y - T.One));

    public static HyperDual<T, U> Pow(T x, HyperDual<T, U> y)
    {
        var pow = Dual<T, U>.Pow(x, y._d0);
        return new(pow, y._d1 * T.Ln(x) * pow);
    }

    // Root functions

    public static HyperDual<T, U> Cbrt(HyperDual<T, U> x)
    {
        var cbrt = Dual<T, U>.Cbrt(x._d0);
        return new(cbrt, x._d1 / (U.CreateSaturating(3) * cbrt * cbrt));
    }

    public static HyperDual<T, U> Root(HyperDual<T, U> x, HyperDual<T, U> n) => Exp(Ln(x) / n);

    public static HyperDual<T, U> Sqrt(HyperDual<T, U> x)
    {
        var sqrt = Dual<T, U>.Sqrt(x._d0);
        return new(sqrt, U.CreateSaturating(0.5) * x._d1 / sqrt);
    }

    // Trigonometric functions

    public static HyperDual<T, U> Acos(HyperDual<T, U> x) => new(Dual<T, U>.Acos(x._d0), -x._d1 / Dual<T, U>.Sqrt(T.One - x._d0 * x._d0));

    public static HyperDual<T, U> Asin(HyperDual<T, U> x) => new(Dual<T, U>.Asin(x._d0), x._d1 / Dual<T, U>.Sqrt(T.One - x._d0 * x._d0));

    public static HyperDual<T, U> Atan(HyperDual<T, U> x) => new(Dual<T, U>.Atan(x._d0), x._d1 / (T.One + x._d0 * x._d0));

    /// <inheritdoc cref="IReal{T, U, V}.Atan2(Real{V}, Real{V})"/>
    public static HyperDual<Real<U>, U> Atan2(in HyperDual<Real<U>, U> y, in HyperDual<Real<U>, U> x)
    {
        var u = Real<U>.One / (x._d0 * x._d0 + y._d0 * y._d0);
        return new(Dual<T, U>.Atan2(y._d0, x._d0), (y._d1 * x._d0 - x._d1 * y._d0) * u);
    }

    public static HyperDual<T, U> Cos(HyperDual<T, U> x) => new(Dual<T, U>.Cos(x._d0), -x._d1 * Dual<T, U>.Sin(x._d0));

    public static HyperDual<T, U> Sin(HyperDual<T, U> x) => new(Dual<T, U>.Sin(x._d0), x._d1 * Dual<T, U>.Cos(x._d0));

    public static HyperDual<T, U> Tan(HyperDual<T, U> x)
    {
        var sec = T.One / Dual<T, U>.Cos(x._d0);
        return new(Dual<T, U>.Tan(x._d0), x._d1 * sec * sec);
    }

    //
    // Custom operations
    //

    /// <summary>Perform forward-mode autodiff using a custom unary operation.</summary>
    /// <param name="x">A hyper-dual number.</param>
    /// <param name="f">A function.</param>
    /// <param name="df">The derivative of the function.</param>
    /// <returns>A hyper-dual number.</returns>
    public static HyperDual<T, U> Operation(HyperDual<T, U> x, Func<Dual<T, U>, Dual<T, U>> f, Func<Dual<T, U>, Dual<T, U>> df)
        => new(f(x._d0), x._d1 * df(x._d0));

    /// <summary>Perform forward-mode autodiff using a custom binary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The derivative of the function with respect to the left variable.</param>
    /// <param name="dfy">The derivative of the function with respect to the right variable.</param>
    /// <returns>A hyper-dual number.</returns>
    public static HyperDual<T, U> Operation(
        HyperDual<T, U> x,
        HyperDual<T, U> y,
        Func<Dual<T, U>, Dual<T, U>, Dual<T, U>> f,
        Func<Dual<T, U>, Dual<T, U>, Dual<T, U>> dfx,
        Func<Dual<T, U>, Dual<T, U>, Dual<T, U>> dfy)
        => new(f(x._d0, y._d0), dfy(x._d0, y._d0) * x._d1 + dfx(x._d0, y._d1) * y._d1);

    //
    // Implicit Operators
    //

    public static implicit operator HyperDual<T, U>(T value) => new(value);

    public static implicit operator HyperDual<T, U>(U value) => new(value);

    /// <summary>Convert a dual number into a hyper-dual number.</summary>
    /// <param name="value">A dual number.</param>
    public static implicit operator HyperDual<T, U>(Dual<T, U> value) => new(value);
}
