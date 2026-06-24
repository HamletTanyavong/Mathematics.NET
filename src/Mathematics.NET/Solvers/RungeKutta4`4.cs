// <copyright file="RungeKutta4`4.cs" company="Mathematics.NET">
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

using System.Numerics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance.Helpers;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Operations;

namespace Mathematics.NET.Solvers;

/// <summary>Represents a fourth-order Runge-Kutta solver.</summary>
/// <typeparam name="TR1T">A rank-one tensor.</typeparam>
/// <typeparam name="TV">The backing type of the tensor.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TB">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <typeparam name="TI">The index of the tensor.</typeparam>
/// <param name="function">A function to use for the integration step.</param>
public sealed class RungeKutta4<TR1T, TV, TN, TB, TI>(Func<TN, TR1T, TR1T> function)
    where TR1T : IRankOneTensor<TR1T, TV, TN, TB, TB, TI>, IMultiplicationOperation<TR1T, TN, TR1T>
    where TV : IVector<TV, TN, TB, TB>
    where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN>
    where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB>
    where TI : IIndex
{
    private readonly struct RK4IntegrateAction(Func<TN, TR1T, TR1T> function, TN time, TN dt) : IRefAction<TR1T>
    {
        private readonly Func<TN, TR1T, TR1T> _function = function;
        private readonly TN _time = time;
        private readonly TN _dt = dt;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(ref TR1T value)
        {
            var k1 = _function(_time, value);
            var k2 = _function(_time + IBinaryFloatingPointIeee754<TB>.Half * _dt, value + IBinaryFloatingPointIeee754<TB>.Half * k1 * _dt);
            var k3 = _function(_time + IBinaryFloatingPointIeee754<TB>.Half * _dt, value + IBinaryFloatingPointIeee754<TB>.Half * k2 * _dt);
            var k4 = _function(_time + _dt, value + k3 * _dt);
            value += _dt / IBinaryFloatingPointIeee754<TB>.Six * (k1 + IBinaryFloatingPointIeee754<TB>.Two * (k2 + k3) + k4);
        }
    }

    private readonly Func<TN, TR1T, TR1T> _function = function;

    /// <inheritdoc cref="RungeKutta4{T, U}.Integrate(State{T, U}, T)"/>
    public void Integrate(State<TR1T, TV, TN, TB, TI> state, TN dt)
    {
        var system = state.System.Span;
        var time = state.Time;
        for (int i = 0; i < system.Length; i++)
        {
            ref var value = ref system[i];
            var k1 = _function(time, value);
            var k2 = _function(time + IBinaryFloatingPointIeee754<TB>.Half * dt, value + IBinaryFloatingPointIeee754<TB>.Half * k1 * dt);
            var k3 = _function(time + IBinaryFloatingPointIeee754<TB>.Half * dt, value + IBinaryFloatingPointIeee754<TB>.Half * k2 * dt);
            var k4 = _function(time + dt, value + k3 * dt);
            value += dt / IBinaryFloatingPointIeee754<TB>.Six * (k1 + IBinaryFloatingPointIeee754<TB>.Two * (k2 + k3) + k4);
        }
        state.Time += dt;
    }

    /// <inheritdoc cref="RungeKutta4{T, U}.IntegrateParallel(State{T, U}, T)"/>
    public void IntegrateParallel(State<TR1T, TV, TN, TB, TI> state, TN dt)
    {
        ParallelHelper.ForEach(state.System, new RK4IntegrateAction(_function, state.Time, dt));
        state.Time += dt;
    }
}
