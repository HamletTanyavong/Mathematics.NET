// <copyright file="StateItem4.cs" company="Mathematics.NET">
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
using System.Runtime.InteropServices;
using Mathematics.NET.Core.Operations;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.Solvers;

/// <summary>Represents a state item with four elements.</summary>
/// <typeparam name="TA">An array-like object that supports addition and multiplication on its elements.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TB">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct StateItem4<TA, TN, TB> : IStateItem<StateItem4<TA, TN, TB>, TA, TN, TB>
    where TA
    : I1DArrayRepresentable<TA, TN, TB, TB>,
      IAdditionOperation<TA, TA>,
      ISubtractionOperation<TA, TA>,
      IMultiplicationOperation<TA, TN, TA>,
      IUnaryMinusOperation<TA, TA>
    where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN>
    where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB>
{
    [InlineArray(4)]
    public struct Buffer
    {
        private TA _element;
    }

    private Buffer _buffer;

    /// <summary>Create a state item.</summary>
    /// <param name="r1">Row one.</param>
    /// <param name="r2">Row two.</param>
    /// <param name="r3">Row three.</param>
    /// <param name="r4">Row four.</param>
    public StateItem4(TA r1, TA r2, TA r3, TA r4)
    {
        _buffer[0] = r1;
        _buffer[1] = r2;
        _buffer[2] = r3;
        _buffer[3] = r4;
    }

    private readonly TA R1 => _buffer[0];
    private readonly TA R2 => _buffer[1];
    private readonly TA R3 => _buffer[2];
    private readonly TA R4 => _buffer[3];

    public static int E1Components => 4;

    public static int E2Components => TA.Components;

    public TA this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _buffer[i];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _buffer[i] = value;
    }

    //
    // Operators
    //

    public static StateItem4<TA, TN, TB> operator +(StateItem4<TA, TN, TB> value) => value;

    public static StateItem4<TA, TN, TB> operator -(StateItem4<TA, TN, TB> value) => new(-value.R1, -value.R2, -value.R3, -value.R4);

    public static StateItem4<TA, TN, TB> operator +(StateItem4<TA, TN, TB> left, StateItem4<TA, TN, TB> right)
        => new(left.R1 + right.R1, left.R2 + right.R2, left.R3 + right.R3, left.R4 + right.R4);

    public static StateItem4<TA, TN, TB> operator -(StateItem4<TA, TN, TB> left, StateItem4<TA, TN, TB> right)
     => new(left.R1 - right.R1, left.R2 - right.R2, left.R3 - right.R3, left.R4 - right.R4);

    public static StateItem4<TA, TN, TB> operator *(StateItem4<TA, TN, TB> left, TN right)
        => new(left.R1 * right, left.R2 * right, left.R3 * right, left.R4 * right);

    public static StateItem4<TA, TN, TB> operator *(TN left, StateItem4<TA, TN, TB> right)
     => new(left * right.R1, left * right.R2, left * right.R3, left * right.R4);
}

