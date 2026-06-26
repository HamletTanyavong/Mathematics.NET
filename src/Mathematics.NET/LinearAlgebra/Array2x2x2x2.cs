// <copyright file="Array2x2x2x2.cs" company="Mathematics.NET">
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a 2x2x2x2 array.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Array2x2x2x2<T, U> : IHypercubic4DArray<Array2x2x2x2<T, U>, T, U, U>
    where T : IComplex<T, U, U>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    public const int Components = 16;
    public const int E1Components = 2;
    public const int E2Components = 2;
    public const int E3Components = 2;
    public const int E4Components = 2;

    public Array2x2x2<T, U> X1;
    public Array2x2x2<T, U> X2;

    //
    // Constants
    //

    static int IArrayRepresentable<Array2x2x2x2<T, U>, T, U, U>.Components => Components;
    static int I4DArrayRepresentable<Array2x2x2x2<T, U>, T, U, U>.E1Components => E1Components;
    static int I4DArrayRepresentable<Array2x2x2x2<T, U>, T, U, U>.E2Components => E2Components;
    static int I4DArrayRepresentable<Array2x2x2x2<T, U>, T, U, U>.E3Components => E3Components;
    static int I4DArrayRepresentable<Array2x2x2x2<T, U>, T, U, U>.E4Components => E4Components;

    //
    // Indexer
    //

    public T this[int i, int j, int k, int l]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if ((uint)i >= 2)
                throw new IndexOutOfRangeException();
            return Unsafe.Add(ref Unsafe.AsRef(in X1), i)[j, k, l];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if ((uint)i >= 2)
                throw new IndexOutOfRangeException();
            Unsafe.Add(ref X1, i)[j, k, l] = value;
        }
    }

    //
    // Operators
    //

    public static Array2x2x2x2<T, U> operator -(Array2x2x2x2<T, U> array)
    {
        Unsafe.SkipInit(out Array2x2x2x2<T, U> result);

        result.X1 = -array.X1;
        result.X2 = -array.X2;

        return result;
    }

    public static Array2x2x2x2<T, U> operator +(Array2x2x2x2<T, U> array)
        => array;

    public static Array2x2x2x2<T, U> operator *(T c, Array2x2x2x2<T, U> array)
    {
        Unsafe.SkipInit(out Array2x2x2x2<T, U> result);

        result.X1 = c * array.X1;
        result.X2 = c * array.X2;

        return result;
    }

    public static Array2x2x2x2<T, U> operator *(Array2x2x2x2<T, U> array, T c)
    {
        Unsafe.SkipInit(out Array2x2x2x2<T, U> result);

        result.X1 = array.X1 * c;
        result.X2 = array.X2 * c;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Array2x2x2x2<T, U> left, Array2x2x2x2<T, U> right)
        => left.X1 == right.X1 && left.X2 == right.X2;

    public static bool operator !=(Array2x2x2x2<T, U> left, Array2x2x2x2<T, U> right)
        => left.X1 != right.X1 || left.X2 != right.X2;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Array2x2x2x2<T, U> other && Equals(other);

    public bool Equals(Array2x2x2x2<T, U> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider) => ToArray().ToString<T, U, U>(format, provider);

    //
    // Methods
    //

    public unsafe T[,,,] ToArray()
    {
        var array = new T[2, 2, 2, 2];
        var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        var pArray = (void*)handle.AddrOfPinnedObject();
        Unsafe.CopyBlock(pArray, Unsafe.AsPointer(ref this), (uint)(Unsafe.SizeOf<T>() * Components));
        handle.Free();
        return array;
    }
}
