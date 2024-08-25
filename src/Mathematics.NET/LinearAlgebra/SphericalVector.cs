// <copyright file="SphericalVector.cs" company="Mathematics.NET">
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

/// <summary>Represents a 3D vector in spherical coordinates according to the physics convention.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct SphericalVector : IVector<SphericalVector, Real>
{
    /// <summary>The radius.</summary>
    public Real R;

    /// <summary>The polar angle.</summary>
    public Real Theta;

    /// <summary>The azimuthal angle.</summary>
    public Real Phi;

    /// <summary>Create a 3D vector in spherical coordinates.</summary>
    /// <param name="r">The radius.</param>
    /// <param name="theta">The polar angle.</param>
    /// <param name="phi">The azimuthal angle.</param>
    /// <exception cref="ArgumentException">Thrown when a negative value is supplied for the radius or when theta does not fall within 0 and π.</exception>
    public SphericalVector(Real r, Real theta, Real phi)
    {
        if (r < Real.Zero)
            throw new ArgumentException("Radius must not be negative");
        if (theta < 0 || theta > Real.Pi)
            throw new ArgumentException("Polar angle must be in between 0 and π, inclusively");

        R = r;
        Theta = theta;
        Phi = phi;
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

    internal static Real GetElement(SphericalVector vector, int index)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();
        return GetElementUnsafe(ref vector, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Real GetElementUnsafe(ref SphericalVector vector, int index)
    {
        Debug.Assert(index is >= 0 and < 3);
        return Unsafe.Add(ref Unsafe.As<SphericalVector, Real>(ref vector), index);
    }

    // Set

    internal static SphericalVector WithElement(SphericalVector vector, int index, Real value)
    {
        if ((uint)index >= 3)
            throw new IndexOutOfRangeException();

        SphericalVector result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref SphericalVector vector, int index, Real value)
    {
        Debug.Assert(index is >= 0 and < 3);
        Unsafe.Add(ref Unsafe.As<SphericalVector, Real>(ref vector), index) = value;
    }

    //
    // Operators
    //

    public static SphericalVector operator -(SphericalVector vector)
        => new(-vector.R, vector.Theta, vector.Phi);

    public static SphericalVector operator +(SphericalVector vector)
        => vector;

    public static SphericalVector operator +(SphericalVector left, SphericalVector right)
    {
        var sinT1 = Real.Sin(left.Theta);
        var sinT2 = Real.Sin(right.Theta);

        var x = left.R * sinT1 * Real.Cos(left.Phi) + right.R * sinT2 * Real.Cos(right.Phi);
        var y = left.R * sinT1 * Real.Sin(left.Phi) + right.R * sinT2 * Real.Sin(right.Phi);
        var z = left.R * Real.Cos(left.Theta) + right.R * Real.Cos(right.Theta);

        var r = Real.Hypot(x, Real.Hypot(y, z));
        return new(r, Real.Acos(z / r), Real.Atan2(y, x));
    }

    public static SphericalVector operator -(SphericalVector left, SphericalVector right)
    {
        var sinT1 = Real.Sin(left.Theta);
        var sinT2 = Real.Sin(right.Theta);

        var x = left.R * sinT1 * Real.Cos(left.Phi) - right.R * sinT2 * Real.Cos(right.Phi);
        var y = left.R * sinT1 * Real.Sin(left.Phi) - right.R * sinT2 * Real.Sin(right.Phi);
        var z = left.R * Real.Cos(left.Theta) - right.R * Real.Cos(right.Theta);

        var r = Real.Hypot(x, Real.Hypot(y, z));
        return new(r, Real.Acos(z / r), Real.Atan2(y, x));
    }

    public static SphericalVector operator *(Real left, SphericalVector right) => new(left * right.R, right.Theta, right.Phi);

    public static SphericalVector operator *(SphericalVector left, Real right) => new(left.R * right, left.Theta, left.Phi);

    //
    // Equality
    //

    public static bool operator ==(SphericalVector left, SphericalVector right)
        => left.R == right.R && left.Theta == right.Theta && left.Phi == right.Phi;

    public static bool operator !=(SphericalVector left, SphericalVector right)
        => left.R != right.R || left.Theta != right.Theta || left.Phi != right.Phi;

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is SphericalVector other && Equals(other);

    public readonly bool Equals(SphericalVector other)
        => R.Equals(other.R) && Theta.Equals(other.Theta) && Phi.Equals(other.Phi);

    public override readonly int GetHashCode() => HashCode.Combine(R, Theta, Phi);

    //
    // Formatting
    //

    public readonly string ToString(string? format, IFormatProvider? provider) => string.Format(provider, "({0}, {1}, {2})",
        R.ToString(format, provider),
        Theta.ToString(format, provider),
        Phi.ToString(format, provider));

    //
    // Methods
    //

    /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
    public static SphericalVector Cross(SphericalVector left, SphericalVector right)
    {
        var r1R2 = left.R * right.R;
        var cosT1 = Real.Cos(left.Theta);
        var sinT1 = Real.Sin(left.Theta);
        var cosT2 = Real.Cos(right.Theta);
        var sinT2 = Real.Sin(right.Theta);

        var x = r1R2 * (sinT1 * cosT2 * Real.Sin(left.Phi) - sinT2 * cosT1 * Real.Sin(right.Phi));
        var y = r1R2 * (sinT2 * cosT1 * Real.Cos(right.Phi) - sinT1 * cosT2 * Real.Cos(left.Phi));
        var z = -r1R2 * sinT1 * sinT2 * Real.Sin(left.Phi - right.Phi);

        var r = Real.Hypot(x, Real.Hypot(y, z));
        return new(r, Real.Acos(z / r), Real.Atan2(y, x));
    }

    /// <summary>Convert a vector in cartesian cordinates to one in spherical coordinates.</summary>
    /// <remarks>The vector must have real components.</remarks>
    /// <param name="x">The vector to convert.</param>
    /// <returns>A spherical vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SphericalVector FromCartesian(Vector3<Real> x)
    {
        var r = Real.Hypot(x.X1, Real.Hypot(x.X2, x.X3));
        return new(r, Real.Acos(x.X3 / r), Real.Atan2(x.X2, x.X1));
    }

    public static Real InnerProduct(SphericalVector left, SphericalVector right)
    {
        var sinT1 = Real.Sin(left.Theta);
        var sinT2 = Real.Sin(right.Theta);

        var x = left.R * sinT1 * Real.Cos(left.Phi) + right.R * sinT2 * Real.Cos(right.Phi);
        var y = left.R * sinT1 * Real.Sin(left.Phi) + right.R * sinT2 * Real.Sin(right.Phi);
        var z = left.R * Real.Cos(left.Theta) + right.R * Real.Cos(right.Theta);

        return x * x + y * y + z * z;
    }

    public readonly Real Norm() => R;

    public readonly SphericalVector Normalize() => new(Real.One, Theta, Phi);

    /// <summary>Convert a vector in spherical coordinates to one in Cartesian coordinates.</summary>
    /// <returns>A vector in Cartesian coordinates.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector3<Real> ToCartesian()
    {
        var sinT = Real.Sin(Theta);
        return new(
            R * sinT * Real.Cos(Phi),
            R * sinT * Real.Sin(Phi),
            R * Real.Cos(Theta));
    }
}
