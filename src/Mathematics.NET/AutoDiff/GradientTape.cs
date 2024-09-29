// <copyright file="GradientTape.cs" company="Mathematics.NET">
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

// Below is an example of a graph that a typical gradient tape stores; nodes on a tape can be
// listed by using the PrintNodes() method:
//
// x => Node 0
// y => Node 1
// z => Node 2
//
// f(x, y, z) = Cos(x) / ((x + y) * Sin(z))
//
//                  ┌───────────╳ Cos ────────────┐
//     Root Node 1: ╳────┐                        │
//                       ╳ Sum ────┐              ╳ Divide ──── Result
//     Root Node 2: ╳────┘         |              │
//                                 ╳ Multiply ────┘
//                                 |
//     Root Node 3: ╳────╳ Sin ────┘
//
// Nodes, ╳, are numbered starting at the root in the order they were added using the CreateVariable()
// method. Then, from left to right in the expression Cos(x) / ((x + y) * Sin(z)), the nodes are
// numbered in increasing order. By convention, for unary operations, the index of the unspecified
// parent will be the index of the node.
//
// Node 3: 'Cos' with parent node/s: (0, 3)
// Node 4: 'Sum' with parent node/s: (0, 1)
// Node 5: 'Sin' with parent node/s: (2, 5)
// Node 6: 'Multiply' with parent node/s: (4, 5)
// Node 7: 'Divide' with parent node/s: (3, 6)
//
// Note: Operators such as '*' and '+' involving constants create nodes like those from unary
// operations:
//
//     ╳────╳ '*', '+', '%', etc. and constant ──── Result

