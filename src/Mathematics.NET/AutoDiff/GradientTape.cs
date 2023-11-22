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
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
public record class GradientTape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    // TODO: Measure performance with Stack<Node<T>> instead of List<Node<T>>
    // TODO: Consider using array pools or something similar
    private List<Node<T>> _nodes;
    private int _variableCount;

    public GradientTape()
    {
        _nodes = [];
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
    public Variable<T> CreateVariable(T seed)
    {
        _nodes.Add(new(_variableCount));
        Variable<T> variable = new(_variableCount++, seed);
        return variable;
    }

    /// <summary>Print the nodes of the gradient tape to the console.</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <param name="limit">The total number of nodes to print</param>
    public void PrintNodes(CancellationToken cancellationToken, int limit = 100)
    {
        const string tab = "    ";

        ReadOnlySpan<Node<T>> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        Node<T> node;

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

    /// <summary>Perform reverse accumulation on the gradient tape and output the resulting gradient.</summary>
    /// <param name="gradients">The gradient</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradients)
        => ReverseAccumulation(out gradients, T.One);

    /// <summary>Perform reverse accumulation on the gradient tape and output the resulting gradient.</summary>
    /// <param name="gradients">The gradient</param>
    /// <param name="seed">A seed value</param>
    /// <exception cref="Exception">The gradient tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradients, T seed)
    {
        if (_variableCount == 0)
        {
            throw new Exception("Gradient tape contains no root nodes");
        }

        ReadOnlySpan<Node<T>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);

        var length = nodes.Length;
        Span<T> gradientSpan = new T[length];
        gradientSpan[length - 1] = seed;

        for (int i = length - 1; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradient = gradientSpan[i];

            gradientSpan[node.PX] += gradient * node.DX;
            gradientSpan[node.PY] += gradient * node.DY;
        }

        gradients = gradientSpan[.._variableCount];
    }

    //
    // Basic operations
    //

    /// <summary>Add two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(T.One, T.One, x._index, y._index));
        return new(_nodes.Count - 1, x.Value + y.Value);
    }

    /// <summary>Add a constant value and a variable</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(T c, Variable<T> x)
    {
        _nodes.Add(new(T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c + x.Value);
    }

    /// <summary>Add a variable and a constant value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Add(Variable<T> x, T c)
    {
        _nodes.Add(new(T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value + c);
    }

    /// <summary>Divide two variables</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(Variable<T> x, Variable<T> y)
    {
        var u = T.One / y.Value;
        _nodes.Add(new(T.One / y.Value, -x.Value * u * u, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Divide a constant value by a variable</summary>
    /// <param name="c">A constant dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(T c, Variable<T> x)
    {
        var u = T.One / x.Value;
        _nodes.Add(new(-c * u * u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Divide a variable by a constant value</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A constant divisor</param>
    /// <returns>A variable</returns>
    public Variable<T> Divide(Variable<T> x, T c)
    {
        var u = T.One / c;
        _nodes.Add(new(u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * u);
    }

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A dividend</param>
    /// <param name="y">A divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="y"/></returns>
    public Variable<Real> Modulo(Variable<Real> x, Variable<Real> y)
    {
        _nodes.Add(new(T.One, x.Value * Real.Floor(x.Value / y.Value), x._index, y._index));
        return new(_nodes.Count - 1, x.Value % y.Value);
    }

    /// <summary>Compute the modulo of a real value given a divisor</summary>
    /// <param name="c">A real dividend</param>
    /// <param name="x">A variable divisor</param>
    /// <returns><paramref name="c"/> mod <paramref name="x"/></returns>
    public Variable<Real> Modulo(Real c, Variable<Real> x)
    {
        _nodes.Add(new(c * Real.Floor(c / x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, c % x.Value);
    }

    /// <summary>Compute the modulo of a variable given a divisor</summary>
    /// <param name="x">A variable dividend</param>
    /// <param name="c">A real divisor</param>
    /// <returns><paramref name="x"/> mod <paramref name="c"/></returns>
    public Variable<Real> Modulo(Variable<Real> x, Real c)
    {
        _nodes.Add(new(T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value % c);
    }

    /// <summary>Multiply two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(y.Value, x.Value, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * y.Value);
    }

    /// <summary>Multiply a constant value by a variable</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(T c, Variable<T> x)
    {
        _nodes.Add(new(c, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c * x.Value);
    }

    /// <summary>Multiply a variable by a constant value</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Multiply(Variable<T> x, T c)
    {
        _nodes.Add(new(c, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * c);
    }

    /// <summary>Subract two variables</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(T.One, -T.One, x._index, y._index));
        return new(_nodes.Count - 1, x.Value - y.Value);
    }

    /// <summary>Subtract a variable from a constant value</summary>
    /// <param name="c">A constant value</param>
    /// <param name="x">A variable</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(T c, Variable<T> x)
    {
        _nodes.Add(new(-T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c - x.Value);
    }

    /// <summary>Subtract a constant value from a variable</summary>
    /// <param name="x">A variable</param>
    /// <param name="c">A constant value</param>
    /// <returns>A variable</returns>
    public Variable<T> Subtract(Variable<T> x, T c)
    {
        _nodes.Add(new(T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value - c);
    }

    //
    // Other operations
    //

    /// <summary>Negate a variable</summary>
    /// <param name="x">A variable</param>
    /// <returns>Minus one times the variable</returns>
    public Variable<T> Negate(Variable<T> x)
    {
        _nodes.Add(new(-T.One, x._index, _nodes.Count));
        return new(_nodes.Count - 1, -x.Value);
    }

    // Exponential functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    public Variable<T> Exp(Variable<T> x)
    {
        var exp = T.Exp(x.Value);
        _nodes.Add(new(exp, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    public Variable<T> Exp2(Variable<T> x)
    {
        var exp2 = T.Exp2(x.Value);
        _nodes.Add(new(Real.Ln2 * exp2, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp2);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    public Variable<T> Exp10(Variable<T> x)
    {
        var exp10 = T.Exp10(x.Value);
        _nodes.Add(new(Real.Ln10 * exp10, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp10);
    }

    // Hyperbolic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    public Variable<T> Acosh(Variable<T> x)
    {
        _nodes.Add(new(T.One / (T.Sqrt(x.Value - T.One) * T.Sqrt(x.Value + T.One)), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Acosh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    public Variable<T> Asinh(Variable<T> x)
    {
        _nodes.Add(new(T.One / T.Sqrt(x.Value * x.Value + T.One), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Asinh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    public Variable<T> Atanh(Variable<T> x)
    {
        _nodes.Add(new(T.One / (T.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Atanh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    public Variable<T> Cosh(Variable<T> x)
    {
        _nodes.Add(new(T.Sinh(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Cosh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    public Variable<T> Sinh(Variable<T> x)
    {
        _nodes.Add(new(T.Cosh(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Sinh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    public Variable<T> Tanh(Variable<T> x)
    {
        var u = T.One / T.Cosh(x.Value);
        _nodes.Add(new(u * u, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Tanh(x.Value));
    }

    // Logarithmic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    public Variable<T> Ln(Variable<T> x)
    {
        _nodes.Add(new(T.One / x.Value, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Ln(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    public Variable<T> Log(Variable<T> x, Variable<T> b)
    {
        var lnB = T.Ln(b.Value);
        _nodes.Add(new(T.One / (x.Value * lnB), -T.Ln(x.Value) / (b.Value * lnB * lnB), x._index, b._index));
        return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    public Variable<T> Log2(Variable<T> x)
    {
        _nodes.Add(new(T.One / (Real.Ln2 * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Log2(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    public Variable<T> Log10(Variable<T> x)
    {
        _nodes.Add(new(T.One / (Real.Ln10 * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Log10(x.Value));
    }

    // Power functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable<T> Pow(Variable<T> x, Variable<T> y)
    {
        var pow = T.Pow(x.Value, y.Value);
        _nodes.Add(new(y.Value * T.Pow(x.Value, y.Value - T.One), T.Ln(x.Value) * pow, x._index, y._index));
        return new(_nodes.Count - 1, pow);
    }

    // Root functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    public Variable<T> Cbrt(Variable<T> x)
    {
        var cbrt = T.Cbrt(x.Value);
        _nodes.Add(new(T.One / (3.0 * cbrt * cbrt), x._index, _nodes.Count));
        return new(_nodes.Count - 1, cbrt);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    public Variable<T> Root(Variable<T> x, Variable<T> n)
    {
        var root = T.Root(x.Value, n.Value);
        _nodes.Add(new(root / (n.Value * x.Value), -T.Ln(x.Value) * root / (n.Value * n.Value), x._index, n._index));
        return new(_nodes.Count - 1, root);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    public Variable<T> Sqrt(Variable<T> x)
    {
        var sqrt = T.Sqrt(x.Value);
        _nodes.Add(new(0.5 / sqrt, x._index, _nodes.Count));
        return new(_nodes.Count - 1, sqrt);
    }

    // Trigonometric functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    public Variable<T> Acos(Variable<T> x)
    {
        _nodes.Add(new(-T.One / T.Sqrt(T.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Acos(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    public Variable<T> Asin(Variable<T> x)
    {
        _nodes.Add(new(T.One / T.Sqrt(T.One - x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Asin(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    public Variable<T> Atan(Variable<T> x)
    {
        _nodes.Add(new(T.One / (T.One + x.Value * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Atan(x.Value));
    }

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public Variable<Real> Atan2(Variable<Real> y, Variable<Real> x)
    {
        var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
        _nodes.Add(new(x.Value * u, -y.Value * u, y._index, x._index));
        return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    public Variable<T> Cos(Variable<T> x)
    {
        _nodes.Add(new(-T.Sin(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Cos(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    public Variable<T> Sin(Variable<T> x)
    {
        _nodes.Add(new(T.Cos(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Sin(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    public Variable<T> Tan(Variable<T> x)
    {
        var sec = T.One / T.Cos(x.Value);
        _nodes.Add(new(sec * sec, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Tan(x.Value));
    }

    //
    // Custom operations
    //

    /// <summary>Add a node to the gradient tape using a custom unary operation.</summary>
    /// <param name="x">A variable</param>
    /// <param name="f">A function</param>
    /// <param name="df">The derivative of the function</param>
    /// <returns>A variable</returns>
    public Variable<T> CustomOperation(Variable<T> x, Func<T, T> f, Func<T, T> df)
    {
        _nodes.Add(new(df(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, f(x.Value));
    }

    /// <summary>Add a node to the gradient tape using a custom binary operation.</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <param name="f">A function</param>
    /// <param name="dfx">The derivative of the function with respect to the first variable</param>
    /// <param name="dfy">The derivative of the function with respect to the second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> CustomOperation(Variable<T> x, Variable<T> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
    {
        _nodes.Add(new(dfx(x.Value, y.Value), dfy(x.Value, y.Value), x._index, y._index));
        return new(_nodes.Count - 1, f(x.Value, y.Value));
    }
}
