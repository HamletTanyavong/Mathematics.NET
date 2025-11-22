// <copyright file="Matrix4x4Impl.cs" company="Mathematics.NET">
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
using System.Runtime.Intrinsics.X86;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Benchmarks.Implementations.LinearAlgebra;

public static class Matrix4x4Impl
{
    public static Matrix4x4<T> TransposeNaive<T>(this Matrix4x4<T> matrix)
        where T : IComplex<T>
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        result.X1 = new(matrix.X1.X1, matrix.X2.X1, matrix.X3.X1, matrix.X4.X1);
        result.X2 = new(matrix.X1.X2, matrix.X2.X2, matrix.X3.X2, matrix.X4.X2);
        result.X3 = new(matrix.X1.X3, matrix.X2.X3, matrix.X3.X3, matrix.X4.X3);
        result.X4 = new(matrix.X1.X4, matrix.X2.X4, matrix.X3.X4, matrix.X4.X4);

        return result;
    }

    public static Matrix4x4<T> TransposeSimdV1<T>(this Matrix4x4<T> matrix)
        where T : IComplex<T>
    {
        Unsafe.SkipInit(out Matrix4x4<T> result);

        if (typeof(T) == typeof(Real))
        {
            if (Avx.IsSupported)
            {
                // Real numbers
                var x1 = matrix.X1.AsVector256();
                var x2 = matrix.X2.AsVector256();
                var x3 = matrix.X3.AsVector256();
                var x4 = matrix.X4.AsVector256();

                var l12 = Avx.UnpackLow(x1, x2);
                var l34 = Avx.UnpackLow(x3, x4);
                var u12 = Avx.UnpackHigh(x1, x2);
                var u34 = Avx.UnpackHigh(x3, x4);

                result.X1 = l12.WithUpper(l34.GetLower()).AsVector4<T>();
                result.X2 = u12.WithUpper(u34.GetLower()).AsVector4<T>();
                result.X3 = l34.WithLower(l12.GetUpper()).AsVector4<T>();
                result.X4 = u34.WithLower(u12.GetUpper()).AsVector4<T>();

                return result;
            }
        }

        result.X1 = new(matrix.X1.X1, matrix.X2.X1, matrix.X3.X1, matrix.X4.X1);
        result.X2 = new(matrix.X1.X2, matrix.X2.X2, matrix.X3.X2, matrix.X4.X2);
        result.X3 = new(matrix.X1.X3, matrix.X2.X3, matrix.X3.X3, matrix.X4.X3);
        result.X4 = new(matrix.X1.X4, matrix.X2.X4, matrix.X3.X4, matrix.X4.X4);

        return result;
    }
}
