// <copyright file="AutoDiffExtensions.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023 Hamlet Tanyavong
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

using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.AutoDiff;

/// <summary>A class containing AutoDiff extension methods</summary>
public static class AutoDiffExtensions
{
    //
    // Variable creation
    //

    /// <summary>Create a three-element vector from a seed vector of length three</summary>
    /// <param name="tape">A gradient tape</param>
    /// <param name="x">A three-element vector of seed values</param>
    /// <returns>A variable vector of length three</returns>
    public static VariableVector3 CreateVariableVector(this GradientTape tape, Vector3<Real> x)
        => new(tape.CreateVariable(x.X1), tape.CreateVariable(x.X2), tape.CreateVariable(x.X3));

    /// <summary>Create a three-element vector from seed values</summary>
    /// <param name="tape">A gradient tape</param>
    /// <param name="x1Seed">The first seed value</param>
    /// <param name="x2Seed">The second seed value</param>
    /// <param name="x3Seed">The third seed value</param>
    /// <returns>A variable vector of length three</returns>
    public static VariableVector3 CreateVariableVector(this GradientTape tape, Real x1Seed, Real x2Seed, Real x3Seed)
        => new(tape.CreateVariable(x1Seed), tape.CreateVariable(x2Seed), tape.CreateVariable(x3Seed));

    //
    // Vector calculus
    //

    // TODO: Improve performance; perhaps see if caching is possible for some of these methods

    /// <summary>Compute the curl of a vector field using reverse-mode automatic differentiation: $ (\nabla\times\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="tape">A gradient tape</param>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the curl</param>
    /// <returns>The curl of the vector field</returns>
    public static Vector3<Real> Curl(
        this GradientTape tape,
        Func<GradientTape, VariableVector3, Variable> fx,
        Func<GradientTape, VariableVector3, Variable> fy,
        Func<GradientTape, VariableVector3, Variable> fz,
        VariableVector3 x)
    {
        _ = fx(tape, x);
        tape.ReverseAccumulation(out var dfx);

        _ = fy(tape, x);
        tape.ReverseAccumulation(out var dfy);

        _ = fz(tape, x);
        tape.ReverseAccumulation(out var dfz);

        return new(
            dfz[1] - dfy[2],
            dfx[2] - dfz[0],
            dfy[0] - dfx[1]);
    }

    /// <summary>Compute the derivative of a scalar function along a particular direction using reverse-mode automatic differentiation: $ \nabla_{\textbf{v}}f(\textbf{x}) $.</summary>
    /// <param name="tape">A gradient tape</param>
    /// <param name="v">A direction</param>
    /// <param name="f">A scalar function</param>
    /// <param name="x">The point at which to compute the directional derivative</param>
    /// <returns>The directional derivative</returns>
    public static Real DirectionalDerivative(
        this GradientTape tape,
        Vector3<Real> v,
        Func<GradientTape, VariableVector3, Variable> f,
        VariableVector3 x)
    {
        _ = f(tape, x);

        tape.ReverseAccumulation(out var gradients);

        return gradients[0] * v.X1 + gradients[1] * v.X2 + gradients[2] * v.X3;
    }

    /// <summary>Compute the divergence of a vector field using reverse-mode automatic differentiation: $ (\nabla\cdot\textbf{F})(\textbf{x}) $.</summary>
    /// <param name="tape">A gradient tape</param>
    /// <param name="fx">The x-component of the vector field</param>
    /// <param name="fy">The y-component of the vector field</param>
    /// <param name="fz">The z-component of the vector field</param>
    /// <param name="x">The point at which to compute the divergence</param>
    /// <returns>The divergence of the vector field</returns>
    public static Real Divergence(
        this GradientTape tape,
        Func<GradientTape, VariableVector3, Variable> fx,
        Func<GradientTape, VariableVector3, Variable> fy,
        Func<GradientTape, VariableVector3, Variable> fz,
        VariableVector3 x)
    {
        var partialSum = Real.Zero;

        _ = fx(tape, x);
        tape.ReverseAccumulation(out var gradients);
        partialSum += gradients[0];

        _ = fy(tape, x);
        tape.ReverseAccumulation(out gradients);
        partialSum += gradients[1];

        _ = fz(tape, x);
        tape.ReverseAccumulation(out gradients);
        partialSum += gradients[2];

        return partialSum;
    }
}
