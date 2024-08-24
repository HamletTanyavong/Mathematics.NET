// <copyright file="CylindricalVector.cs" company="Mathematics.NET">
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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.LinearAlgebra;

/// <summary>Represents a 3D vector in cylindrical coordinates.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CylindricalVector : IVector<CylindricalVector, Real>
{
    /// <summary>The radial distance.</summary>
    public Real Rho;

    /// <summary>The azimuthal angle.</summary>
    public Real Phi;

    /// <summary>The axial coordinate.</summary>
    public Real Z;

    /// <summary>Create a 3D vector in cylindrical coordinates.</summary>
    /// <param name="rho">The radial distance.</param>
    /// <param name="phi">The azimuthal angle.</param>
    /// <param name="z">The axial coordinate.</param>
    /// <exception cref="ArgumentException">Thrown when a negative value is supplied for the radial distance.</exception>
    public CylindricalVector(Real rho, Real phi, Real z)
    {
        if (rho < 0)
        {
            throw new ArgumentException("Rho must not be negative");
        }

        Rho = rho;
        Phi = phi;
        Z = z;
    }

    //
    // IArrayRepresentable & relevant interfaces
    //

    public static int Components => 3;

    public static int E1Components => 3;

    //
    // Indexer
    //

    public Real this[int index]
    {
        readonly get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    // Get

    internal static Real GetElement(CylindricalVector vector, int index)
    {
        if ((uint)index >= 3)
        {
            throw new IndexOutOfRangeException();
        }

        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Real GetElementUnsafe(ref CylindricalVector vector, int index)
    {
        Debug.Assert(index is >= 0 and < 3);
        return Unsafe.Add(ref Unsafe.As<CylindricalVector, Real>(ref vector), index);
    }

    // Set

    internal static CylindricalVector WithElement(CylindricalVector vector, int index, Real value)
    {
        if ((uint)index >= 3)
        {
            throw new IndexOutOfRangeException();
        }

        CylindricalVector result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref CylindricalVector vector, int index, Real value)
    {
        Debug.Assert(index is >= 0 and < 3);
        Unsafe.Add(ref Unsafe.As<CylindricalVector, Real>(ref vector), index) = value;
    }

    //
    // Operators
    //

    public static CylindricalVector operator -(CylindricalVector vector)
        => new(-vector.Rho, vector.Phi, -vector.Z);

    public static CylindricalVector operator +(CylindricalVector vector)
        => vector;

    public static CylindricalVector operator +(CylindricalVector left, CylindricalVector right)
    {
        var x = left.Rho * Real.Cos(left.Phi) + right.Rho * Real.Cos(right.Phi);
        var y = left.Rho * Real.Sin(left.Phi) + right.Rho * Real.Sin(right.Phi);
        var z = left.Z + right.Z;

        return new(Real.Hypot(x, y), Real.Atan2(y, x), z);
    }

    public static CylindricalVector operator -(CylindricalVector left, CylindricalVector right)
    {
        var x = left.Rho * Real.Cos(left.Phi) - right.Rho * Real.Cos(right.Phi);
        var y = left.Rho * Real.Sin(left.Phi) - right.Rho * Real.Sin(right.Phi);
        var z = left.Z - right.Z;

        return new(Real.Hypot(x, y), Real.Atan2(y, x), z);
    }

    public static CylindricalVector operator *(Real left, CylindricalVector right) => new(left * right.Rho, right.Phi, left * right.Z);

    public static CylindricalVector operator *(CylindricalVector left, Real right) => new(left.Rho * right, left.Phi, left.Z * right);

    //
    // Equality
    //

    public static bool operator ==(CylindricalVector left, CylindricalVector right)
        => left.Rho == right.Rho && left.Phi == right.Phi && left.Z == right.Z;

    public static bool operator !=(CylindricalVector left, CylindricalVector right)
        => left.Rho != right.Rho || left.Phi != right.Phi || left.Z != right.Z;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is CylindricalVector other && Equals(other);

    public readonly bool Equals(CylindricalVector other)
        => Rho.Equals(other.Rho) && Phi.Equals(other.Phi) && Z.Equals(other.Z);

    public override readonly int GetHashCode() => HashCode.Combine(Rho, Phi, Z);

    //
    // Formatting
    //

    public readonly string ToString(string? format, IFormatProvider? provider) => string.Format(provider, "({0}, {1}, {2})",
        Rho.ToString(format, provider),
        Phi.ToString(format, provider),
        Z.ToString(format, provider));

    //
    // Methods
    //

    /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
    public static CylindricalVector Cross(CylindricalVector left, CylindricalVector right)
    {
        var x = left.Rho * Real.Sin(left.Phi) * right.Z - right.Rho * Real.Sin(right.Phi) * left.Z;
        var y = right.Rho * Real.Cos(right.Phi) * left.Z - left.Rho * Real.Cos(left.Phi) * right.Z;
        var z = -left.Rho * right.Rho * Real.Sin(left.Phi - right.Phi);

        return new(Real.Hypot(x, y), Real.Atan2(y, x), z);
    }

    /// <summary>Convert a vector in cartesian cordinates to one in cylindrical coordinates.</summary>
    /// <remarks>The vector must have real components.</remarks>
    /// <param name="x">The vector to convert.</param>
    /// <returns>A cylindrical vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CylindricalVector FromCartesian(Vector3<Real> x) => new(Real.Hypot(x.X1, x.X2), Real.Atan2(x.X2, x.X1), x.X3);

    public static Real InnerProduct(CylindricalVector left, CylindricalVector right)
        => left.Rho * right.Rho * Real.Cos(left.Phi - right.Phi) + left.Z * right.Z;

    public readonly Real Norm() => Real.Hypot(Rho, Z);

    public readonly CylindricalVector Normalize()
    {
        var norm = Real.Hypot(Rho, Z);
        return new(Rho / norm, Phi, Z / norm);
    }

    /// <summary>Convert a vector in cylindrical coordinates to one in Cartesian coordinates.</summary>
    /// <returns>A vector in Cylindrical coordinates.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector3<Real> ToCartesian() => new(Rho * Real.Cos(Phi), Rho * Real.Sin(Phi), Z);
}
