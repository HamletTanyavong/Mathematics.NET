// <copyright file="GradientTapeWithLinkedList.cs" company="Mathematics.NET">
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
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.Exceptions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.Benchmarks.Implementations.AutoDiff;

// For small tapes, this seems to be slower. This may be different for large
// tapes because of the way lists expand internally.

#pragma warning disable IDE0032
#pragma warning disable IDE0058

public record class GradientTapeWithLinkedList<T, U> : ITape<T, U>
    where T : IComplex<T, U, U>, IDifferentiableFunctions<T>
    where U : IBinaryFloatingPointIeee754<U>, IMinMaxValue<U>
{
    private bool _isTracking;
    private LinkedList<GradientNode<T, U>> _nodes;
    private int _variableCount;

    public GradientTapeWithLinkedList(bool isTracking = true)
    {
        _nodes = [];
        _isTracking = isTracking;
    }

    public bool IsTracking { get => _isTracking; set => _isTracking = value; }

    public int NodeCount => _nodes.Count;

    public int VariableCount => _variableCount;

    //
    // Methods
    //

    public Variable<T, U> CreateVariable(T seed)
    {
        _nodes.AddLast(new GradientNode<T, U>(_variableCount));
        Variable<T, U> variable = new(_variableCount++, seed);
        return variable;
    }

    // There is no need to benchmark this.
    public void LogNodes(ILogger<ITape<T, U>> logger, CancellationToken cancellationToken, int limit = 100)
        => throw new NotImplementedException();

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient)
        => ReverseAccumulate(out gradient, T.One);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, int index)
        => ReverseAccumulate(out gradient, T.One, index);

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("Gradient tape contains no root nodes");

        var length = _nodes.Count;
        Span<T> gradientSpan = new T[length];
        gradientSpan[length - 1] = seed;

#nullable disable
        LinkedListNode<GradientNode<T, U>> current = _nodes.Last;
        for (int i = length - 1; i >= _variableCount; i--)
        {
            var gradientElement = gradientSpan[i];

            gradientSpan[current.Value.PX] += gradientElement * current.Value.DX;
            gradientSpan[current.Value.PY] += gradientElement * current.Value.DY;
            current = current.Previous;
        }
