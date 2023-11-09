// <copyright file="GradientTape.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a gradient tape</summary>
public record class GradientTape
{
    // TODO: Measure performance with Stack<Node> instead of List<Node>
    // TODO: Consider using array pools or something similar
    private List<Node> _nodes;
    private int _variableCount;

    public GradientTape()
    {
        _nodes = new();
    }

    /// <summary>Get the number of nodes on the gradient tape.</summary>
    public int NodeCount => _nodes.Count;

    /// <summary>Get the number of variables that are being tracked.</summary>
    public int VariableCount => _variableCount;

    //
    // Methods
    //

    /// <summary>Create a variable for the gradient tape to track.</summary>
    /// <param name="seed">A seed value</param>
    /// <returns>A variable</returns>
    public Variable CreateVariable(Real seed)
    {
        _nodes.Add(new(_variableCount));
        Variable variable = new(_variableCount++, seed);
        return variable;
    }

    /// <summary>Print the nodes of the gradient tape to the console.</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <param name="limit">The total number of nodes to print</param>
    public void PrintNodes(CancellationToken cancellationToken, int limit = 100)
    {
        const string tab = "    ";

        ReadOnlySpan<Node> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        Node node;

        int i = 0;
        while (i < Math.Min(_variableCount, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            Console.WriteLine($"Root Node {i}:");
            Console.WriteLine($"{tab}Weights: [{node.DX}, {node.DY}]");
            Console.WriteLine($"{tab}Parents: [{node.PX}, {node.PY}]");
            i++;
        }

        CheckForCancellation(cancellationToken);
        Console.WriteLine();

        while (i < Math.Min(nodeSpan.Length, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            Console.WriteLine($"Node {i}:");
            Console.WriteLine($"{tab}Weights: [{node.DX}, {node.DY}]");
            Console.WriteLine($"{tab}Parents: [{node.PX}, {node.PY}]");
            i++;
        }

        static void CheckForCancellation(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Print node operation cancelled");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    /// <summary>Perform reverse accumulation on the gradient tape and output the resulting gradients.</summary>
    /// <param name="gradients">The gradients</param>
    /// <param name="seed">A seed value</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<Real> gradients, double seed = 1.0)
    {
        if (_variableCount == 0)
        {
            throw new Exception("Gradient tape contains no root nodes");
        }

        ReadOnlySpan<Node> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        var length = nodes.Length;
        Span<Real> partialGradients = new Real[length];
        partialGradients[length - 1] = seed;

        for (int i = length - 1; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var partialGradient = partialGradients[i];

            partialGradients[node.PX] += partialGradient * node.DX;
            partialGradients[node.PY] += partialGradient * node.DY;
        }

        gradients = partialGradients[.._variableCount];
    }

    //
    // Basic operations
    //

    /// <summary>Add two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable Add(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, Real.One, x._index, y._index));
        return new(_nodes.Count - 1, x.Value + y.Value);
    }

    /// <summary>Add a real value and a variable</summary>
    /// <param name="c">A real value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable Add(Real c, Variable x)
    {
        _nodes.Add(new(Real.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c + x.Value);
    }

    /// <summary>Add a variable and a real value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A real value</param>
    /// <returns>A variable</returns>
    public Variable Add(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value + c);
    }

    /// <summary>Divide two variables</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns>A variable</returns>
    public Variable Divide(Variable x, Variable y)
    {
        var u = Real.One / y.Value;
        _nodes.Add(new(Real.One / y.Value, -x.Value * u * u, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Divide a real value by a variable</summary>
    /// <param name="c">A real dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns>A variable</returns>
    public Variable Divide(Real c, Variable x)
    {
        var u = Real.One / x.Value;
        _nodes.Add(new(-c.Value * u * u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Divide a variable by a real value</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A real divisor</param>
    /// <returns>A variable</returns>
    public Variable Divide(Variable x, Real c)
    {
        var u = Real.One / c.Value;
        _nodes.Add(new(u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/></returns>
    public Variable Modulo(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, x.Value * Real.Floor(x.Value / y.Value), x._index, y._index));
        return new(_nodes.Count - 1, x.Value % y.Value);
    }

    /// <summary>Compute the modulo of a real value given a divisor</summary>
    /// <param name="c">A real dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns><paramref name="c"/> mod <paramref name="x"/></returns>
    public Variable Modulo(Real c, Variable x)
    {
        _nodes.Add(new(c * Real.Floor(c / x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, c % x.Value);
    }

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A real divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="c"/></returns>
    public Variable Modulo(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value % c);
    }

    /// <summary>Multiply two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable Multiply(Variable x, Variable y)
    {
        _nodes.Add(new(y.Value, x.Value, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * y.Value);
    }

    /// <summary>Multiply a real value by a variable</summary>
    /// <param name="c">A real value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable Multiply(Real c, Variable x)
    {
        _nodes.Add(new(c, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c * x.Value);
    }

    /// <summary>Multiply a variable by a real value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A real value</param>
    /// <returns>A variable</returns>
    public Variable Multiply(Variable x, Real c)
    {
        _nodes.Add(new(c, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * c);
    }

    /// <summary>Subract two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable Subtract(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, -Real.One, x._index, y._index));
        return new(_nodes.Count - 1, x.Value - y.Value);
    }

    /// <summary>Subtract a variable from a real value</summary>
    /// <param name="c">A real value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable Subtract(Real c, Variable x)
    {
        _nodes.Add(new(-Real.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c - x.Value);
    }

    /// <summary>Subtract a real value from a variable</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A real value</param>
    /// <returns>A variable</returns>
    public Variable Subtract(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value - c);
    }

    //
    // Other operations
    //

    // Exponential functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    public Variable Exp(Variable x)
    {
        var exp = Real.Exp(x.Value);
        _nodes.Add(new(exp, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    public Variable Exp2(Variable x)
    {
        var exp2 = Real.Exp2(x.Value);
        _nodes.Add(new(Real.Ln2 * exp2, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp2);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    public Variable Exp10(Variable x)
    {
        var exp10 = Real.Exp10(x.Value);
        _nodes.Add(new(Real.Ln10 * exp10, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp10);
    }

    // Hyperbolic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    public Variable Acosh(Variable x)
    {
        _nodes.Add(new(Real.One / (Complex.Sqrt(x.Value - Real.One) * Complex.Sqrt(x.Value + Real.One)).Re, x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Acosh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    public Variable Asinh(Variable x)
    {
        _nodes.Add(new(Real.One / Real.Sqrt(x.Value * x.Value + Real.One), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Asinh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    public Variable Atanh(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Atanh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    public Variable Cosh(Variable x)
    {
        _nodes.Add(new(Real.Sinh(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Cosh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    public Variable Sinh(Variable x)
    {
        _nodes.Add(new(Real.Cosh(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Sinh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    public Variable Tanh(Variable x)
    {
        var u = Real.One / Real.Cosh(x.Value);
        _nodes.Add(new(u * u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Tanh(x.Value));
    }

    // Logarithmic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    public Variable Ln(Variable x)
    {
        _nodes.Add(new(Real.One / x.Value, x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Ln(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    public Variable Log(Variable x, Variable b)
    {
        var lnB = Real.Ln(b.Value);
        _nodes.Add(new(Real.One / (x.Value * lnB), -Real.Ln(x.Value) / (b.Value * lnB * lnB), x._index, b._index));
        return new(_nodes.Count - 1, Real.Log(x.Value, b.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    public Variable Log2(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.Ln2 * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Log2(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    public Variable Log10(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.Ln10 * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Log10(x.Value));
    }

    // Power functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable Pow(Variable x, Variable y)
    {
        var pow = Real.Pow(x.Value, y.Value);
        _nodes.Add(new(y.Value * Real.Pow(x.Value, y.Value - Real.One), Real.Ln(x.Value) * pow, x._index, y._index));
        return new(_nodes.Count - 1, pow);
    }

    // Root functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    public Variable Cbrt(Variable x)
    {
        var cbrt = Real.Cbrt(x.Value);
        _nodes.Add(new(Real.One / (3.0 * cbrt * cbrt), x._index, _nodes.Count));
        return new(_nodes.Count - 1, cbrt);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    public Variable Root(Variable x, Variable n)
    {
        var root = Real.Root(x.Value, n.Value);
        _nodes.Add(new(root / (n.Value * x.Value), -Real.Ln(x.Value) * root / (n.Value * n.Value), x._index, n._index));
        return new(_nodes.Count - 1, root);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    public Variable Sqrt(Variable x)
    {
        var sqrt = Real.Sqrt(x.Value);
        _nodes.Add(new(0.5 / sqrt, x._index, _nodes.Count));
        return new(_nodes.Count - 1, sqrt);
    }

    // Trigonometric functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    public Variable Acos(Variable x)
    {
        _nodes.Add(new(-Real.One / Real.Sqrt(Real.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Acos(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    public Variable Asin(Variable x)
    {
        _nodes.Add(new(Real.One / Real.Sqrt(Real.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Asin(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    public Variable Atan(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.One + x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Atan(x.Value));
    }

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public Variable Atan2(Variable y, Variable x)
    {
        var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
        _nodes.Add(new(-x.Value * u, y.Value * u, y._index, x._index));
        return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    public Variable Cos(Variable x)
    {
        _nodes.Add(new(-Real.Sin(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Cos(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    public Variable Sin(Variable x)
    {
        _nodes.Add(new(Real.Cos(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Sin(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    public Variable Tan(Variable x)
    {
        var sec = Real.One / Real.Cos(x.Value);
        _nodes.Add(new(sec * sec, x._index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Tan(x.Value));
    }
}
