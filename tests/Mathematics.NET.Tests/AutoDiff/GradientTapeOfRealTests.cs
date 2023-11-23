﻿// <copyright file="GradientTapeOfRealTests.cs" company="Mathematics.NET">
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

using System.Data;
using Mathematics.NET.AutoDiff;

namespace Mathematics.NET.Tests.AutoDiff;

[TestClass]
[TestCategory("AutoDiff"), TestCategory("Gradient Tape")]
public sealed class GradientTapeOfRealTests
{
    private GradientTape<Real> _tape;

    public GradientTapeOfRealTests()
    {
        _tape = new();
    }

    //
    // Tests
    //

    [TestMethod]
    [DataRow(0.123, -1.007651429146436)]
    public void Acos_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Acos, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.396315794095838)]
    public void Acosh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Acosh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, 1)]
    public void Add_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Add, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Add(left, x);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Add(x, right);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(0.123, 1.007651429146436)]
    public void Asin_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Asin, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.6308300845448597)]
    public void Asinh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Asinh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3979465955668749)]
    public void Atan_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Atan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179, -0.1760034342133505)]
    public void Atan2_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Atan2, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -1.94969779684149)]
    public void Atanh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Atanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2903634877210767)]
    public void Cbrt_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Cbrt, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -0.942488801931697)]
    public void Cos_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Cos, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.564468479304407)]
    public void Cosh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Cosh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public void CreateVariable_Multiple_TracksCorrectNumberOfVariables(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _ = _tape.CreateVariable(0);
        }

        var actual = _tape.VariableCount;

        Assert.AreEqual(amount, actual);
    }

    [TestMethod]
    [DataRow(1.23)]
    public void CreateVariable_WithSeedValue_ReturnsVariable(double value)
    {
        var actual = _tape.CreateVariable(value).Value;

        Assert.AreEqual(value, actual);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179, -0.1760034342133505)]
    public void CustomOperation_Binary_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        var y = _tape.CreateVariable(left);
        var x = _tape.CreateVariable(right);
        var u = Real.One / (x.Value * x.Value + y.Value * y.Value);
        _ = _tape.CustomOperation(y, x, Real.Atan2, (y, x) => x.Value * u, (y, x) => -y.Value * u);
        _tape.ReverseAccumulation(out var actual);

        Real[] expected = [expectedLeft, expectedRight];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void CustomOperation_Unary_ReturnsGradient(double input, double expected)
    {
        var x = _tape.CreateVariable(input);
        _ = _tape.CustomOperation(x, Real.Sin, Real.Cos);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274, -0.2246329169406093)]
    public void Divide_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Divide, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.2246329169406093)]
    public void Divide_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Divide(left, x);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274)]
    public void Divide_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Divide(x, right);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 3.421229536289673)]
    public void Exp_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Exp, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.625894476644487)]
    public void Exp2_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Exp2, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 39.10350518430174)]
    public void Exp10_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Exp10, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.813008130081301)]
    public void Ln_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Ln, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.9563103467806, -0.1224030239537303)]
    public void Log_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Log, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.172922797470702)]
    public void Log2_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Log2, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.35308494463679)]
    public void Log10_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Log10, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, 0)]
    public void Modulo_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Modulo, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Modulo(left, x);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Modulo_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Modulo(x, right);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34, 1.23)]
    public void Multiply_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Multiply, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.23)]
    public void Multiply_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Multiply(left, x);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34)]
    public void Multiply_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Multiply(x, right);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, -1)]
    public void Negate_Variable_ReturnsNegation(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Negate, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.088081166620949, 0.3360299854573856)]
    public void Pow_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Pow, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3795771135606888, -0.04130373687338086)]
    public void Root_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Root, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void Sin_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Sin, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.856761056985266)]
    public void Sinh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Sinh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.4508348173337161)]
    public void Sqrt_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Sqrt, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, -1)]
    public void Subtract_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        Real[] expected = [expectedLeft, expectedRight];

        var actual = ComputeGradient(_tape.Subtract, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1)]
    public void Subtract_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Subtract(left, x);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Subtract_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Subtract(x, right);
        _tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 8.95136077522624)]
    public void Tan_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Tan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2900600799721436)]
    public void Tanh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Tanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    private Real ComputeGradient(Func<Variable<Real>, Variable<Real>> function, Real input)
    {
        var x = _tape.CreateVariable(input);
        _ = function(x);
        _tape.ReverseAccumulation(out var gradient);
        return gradient[0];
    }

    private Real[] ComputeGradient(Func<Variable<Real>, Variable<Real>, Variable<Real>> function, Real left, Real right)
    {
        var x = _tape.CreateVariable(left);
        var y = _tape.CreateVariable(right);
        _ = function(x, y);
        _tape.ReverseAccumulation(out var gradient);
        return gradient.ToArray();
    }
}
