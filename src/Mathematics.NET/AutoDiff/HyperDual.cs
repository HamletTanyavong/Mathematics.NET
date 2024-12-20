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
using System.Runtime.InteropServices;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a hyper-dual number.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <param name="d0">The primal part of a hyper-dual number.</param>
/// <param name="d1">The tangent part of a hyper-dual number.</param>
[Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct HyperDual<T>(Dual<T> d0, Dual<T> d1) : IDual<HyperDual<T>, T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    private readonly Dual<T> _d0 = d0;
    private readonly Dual<T> _d1 = d1;

    public HyperDual(Dual<T> value) : this(value, new(T.Zero)) { }

    /// <summary>Represents the primal part of the hyper-dual number.</summary>
    public T D0 => _d0.D0;

    /// <summary>Represents the first tangent part of the hyper-dual number.</summary>
    public T D1 => _d0.D1;

    /// <summary>Represents the second tangent part of the hyper-dual number.</summary>
    public T D2 => _d1.D0;

    /// <summary>Represents the third tangent part, the cross term, of a hyper-dual number.</summary>
    public T D3 => _d1.D1;

    //
    // Operators
    //

    public static HyperDual<T> operator +(HyperDual<T> x) => x;

    public static HyperDual<T> operator -(HyperDual<T> x) => new(-x._d0, -x._d1);

    public static HyperDual<T> operator +(HyperDual<T> x, HyperDual<T> y) => new(x._d0 + y._d0, x._d1 + y._d1);

    public static HyperDual<T> operator +(T c, HyperDual<T> x) => new(c + x._d0, x._d1);

    public static HyperDual<T> operator +(HyperDual<T> x, T c) => new(x._d0 + c, x._d1);

    public static HyperDual<T> operator -(HyperDual<T> x, HyperDual<T> y) => new(x._d0 - y._d0, x._d1 - y._d1);

    public static HyperDual<T> operator -(T c, HyperDual<T> x) => new(c - x._d0, -x._d1);

    public static HyperDual<T> operator -(HyperDual<T> x, T c) => new(x._d0 - c, x._d1);

    public static HyperDual<T> operator *(HyperDual<T> x, HyperDual<T> y) => new(x._d0 * y._d0, x._d0 * y._d1 + y._d0 * x._d1);

    public static HyperDual<T> operator *(T c, HyperDual<T> x) => new(c * x._d0, c * x._d1);

    public static HyperDual<T> operator *(HyperDual<T> x, T c) => new(x._d0 * c, c * x._d1);

    public static HyperDual<T> operator /(HyperDual<T> x, HyperDual<T> y)
        => new(x._d0 / y._d0, (x._d1 * y._d0 - y._d1 * x._d0) / (y._d0 * y._d0));

    public static HyperDual<T> operator /(T c, HyperDual<T> x) => new(c / x._d0, -x._d1 * c / (x._d0 * x._d0));

    public static HyperDual<T> operator /(HyperDual<T> x, T c) => new(x._d0 / c, x._d1 / c);

    public static HyperDual<Real> Modulo(in HyperDual<Real> x, in HyperDual<Real> y)
        => new(Dual<Real>.Modulo(x._d0, y._d0), x._d1 - y._d1 * Dual<Real>.Floor(x._d0 / y._d0));

    public static HyperDual<Real> Modulo(in HyperDual<Real> x, in Dual<Real> c) => new(Dual<Real>.Modulo(x._d0, c), x._d1);

    public static HyperDual<Real> Modulo(in Dual<Real> c, in HyperDual<Real> x) => new(Dual<Real>.Modulo(c, x._d0), -x._d1 * Dual<Real>.Floor(c / x._d0));

    //
    // Equality
    //

    public static bool operator ==(HyperDual<T> left, HyperDual<T> right) => left._d0 == right._d0 && left._d1 == right._d1;

    public static bool operator !=(HyperDual<T> left, HyperDual<T> right) => left._d0 != right._d0 && left._d1 != right._d1;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is HyperDual<T> other && Equals(other);

    public bool Equals(HyperDual<T> value) => _d0.Equals(value._d0) && _d1.Equals(value._d1);

    public override int GetHashCode() => HashCode.Combine(_d0, _d0);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? formatProvider) => $"({_d0.D0}, {_d0.D1}, {_d1.D0} {_d1.D1})";

    //
    // Other operations
    //

    public static HyperDual<T> CreateVariable(T value) => new(new(value, T.Zero));

    public static HyperDual<T> CreateVariable(T value, T seed) => new(new(value, seed));

    /// <summary>Create an instance of the type with a specified value and two seed values.</summary>
    /// <remarks>Use this to compute mixed second derivatives.</remarks>
    /// <param name="value">A value.</param>
    /// <param name="e1Seed">A seed value for the first variable of interest.</param>
    /// <param name="e2Seed">A seec value for the second variable of interest.</param>
    /// <returns>An instance of the type.</returns>
    public static HyperDual<T> CreateVariable(T value, T e1Seed, T e2Seed) => new(new(value, e1Seed), new(e2Seed));

    /// <inheritdoc cref="Real.Ceiling(Real)"/>
    public static HyperDual<Real> Ceiling(in HyperDual<Real> x) => new(Dual<Real>.Ceiling(x._d0));

    /// <inheritdoc cref="Real.Floor(Real)"/>
    public static HyperDual<Real> Floor(in HyperDual<Real> x) => new(Dual<Real>.Floor(x._d0));

    public HyperDual<T> WithSeed(T seed) => new(new(_d0.D0, seed));

    /// <summary>Create a seeded instance of this type.</summary>
    /// <param name="e1Seed">The first seed.</param>
    /// <param name="e2Seed">The second seed.</param>
    /// <returns>A seeded value.</returns>
    public HyperDual<T> WithSeed(T e1Seed, T e2Seed) => new(new(_d0.D0, e1Seed), new(e2Seed));

    // Exponential functions

    public static HyperDual<T> Exp(HyperDual<T> x)
    {
        var exp = Dual<T>.Exp(x._d0);
        return new(exp, x._d1 * exp);
    }

    public static HyperDual<T> Exp2(HyperDual<T> x)
    {
        var exp2 = Dual<T>.Exp2(x._d0);
        return new(exp2, Real.Ln2 * x._d1 * exp2);
    }

    public static HyperDual<T> Exp10(HyperDual<T> x)
    {
        var exp10 = Dual<T>.Exp10(x._d0);
        return new(exp10, Real.Ln10 * x._d1 * exp10);
    }

    // Hyperbolic functions

    public static HyperDual<T> Acosh(HyperDual<T> x)
        => new(Dual<T>.Acosh(x._d0), x._d1 / (Dual<T>.Sqrt(x._d0 - T.One) * Dual<T>.Sqrt(x._d0 + T.One)));

    public static HyperDual<T> Asinh(HyperDual<T> x)
        => new(Dual<T>.Asinh(x._d0), x._d1 / Dual<T>.Sqrt(x._d0 * x._d0 + T.One));

    public static HyperDual<T> Atanh(HyperDual<T> x)
        => new(Dual<T>.Atanh(x._d0), x._d1 / (T.One - x._d0 * x._d0));

    public static HyperDual<T> Cosh(HyperDual<T> x) => new(Dual<T>.Cosh(x._d0), x._d1 * Dual<T>.Sinh(x._d0));

    public static HyperDual<T> Sinh(HyperDual<T> x) => new(Dual<T>.Sinh(x._d0), x._d1 * Dual<T>.Cosh(x._d0));

    public static HyperDual<T> Tanh(HyperDual<T> x)
    {
        var u = T.One / Dual<T>.Cosh(x._d0);
        return new(Dual<T>.Tanh(x._d0), x._d1 * u * u);
    }

    // Logarithmic functions

    public static HyperDual<T> Ln(HyperDual<T> x) => new(Dual<T>.Ln(x._d0), x._d1 / x._d0);

    public static HyperDual<T> Log(HyperDual<T> x, HyperDual<T> b) => Ln(x) / Ln(b);

    public static HyperDual<T> Log2(HyperDual<T> x) => new(Dual<T>.Log2(x._d0), x._d1 / (Real.Ln2 * x._d0));

    public static HyperDual<T> Log10(HyperDual<T> x) => new(Dual<T>.Log10(x._d0), x._d1 / (Real.Ln10 * x._d0));

    // Power functions

    public static HyperDual<T> Pow(HyperDual<T> x, HyperDual<T> y) => Exp(y * Ln(x));

    public static HyperDual<T> Pow(HyperDual<T> x, T y) => new(Dual<T>.Pow(x._d0, y), x._d1 * y * Dual<T>.Pow(x._d0, y - T.One));

    public static HyperDual<T> Pow(T x, HyperDual<T> y)
    {
        var pow = Dual<T>.Pow(x, y._d0);
        return new(pow, y._d1 * T.Ln(x) * pow);
    }

    // Root functions

    public static HyperDual<T> Cbrt(HyperDual<T> x)
    {
        var cbrt = Dual<T>.Cbrt(x._d0);
        return new(cbrt, x._d1 / (3.0 * cbrt * cbrt));
    }

    public static HyperDual<T> Root(HyperDual<T> x, HyperDual<T> n) => Exp(Ln(x) / n);

    public static HyperDual<T> Sqrt(HyperDual<T> x)
    {
        var sqrt = Dual<T>.Sqrt(x._d0);
        return new(sqrt, 0.5 * x._d1 / sqrt);
    }

    // Trigonometric functions

    public static HyperDual<T> Acos(HyperDual<T> x) => new(Dual<T>.Acos(x._d0), -x._d1 / Dual<T>.Sqrt(T.One - x._d0 * x._d0));

    public static HyperDual<T> Asin(HyperDual<T> x) => new(Dual<T>.Asin(x._d0), x._d1 / Dual<T>.Sqrt(T.One - x._d0 * x._d0));

    public static HyperDual<T> Atan(HyperDual<T> x) => new(Dual<T>.Atan(x._d0), x._d1 / (T.One + x._d0 * x._d0));

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public static HyperDual<Real> Atan2(in HyperDual<Real> y, in HyperDual<Real> x)
    {
        var u = Real.One / (x._d0 * x._d0 + y._d0 * y._d0);
        return new(Dual<T>.Atan2(y._d0, x._d0), (y._d1 * x._d0 - x._d1 * y._d0) * u);
    }

    public static HyperDual<T> Cos(HyperDual<T> x) => new(Dual<T>.Cos(x._d0), -x._d1 * Dual<T>.Sin(x._d0));

    public static HyperDual<T> Sin(HyperDual<T> x) => new(Dual<T>.Sin(x._d0), x._d1 * Dual<T>.Cos(x._d0));

    public static HyperDual<T> Tan(HyperDual<T> x)
    {
        var sec = T.One / Dual<T>.Cos(x._d0);
        return new(Dual<T>.Tan(x._d0), x._d1 * sec * sec);
    }

    //
    // Custom operations
    //

    /// <summary>Perform forward-mode autodiff using a custom unary operation.</summary>
    /// <param name="x">A hyper-dual number.</param>
    /// <param name="f">A function.</param>
    /// <param name="df">The derivative of the function.</param>
    /// <returns>A hyper-dual number.</returns>
    public static HyperDual<T> CustomOperation(HyperDual<T> x, Func<Dual<T>, Dual<T>> f, Func<Dual<T>, Dual<T>> df)
        => new(f(x._d0), x._d1 * df(x._d0));

    /// <summary>Perform forward-mode autodiff using a custom binary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="y">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The derivative of the function with respect to the left variable.</param>
    /// <param name="dfy">The derivative of the function with respect to the right variable.</param>
    /// <returns>A hyper-dual number.</returns>
    public static HyperDual<T> CustomOperation(
        HyperDual<T> x,
        HyperDual<T> y,
        Func<Dual<T>, Dual<T>, Dual<T>> f,
        Func<Dual<T>, Dual<T>, Dual<T>> dfx,
        Func<Dual<T>, Dual<T>, Dual<T>> dfy)
        => new(f(x._d0, y._d0), dfy(x._d0, y._d0) * x._d1 + dfx(x._d0, y._d1) * y._d1);

    //
    // DifGeo
    //

    /// <inheritdoc cref="IDual{TDN, TN}.CreateAutoDiffTensor{TI}(in Dual{TN}, in Dual{TN})" />
    public static AutoDiffTensor2<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in HyperDual<T> x0, in HyperDual<T> x1)
        where U : IIndex
        => new(x0, x1);

    /// <inheritdoc cref="IDual{TDN, TN}.CreateAutoDiffTensor{TI}(in Dual{TN}, in Dual{TN}, in Dual{TN})" />
    public static AutoDiffTensor3<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in HyperDual<T> x0, in HyperDual<T> x1, in HyperDual<T> x2)
        where U : IIndex
        => new(x0, x1, x2);

    /// <inheritdoc cref="IDual{TDN, TN}.CreateAutoDiffTensor{TI}(in Dual{TN}, in Dual{TN}, in Dual{TN}, in Dual{TN})"/>
    public static AutoDiffTensor4<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in HyperDual<T> x0, in HyperDual<T> x1, in HyperDual<T> x2, in HyperDual<T> x3)
        where U : IIndex
        => new(x0, x1, x2, x3);

    public static AutoDiffTensor2<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in Dual<T> x0, in Dual<T> x1)
        where U : IIndex
        => new(new(x0), new(x1));

    public static AutoDiffTensor3<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in Dual<T> x0, in Dual<T> x1, in Dual<T> x2)
        where U : IIndex
        => new(new(x0), new(x1), new(x2));

    public static AutoDiffTensor4<HyperDual<T>, T, U> CreateAutoDiffTensor<U>(in Dual<T> x0, in Dual<T> x1, in Dual<T> x2, in Dual<T> x3)
        where U : IIndex
        => new(new(x0), new(x1), new(x2), new(x3));
}
