// <copyright file="RungeKutta4`3.cs" company="Mathematics.NET">
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
using CommunityToolkit.HighPerformance.Helpers;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.Solvers;

/// <summary>Represents a fourth-order Runge-Kutta solver.</summary>
/// <typeparam name="TSI">A type that implements <see cref="IStateItem{TSC, TA, TN}"/>.</typeparam>
/// <typeparam name="TA">An array-like object that supports addition and multiplication on its elements.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <param name="function">A function to use for the integration step.</param>
public sealed class RungeKutta4<TSI, TA, TN>(Func<TN, TSI, TSI> function)
    where TSI : IStateItem<TSI, TA, TN>
    where TA
    : I1DArrayRepresentable<TA, TN>,
      IAdditionOperation<TA, TA>,
      ISubtractionOperation<TA, TA>,
      IMultiplicationOperation<TA, TN, TA>,
      IUnaryMinusOperation<TA, TA>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
{
    private readonly struct RK4IntegrateAction(Func<TN, TSI, TSI> function, TN time, TN dt) : IRefAction<TSI>
    {
        private readonly Func<TN, TSI, TSI> _function = function;
        private readonly TN _time = time;
        private readonly TN _dt = dt;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(ref TSI value)
        {
            var k1 = _function(_time, value);
            var k2 = _function(_time + 0.5 * _dt, value + 0.5 * k1 * _dt);
            var k3 = _function(_time + 0.5 * _dt, value + 0.5 * k2 * _dt);
            var k4 = _function(_time + _dt, value + k3 * _dt);
            value += _dt / 6.0 * (k1 + 2 * (k2 + k3) + k4);
        }
    }

    private readonly Func<TN, TSI, TSI> _function = function;

    /// <inheritdoc cref="RungeKutta4{T}.Integrate(State{T}, T)"/>
    public void Integrate(State<TSI, TA, TN> state, TN dt)
    {
        var system = state.System.Span;
        var time = state.Time;
        for (int i = 0; i < system.Length; i++)
        {
            ref var value = ref system[i];
            var k1 = _function(time, value);
            var k2 = _function(time + 0.5 * dt, value + 0.5 * k1 * dt);
            var k3 = _function(time + 0.5 * dt, value + 0.5 * k2 * dt);
            var k4 = _function(time + dt, value + k3 * dt);
            value += dt / 6.0 * (k1 + 2 * (k2 + k3) + k4);
        }
        state.Time += dt;
    }

    /// <inheritdoc cref="RungeKutta4{T}.IntegrateParallel(State{T}, T)"/>
    public void IntegrateParallel(State<TSI, TA, TN> state, TN dt)
    {
        ParallelHelper.ForEach(state.System, new RK4IntegrateAction(_function, state.Time, dt));
        state.Time += dt;
    }
}
