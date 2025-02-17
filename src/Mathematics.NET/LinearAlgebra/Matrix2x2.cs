// <copyright file="Matrix2x2.cs" company="Mathematics.NET">
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a 2x2 matrix.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Matrix2x2<T> : ISquareMatrix<Matrix2x2<T>, T>
    where T : IComplex<T>
{
    private static readonly Matrix2x2<T> s_identity = CreateDiagonal(T.One, T.One);

    public const int Components = 4;
    public const int E1Components = 2;
    public const int E2Components = 2;

    public static readonly Matrix2x2<T> NaM = CreateDiagonal(T.NaN, T.NaN);

    public Vector2<T> X1;
    public Vector2<T> X2;

    /// <summary>Create a 2x2 matrix given a set of 4 values.</summary>
    /// <param name="e11">The $ e_{11} $ component.</param>
    /// <param name="e12">The $ e_{12} $ component.</param>
    /// <param name="e21">The $ e_{21} $ component.</param>
    /// <param name="e22">The $ e_{22} $ component.</param>
    public Matrix2x2(T e11, T e12, T e21, T e22)
    {
        X1 = new(e11, e12);
        X2 = new(e21, e22);
    }

    //
    // Constants
    //

    static int IArrayRepresentable<Matrix2x2<T>, T>.Components => Components;
    static int I2DArrayRepresentable<Matrix2x2<T>, T>.E1Components => E1Components;
    static int I2DArrayRepresentable<Matrix2x2<T>, T>.E2Components => E2Components;
    static Matrix2x2<T> ISquareMatrix<Matrix2x2<T>, T>.Identity => s_identity;
    static Matrix2x2<T> IMatrix<Matrix2x2<T>, T>.NaM => NaM;

    //
    // Indexer
    //

    public T this[int i, int j]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if ((uint)i >= 2)
                throw new IndexOutOfRangeException();
            return Unsafe.Add(ref Unsafe.AsRef(in X1), i)[j];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if ((uint)i >= 2)
                throw new IndexOutOfRangeException();
            Unsafe.Add(ref X1, i)[j] = value;
        }
    }

    //
    // Operators
    //

    public static Matrix2x2<T> operator -(Matrix2x2<T> matrix)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);
        result.X1 = -matrix.X1;
        result.X2 = -matrix.X2;
        return result;
    }

    public static Matrix2x2<T> operator +(Matrix2x2<T> matrix) => matrix;

    public static Matrix2x2<T> operator +(Matrix2x2<T> left, Matrix2x2<T> right)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);
        result.X1 = left.X1 + right.X1;
        result.X2 = left.X2 + right.X2;
        return result;
    }

    public static Matrix2x2<T> operator -(Matrix2x2<T> left, Matrix2x2<T> right)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);
        result.X1 = left.X1 - right.X1;
        result.X2 = left.X2 - right.X2;
        return result;
    }

    public static Matrix2x2<T> operator *(Matrix2x2<T> left, Matrix2x2<T> right)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);

        result.X1 = right.X1 * left.X1.X1 + right.X2 * left.X1.X2;
        result.X2 = right.X1 * left.X2.X1 + right.X2 * left.X2.X2;

        return result;
    }

    public static Matrix2x2<T> operator *(T left, Matrix2x2<T> right)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);

        result.X1 = left * right.X1;
        result.X2 = left * right.X2;

        return result;
    }

    public static Matrix2x2<T> operator *(Matrix2x2<T> left, T right)
    {
        Unsafe.SkipInit(out Matrix2x2<T> result);

        result.X1 = left.X1 * right;
        result.X2 = left.X2 * right;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Matrix2x2<T> left, Matrix2x2<T> right)
        => left.X1 == right.X1 && left.X2 == right.X2;

    public static bool operator !=(Matrix2x2<T> left, Matrix2x2<T> right)
        => left.X1 != right.X1 || left.X2 != right.X2;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix2x2<T> other && Equals(other);

    public bool Equals(Matrix2x2<T> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2);

    //
    // Formatting
    //

    public string ToString(string? format, IFormatProvider? provider)
#pragma warning disable EPS06
        => this.AsSpan2D().ToDisplayString(format, provider);
#pragma warning restore EPS06

    //
    // Methods
    //

    /// <summary>Create a diagonal matrix from specified values along the diagonal.</summary>
    /// <param name="e11">The $ e_{11} $ component.</param>
    /// <param name="e22">The $ e_{22} $ component.</param>
    /// <returns>A diagonal matrix.</returns>
    public static Matrix2x2<T> CreateDiagonal(T e11, T e22)
        => new(e11, T.Zero, T.Zero, e22);

    public readonly T Determinant() => X1.X1 * X2.X2 - X1.X2 * X2.X1;

    public readonly Matrix2x2<T> Inverse()
    {
        var det = Determinant();
        if (det == T.Zero)
            return NaM;
        T invDet = T.One / det;

        return new(X2.X2 * invDet, -X1.X2 * invDet, -X2.X1 * invDet, X1.X1 * invDet);
    }

    public static bool IsNaM(Matrix2x2<T> matrix)
        => T.IsNaN(matrix.X1.X1) && T.IsNaN(matrix.X2.X2);

    public unsafe T[,] ToArray()
    {
        var array = new T[2, 2];
        var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        var pArray = (void*)handle.AddrOfPinnedObject();
        Unsafe.CopyBlock(pArray, Unsafe.AsPointer(ref this), (uint)(Unsafe.SizeOf<T>() * Components));
        handle.Free();
        return array;
    }

    public readonly T Trace() => X1.X1 + X2.X2;

    public readonly Matrix2x2<T> Transpose() => new(X1.X1, X2.X1, X1.X2, X2.X2);
}
