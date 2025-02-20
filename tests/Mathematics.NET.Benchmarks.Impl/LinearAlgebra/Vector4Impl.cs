// <copyright file="Vector4Impl.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Benchmarks.Impl.LinearAlgebra;

public static class Vector4Impl
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T> AddNaive<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        return new(
            left.X1 + right.X1,
            left.X2 + right.X2,
            left.X3 + right.X3,
            left.X4 + right.X4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T> AddSimd<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        if (typeof(T) == typeof(Real))
        {
            return Vector256.Add(left.AsVector256(), right.AsVector256()).AsVector4<T>();
        }
        else if (typeof(T) == typeof(Complex))
        {
            return Vector512.Add(left.AsVector512(), right.AsVector512()).AsVector4<T>();
        }
        else
        {
            return new(
                left.X1 + right.X1,
                left.X2 + right.X2,
                left.X3 + right.X3,
                left.X4 + right.X4);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsNaive<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        return left.X1 == right.X1
            && left.X2 == right.X2
            && left.X3 == right.X3
            && left.X4 == right.X4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsSimd<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        if (typeof(T) == typeof(Real))
        {
            return Vector256.EqualsAll(left.AsVector256(), right.AsVector256());
        }
        else if (typeof(T) == typeof(Complex))
        {
            return Vector512.EqualsAll(left.AsVector512(), right.AsVector512());
        }
        else
        {
            return left.X1 == right.X1
                && left.X2 == right.X2
                && left.X3 == right.X3
                && left.X4 == right.X4;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T> MultiplyNaive<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        return new(
            left.X1 * right.X1,
            left.X2 * right.X2,
            left.X3 * right.X3,
            left.X4 * right.X4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4<T> MultiplySimd<T>(Vector4<T> left, Vector4<T> right)
        where T : IComplex<T>
    {
        if (typeof(T) == typeof(Real))
        {
            return Vector256.Multiply(left.AsVector256(), right.AsVector256()).AsVector4<T>();
        }
        else
        {
            return new(
                left.X1 * right.X1,
                left.X2 * right.X2,
                left.X3 * right.X3,
                left.X4 * right.X4);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Real NormNaive<T>(this Vector4<T> vector)
        where T : IComplex<T>
    {
        Span<double> components = [
            (vector.X1 * T.Conjugate(vector.X1)).Re.AsDouble(),
            (vector.X2 * T.Conjugate(vector.X2)).Re.AsDouble(),
            (vector.X3 * T.Conjugate(vector.X3)).Re.AsDouble(),
            (vector.X4 * T.Conjugate(vector.X4)).Re.AsDouble()];

        var max = components[0];
        for (int i = 1; i < 4; i++)
        {
            if (components[i] > max)
            {
                max = components[i];
            }
        }

        var partialSum = 0.0;
        var maxSquared = max * max;
        partialSum += components[0] / maxSquared;
        partialSum += components[1] / maxSquared;
        partialSum += components[2] / maxSquared;
        partialSum += components[3] / maxSquared;

        return max * Math.Sqrt(partialSum);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Real NormSimd<T>(this Vector4<T> vector)
        where T : IComplex<T>
    {
        var vec = Vector256<double>.Zero
            .WithElement(0, (vector.X1 * T.Conjugate(vector.X1)).Re.AsDouble())
            .WithElement(1, (vector.X2 * T.Conjugate(vector.X2)).Re.AsDouble())
            .WithElement(2, (vector.X3 * T.Conjugate(vector.X3)).Re.AsDouble())
            .WithElement(3, (vector.X4 * T.Conjugate(vector.X4)).Re.AsDouble());

        var max = vec[0];
        for (int i = 1; i < 4; i++)
        {
            if (vec[i] > max)
            {
                max = vec[i];
            }
        }

        return max * Real.Sqrt(Vector256.Sum(Vector256.Divide(vec, max * max)));
    }
}