#nullable enable

        gradient = gradientSpan[.._variableCount];
    }

    public void ReverseAccumulate(out ReadOnlySpan<T> gradient, T seed, int index)
    {
        if (_variableCount == 0)
            throw new AutoDiffException("The gradient tape contains no root nodes.");

        if (index < _variableCount || index >= _nodes.Count)
            throw new IndexOutOfRangeException();

        Span<T> gradientSpan = new T[index + 1];
        gradientSpan[index] = seed;

#nullable disable
        LinkedListNode<GradientNode<T, U>> current = _nodes.Last;
        for (int i = index; i >= _variableCount; i--)
        {
            var gradientElement = gradientSpan[i];

            gradientSpan[current.Value.PX] += gradientElement * current.Value.DX;
            gradientSpan[current.Value.PY] += gradientElement * current.Value.DY;
        }
#nullable enable

        gradient = gradientSpan[.._variableCount];
    }

    //
    // Basic Operations
    //

    public Variable<T, U> Add(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value + y.Value);
        }
        return new(_nodes.Count, x.Value + y.Value);
    }

    public Variable<T, U> Add(T c, Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c + x.Value);
        }
        return new(_nodes.Count, c + x.Value);
    }

    public Variable<T, U> Add(Variable<T, U> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value + c);
        }
        return new(_nodes.Count, x.Value + c);
    }

    public Variable<T, U> Divide(Variable<T, U> x, Variable<T, U> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / y.Value, -x.Value * u * u, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<T, U> Divide(T c, Variable<T, U> x)
    {
        var u = T.One / x.Value;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(-c * u * u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c * u);
        }
        return new(_nodes.Count, c * u);
    }

    public Variable<T, U> Divide(Variable<T, U> x, T c)
    {
        var u = T.One / c;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, in Variable<Real<U>, U> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, (U)(x.Value * Real<U>.Floor(x.Value / y.Value)), x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value % y.Value);
        }
        return new(_nodes.Count, x.Value % y.Value);
    }

    public Variable<Real<U>, U> Modulo(Real<U> c, in Variable<Real<U>, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>((U)(c * Real<U>.Floor(c / x.Value)), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c % x.Value);
        }
        return new(_nodes.Count, c % x.Value);
    }

    public Variable<Real<U>, U> Modulo(in Variable<Real<U>, U> x, Real<U> c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value % c);
        }
        return new(_nodes.Count, x.Value % c);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(y.Value, x.Value, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * y.Value);
        }
        return new(_nodes.Count, x.Value * y.Value);
    }

    public Variable<T, U> Multiply(T c, Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(c, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c * x.Value);
        }
        return new(_nodes.Count, c * x.Value);
    }

    public Variable<T, U> Multiply(Variable<T, U> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(c, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * c);
        }
        return new(_nodes.Count, x.Value * c);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, Variable<T, U> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, -T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value - y.Value);
        }
        return new(_nodes.Count, x.Value - y.Value);
    }

    public Variable<T, U> Subtract(T c, Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(-T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c - x.Value);
        }
        return new(_nodes.Count, c - x.Value);
    }

    public Variable<T, U> Subtract(Variable<T, U> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value - c);
        }
        return new(_nodes.Count, x.Value - c);
    }

    //
    // Other Operations
    //

    public Variable<T, U> Negate(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(-T.One, x.Index, _nodes.Count));
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
            _nodes.AddLast(new GradientNode<T, U>(exp, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp);
        }
        return new(_nodes.Count, exp);
    }

    public Variable<T, U> Exp2(Variable<T, U> x)
    {
        var exp2 = T.Exp2(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>((U)Real<U>.Ln2 * exp2, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp2);
        }
        return new(_nodes.Count, exp2);
    }

    public Variable<T, U> Exp10(Variable<T, U> x)
    {
        var exp10 = T.Exp10(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>((U)Real<U>.Ln10 * exp10, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp10);
        }
        return new(_nodes.Count, exp10);
    }

    // Hyperbolic functions.

    public Variable<T, U> Acosh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / (T.Sqrt(x.Value - T.One) * T.Sqrt(x.Value + T.One)), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acosh(x.Value));
        }
        return new(_nodes.Count, T.Acosh(x.Value));
    }

    public Variable<T, U> Asinh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / T.Sqrt(x.Value * x.Value + T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asinh(x.Value));
        }
        return new(_nodes.Count, T.Asinh(x.Value));
    }

    public Variable<T, U> Atanh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / (T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atanh(x.Value));
        }
        return new(_nodes.Count, T.Atanh(x.Value));
    }

    public Variable<T, U> Cosh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.Sinh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cosh(x.Value));
        }
        return new(_nodes.Count, T.Cosh(x.Value));
    }

    public Variable<T, U> Sinh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.Cosh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sinh(x.Value));
        }
        return new(_nodes.Count, T.Sinh(x.Value));
    }

    public Variable<T, U> Tanh(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var u = T.One / T.Cosh(x.Value);
            _nodes.AddLast(new GradientNode<T, U>(u * u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tanh(x.Value));
        }
        return new(_nodes.Count, T.Tanh(x.Value));
    }

    // Logarithmic functions.

    public Variable<T, U> Ln(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / x.Value, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Ln(x.Value));
        }
        return new(_nodes.Count, T.Ln(x.Value));
    }

    public Variable<T, U> Log(Variable<T, U> x, Variable<T, U> b)
    {
        if (_isTracking)
        {
            var lnB = T.Ln(b.Value);
            _nodes.AddLast(new GradientNode<T, U>(T.One / (x.Value * lnB), -T.Ln(x.Value) / (b.Value * lnB * lnB), x.Index, b.Index));
            return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
        }
        return new(_nodes.Count, T.Log(x.Value, b.Value));
    }

    public Variable<T, U> Log2(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / ((U)Real<U>.Ln2 * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log2(x.Value));
        }
        return new(_nodes.Count, T.Log2(x.Value));
    }

    public Variable<T, U> Log10(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / ((U)Real<U>.Ln10 * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log10(x.Value));
        }
        return new(_nodes.Count, T.Log10(x.Value));
    }

    // Power functions.

    public Variable<T, U> Pow(Variable<T, U> x, Variable<T, U> y)
    {
        var pow = T.Pow(x.Value, y.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(y.Value * T.Pow(x.Value, y.Value - T.One), T.Ln(x.Value) * pow, x.Index, y.Index));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(Variable<T, U> x, T y)
    {
        var pow = T.Pow(x.Value, y);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(y * T.Pow(x.Value, y - T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T, U> Pow(T x, Variable<T, U> y)
    {
        var pow = T.Pow(x, y.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.Ln(x) * pow, y.Index, _nodes.Count));
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
            _nodes.AddLast(new GradientNode<T, U>(T.One / (U.CreateSaturating(3) * cbrt * cbrt), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cbrt);
        }
        return new(_nodes.Count, cbrt);
    }

    public Variable<T, U> Root(Variable<T, U> x, Variable<T, U> n)
    {
        var root = T.Root(x.Value, n.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(root / (n.Value * x.Value), -T.Ln(x.Value) * root / (n.Value * n.Value), x.Index, n.Index));
            return new(_nodes.Count - 1, root);
        }
        return new(_nodes.Count, root);
    }

    public Variable<T, U> Sqrt(Variable<T, U> x)
    {
        var sqrt = T.Sqrt(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(U.CreateSaturating(0.5) / sqrt, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sqrt);
        }
        return new(_nodes.Count, sqrt);
    }

    // Trigonometric functions.

    public Variable<T, U> Acos(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(-T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acos(x.Value));
        }
        return new(_nodes.Count, T.Acos(x.Value));
    }

    public Variable<T, U> Asin(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asin(x.Value));
        }
        return new(_nodes.Count, T.Asin(x.Value));
    }

    public Variable<T, U> Atan(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.One / (T.One + x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atan(x.Value));
        }
        return new(_nodes.Count, T.Atan(x.Value));
    }

    public Variable<Real<U>, U> Atan2(in Variable<Real<U>, U> y, in Variable<Real<U>, U> x)
    {
        if (_isTracking)
        {
            var u = Real<U>.One / (x.Value * x.Value + y.Value * y.Value);
            _nodes.AddLast(new GradientNode<T, U>((U)(x.Value * u), (U)(-y.Value * u), y.Index, x.Index));
            return new(_nodes.Count - 1, Real<U>.Atan2(y.Value, x.Value));
        }
        return new(_nodes.Count, Real<U>.Atan2(y.Value, x.Value));
    }

    public Variable<T, U> Cos(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(-T.Sin(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cos(x.Value));
        }
        return new(_nodes.Count, T.Cos(x.Value));
    }

    public Variable<T, U> Sin(Variable<T, U> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T, U>(T.Cos(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sin(x.Value));
        }
        return new(_nodes.Count, T.Sin(x.Value));
    }

    public Variable<T, U> Tan(Variable<T, U> x)
    {
        if (_isTracking)
        {
            var sec = T.One / T.Cos(x.Value);
            _nodes.AddLast(new GradientNode<T, U>(sec * sec, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tan(x.Value));
        }
        return new(_nodes.Count, T.Tan(x.Value));
    }

    //
    // Custom Operations
    //

    public Variable<T, U> Operation(Variable<T, U> x, Func<T, T> f, Func<T, T> df)
    {
        _nodes.AddLast(new GradientNode<T, U>(df(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, f(x.Value));
    }

    public Variable<T, U> Operation(Variable<T, U> x, Variable<T, U> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
    {
        _nodes.AddLast(new GradientNode<T, U>(dfx(x.Value, y.Value), dfy(x.Value, y.Value), x.Index, y.Index));
        return new(_nodes.Count - 1, f(x.Value, y.Value));
    }

    //
    // DifGeo
    //

    public AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in Vector2<T, U> x) where V : IIndex => throw new NotImplementedException();
    public AutoDiffTensor2<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1) where V : IIndex => throw new NotImplementedException();
    public AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in Vector3<T, U> x) where V : IIndex => throw new NotImplementedException();
    public AutoDiffTensor3<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2) where V : IIndex => throw new NotImplementedException();
    public AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in Vector4<T, U> x) where V : IIndex => throw new NotImplementedException();
    public AutoDiffTensor4<T, U, V> CreateAutoDiffTensor<V>(in T x0, in T x1, in T x2, in T x3) where V : IIndex => throw new NotImplementedException();
}
