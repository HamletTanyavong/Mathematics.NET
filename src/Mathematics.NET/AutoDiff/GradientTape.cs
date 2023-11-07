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
    private List<Node> _nodes;
    private int _variableCount;

    public GradientTape()
    {
        _nodes = new();
    }

    /// <summary>Get the number of nodes on the gradient tape</summary>
    public int NodeCount => _nodes.Count;

    /// <summary>Get the number of variables that are being tracked</summary>
    public int VariableCount => _variableCount;

    //
    // Methods
    //

    /// <summary>Print the nodes of the gradient tape to the console</summary>
    /// <param name="limit">The total number of nodes to print</param>
    public void PrintNodes(int limit = 100)
    {
        const string tab = "    ";

        ReadOnlySpan<Node> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        Node node;

        int i = 0;
        while (i < Math.Min(_variableCount, limit))
        {
            node = nodeSpan[i];
            Console.WriteLine($"Root Node {i}:");
            Console.WriteLine($"{tab}Weights: [{node.DX}, {node.DY}]");
            Console.WriteLine($"{tab}Parents: [{node.PX}, {node.PY}]");
            i++;
        }
        Console.WriteLine();
        while (i < Math.Min(nodeSpan.Length, limit))
        {
            node = nodeSpan[i];
            Console.WriteLine($"Node {i}:");
            Console.WriteLine($"{tab}Weights: [{node.DX}, {node.DY}]");
            Console.WriteLine($"{tab}Parents: [{node.PX}, {node.PY}]");
            i++;
        }
    }

    /// <summary>Perform reverse accumulation on the gradient tape and output the resulting gradients</summary>
    /// <param name="gradients">The gradients</param>
    /// <param name="seed">A seed value</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<Real> gradients, double seed = 1.0)
    {
        ReadOnlySpan<Node> nodesAsSpan = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodesAsSpan);

        var length = nodesAsSpan.Length;
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

    /// <summary>Create a variable for the gradient tape to track</summary>
    /// <param name="seed">A seed value</param>
    /// <returns>A variable</returns>
    public Variable CreateVariable(Real seed)
    {
        _nodes.Add(new(_variableCount));
        Variable variable = new(_variableCount, seed);
        _variableCount++;
        return variable;
    }

    //
    // Basic operations
    //

    public Variable Add(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, Real.One, x.Index, y.Index));
        return new(_nodes.Count - 1, x.Value + y.Value);
    }

    public Variable Add(Real c, Variable x)
    {
        _nodes.Add(new(Real.One, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, c + x.Value);
    }

    public Variable Add(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value + c);
    }

    public Variable Divide(Variable x, Variable y)
    {
        var u = Real.One / y.Value;
        _nodes.Add(new(Real.One / y.Value, -x.Value * u * u, x.Index, y.Index));
        return new(_nodes.Count - 1, x.Value * u);
    }

    public Variable Divide(Real c, Variable x)
    {
        var u = Real.One / x.Value;
        _nodes.Add(new(-c.Value * u * u, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    public Variable Divide(Variable x, Real c)
    {
        var u = Real.One / c.Value;
        _nodes.Add(new(u, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    public Variable Modulo(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, x.Value * Real.Floor(x.Value / y.Value), x.Index, y.Index));
        return new(_nodes.Count - 1, x.Value % y.Value);
    }

    public Variable Modulo(Real c, Variable x)
    {
        _nodes.Add(new(c * Real.Floor(c / x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, c % x.Value);
    }

    public Variable Modulo(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value % c);
    }

    public Variable Multiply(Variable x, Variable y)
    {
        _nodes.Add(new(y.Value, x.Value, x.Index, y.Index));
        return new(_nodes.Count - 1, x.Value * y.Value);
    }

    public Variable Multiply(Real c, Variable x)
    {
        _nodes.Add(new(c, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, c * x.Value);
    }

    public Variable Multiply(Variable x, Real c)
    {
        _nodes.Add(new(c, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * c);
    }

    public Variable Subtract(Variable x, Variable y)
    {
        _nodes.Add(new(Real.One, -Real.One, x.Index, y.Index));
        return new(_nodes.Count - 1, x.Value - y.Value);
    }

    public Variable Subtract(Real c, Variable x)
    {
        _nodes.Add(new(-Real.One, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, c - x.Value);
    }

    public Variable Subtract(Variable x, Real c)
    {
        _nodes.Add(new(Real.One, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value - c);
    }

    //
    // Other operations
    //

    // Exponential functions

    public Variable Exp(Variable x)
    {
        var exp = Real.Exp(x.Value);
        _nodes.Add(new(exp, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, exp);
    }

    public Variable Exp2(Variable x)
    {
        var exp2 = Real.Exp2(x.Value);
        _nodes.Add(new(Real.Ln2 * exp2, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, exp2);
    }

    public Variable Exp10(Variable x)
    {
        var exp10 = Real.Exp10(x.Value);
        _nodes.Add(new(Real.Ln10 * exp10, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, exp10);
    }

    // Hyperbolic functions

    public Variable Acosh(Variable x)
    {
        _nodes.Add(new(Real.One / (Complex.Sqrt(x.Value - Real.One) * Complex.Sqrt(x.Value + Real.One)).Re, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Acosh(x.Value));
    }

    public Variable Asinh(Variable x)
    {
        _nodes.Add(new(Real.One / Real.Sqrt(x.Value * x.Value + Real.One), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Asinh(x.Value));
    }

    public Variable Atanh(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.One - x.Value * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Atanh(x.Value));
    }

    public Variable Cosh(Variable x)
    {
        _nodes.Add(new(Real.Sinh(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Cosh(x.Value));
    }

    public Variable Sinh(Variable x)
    {
        _nodes.Add(new(Real.Cosh(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Sinh(x.Value));
    }

    public Variable Tanh(Variable x)
    {
        var u = Real.One / Real.Cosh(x.Value);
        _nodes.Add(new(u * u, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Tanh(x.Value));
    }

    // Logarithmic functions

    public Variable Ln(Variable x)
    {
        _nodes.Add(new(Real.One / x.Value, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Ln(x.Value));
    }

    public Variable Log(Variable x, Variable b)
    {
        var lnB = Real.Ln(b.Value);
        _nodes.Add(new(Real.One / (x.Value * lnB), -Real.Ln(x.Value) / (b.Value * lnB * lnB), x.Index, b.Index));
        return new(_nodes.Count - 1, Real.Log(x.Value, b.Value));
    }

    public Variable Log2(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.Ln2 * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Log2(x.Value));
    }

    public Variable Log10(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.Ln10 * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Log10(x.Value));
    }

    // Power functions

    public Variable Pow(Variable x, Variable y)
    {
        var pow = Real.Pow(x.Value, y.Value);
        _nodes.Add(new(y.Value * Real.Pow(x.Value, y.Value - Real.One), Real.Ln(x.Value) * pow, x.Index, y.Index));
        return new(_nodes.Count - 1, pow);
    }

    // Root functions

    public Variable Cbrt(Variable x)
    {
        var cbrt = Real.Cbrt(x.Value);
        _nodes.Add(new(Real.One / (3.0 * cbrt * cbrt), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, cbrt);
    }

    public Variable Root(Variable x, Variable n)
    {
        var root = Real.Root(x.Value, n.Value);
        _nodes.Add(new(root / (n.Value * x.Value), -Real.Ln(x.Value) * root / (n.Value * n.Value), x.Index, n.Index));
        return new(_nodes.Count - 1, root);
    }

    public Variable Sqrt(Variable x)
    {
        var sqrt = Real.Sqrt(x.Value);
        _nodes.Add(new(0.5 / sqrt, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, sqrt);
    }

    // Trigonometric functions

    public Variable Acos(Variable x)
    {
        _nodes.Add(new(-Real.One / Real.Sqrt(Real.One - x.Value * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Acos(x.Value));
    }

    public Variable Asin(Variable x)
    {
        _nodes.Add(new(Real.One / Real.Sqrt(Real.One - x.Value * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Asin(x.Value));
    }

    public Variable Atan(Variable x)
    {
        _nodes.Add(new(Real.One / (Real.One + x.Value * x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Atan(x.Value));
    }

    public Variable Atan2(Variable y, Variable x)
    {
        var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
        _nodes.Add(new(-x.Value * u, y.Value * u, y.Index, x.Index));
        return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
    }

    public Variable Cos(Variable x)
    {
        _nodes.Add(new(-Real.Sin(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Cos(x.Value));
    }

    public Variable Sin(Variable x)
    {
        _nodes.Add(new(Real.Cos(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Sin(x.Value));
    }

    public Variable Tan(Variable x)
    {
        var sec = Real.One / Real.Cos(x.Value);
        _nodes.Add(new(sec * sec, x.Index, _nodes.Count));
        return new(_nodes.Count - 1, Real.Tan(x.Value));
    }
}
