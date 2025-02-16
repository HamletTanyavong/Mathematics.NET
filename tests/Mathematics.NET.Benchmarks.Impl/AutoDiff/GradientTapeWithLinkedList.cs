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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.Exceptions;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.Benchmarks.Impl.AutoDiff;

// For small tapes, this seems to be slower. This may be different for large
// tapes because of the way lists expand internally.

#pragma warning disable IDE0032
#pragma warning disable IDE0058

public record class GradientTapeWithLinkedList<T> : ITape<T>
    where T : IComplex<T>, IDifferentiableFunctions<T>
{
    private bool _isTracking;
    private LinkedList<GradientNode<T>> _nodes;
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

    public Variable<T> CreateVariable(T seed)
    {
        _nodes.AddLast(new GradientNode<T>(_variableCount));
        Variable<T> variable = new(_variableCount++, seed);
        return variable;
    }

    // There is no need to benchmark this.
    public void LogNodes(ILogger<ITape<T>> logger, CancellationToken cancellationToken, int limit = 100)
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
        LinkedListNode<GradientNode<T>> current = _nodes.Last;
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
        LinkedListNode<GradientNode<T>> current = _nodes.Last;
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

    public Variable<T> Add(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value + y.Value);
        }
        return new(_nodes.Count, x.Value + y.Value);
    }

    public Variable<T> Add(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c + x.Value);
        }
        return new(_nodes.Count, c + x.Value);
    }

    public Variable<T> Add(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value + c);
        }
        return new(_nodes.Count, x.Value + c);
    }

    public Variable<T> Divide(Variable<T> x, Variable<T> y)
    {
        var u = T.One / y.Value;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / y.Value, -x.Value * u * u, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<T> Divide(T c, Variable<T> x)
    {
        var u = T.One / x.Value;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(-c * u * u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c * u);
        }
        return new(_nodes.Count, c * u);
    }

    public Variable<T> Divide(Variable<T> x, T c)
    {
        var u = T.One / c;
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * u);
        }
        return new(_nodes.Count, x.Value * u);
    }

    public Variable<Real> Modulo(in Variable<Real> x, in Variable<Real> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, x.Value * Real.Floor(x.Value / y.Value), x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value % y.Value);
        }
        return new(_nodes.Count, x.Value % y.Value);
    }

    public Variable<Real> Modulo(Real c, in Variable<Real> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(c * Real.Floor(c / x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c % x.Value);
        }
        return new(_nodes.Count, c % x.Value);
    }

    public Variable<Real> Modulo(in Variable<Real> x, Real c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value % c);
        }
        return new(_nodes.Count, x.Value % c);
    }

    public Variable<T> Multiply(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(y.Value, x.Value, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value * y.Value);
        }
        return new(_nodes.Count, x.Value * y.Value);
    }

    public Variable<T> Multiply(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(c, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c * x.Value);
        }
        return new(_nodes.Count, c * x.Value);
    }

    public Variable<T> Multiply(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(c, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value * c);
        }
        return new(_nodes.Count, x.Value * c);
    }

    public Variable<T> Subtract(Variable<T> x, Variable<T> y)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, -T.One, x.Index, y.Index));
            return new(_nodes.Count - 1, x.Value - y.Value);
        }
        return new(_nodes.Count, x.Value - y.Value);
    }

    public Variable<T> Subtract(T c, Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(-T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, c - x.Value);
        }
        return new(_nodes.Count, c - x.Value);
    }

    public Variable<T> Subtract(Variable<T> x, T c)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, x.Value - c);
        }
        return new(_nodes.Count, x.Value - c);
    }

    //
    // Other Operations
    //

    public Variable<T> Negate(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(-T.One, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, -x.Value);
        }
        return new(_nodes.Count, -x.Value);
    }

    // Exponential functions.

    public Variable<T> Exp(Variable<T> x)
    {
        var exp = T.Exp(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(exp, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp);
        }
        return new(_nodes.Count, exp);
    }

    public Variable<T> Exp2(Variable<T> x)
    {
        var exp2 = T.Exp2(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(Real.Ln2 * exp2, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp2);
        }
        return new(_nodes.Count, exp2);
    }

    public Variable<T> Exp10(Variable<T> x)
    {
        var exp10 = T.Exp10(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(Real.Ln10 * exp10, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, exp10);
        }
        return new(_nodes.Count, exp10);
    }

    // Hyperbolic functions.

    public Variable<T> Acosh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (T.Sqrt(x.Value - T.One) * T.Sqrt(x.Value + T.One)), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acosh(x.Value));
        }
        return new(_nodes.Count, T.Acosh(x.Value));
    }

    public Variable<T> Asinh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / T.Sqrt(x.Value * x.Value + T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asinh(x.Value));
        }
        return new(_nodes.Count, T.Asinh(x.Value));
    }

    public Variable<T> Atanh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atanh(x.Value));
        }
        return new(_nodes.Count, T.Atanh(x.Value));
    }

    public Variable<T> Cosh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.Sinh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cosh(x.Value));
        }
        return new(_nodes.Count, T.Cosh(x.Value));
    }

    public Variable<T> Sinh(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.Cosh(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sinh(x.Value));
        }
        return new(_nodes.Count, T.Sinh(x.Value));
    }

    public Variable<T> Tanh(Variable<T> x)
    {
        if (_isTracking)
        {
            var u = T.One / T.Cosh(x.Value);
            _nodes.AddLast(new GradientNode<T>(u * u, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tanh(x.Value));
        }
        return new(_nodes.Count, T.Tanh(x.Value));
    }

    // Logarithmic functions.

    public Variable<T> Ln(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / x.Value, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Ln(x.Value));
        }
        return new(_nodes.Count, T.Ln(x.Value));
    }

    public Variable<T> Log(Variable<T> x, Variable<T> b)
    {
        if (_isTracking)
        {
            var lnB = T.Ln(b.Value);
            _nodes.AddLast(new GradientNode<T>(T.One / (x.Value * lnB), -T.Ln(x.Value) / (b.Value * lnB * lnB), x.Index, b.Index));
            return new(_nodes.Count - 1, T.Log(x.Value, b.Value));
        }
        return new(_nodes.Count, T.Log(x.Value, b.Value));
    }

    public Variable<T> Log2(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (Real.Ln2 * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log2(x.Value));
        }
        return new(_nodes.Count, T.Log2(x.Value));
    }

    public Variable<T> Log10(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (Real.Ln10 * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Log10(x.Value));
        }
        return new(_nodes.Count, T.Log10(x.Value));
    }

    // Power functions.

    public Variable<T> Pow(Variable<T> x, Variable<T> y)
    {
        var pow = T.Pow(x.Value, y.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(y.Value * T.Pow(x.Value, y.Value - T.One), T.Ln(x.Value) * pow, x.Index, y.Index));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T> Pow(Variable<T> x, T y)
    {
        var pow = T.Pow(x.Value, y);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(y * T.Pow(x.Value, y - T.One), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    public Variable<T> Pow(T x, Variable<T> y)
    {
        var pow = T.Pow(x, y.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.Ln(x) * pow, y.Index, _nodes.Count));
            return new(_nodes.Count - 1, pow);
        }
        return new(_nodes.Count, pow);
    }

    // Root functions.

    public Variable<T> Cbrt(Variable<T> x)
    {
        var cbrt = T.Cbrt(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (3.0 * cbrt * cbrt), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, cbrt);
        }
        return new(_nodes.Count, cbrt);
    }

    public Variable<T> Root(Variable<T> x, Variable<T> n)
    {
        var root = T.Root(x.Value, n.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(root / (n.Value * x.Value), -T.Ln(x.Value) * root / (n.Value * n.Value), x.Index, n.Index));
            return new(_nodes.Count - 1, root);
        }
        return new(_nodes.Count, root);
    }

    public Variable<T> Sqrt(Variable<T> x)
    {
        var sqrt = T.Sqrt(x.Value);
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(0.5 / sqrt, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, sqrt);
        }
        return new(_nodes.Count, sqrt);
    }

    // Trigonometric functions.

    public Variable<T> Acos(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(-T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Acos(x.Value));
        }
        return new(_nodes.Count, T.Acos(x.Value));
    }

    public Variable<T> Asin(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / T.Sqrt(T.One - x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Asin(x.Value));
        }
        return new(_nodes.Count, T.Asin(x.Value));
    }

    public Variable<T> Atan(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.One / (T.One + x.Value * x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Atan(x.Value));
        }
        return new(_nodes.Count, T.Atan(x.Value));
    }

    public Variable<Real> Atan2(in Variable<Real> y, in Variable<Real> x)
    {
        if (_isTracking)
        {
            var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
            _nodes.AddLast(new GradientNode<T>(x.Value * u, -y.Value * u, y.Index, x.Index));
            return new(_nodes.Count - 1, Real.Atan2(y.Value, x.Value));
        }
        return new(_nodes.Count, Real.Atan2(y.Value, x.Value));
    }

    public Variable<T> Cos(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(-T.Sin(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Cos(x.Value));
        }
        return new(_nodes.Count, T.Cos(x.Value));
    }

    public Variable<T> Sin(Variable<T> x)
    {
        if (_isTracking)
        {
            _nodes.AddLast(new GradientNode<T>(T.Cos(x.Value), x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Sin(x.Value));
        }
        return new(_nodes.Count, T.Sin(x.Value));
    }

    public Variable<T> Tan(Variable<T> x)
    {
        if (_isTracking)
        {
            var sec = T.One / T.Cos(x.Value);
            _nodes.AddLast(new GradientNode<T>(sec * sec, x.Index, _nodes.Count));
            return new(_nodes.Count - 1, T.Tan(x.Value));
        }
        return new(_nodes.Count, T.Tan(x.Value));
    }

    //
    // Custom Operations
    //

    public Variable<T> CustomOperation(Variable<T> x, Func<T, T> f, Func<T, T> df)
    {
        _nodes.AddLast(new GradientNode<T>(df(x.Value), x.Index, _nodes.Count));
        return new(_nodes.Count - 1, f(x.Value));
    }

    public Variable<T> CustomOperation(Variable<T> x, Variable<T> y, Func<T, T, T> f, Func<T, T, T> dfx, Func<T, T, T> dfy)
    {
        _nodes.AddLast(new GradientNode<T>(dfx(x.Value, y.Value), dfy(x.Value, y.Value), x.Index, y.Index));
        return new(_nodes.Count - 1, f(x.Value, y.Value));
    }

    //
    // DifGeo
    //

    public AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in Vector2<T> x) where U : IIndex => throw new NotImplementedException();
    public AutoDiffTensor2<T, U> CreateAutoDiffTensor<U>(in T x0, in T x1) where U : IIndex => throw new NotImplementedException();
    public AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in Vector3<T> x) where U : IIndex => throw new NotImplementedException();
    public AutoDiffTensor3<T, U> CreateAutoDiffTensor<U>(in T x0, in T x1, in T x2) where U : IIndex => throw new NotImplementedException();
    public AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in Vector4<T> x) where U : IIndex => throw new NotImplementedException();
    public AutoDiffTensor4<T, U> CreateAutoDiffTensor<U>(in T x0, in T x1, in T x2, in T x3) where U : IIndex => throw new NotImplementedException();
}
