// <copyright file="HessianTape.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mathematics.NET.AutoDiff;

/// <summary>Represents a Hessian tape</summary>
/// <typeparam name="T">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/></typeparam>
public record class HessianTape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    private List<HessianNode<T>> _nodes;
    private int _variableCount;

    public HessianTape()
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

        ReadOnlySpan<HessianNode<T>> nodeSpan = CollectionsMarshal.AsSpan(_nodes);
        HessianNode<T> node;

        int i = 0;
        while (i < Math.Min(_variableCount, limit))
        {
            CheckForCancellation(cancellationToken);
            node = nodeSpan[i];
            Console.WriteLine($"Root Node {i}:");
            Console.WriteLine($"{tab}Weights: [[{node.DX}, {node.DXX}, {node.DXY}],");
            Console.WriteLine($"{tab}          [{node.DY}, {node.DYY}, {node.DXY}]]");
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
            Console.WriteLine($"{tab}Weights: [[{node.DX}, {node.DXX}, {node.DXY}],");
            Console.WriteLine($"{tab}          [{node.DY}, {node.DYY}, {node.DXY}]]");
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

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradient)
        => ReverseAccumulation(out gradient, T.One);

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan2D<T> hessian)
        => ReverseAccumulation(out hessian, T.One);

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient</param>
    /// <param name="hessian">The Hessian</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian)
        => ReverseAccumulation(out gradient, out hessian, T.One);

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient.</summary>
    /// <param name="gradient">The gradient</param>
    /// <param name="seed">A seed value</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradient, T seed)
    {
        if (_variableCount == 0)
        {
            throw new Exception("Hessian tape contains no root nodes");
        }

        ReadOnlySpan<HessianNode<T>> nodes = CollectionsMarshal.AsSpan(_nodes);
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

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting Hessian.</summary>
    /// <param name="hessian">The Hessian</param>
    /// <param name="seed">A seed value</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan2D<T> hessian, T seed)
    {
        if (_variableCount == 0)
        {
            throw new Exception("Hessian tape contains no root nodes");
        }

        ReadOnlySpan<HessianNode<T>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);
        var length = nodes.Length;

        Span<T> gradientSpan = new T[length];
        gradientSpan[length - 1] = seed;

        Span2D<T> hessianSpan = new T[length, length];

        for (int i = length - 1; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            EdgePush(hessianSpan, ref node, i);
            Accumulate(hessianSpan, ref node, gradientElement);

            gradientSpan[node.PX] += gradientElement * node.DX;
            gradientSpan[node.PY] += gradientElement * node.DY;
        }

        hessian = hessianSpan.Slice(0, 0, _variableCount, _variableCount);
    }

    // The following method uses the edge-pushing algorithm outlined by Gower and Mello: https://arxiv.org/pdf/2007.15040.pdf.
    // TODO: use newer variations/versions of this algorithm since they are more performant

    /// <summary>Perform reverse accumulation on the Hessian tape and output the resulting gradient and Hessian.</summary>
    /// <param name="gradient">The gradient</param>
    /// <param name="hessian">The Hessian</param>
    /// <param name="seed">A seed value</param>
    /// <exception cref="Exception">The Hessian tape does not have any tracked variables.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ReverseAccumulation(out ReadOnlySpan<T> gradient, out ReadOnlySpan2D<T> hessian, T seed)
    {
        if (_variableCount == 0)
        {
            throw new Exception("Hessian tape contains no root nodes");
        }

        ReadOnlySpan<HessianNode<T>> nodes = CollectionsMarshal.AsSpan(_nodes);
        ref var start = ref MemoryMarshal.GetReference(nodes);
        var length = nodes.Length;

        Span<T> gradientSpan = new T[length];
        gradientSpan[length - 1] = seed;

        Span2D<T> hessianSpan = new T[length, length];

        for (int i = length - 1; i >= _variableCount; i--)
        {
            var node = Unsafe.Add(ref start, i);
            var gradientElement = gradientSpan[i];

            EdgePush(hessianSpan, ref node, i);
            Accumulate(hessianSpan, ref node, gradientElement);

            gradientSpan[node.PX] += gradientElement * node.DX;
            gradientSpan[node.PY] += gradientElement * node.DY;
        }

        gradient = gradientSpan[.._variableCount];
        hessian = hessianSpan.Slice(0, 0, _variableCount, _variableCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void EdgePush(Span2D<T> weight, ref HessianNode<T> node, int i)
    {
        for (int p = 0; p <= i; p++)
        {
            if (weight[i, p] == 0 || weight[p, i] == 0)
            {
                continue;
            }
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
                    weight[p, p] += 2 * node.DX * weight[i, p];
                }

                if (node.PY != p)
                {
                    var x = node.DY * weight[i, p];
                    weight[node.PY, p] += x;
                    weight[p, node.PY] += x;
                }
                else
                {
                    weight[p, p] += 2 * node.DY * weight[i, p];
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
    private static void Accumulate(Span2D<T> weight, ref HessianNode<T> node, T v)
    {
        weight[node.PX, node.PX] += v * node.DXX;
        weight[node.PX, node.PY] += v * node.DXY;
        weight[node.PY, node.PX] += v * node.DXY;
        weight[node.PY, node.PY] += v * node.DYY;
    }

    //
    // Basic operations
    //

    /// <inheritdoc cref="GradientTape{T}.Add(Variable{T}, Variable{T})"/>
    public Variable<T> Add(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(T.One, T.Zero, T.Zero, T.One, T.Zero, x._index, y._index));
        return new(_nodes.Count - 1, x.Value + y.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Add(T, Variable{T})"/>
    public Variable<T> Add(T c, Variable<T> x)
    {
        _nodes.Add(new(T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c + x.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Add(Variable{T}, T)"/>
    public Variable<T> Add(Variable<T> x, T c)
    {
        _nodes.Add(new(T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value + c);
    }

    /// <inheritdoc cref="GradientTape{T}.Divide(Variable{T}, Variable{T})"/>
    public Variable<T> Divide(Variable<T> x, Variable<T> y)
    {
        var n = T.One / y.Value;
        var dfxy = -n * n;
        _nodes.Add(new(n, T.Zero, dfxy, x.Value * dfxy, -2.0 * n * x.Value * dfxy, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * n);
    }

    /// <inheritdoc cref="GradientTape{T}.Divide(T, Variable{T})"/>
    public Variable<T> Divide(T c, Variable<T> x)
    {
        var n = T.One / x.Value;
        var dfxy = -n * n;
        _nodes.Add(new(c * dfxy, -2.0 * n * c * dfxy, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c * n);
    }

    /// <inheritdoc cref="GradientTape{T}.Divide(Variable{T}, T)"/>
    public Variable<T> Divide(Variable<T> x, T c)
    {
        var n = T.One / c;
        _nodes.Add(new(n, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * n);
    }

    /// <inheritdoc cref="GradientTape{T}.Modulo(Variable{Real}, Variable{Real})"/>
    public Variable<Real> Modulo(Variable<Real> x, Variable<Real> y)
    {
        _nodes.Add(new(T.One, T.Zero, T.Zero, x.Value * Real.Floor(x.Value / y.Value), T.Zero, x._index, y._index));
        return new(_nodes.Count - 1, x.Value % y.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Modulo(Real, Variable{Real})"/>
    public Variable<Real> Modulo(Real c, Variable<Real> x)
    {
        _nodes.Add(new(c.Value * Real.Floor(c.Value / x.Value), T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c % x.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Modulo(Variable{Real}, Real)"/>
    public Variable<Real> Modulo(Variable<Real> x, Real c)
    {
        _nodes.Add(new(T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value % c);
    }

    /// <inheritdoc cref="GradientTape{T}.Multiply(Variable{T}, Variable{T})"/>
    public Variable<T> Multiply(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(y.Value, T.Zero, T.One, x.Value, T.Zero, x._index, y._index));
        return new(_nodes.Count - 1, x.Value * y.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Multiply(T, Variable{T})"/>
    public Variable<T> Multiply(T c, Variable<T> x)
    {
        _nodes.Add(new(c, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c * x.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Multiply(Variable{T}, T)"/>
    public Variable<T> Multiply(Variable<T> x, T c)
    {
        _nodes.Add(new(c, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value * c);
    }

    /// <inheritdoc cref="GradientTape{T}.Subtract(Variable{T}, Variable{T})"/>
    public Variable<T> Subtract(Variable<T> x, Variable<T> y)
    {
        _nodes.Add(new(T.One, T.Zero, T.Zero, -T.One, T.Zero, x._index, y._index));
        return new(_nodes.Count - 1, x.Value - y.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Subtract(T, Variable{T})"/>
    public Variable<T> Subtract(T c, Variable<T> x)
    {
        _nodes.Add(new(-T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, c - x.Value);
    }

    /// <inheritdoc cref="GradientTape{T}.Subtract(Variable{T}, T)"/>
    public Variable<T> Subtract(Variable<T> x, T c)
    {
        _nodes.Add(new(T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, x.Value - c);
    }

    //
    // Other operations
    //

    /// <inheritdoc cref="GradientTape{T}.Negate(Variable{T})"/>
    public Variable<T> Negate(Variable<T> x)
    {
        _nodes.Add(new(-T.One, T.Zero, x._index, _nodes.Count));
        return new(_nodes.Count - 1, -x.Value);
    }

    // Exponential functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp(T)"/>
    public Variable<T> Exp(Variable<T> x)
    {
        var exp = T.Exp(x.Value);
        _nodes.Add(new(exp, exp, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp2(T)"/>
    public Variable<T> Exp2(Variable<T> x)
    {
        var exp2 = T.Exp2(x.Value);
        var df = Real.Ln2 * exp2;
        _nodes.Add(new(df, Real.Ln2 * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp2);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Exp10(T)"/>
    public Variable<T> Exp10(Variable<T> x)
    {
        var exp10 = T.Exp10(x.Value);
        var df = Real.Ln10 * exp10;
        _nodes.Add(new(df, Real.Ln10 * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, exp10);
    }

    // Hyperbolic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acosh(T)"/>
    public Variable<T> Acosh(Variable<T> x)
    {
        var u = x.Value - T.One;
        var v = x.Value + T.One;
        _nodes.Add(new(T.One / (T.Sqrt(u) * T.Sqrt(v)), -x.Value * T.Pow(u, -1.5) * T.Pow(v, -1.5), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Acosh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asinh(T)"/>
    public Variable<T> Asinh(Variable<T> x)
    {
        var u = T.One + x.Value * x.Value;
        _nodes.Add(new(T.One / T.Sqrt(u), -x.Value * T.Pow(u, -1.5), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Asinh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atanh(T)"/>
    public Variable<T> Atanh(Variable<T> x)
    {
        var df = T.One / (T.One - x.Value * x.Value);
        _nodes.Add(new(df, 2.0 * df * x.Value * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Atanh(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cosh(T)"/>
    public Variable<T> Cosh(Variable<T> x)
    {
        var cosh = T.Cosh(x.Value);
        _nodes.Add(new(T.Sinh(x.Value), cosh, x._index, _nodes.Count));
        return new(_nodes.Count - 1, cosh);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sinh(T)"/>
    public Variable<T> Sinh(Variable<T> x)
    {
        var sinh = T.Sinh(x.Value);
        _nodes.Add(new(T.Cosh(x.Value), sinh, x._index, _nodes.Count));
        return new(_nodes.Count - 1, sinh);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tanh(T)"/>
    public Variable<T> Tanh(Variable<T> x)
    {
        var tanh = T.Tanh(x.Value);
        var u = T.One / T.Cosh(x.Value);
        var df = u * u;
        _nodes.Add(new(df, -2.0 * df * tanh, x._index, _nodes.Count));
        return new(_nodes.Count - 1, tanh);
    }

    // Logarithmic functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Ln(T)"/>
    public Variable<T> Ln(Variable<T> x)
    {
        var df = T.One / x.Value;
        _nodes.Add(new(df, -df * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Ln(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log(T, T)"/>
    public Variable<T> Log(Variable<T> x, Variable<T> b)
    {
        var lnx = T.Ln(x.Value);
        var lnb = T.Ln(b.Value);
        var dfx = T.One / (lnb * x.Value);
        var dfb = -lnx / (lnb * lnb * b.Value);
        _nodes.Add(new(dfx, -dfx / x.Value, -dfx / (lnb * b.Value), dfb, -dfb * (2.0 / lnb + T.One) / b.Value, x._index, b._index));
        return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log2(T)"/>
    public Variable<T> Log2(Variable<T> x)
    {
        var u = T.One / x.Value;
        var df = u / Real.Ln2;
        _nodes.Add(new(df, -u * u / Real.Ln2, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Log2(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Log10(T)"/>
    public Variable<T> Log10(Variable<T> x)
    {
        var u = T.One / x.Value;
        var df = u / Real.Ln10;
        _nodes.Add(new(df, -u * u / Real.Ln10, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Log10(x.Value));
    }

    // Power functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Pow(T, T)"/>
    public Variable<T> Pow(Variable<T> x, Variable<T> n)
    {
        var pow = T.Pow(x.Value, n.Value);
        var lnx = T.Ln(x.Value);
        var pownmo = T.Pow(x.Value, n.Value - T.One);
        var dfn = lnx * pow;
        _nodes.Add(new(
            n.Value * pownmo,
            (n.Value - T.One) * n.Value * T.Pow(x.Value, n.Value - 2.0),
            (T.One + lnx * n.Value) * pownmo,
            dfn,
            lnx * dfn,
            x._index,
            n._index));
        return new(_nodes.Count - 1, pow);
    }

    // Root functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cbrt(T)"/>
    public Variable<T> Cbrt(Variable<T> x)
    {
        var cbrt = T.Cbrt(x.Value);
        var df = T.One / (3.0 * cbrt * cbrt);
        _nodes.Add(new(df, -2.0 * df / (3.0 * x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, cbrt);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Root(T, T)"/>
    public Variable<T> Root(Variable<T> x, Variable<T> n)
    {
        var root = T.Root(x.Value, n.Value);
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
            -(2.0 * u + lnx * uu) * dfn,
            x._index,
            n._index));
        return new(_nodes.Count - 1, root);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sqrt(T)"/>
    public Variable<T> Sqrt(Variable<T> x)
    {
        var sqrt = T.Sqrt(x.Value);
        var df = 0.5 / sqrt;
        _nodes.Add(new(df, -0.5 / x.Value * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, sqrt);
    }

    // Trigonometric functions

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Cos(T)"/>
    public Variable<T> Cos(Variable<T> x)
    {
        var cos = T.Cos(x.Value);
        _nodes.Add(new(-T.Sin(x.Value), -cos, x._index, _nodes.Count));
        return new(_nodes.Count - 1, cos);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Acos(T)"/>
    public Variable<T> Acos(Variable<T> x)
    {
        var u = T.One - x.Value * x.Value;
        _nodes.Add(new(-T.One / T.Sqrt(u), -x.Value * T.Pow(u, -1.5), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Acos(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Asin(T)"/>
    public Variable<T> Asin(Variable<T> x)
    {
        var u = T.One - x.Value * x.Value;
        _nodes.Add(new(T.One / T.Sqrt(u), x.Value * T.Pow(u, -1.5), x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Asin(x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Atan(T)"/>
    public Variable<T> Atan(Variable<T> x)
    {
        var df = T.One / (T.One + x.Value * x.Value);
        _nodes.Add(new(df, -2.0 * df * x.Value * df, x._index, _nodes.Count));
        return new(_nodes.Count - 1, T.Asin(x.Value));
    }

    /// <inheritdoc cref="IReal{T}.Atan2(T, T)"/>
    public Variable<Real> Atan2(Variable<Real> y, Variable<Real> x)
    {
        var u = y.Value * y.Value;
        var v = x.Value * x.Value;
        var a = Real.One / (u + v);
        var b = a * a;
        var dfyy = -2.0 * x.Value * b * y.Value;
        _nodes.Add(new(
            x.Value * a,
            dfyy,
            (u - v) * b,
            -y.Value * a,
            -dfyy,
            y._index,
            x._index));
        return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Sin(T)"/>
    public Variable<T> Sin(Variable<T> x)
    {
        var sin = T.Sin(x.Value);
        _nodes.Add(new(T.Cos(x.Value), -sin, x._index, _nodes.Count));
        return new(_nodes.Count - 1, sin);
    }

    /// <inheritdoc cref="IDifferentiableFunctions{T}.Tan(T)"/>
    public Variable<T> Tan(Variable<T> x)
    {
        var tan = T.Tan(x.Value);
        var sec = T.One / T.Cos(x.Value);
        var df = sec * sec;
        _nodes.Add(new(df, 2.0 * df * tan, x._index, _nodes.Count));
        return new(_nodes.Count - 1, tan);
    }

    //
    // Custom operations
    //

    /// <summary>Add a node to the Hessian tape using a custom unary operation.</summary>
    /// <param name="x">A variable</param>
    /// <param name="f">A function</param>
    /// <param name="df">The derivative of the function</param>
    /// <param name="dfxx">The second derivative of the function</param>
    /// <returns>A variable</returns>
    public Variable<T> CustomOperation(Variable<T> x, Func<T, T> f, Func<T, T> dfx, Func<T, T> dfxx)
    {
        _nodes.Add(new(dfx(x.Value), dfxx(x.Value), x._index, _nodes.Count));
        return new(_nodes.Count - 1, f(x.Value));
    }

    /// <summary>Add a node to the Hessian tape using a custom binary operation.</summary>
    /// <param name="x">The first variable</param>
    /// <param name="y">The second variable</param>
    /// <param name="f">A function</param>
    /// <param name="dfx">The first derivative of the function with respect to the first variable</param>
    /// <param name="dfxx">The second derivative of the function with respect to the first variable</param>
    /// <param name="dfxy">The second derivative of the function with respect to both variables</param>
    /// <param name="dfy">The first derivative of the function with respect to the second variable</param>
    /// <param name="dfyy">The second derivative of the function with respect to the second variable</param>
    /// <returns>A variable</returns>
    public Variable<T> CustomOperation(
        Variable<T> x,
        Variable<T> y,
        Func<T, T, T> f,
        Func<T, T, T> dfx,
        Func<T, T, T> dfxx,
        Func<T, T, T> dfxy,
        Func<T, T, T> dfy,
        Func<T, T, T> dfyy)
    {
        _nodes.Add(new(dfx(x.Value, y.Value), dfxx(x.Value, y.Value), dfxy(x.Value, y.Value), dfy(x.Value, y.Value), dfyy(x.Value, y.Value), x._index, y._index));
        return new(_nodes.Count - 1, f(x.Value, y.Value));
    }
}