#pragma warning disable IDE0032

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a gradient tape.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
public record class GradientTape<T> : ITape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    private bool _isTracking;
    private List<GradientNode<T>> _nodes;
    private int _variableCount;

    /// <summary>Create an instance of a gradient tape.</summary>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public GradientTape(bool isTracking = true)
    {
        _nodes = [];
        _isTracking = isTracking;
    }

    /// <summary>Create an instance of a gradient tape that will hold an expected number of nodes.</summary>
    /// <param name="n">An integer.</param>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public GradientTape(int n, bool isTracking = true)
    {
        _nodes = new(n);
        _isTracking = isTracking;
    }

    public bool IsTracking { get => _isTracking; set => _isTracking = value; }

    public int NodeCount => _nodes.Count;

    public int VariableCount => _variableCount;

    //
    // Methods
    //

    public Variable<T> CreateVariable(T seed)
    {
        _nodes.Add(new(_variableCount));
        Variable<T> variable = new(_variableCount++, seed);
        return variable;
    }

    public void LogNodes(ILogger<ITape<T>> logger, CancellationToken cancellationToken, int limit = 100)
    {
        const string template = """
            {NodeType}: {NodeNumber}
                Weights: [{DX}, {DY}]
                Parents: [{PX}, {PY}]
            """;

        ReadOnlySpan<GradientNode<T>> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        GradientNode<T> node;

        int i = 0;
        while (i < Math.Min(_variableCount, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            logger.LogInformation(template, "Root Node", i, node.DX, node.DY, node.PX, node.PY);
            i++;
        }

        while (i < Math.Min(nodeSpan.Length, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            logger.LogInformation(template, "Node", i, node.DX, node.DY, node.PX, node.PY);
            i++;
        }

        void CheckForCancellation(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("Log nodes operation cancelled");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient)
        => ReverseAccumulate(out gradient, T.One);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed)
    {
        if (_variableCount == 0)
            throw new Exception("The gradient tape contains no root nodes.");

        ReadOnlySpan<GradientNode<T>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        var length = nodes.Length;
        Span<T> gradientSpan = new T[length];
        gradientSpan[length - 1] = seed;

        for (int i = length - 1; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            gradientSpan[node.PX] += gradientElement * node.DX;
            gradientSpan[node.PY] += gradientElement * node.DY;
        }

        gradient = gradientSpan[.._variableCount];
    }

    //
    // Basic operations
    //

    public Variable<T> Add(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.One, x._index, y._index));
            return new(_nodes.Count - 1, x.Value + y.Value);
        }
        return new(_nodes.Count, x.Value + y.Value);
    }

    public Variable<T> Add(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, c + x.Value);
        }
        return new(_nodes.Count, c + x.Value);
    }

    public Variable<T> Add(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value + c);
        }
        return new(_nodes.Count, x.Value + c);
    }

    public Variable<T> Divide(Variable<T> x, Variable<T> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            _nodes.Add(new(T.One / y.Value, -x.Value * u * u, x._index, y._index));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<T> Divide(T c, Variable<T> x)
    {
        var u = T.One / x.Value;
        if (_isTracking)
        {
            _nodes.Add(new(-c * u * u, x._index, _nodes.Count));
            return new(_nodes.Count - 1, c * u);
        }
        return new(_nodes.Count, c * u);
    }

    public Variable<T> Divide(Variable<T> x, T c)
    {
        var u = T.One / c;
        if (_isTracking)
        {
            _nodes.Add(new(u, x._index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<Real> Modulo(in Variable<Real> x, in Variable<Real> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x.Value * Real.Floor(x.Value / y.Value), x._index, y._index));
            return new(_nodes.Count - 1, x.Value % y.Value);
        }
        return new(_nodes.Count, x.Value % y.Value);
    }

    public Variable<Real> Modulo(Real c, in Variable<Real> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(c * Real.Floor(c / x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, c % x.Value);
        }
        return new(_nodes.Count, c % x.Value);
    }

    public Variable<Real> Modulo(in Variable<Real> x, Real c)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value % c);
        }
        return new(_nodes.Count, x.Value % c);
    }

    public Variable<T> Multiply(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(y.Value, x.Value, x._index, y._index));
            return new(_nodes.Count - 1, x.Value * y.Value);
        }
        return new(_nodes.Count, x.Value * y.Value);
    }

    public Variable<T> Multiply(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(c, x._index, _nodes.Count));
            return new(_nodes.Count - 1, c * x.Value);
        }
        return new(_nodes.Count, c * x.Value);
    }

    public Variable<T> Multiply(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.Add(new(c, x._index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * c);
        }
        return new(_nodes.Count, x.Value * c);
    }

    public Variable<T> Subtract(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, -T.One, x._index, y._index));
            return new(_nodes.Count - 1, x.Value - y.Value);
        }
        return new(_nodes.Count, x.Value - y.Value);
    }

    public Variable<T> Subtract(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, c - x.Value);
        }
        return new(_nodes.Count, c - x.Value);
    }

    public Variable<T> Subtract(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value - c);
        }
        return new(_nodes.Count, x.Value - c);
    }

    //
    // Other operations
    //

    public Variable<T> Negate(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One, x._index, _nodes.Count));
            return new(_nodes.Count - 1, -x.Value);
        }
        return new(_nodes.Count, -x.Value);
    }

    // Exponential functions

    public Variable<T> Exp(Variable<T> x)
    {
        var exp = T.Exp(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(exp, x._index, _nodes.Count));
            return new(_nodes.Count - 1, exp);
        }
        return new(_nodes.Count, exp);
    }

    public Variable<T> Exp2(Variable<T> x)
    {
        var exp2 = T.Exp2(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(Real.Ln2 * exp2, x._index, _nodes.Count));
            return new(_nodes.Count - 1, exp2);
        }
        return new(_nodes.Count, exp2);
    }

    public Variable<T> Exp10(Variable<T> x)
    {
        var exp10 = T.Exp10(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(Real.Ln10 * exp10, x._index, _nodes.Count));
            return new(_nodes.Count - 1, exp10);
        }
        return new(_nodes.Count, exp10);
    }

    // Hyperbolic functions

    public Variable<T> Acosh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.Sqrt(x.Value - T.One) * T.Sqrt(x.Value + T.One)), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acosh(x.Value));
        }
        return new(_nodes.Count, T.Acosh(x.Value));
    }

    public Variable<T> Asinh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / T.Sqrt(x.Value * x.Value + T.One), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asinh(x.Value));
        }
        return new(_nodes.Count, T.Asinh(x.Value));
    }

    public Variable<T> Atanh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.One - x.Value * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atanh(x.Value));
        }
        return new(_nodes.Count, T.Atanh(x.Value));
    }

    public Variable<T> Cosh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Sinh(x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cosh(x.Value));
        }
        return new(_nodes.Count, T.Cosh(x.Value));
    }

    public Variable<T> Sinh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Cosh(x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sinh(x.Value));
        }
        return new(_nodes.Count, T.Sinh(x.Value));
    }

    public Variable<T> Tanh(Variable<T> x)
    {
        if (_isTracking)
        {
            var u = T.One / T.Cosh(x.Value);
            _nodes.Add(new(u * u, x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tanh(x.Value));
        }
        return new(_nodes.Count, T.Tanh(x.Value));
    }

    // Logarithmic functions

    public Variable<T> Ln(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / x.Value, x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Ln(x.Value));
        }
        return new(_nodes.Count, T.Ln(x.Value));
    }

    public Variable<T> Log(Variable<T> x, Variable<T> b)
    {
        if (_isTracking)
        {
            var lnB = T.Ln(b.Value);
            _nodes.Add(new(T.One / (x.Value * lnB), -T.Ln(x.Value) / (b.Value * lnB * lnB), x._index, b._index));
            return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
        }
        return new(_nodes.Count, T.Log(x.Value, b.Value));
    }

    public Variable<T> Log2(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (Real.Ln2 * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log2(x.Value));
        }
        return new(_nodes.Count, T.Log2(x.Value));
    }

    public Variable<T> Log10(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (Real.Ln10 * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log10(x.Value));
        }
        return new(_nodes.Count, T.Log10(x.Value));
    }

    // Power functions

    public Variable<T> Pow(Variable<T> x, Variable<T> y)
    {
        var pow = T.Pow(x.Value, y.Value);
        if (_isTracking)
        {
            _nodes.Add(new(y.Value * T.Pow(x.Value, y.Value - T.One), T.Ln(x.Value) * pow, x._index, y._index));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T> Pow(Variable<T> x, T y)
    {
        var pow = T.Pow(x.Value, y);
        if (_isTracking)
        {
            _nodes.Add(new(y * T.Pow(x.Value, y - T.One), x._index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T> Pow(T x, Variable<T> y)
    {
        var pow = T.Pow(x, y.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.Ln(x) * pow, y._index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    // Root functions

    public Variable<T> Cbrt(Variable<T> x)
    {
        var cbrt = T.Cbrt(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (3.0 * cbrt * cbrt), x._index, _nodes.Count));
            return new(_nodes.Count - 1, cbrt);
        }
        return new(_nodes.Count, cbrt);
    }

    public Variable<T> Root(Variable<T> x, Variable<T> n)
    {
        var root = T.Root(x.Value, n.Value);
        if (_isTracking)
        {
            _nodes.Add(new(root / (n.Value * x.Value), -T.Ln(x.Value) * root / (n.Value * n.Value), x._index, n._index));
            return new(_nodes.Count - 1, root);
        }
        return new(_nodes.Count, root);
    }

    public Variable<T> Sqrt(Variable<T> x)
    {
        var sqrt = T.Sqrt(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(0.5 / sqrt, x._index, _nodes.Count));
            return new(_nodes.Count - 1, sqrt);
        }
        return new(_nodes.Count, sqrt);
    }

    // Trigonometric functions

    public Variable<T> Acos(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One / T.Sqrt(T.One - x.Value * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acos(x.Value));
        }
        return new(_nodes.Count, T.Acos(x.Value));
    }

    public Variable<T> Asin(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / T.Sqrt(T.One - x.Value * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asin(x.Value));
        }
        return new(_nodes.Count, T.Asin(x.Value));
    }

    public Variable<T> Atan(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.One + x.Value * x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atan(x.Value));
        }
        return new(_nodes.Count, T.Atan(x.Value));
    }

    public Variable<Real> Atan2(in Variable<Real> y, in Variable<Real> x)
    {
        if (_isTracking)
        {
            var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
            _nodes.Add(new(x.Value * u, -y.Value * u, y._index, x._index));
            return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
        }
        return new(_nodes.Count, Real.Atan2(y.Value, x.Value));
    }

    public Variable<T> Cos(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.Sin(x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cos(x.Value));
        }
        return new(_nodes.Count, T.Cos(x.Value));
    }

    public Variable<T> Sin(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Cos(x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sin(x.Value));
        }
        return new(_nodes.Count, T.Sin(x.Value));
    }

    public Variable<T> Tan(Variable<T> x)
    {
        if (_isTracking)
        {
            var sec = T.One / T.Cos(x.Value);
            _nodes.Add(new(sec * sec, x._index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tan(x.Value));
        }
        return new(_nodes.Count, T.Tan(x.Value));
    }

    //
    // Custom operations
    //

    /// <summary>Add a node to the gradient tape using a custom unary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="df">The derivative of the function.</param>
    /// <returns>A variable.</returns>
    public Variable<T> CustomOperation(Variable<T> x, Func<T, T> f, Func<T, T> df)
    {
        if (_isTracking)
        {
            _nodes.Add(new(df(x.Value), x._index, _nodes.Count));
            return new(_nodes.Count - 1, f(x.Value));
        }
        return new(_nodes.Count, f(x.Value));
    }

    /// <summary>Add a node to the gradient tape using a custom binary operation.</summary>
    /// <param name="x">The first variable.</param>
    /// <param name="y">The second variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="dfx">The derivative of the function with respect to the first variable.</param>
    /// <param name="dfy">The derivative of the function with respect to the second variable.</param>
    /// <returns>A variable.</returns>
    public Variable<T> CustomOperation(Variable<T> x, Variable<T> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
    {
        if (_isTracking)
        {
            _nodes.Add(new(dfx(x.Value, y.Value), dfy(x.Value, y.Value), x._index, y._index));
            return new(_nodes.Count - 1, f(x.Value, y.Value));
        }
        return new(_nodes.Count, f(x.Value, y.Value));
    }

    //
    // DifGeo
    //

    public AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in Vector2<T> x)
        where U : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2));

    public AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed)
        where U : IIndex
        => new(CreateVariable(x0Seed), CreateVariable(x1Seed));

    public AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in Vector3<T> x)
        where U : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2), CreateVariable(x.X3));

    public AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed, in T x2Seed)
        where U : IIndex
        => new(CreateVariable(x0Seed), CreateVariable(x1Seed), CreateVariable(x2Seed));

    public AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in Vector4<T> x)
        where U : IIndex
        => new(CreateVariable(x.X1), CreateVariable(x.X2), CreateVariable(x.X3), CreateVariable(x.X4));

    public AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in T x0Seed, in T x1Seed, in T x2Seed, in T x3Seed)
        where U : IIndex
        => new(CreateVariable(x0Seed), CreateVariable(x1Seed), CreateVariable(x2Seed), CreateVariable(x3Seed));
}
