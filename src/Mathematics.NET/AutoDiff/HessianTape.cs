// <copyright file="HessianTape.cs" company="Mathematics.NET">
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

// TODO: Find out if there is a way to add checkpoiting for Hessian tapes. The method used
// for gradient tapes does not work for Hessian tapes since previously computed gradients
// need to be known to compute second derivatives.

#pragma warning disable IDE0032

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a Hessian tape.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
public record class HessianTape<T, U> : ITape<T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private bool _isTracking;
    private int _variableCount;
    private List<HessianNode<T, U>> _nodes;

    /// <summary>Create an instance of a Hessian tape.</summary>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public HessianTape(bool isTracking = true)
    {
        _isTracking = isTracking;
        _nodes = [];
    }

    /// <summary>Create an instance of a Hessian tape that will hold an expected number of nodes.</summary>
    /// <param name="n">An integer.</param>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public HessianTape(int n, bool isTracking = true)
    {
        _isTracking = isTracking;
        _nodes = new(n);
    }

    public bool IsTracking { get => _isTracking; set => _isTracking = value; }

    public int NodeCount => _nodes.Count;

    public int VariableCount => _variableCount;

    //
    // Methods
    //

    public Variable<T, U> CreateVariable(T value)
    {
        _nodes.Add(new(_variableCount));
        Variable<T, U> variable = new(_variableCount++, value);
        return variable;
    }

    public void LogNodes(ILogger<ITape<T, U>> logger, CancellationToken cancellationToken, int limit = 100)
    {
        const string template = """
            {NodeType}: {NodeNumber}
                Weights: [[{DX}, {DXX}, {DXY}],
                          [{DY}, {DYY}, {DXY}]]
                Parents: [{PX}, {PY}]
            """;

        ReadOnlySpan<HessianNode<T, U>> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        HessianNode<T, U> node;

        int i = 0;
        while (i < Math.Min(_variableCount, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            logger.LogInformation(template, "Root Node", i, node.DX, node.DXX, node.DXY, node.DY, node.DYY, node.DXY, node.PX, node.PY);
            i++;
        }

        while (i < Math.Min(nodeSpan.Length, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            logger.LogInformation(template, "Node", i, node.DX, node.DXX, node.DXY, node.DY, node.DYY, node.DXY, node.PX, node.PY);
            i++;
        }

        void CheckForCancellation(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("Log nodes operation cancelled.");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient)
        => ReverseAccumulate(out gradient, T.One);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, int index)
        => ReverseAccumulate(out gradient, T.One, index);

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan2D<T> hessian)
        => ReverseAccumulate(out hessian, T.One);

    /// <summary>Perform reverse accumulation on the Hessian tape starting at a specific node and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    public void ReverseAccumulate(out ReadOnlySpan2D<T> hessian, int index)
        => ReverseAccumulate(out hessian, T.One, index);

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="hessian">The Hessian.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian)
        => ReverseAccumulate(out gradient, out hessian, T.One);

    /// <summary>Perform reverse accumulation on the Hessian tape starting at a specific node and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian, int index)
        => ReverseAccumulate(out gradient, out hessian, T.One, index);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed)
        => ReverseAccumulate(out gradient, seed, _nodes.Count - 1);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed, int index)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("The Hessian tape contains no root nodes.");

        if (index < _variableCount || index >= _nodes.Count)
            throw new IndexOutOfRangeException();

        ReadOnlySpan<HessianNode<T, U>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        Span<T> gradientSpan = new T[index + 1];
        gradientSpan[index] = seed;

        for (int i = index; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            gradientSpan[node.PX] += gradientElement * node.DX;
            if (node.PY != i)
                gradientSpan[node.PY] += gradientElement * node.DY;
        }

        gradient = gradientSpan[.._variableCount];
    }

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="seed">A seed value.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan2D<T> hessian, T seed)
        => ReverseAccumulate(out hessian, seed, _nodes.Count - 1);

    /// <summary>Perform reverse accumulation on the Hessian tape starting at a specific node and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="seed">A seed value.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The Hessian tape contains no root nodes.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    public void ReverseAccumulate(out ReadOnlySpan2D<T> hessian, T seed, int index)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("The Hessian tape contains no root nodes.");

        if (index < _variableCount || index >= _nodes.Count)
            throw new IndexOutOfRangeException();

        ReadOnlySpan<HessianNode<T, U>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        Span<T> gradientSpan = new T[index + 1];
        gradientSpan[index] = seed;

        Span2D<T> hessianSpan = new T[index + 1, index + 1];

        for (int i = index; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            gradientSpan[node.PX] += gradientElement * node.DX;
            if (node.PY != i)
                gradientSpan[node.PY] += gradientElement * node.DY;

            EdgePush(hessianSpan, in node, i);
            if (gradientElement != T.Zero)
                Accumulate(hessianSpan, in node, gradientElement);
        }

        hessian = hessianSpan.Slice(0, 0, _variableCount, _variableCount);
    }

    // The following method was inspired by the edge-pushing algorithm outlined by Gower and Mello: https://arxiv.org/pdf/2007.15040.pdf.
    // TODO: use newer variations/versions of this algorithm since they are more performant

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="seed">A seed value.</param>
    /// <exception cref="AutoDiffException">The Hessian tape does not have any tracked variables.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian, T seed)
        => ReverseAccumulate(out gradient, out hessian, seed, _nodes.Count - 1);

    /// <summary>Perform reverse accumulation on the Hessian tape starting at a specific node and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient.</param>
    /// <param name="hessian">The Hessian.</param>
    /// <param name="seed">A seed value.</param>
    /// <param name="index">The index of the starting node.</param>
    /// <exception cref="AutoDiffException">The Hessian tape contains no root nodes.</exception>
    /// <exception cref="IndexOutOfRangeException">The index does not refer to a valid starting node.</exception>
    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian, T seed, int index)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("The Hessian tape contains no root nodes.");

        if (index < _variableCount || index >= _nodes.Count)
            throw new IndexOutOfRangeException();

        ReadOnlySpan<HessianNode<T, U>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        Span<T> gradientSpan = new T[index + 1];
        gradientSpan[index] = seed;

        Span2D<T> hessianSpan = new T[index + 1, index + 1];

        for (int i = index; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            gradientSpan[node.PX] += gradientElement * node.DX;
            if (node.PY != i)
                gradientSpan[node.PY] += gradientElement * node.DY;

            EdgePush(hessianSpan, in node, i);
            if (gradientElement != T.Zero)
                Accumulate(hessianSpan, in node, gradientElement);
        }

        gradient = gradientSpan[.._variableCount];
        hessian = hessianSpan.Slice(0, 0, _variableCount, _variableCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void EdgePush(Span2D<T> weight, ref readonly HessianNode<T, U> node, int i)
    {
        for (int p = 0; p <= i; p++)
        {
            if (weight[i, p] == T.Zero || weight[p, i] == T.Zero)
                continue;
            if (p != i)
            {
                if (node.PX != p)
                {
                    var x = node.DX * weight[i, p];
                    weight[node.PX, p] += x;
                    weight[p, node.PX] += x;
                }
                else
                {
                    weight[p, p] += IBinaryFloatingPointIeee754<U>.Two * node.DX * weight[i, p];
                }

                if (node.PY != p)
                {
                    var x = node.DY * weight[i, p];
                    weight[node.PY, p] += x;
                    weight[p, node.PY] += x;
                }
                else
                {
                    weight[p, p] += IBinaryFloatingPointIeee754<U>.Two * node.DY * weight[i, p];
                }
            }
            else
            {
                var x = weight[i, i];
                weight[node.PX, node.PX] += node.DX * node.DX * x;
                weight[node.PX, node.PY] += node.DX * node.DY * x;
                weight[node.PY, node.PX] += node.DY * node.DX * x;
                weight[node.PY, node.PY] += node.DY * node.DY * x;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void Accumulate(Span2D<T> weight, ref readonly HessianNode<T, U> node, T v)
    {
        weight[node.PX, node.PX] += v * node.DXX;
        weight[node.PX, node.PY] += v * node.DXY;
        weight[node.PY, node.PX] += v * node.DXY;
        weight[node.PY, node.PY] += v * node.DYY;
    }

    //
    // Basic Operations
    //

    public Variable<T, U> Add(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, T.Zero, T.One, T.Zero, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value + y.Value);
        }
        return new(_nodes.Count, x.Value + y.Value);
    }

    public Variable<T, U> Add(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x + y.Value);
        }
        return new(_nodes.Count, x + y.Value);
    }

    public Variable<T, U> Add(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value + y);
        }
        return new(_nodes.Count, x.Value + y);
    }

    public Variable<T, U> Divide(Variable<T, U> x, Variable<T, U> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            var dfxy = -u * u;
            _nodes.Add(new(u, T.Zero, dfxy, x.Value * dfxy, -IBinaryFloatingPointIeee754<U>.Two * u * x.Value * dfxy, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<T, U> Divide(T x, Variable<T, U> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            var dfxy = -u * u;
            _nodes.Add(new(x * dfxy, -IBinaryFloatingPointIeee754<U>.Two * u * x * dfxy, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x * u);
        }
        return new(_nodes.Count, x * u);
    }

    public Variable<T, U> Divide(Variable<T, U> x, T y)
    {
        var u = T.One / y;
        if (_isTracking)
        {
            _nodes.Add(new(u, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, in Variable<Real<U>, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, T.Zero, (U)(x.Value * Real<U>.Floor(x.Value / y.Value)), T.Zero, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value % y.Value);
        }
        return new(_nodes.Count, x.Value % y.Value);
    }

    public Variable<Real<U>, U> Modulo(Real<U> x, in Variable<Real<U>, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new((U)(x * Real<U>.Floor(x / y.Value)), T.Zero, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x % y.Value);
        }
        return new(_nodes.Count, x % y.Value);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, Real<U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value % y);
        }
        return new(_nodes.Count, x.Value % y);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(y.Value, T.Zero, T.One, x.Value, T.Zero, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * y.Value);
        }
        return new(_nodes.Count, x.Value * y.Value);
    }

    public Variable<T, U> Multiply(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(x, T.Zero, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x * y.Value);
        }
        return new(_nodes.Count, x * y.Value);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(y, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * y);
        }
        return new(_nodes.Count, x.Value * y);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, T.Zero, -T.One, T.Zero, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value - y.Value);
        }
        return new(_nodes.Count, x.Value - y.Value);
    }

    public Variable<T, U> Subtract(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One, T.Zero, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x - y.Value);
        }
        return new(_nodes.Count, x - y.Value);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value - y);
        }
        return new(_nodes.Count, x.Value - y);
    }

    //
    // Other Operations
    //

    public Variable<T, U> Negate(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One, T.Zero, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, -x.Value);
        }
        return new(_nodes.Count, -x.Value);
    }

    // Exponential functions.

    public Variable<T, U> Exp(Variable<T, U> x)
    {
        var exp = T.Exp(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(exp, exp, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp);
        }
        return new(_nodes.Count, exp);
    }

    public Variable<T, U> Exp2(Variable<T, U> x)
    {
        var exp2 = T.Exp2(x.Value);
        if (_isTracking)
        {
            var df = (U)Real<U>.Ln2 * exp2;
            _nodes.Add(new(df, (U)Real<U>.Ln2 * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp2);
        }
        return new(_nodes.Count, exp2);
    }

    public Variable<T, U> Exp10(Variable<T, U> x)
    {
        var exp10 = T.Exp10(x.Value);
        if (_isTracking)
        {
            var df = (U)Real<U>.Ln10 * exp10;
            _nodes.Add(new(df, (U)Real<U>.Ln10 * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp10);
        }
        return new(_nodes.Count, exp10);
    }

    // Hyperbolic functions.

    public Variable<T, U> Acosh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = x.Value - T.One;
            var v = x.Value + T.One;
            _nodes.Add(new(T.One / (T.Sqrt(u) * T.Sqrt(v)), -x.Value * T.Pow(u, -IBinaryFloatingPointIeee754<U>.OneAndHalf) * T.Pow(v, -IBinaryFloatingPointIeee754<U>.OneAndHalf), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acosh(x.Value));
        }
        return new(_nodes.Count, T.Acosh(x.Value));
    }

    public Variable<T, U> Asinh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One + x.Value * x.Value;
            _nodes.Add(new(T.One / T.Sqrt(u), -x.Value * T.Pow(u, -IBinaryFloatingPointIeee754<U>.OneAndHalf), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asinh(x.Value));
        }
        return new(_nodes.Count, T.Asinh(x.Value));
    }

    public Variable<T, U> Atanh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var df = T.One / (T.One - x.Value * x.Value);
            _nodes.Add(new(df, IBinaryFloatingPointIeee754<U>.Two * df * x.Value * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atanh(x.Value));
        }
        return new(_nodes.Count, T.Atanh(x.Value));
    }

    public Variable<T, U> Cosh(Variable<T, U> x)
    {
        var cosh = T.Cosh(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.Sinh(x.Value), cosh, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cosh);
        }
        return new(_nodes.Count, cosh);
    }

    public Variable<T, U> Sinh(Variable<T, U> x)
    {
        var sinh = T.Sinh(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.Cosh(x.Value), sinh, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sinh);
        }
        return new(_nodes.Count, sinh);
    }

    public Variable<T, U> Tanh(Variable<T, U> x)
    {
        var tanh = T.Tanh(x.Value);
        if (_isTracking)
        {
            var u = T.One / T.Cosh(x.Value);
            var df = u * u;
            _nodes.Add(new(df, -IBinaryFloatingPointIeee754<U>.Two * df * tanh, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, tanh);
        }
        return new(_nodes.Count, tanh);
    }

    // Logarithmic functions.

    public Variable<T, U> Ln(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var df = T.One / x.Value;
            _nodes.Add(new(df, -df * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Ln(x.Value));
        }
        return new(_nodes.Count, T.Ln(x.Value));
    }

    public Variable<T, U> Log(Variable<T, U> x, Variable<T, U> b)
    {
        if (_isTracking)
        {
            var lnx = T.Ln(x.Value);
            var lnb = T.Ln(b.Value);
            var dfx = T.One / (lnb * x.Value);
            var dfb = -lnx / (lnb * lnb * b.Value);
            _nodes.Add(new(dfx, -dfx / x.Value, -dfx / (lnb * b.Value), dfb, -dfb * (IBinaryFloatingPointIeee754<U>.Two / lnb + T.One) / b.Value, x.Index, b.Index));
            return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
        }
        return new(_nodes.Count, T.Log(x.Value, b.Value));
    }

    public Variable<T, U> Log2(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One / x.Value;
            var df = u / (U)Real<U>.Ln2;
            _nodes.Add(new(df, -u * u / (U)Real<U>.Ln2, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log2(x.Value));
        }
        return new(_nodes.Count, T.Log2(x.Value));
    }

    public Variable<T, U> Log10(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One / x.Value;
            var df = u / (U)Real<U>.Ln10;
            _nodes.Add(new(df, -u * u / (U)Real<U>.Ln10, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log10(x.Value));
        }
        return new(_nodes.Count, T.Log10(x.Value));
    }

    // Power functions.

    public Variable<T, U> Pow(Variable<T, U> x, Variable<T, U> n)
    {
        var pow = T.Pow(x.Value, n.Value);
        if (_isTracking)
        {
            var lnx = T.Ln(x.Value);
            var pownmo = T.Pow(x.Value, n.Value - T.One);
            var dfn = lnx * pow;
            _nodes.Add(new(
                n.Value * pownmo,
                (n.Value - T.One) * n.Value * T.Pow(x.Value, n.Value - IBinaryFloatingPointIeee754<U>.Two),
                (T.One + lnx * n.Value) * pownmo,
                dfn,
                lnx * dfn,
                x.Index,
                n.Index));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(Variable<T, U> x, T n)
    {
        var pow = T.Pow(x.Value, n);
        if (_isTracking)
        {
            _nodes.Add(new(n * T.Pow(x.Value, n - T.One), (n - T.One) * n * T.Pow(x.Value, n - IBinaryFloatingPointIeee754<U>.Two), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(T x, Variable<T, U> n)
    {
        var pow = T.Pow(x, n.Value);
        if (_isTracking)
        {
            var lnx = T.Ln(x);
            _nodes.Add(new(lnx * pow, lnx * lnx * pow, n.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count - 1, pow);
    }

    // Root functions.

    public Variable<T, U> Cbrt(Variable<T, U> x)
    {
        var cbrt = T.Cbrt(x.Value);
        if (_isTracking)
        {
            var df = T.One / (IBinaryFloatingPointIeee754<U>.Three * cbrt * cbrt);
            _nodes.Add(new(df, -IBinaryFloatingPointIeee754<U>.Two * df / (IBinaryFloatingPointIeee754<U>.Three * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cbrt);
        }
        return new(_nodes.Count, cbrt);
    }

    public Variable<T, U> Root(Variable<T, U> x, Variable<T, U> n)
    {
        var root = T.Root(x.Value, n.Value);
        if (_isTracking)
        {
            var lnx = T.Ln(x.Value);
            var u = T.One / n.Value;
            var v = T.One / x.Value;
            var uu = u * u;
            var dfx = u * v * root;
            var dfn = -lnx * root * uu;
            _nodes.Add(new(
                dfx,
                (uu - u) * root * v * v,
                -root * (lnx * u + T.One) * v * uu,
                dfn,
                -(IBinaryFloatingPointIeee754<U>.Two * u + lnx * uu) * dfn,
                x.Index,
                n.Index));
            return new(_nodes.Count - 1, root);
        }
        return new(_nodes.Count, root);
    }

    public Variable<T, U> Sqrt(Variable<T, U> x)
    {
        var sqrt = T.Sqrt(x.Value);
        if (_isTracking)
        {
            var df = IBinaryFloatingPointIeee754<U>.Half / sqrt;
            _nodes.Add(new(df, -IBinaryFloatingPointIeee754<U>.Half / x.Value * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sqrt);
        }
        return new(_nodes.Count, sqrt);
    }

    // Trigonometric functions.

    public Variable<T, U> Cos(Variable<T, U> x)
    {
        var cos = T.Cos(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(-T.Sin(x.Value), -cos, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cos);
        }
        return new(_nodes.Count, cos);
    }

    public Variable<T, U> Acos(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One - x.Value * x.Value;
            _nodes.Add(new(-T.One / T.Sqrt(u), -x.Value * T.Pow(u, -IBinaryFloatingPointIeee754<U>.OneAndHalf), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acos(x.Value));
        }
        return new(_nodes.Count, T.Acos(x.Value));
    }

    public Variable<T, U> Asin(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One - x.Value * x.Value;
            _nodes.Add(new(T.One / T.Sqrt(u), x.Value * T.Pow(u, -IBinaryFloatingPointIeee754<U>.OneAndHalf), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asin(x.Value));
        }
        return new(_nodes.Count, T.Asin(x.Value));
    }

    public Variable<T, U> Atan(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var df = T.One / (T.One + x.Value * x.Value);
            _nodes.Add(new(df, -IBinaryFloatingPointIeee754<U>.Two * df * x.Value * df, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atan(x.Value));
        }
        return new(_nodes.Count, T.Atan(x.Value));
    }

    public Variable<Real<U>, U> Atan2(in Variable<Real<U>, U> y, in Variable<Real<U>, U> x)
    {
        if (_isTracking)
        {
            var u = y.Value * y.Value;
            var v = x.Value * x.Value;
            var a = Real<U>.One / (u + v);
            var b = a * a;
            var dfyy = -IBinaryFloatingPointIeee754<U>.Two * x.Value * b * y.Value;
            _nodes.Add(new(
                (U)(x.Value * a),
                (U)dfyy,
                (U)((u - v) * b),
                (U)(-y.Value * a),
                (U)(-dfyy),
                y.Index,
                x.Index));
            return new(_nodes.Count - 1, Real<U>.Atan2(y.Value, x.Value));
        }
        return new(_nodes.Count, Real<U>.Atan2(y.Value, x.Value));
    }

    public Variable<T, U> Sin(Variable<T, U> x)
    {
        var sin = T.Sin(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.Cos(x.Value), -sin, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sin);
        }
        return new(_nodes.Count, sin);
    }

    public Variable<T, U> Tan(Variable<T, U> x)
    {
        var tan = T.Tan(x.Value);
        if (_isTracking)
        {
            var sec = T.One / T.Cos(x.Value);
            var df = sec * sec;
            _nodes.Add(new(df, IBinaryFloatingPointIeee754<U>.Two * df * tan, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, tan);
        }
        return new(_nodes.Count, tan);
    }

    //
    // Custom Operations
    //

    /// <summary>Add a node to the Hessian tape using a custom unary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The derivative of the function.</param>
    /// <param name="dfxx">The second derivative of the function.</param>
    /// <returns>A variable.</returns>
    public Variable<T, U> Operation(Variable<T, U> x, Func<T, T> f, Func<T, T> dfx, Func<T, T> dfxx)
    {
        if (_isTracking)
        {
            _nodes.Add(new(dfx(x.Value), dfxx(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, f(x.Value));
        }
        return new(_nodes.Count, f(x.Value));
    }

    /// <summary>Add a node to the Hessian tape using a custom binary operation.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The first derivative of the function with respect to the first variable.</param>
    /// <param name="dfxx">The second derivative of the function with respect to the first variable.</param>
    /// <param name="dfxy">The second derivative of the function with respect to both variables.</param>
    /// <param name="dfy">The first derivative of the function with respect to the second variable.</param>
    /// <param name="dfyy">The second derivative of the function with respect to the second variable.</param>
    /// <returns>A variable.</returns>
    public Variable<T, U> Operation(
        Variable<T, U> x,
        Variable<T, U> y,
        Func<T, T, T> f,
        Func<T, T, T> dfx,
        Func<T, T, T> dfxx,
        Func<T, T, T> dfxy,
        Func<T, T, T> dfy,
        Func<T, T, T> dfyy)
    {
        if (_isTracking)
        {
            _nodes.Add(new(dfx(x.Value, y.Value), dfxx(x.Value, y.Value), dfxy(x.Value, y.Value), dfy(x.Value, y.Value), dfyy(x.Value, y.Value), x.Index, y.Index));
            return new(_nodes.Count - 1, f(x.Value, y.Value));
        }
        return new(_nodes.Count, f(x.Value, y.Value));
    }

    //
    // DifGeo
    //

    public AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in Vector2<T, U> x)
        where V : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2));

    public AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1)
        where V : IIndex
        => new(CreateVariable(x0), CreateVariable(x1));

    public AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in Vector3<T, U> x)
        where V : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2), CreateVariable(x.X3));

    public AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2)
        where V : IIndex
        => new(CreateVariable(x0), CreateVariable(x1), CreateVariable(x2));

    public AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in Vector4<T, U> x)
        where V : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2), CreateVariable(x.X3), CreateVariable(x.X4));

    public AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2, in T x3)
        where V : IIndex
        => new(CreateVariable(x0), CreateVariable(x1), CreateVariable(x2), CreateVariable(x3));
}
