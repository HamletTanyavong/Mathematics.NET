// <copyright file="Array3x3x3.cs" company="Mathematics.NET">
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

/// <summary>Represents a 3x3x3 array.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct Array3x3x3<T, U> : ICubicArray<Array3x3x3<T, U>, T, U, U>
    where T : IComplex<T, U, U>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    public const int Components = 27;
    public const int E1Components = 3;
    public const int E2Components = 3;
    public const int E3Components = 3;

    public Matrix3x3<T, U> X1;
    public Matrix3x3<T, U> X2;
    public Matrix3x3<T, U> X3;

    //
    // Constants
    //

    static int IArrayRepresentable<Array3x3x3<T, U>, T, U, U>.Components => Components;
    static int I3DArrayRepresentable<Array3x3x3<T, U>, T, U, U>.E1Components => E1Components;
    static int I3DArrayRepresentable<Array3x3x3<T, U>, T, U, U>.E2Components => E2Components;
    static int I3DArrayRepresentable<Array3x3x3<T, U>, T, U, U>.E3Components => E3Components;

    //
    // Indexer
    //

    public T this[int i, int j, int k]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if ((uint)i >= 3)
                throw new IndexOutOfRangeException();
            return Unsafe.Add(ref Unsafe.AsRef(in X1), i)[j, k];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if ((uint)i >= 3)
                throw new IndexOutOfRangeException();
            Unsafe.Add(ref X1, i)[j, k] = value;
        }
    }

    //
    // Operators
    //

    public static Array3x3x3<T, U> operator -(Array3x3x3<T, U> array)
    {
        Unsafe.SkipInit(out Array3x3x3<T, U> result);

        result.X1 = -array.X1;
        result.X2 = -array.X2;
        result.X3 = -array.X3;

        return result;
    }

    public static Array3x3x3<T, U> operator +(Array3x3x3<T, U> array) => array;

    public static Array3x3x3<T, U> operator *(T c, Array3x3x3<T, U> array)
    {
        Unsafe.SkipInit(out Array3x3x3<T, U> result);

        result.X1 = c * array.X1;
        result.X2 = c * array.X2;
        result.X3 = c * array.X3;

        return result;
    }

    public static Array3x3x3<T, U> operator *(Array3x3x3<T, U> array, T c)
    {
        Unsafe.SkipInit(out Array3x3x3<T, U> result);

        result.X1 = array.X1 * c;
        result.X2 = array.X2 * c;
        result.X3 = array.X3 * c;

        return result;
    }

    //
    // Equality
    //

    public static bool operator ==(Array3x3x3<T, U> left, Array3x3x3<T, U> right)
        => left.X1 == right.X1 && left.X2 == right.X2 && left.X3 == right.X3;

    public static bool operator !=(Array3x3x3<T, U> left, Array3x3x3<T, U> right)
        => left.X1 != right.X1 || left.X2 != right.X2 || left.X3 != right.X3;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Array3x3x3<T, U> other && Equals(other);

    public bool Equals(Array3x3x3<T, U> value)
        => X1.Equals(value.X1) && X2.Equals(value.X2) && X3.Equals(value.X3);

    public override readonly int GetHashCode() => HashCode.Combine(X1, X2, X3);

    //
    // Formatting
    //

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? provider) => ToArray().ToDisplayString<T, U, U>(format, provider);

    //
    // Methods
    //

    public unsafe T[,,] ToArray()
    {
        var array = new T[3, 3, 3];
        var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        var pArray = (void*)handle.AddrOfPinnedObject();
        Unsafe.CopyBlock(pArray, Unsafe.AsPointer(ref this), (uint)(Unsafe.SizeOf<T>() * Components));
        handle.Free();
        return array;
    }
}
