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
//     Root Node 2: ╳────┘         │              │
//                                 ╳ Multiply ────┘
//                                 │
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

#pragma warning disable IDE0032, IDE0058

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a gradient tape.</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T, U, V}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="U">A type that implements <see cref="IBinaryFloatingPointIeee754{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
public record class GradientTape<T, U> : ITape<T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private record struct Checkpoint
    {
        public Memory<T> Value;
        public HashSet<int> Visited;

        public Checkpoint()
        {
            Visited = [];
        }
    }

    private static NodeIndexComparer s_comparer = new();

    private bool _isTracking;
    private int _variableCount;
    private List<GradientNode<T, U>> _nodes;
    private Dictionary<int, Checkpoint> _checkpoints;

    /// <summary>Create an instance of a gradient tape.</summary>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public GradientTape(bool isTracking = true)
    {
        _isTracking = isTracking;
        _nodes = [];
        _checkpoints = [];
    }

    /// <summary>Create an instance of a gradient tape that will hold an expected number of nodes.</summary>
    /// <param name="n">An integer.</param>
    /// <param name="isTracking">Whether or not the tape should be tracking nodes.</param>
    public GradientTape(int n, bool isTracking = true)
    {
        _isTracking = isTracking;
        _nodes = new(n);
        _checkpoints = new(n);
    }

    public bool IsTracking { get => _isTracking; set => _isTracking = value; }

    public int NodeCount => _nodes.Count;

    public int VariableCount => _variableCount;

    //
    // Methods
    //

    // TODO: Add a diagnostic message to remind consumers to use the checkpoint if it is not being used.
    public Variable<T, U> CreateCheckpoint(Variable<T, U> x)
    {
        if (!_checkpoints.ContainsKey(x.Index))
        {
            if (_variableCount == 0)
                throw new AutoDiffException("The gradient tape contains no root nodes.");

            ReadOnlySpan<GradientNode<T, U>> nodes = CollectionsMarshal.AsSpan(_nodes);
            ref var start = ref MemoryMarshal.GetReference(nodes);

            // TODO: Figure out the ideal initial capacity.
            PriorityQueue<int, int> indices = new(_nodes.Count, s_comparer);
            HashSet<int> visited = new(_nodes.Count);

            indices.Enqueue(x.Index, x.Index);
            visited.Add(x.Index);

            Span<T> gradientSpan = new T[x.Index + 1];
            gradientSpan[x.Index] = T.One;

            while (indices.Count > 0)
            {
                var i = indices.Dequeue();

                var node = Unsafe.Add(ref start, i);
                var gradientElement = gradientSpan[i];

                if (_checkpoints.TryGetValue(i, out var checkpoint))
                {
                    var checkpointSpan = checkpoint.Value.Span;
                    for (int j = 0; j < _variableCount; j++)
                    {
                        gradientSpan[j] += checkpointSpan[j] * gradientElement;
                    }
                    visited.UnionWith(checkpoint.Visited);
                    continue;
                }

                gradientSpan[node.PX] += gradientElement * node.DX;
                if (node.PY != i)
                    gradientSpan[node.PY] += gradientElement * node.DY;

                // Do not change the order of checks within the if statements without careful consideration.
                if (node.PX >= _variableCount && visited.Add(node.PX))
                    indices.Enqueue(node.PX, node.PX);
                if (node.PY != i && node.PY >= _variableCount && visited.Add(node.PY))
                    indices.Enqueue(node.PY, node.PY);
            }

            _checkpoints.Add(x.Index, new()
            {
                Value = gradientSpan[.._variableCount].ToArray(),
                Visited = visited
            });
        }

        return x;
    }

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
                Weights: [{DX}, {DY}]
                Parents: [{PX}, {PY}]
            """;

        ReadOnlySpan<GradientNode<T, U>> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        GradientNode<T, U> node;

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
                logger.LogInformation("Log nodes operation cancelled.");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient)
        => ReverseAccumulate(out gradient, T.One, _nodes.Count - 1);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, int index)
        => ReverseAccumulate(out gradient, T.One, index);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed)
        => ReverseAccumulate(out gradient, seed, _nodes.Count - 1);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed, int index)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("The gradient tape contains no root nodes.");
        if (index < _variableCount || index >= _nodes.Count)
            throw new IndexOutOfRangeException();

        ReadOnlySpan<GradientNode<T, U>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        PriorityQueue<int, int> indices = new(_nodes.Count, s_comparer);
        HashSet<int> visited = new(_nodes.Count);

        indices.Enqueue(index, index);
        visited.Add(index);

        Span<T> gradientSpan = new T[index + 1];
        gradientSpan[index] = seed;

        while (indices.Count > 0)
        {
            var i = indices.Dequeue();

            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            if (_checkpoints.TryGetValue(i, out var checkpoint))
            {
                var checkpointSpan = checkpoint.Value.Span;
                for (int j = 0; j < _variableCount; j++)
                {
                    gradientSpan[j] += checkpointSpan[j] * gradientElement;
                }
                visited.UnionWith(checkpoint.Visited);
                continue;
            }

            gradientSpan[node.PX] += gradientElement * node.DX;
            if (node.PY != i)
                gradientSpan[node.PY] += gradientElement * node.DY;

            if (node.PX >= _variableCount && visited.Add(node.PX))
                indices.Enqueue(node.PX, node.PX);
            if (node.PY != i && node.PY >= _variableCount && visited.Add(node.PY))
                indices.Enqueue(node.PY, node.PY);
        }

        gradient = gradientSpan[.._variableCount];
    }

    //
    // Basic Operations
    //

    public Variable<T, U> Add(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value + y.Value);
        }
        return new(_nodes.Count, x.Value + y.Value);
    }

    public Variable<T, U> Add(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x + y.Value);
        }
        return new(_nodes.Count, x + y.Value);
    }

    public Variable<T, U> Add(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value + y);
        }
        return new(_nodes.Count, x.Value + y);
    }

    public Variable<T, U> Divide(Variable<T, U> x, Variable<T, U> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            _nodes.Add(new(T.One / y.Value, -x.Value * u * u, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<T, U> Divide(T x, Variable<T, U> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            _nodes.Add(new(-x * u * u, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x * u);
        }
        return new(_nodes.Count, x * u);
    }

    public Variable<T, U> Divide(Variable<T, U> x, T y)
    {
        var u = T.One / y;
        if (_isTracking)
        {
            _nodes.Add(new(u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, in Variable<Real<U>, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, (U)(x.Value * Real<U>.Floor(x.Value / y.Value)), x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value % y.Value);
        }
        return new(_nodes.Count, x.Value % y.Value);
    }

    public Variable<Real<U>, U> Modulo(Real<U> x, in Variable<Real<U>, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new((U)(x * Real<U>.Floor(x / y.Value)), y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x % y.Value);
        }
        return new(_nodes.Count, x % y.Value);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, Real<U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value % y);
        }
        return new(_nodes.Count, x.Value % y);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(y.Value, x.Value, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * y.Value);
        }
        return new(_nodes.Count, x.Value * y.Value);
    }

    public Variable<T, U> Multiply(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(x, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x * y.Value);
        }
        return new(_nodes.Count, x * y.Value);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(y, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * y);
        }
        return new(_nodes.Count, x.Value * y);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, -T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value - y.Value);
        }
        return new(_nodes.Count, x.Value - y.Value);
    }

    public Variable<T, U> Subtract(T x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, x - y.Value);
        }
        return new(_nodes.Count, x - y.Value);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, T y)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One, x.Index, _nodes.Count));
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
            _nodes.Add(new(-T.One, x.Index, _nodes.Count));
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
            _nodes.Add(new(exp, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp);
        }
        return new(_nodes.Count, exp);
    }

    public Variable<T, U> Exp2(Variable<T, U> x)
    {
        var exp2 = T.Exp2(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new((U)Real<U>.Ln2 * exp2, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp2);
        }
        return new(_nodes.Count, exp2);
    }

    public Variable<T, U> Exp10(Variable<T, U> x)
    {
        var exp10 = T.Exp10(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new((U)Real<U>.Ln10 * exp10, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp10);
        }
        return new(_nodes.Count, exp10);
    }

    // Hyperbolic functions.

    public Variable<T, U> Acosh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.Sqrt(x.Value - T.One) * T.Sqrt(x.Value + T.One)), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acosh(x.Value));
        }
        return new(_nodes.Count, T.Acosh(x.Value));
    }

    public Variable<T, U> Asinh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / T.Sqrt(x.Value * x.Value + T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asinh(x.Value));
        }
        return new(_nodes.Count, T.Asinh(x.Value));
    }

    public Variable<T, U> Atanh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atanh(x.Value));
        }
        return new(_nodes.Count, T.Atanh(x.Value));
    }

    public Variable<T, U> Cosh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Sinh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cosh(x.Value));
        }
        return new(_nodes.Count, T.Cosh(x.Value));
    }

    public Variable<T, U> Sinh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Cosh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sinh(x.Value));
        }
        return new(_nodes.Count, T.Sinh(x.Value));
    }

    public Variable<T, U> Tanh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One / T.Cosh(x.Value);
            _nodes.Add(new(u * u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tanh(x.Value));
        }
        return new(_nodes.Count, T.Tanh(x.Value));
    }

    // Logarithmic functions.

    public Variable<T, U> Ln(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / x.Value, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Ln(x.Value));
        }
        return new(_nodes.Count, T.Ln(x.Value));
    }

    public Variable<T, U> Log(Variable<T, U> x, Variable<T, U> b)
    {
        if (_isTracking)
        {
            var lnB = T.Ln(b.Value);
            _nodes.Add(new(T.One / (x.Value * lnB), -T.Ln(x.Value) / (b.Value * lnB * lnB), x.Index, b.Index));
            return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
        }
        return new(_nodes.Count, T.Log(x.Value, b.Value));
    }

    public Variable<T, U> Log2(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / ((U)Real<U>.Ln2 * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log2(x.Value));
        }
        return new(_nodes.Count, T.Log2(x.Value));
    }

    public Variable<T, U> Log10(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / ((U)Real<U>.Ln10 * x.Value), x.Index, _nodes.Count));
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
            _nodes.Add(new(n.Value * T.Pow(x.Value, n.Value - T.One), T.Ln(x.Value) * pow, x.Index, n.Index));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(Variable<T, U> x, T n)
    {
        var pow = T.Pow(x.Value, n);
        if (_isTracking)
        {
            _nodes.Add(new(n * T.Pow(x.Value, n - T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(T x, Variable<T, U> n)
    {
        var pow = T.Pow(x, n.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.Ln(x) * pow, n.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    // Root functions.

    public Variable<T, U> Cbrt(Variable<T, U> x)
    {
        var cbrt = T.Cbrt(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (IBinaryFloatingPointIeee754<U>.Three * cbrt * cbrt), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cbrt);
        }
        return new(_nodes.Count, cbrt);
    }

    public Variable<T, U> Root(Variable<T, U> x, Variable<T, U> n)
    {
        var root = T.Root(x.Value, n.Value);
        if (_isTracking)
        {
            _nodes.Add(new(root / (n.Value * x.Value), -T.Ln(x.Value) * root / (n.Value * n.Value), x.Index, n.Index));
            return new(_nodes.Count - 1, root);
        }
        return new(_nodes.Count, root);
    }

    public Variable<T, U> Sqrt(Variable<T, U> x)
    {
        var sqrt = T.Sqrt(x.Value);
        if (_isTracking)
        {
            _nodes.Add(new(IBinaryFloatingPointIeee754<U>.Half / sqrt, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sqrt);
        }
        return new(_nodes.Count, sqrt);
    }

    // Trigonometric functions.

    public Variable<T, U> Acos(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acos(x.Value));
        }
        return new(_nodes.Count, T.Acos(x.Value));
    }

    public Variable<T, U> Asin(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asin(x.Value));
        }
        return new(_nodes.Count, T.Asin(x.Value));
    }

    public Variable<T, U> Atan(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.One / (T.One + x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atan(x.Value));
        }
        return new(_nodes.Count, T.Atan(x.Value));
    }

    public Variable<Real<U>, U> Atan2(in Variable<Real<U>, U> y, in Variable<Real<U>, U> x)
    {
        if (_isTracking)
        {
            var u = Real<U>.One / (x.Value * x.Value + y.Value * y.Value);
            _nodes.Add(new((U)(x.Value * u), (U)(-y.Value * u), y.Index, x.Index));
            return new(_nodes.Count - 1, Real<U>.Atan2(y.Value, x.Value));
        }
        return new(_nodes.Count, Real<U>.Atan2(y.Value, x.Value));
    }

    public Variable<T, U> Cos(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(-T.Sin(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cos(x.Value));
        }
        return new(_nodes.Count, T.Cos(x.Value));
    }

    public Variable<T, U> Sin(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.Add(new(T.Cos(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sin(x.Value));
        }
        return new(_nodes.Count, T.Sin(x.Value));
    }

    public Variable<T, U> Tan(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var sec = T.One / T.Cos(x.Value);
            _nodes.Add(new(sec * sec, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tan(x.Value));
        }
        return new(_nodes.Count, T.Tan(x.Value));
    }

    //
    // Custom Operations
    //

    /// <summary>Add a node to the gradient tape using a custom unary operation.</summary>
    /// <param name="x">A variable.</param>
    /// <param name="f">A function.</param>
    /// <param name="df">The derivative of the function.</param>
    /// <returns>A variable.</returns>
    public Variable<T, U> Operation(Variable<T, U> x, Func<T, T> f, Func<T, T> df)
    {
        if (_isTracking)
        {
            _nodes.Add(new(df(x.Value), x.Index, _nodes.Count));
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
    public Variable<T, U> Operation(Variable<T, U> x, Variable<T, U> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
    {
        if (_isTracking)
        {
            _nodes.Add(new(dfx(x.Value, y.Value), dfy(x.Value, y.Value), x.Index, y.Index));
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
