// <copyright file="LinearAlgebraImplementationExtensions.cs" company="Mathematics.NET">
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
using System.Runtime.Intrinsics;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Benchmarks.Implementations.LinearAlgebra;

internal static class LinearAlgebraImplementationExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<double> AsVector128<T>(this Vector2<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector2<T>, Vector128<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2<T> AsVector2<T>(this Vector128<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector128<double>, Vector2<T>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<double> AsVector256<T>(this Vector2<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector2<T>, Vector256<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2<T> AsVector2<T>(this Vector256<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector256<double>, Vector2<T>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<double> AsVector256<T>(this Vector4<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector4<T>, Vector256<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4<T> AsVector4<T>(this Vector256<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector256<double>, Vector4<T>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector512<double> AsVector512<T>(this Vector4<T> value)
        where T : IComplex<T>
        => Unsafe.As<Vector4<T>, Vector512<double>>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4<T> AsVector4<T>(this Vector512<double> value)
        where T : IComplex<T>
        => Unsafe.As<Vector512<double>, Vector4<T>>(ref value);
}
