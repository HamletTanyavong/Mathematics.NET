// <copyright file="GradientTapeOfRealTests.cs" company="Mathematics.NET">
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

using System.Data;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.LinearAlgebra;

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
    [DataRow(0.123, 1.447484051603025)]
    public void Acos_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Acos, input);

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
    [DataRow(1.23, 0.6658635291565548)]
    public void Acosh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Acosh, input);

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
    [DataRow(1.23, 2.34, 3.57)]
    public void Add_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Add, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Add(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.57)]
    public void Add_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Add(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Add(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.57)]
    public void Add_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Add(x, right).Value;

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
    [DataRow(0.123, 0.123312275191872)]
    public void Asin_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Asin, input);

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
    [DataRow(1.23, 1.035037896192308)]
    public void Asinh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Asinh, input);

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
    [DataRow(1.23, 0.88817377437768)]
    public void Atan_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Atan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179, -0.1760034342133505)]
    public void Atan2_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        var y = _tape.CreateVariable(left);
        var x = _tape.CreateVariable(right);
        _ = _tape.Atan2(y, x);

        Real[] expected = [expectedLeft, expectedRight];

        _tape.ReverseAccumulate(out var actual);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4839493878600246)]
    public void Atan2_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var y = _tape.CreateVariable(left);
        var x = _tape.CreateVariable(right);

        var actual = _tape.Atan2(y, x).Value;

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
    [DataRow(0.123, 0.1236259811831301)]
    public void Atanh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Atanh, input);

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
    [DataRow(1.23, 1.071441269690773)]
    public void Cbrt_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Cbrt, input);

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
    [DataRow(1.23, 0.3342377271245026)]
    public void Cos_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Cos, input);

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
    [DataRow(1.23, 1.856761056985266)]
    public void Cosh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Cosh, input);

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
        _ = _tape.CustomOperation(y, x, Real.Atan2, (y, x) => x * u, (y, x) => -y * u);

        Real[] expected = [expectedLeft, expectedRight];

        _tape.ReverseAccumulate(out var actual);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4839493878600246)]
    public void CustomOperation_Binary_ReturnsValue(double left, double right, double expected)
    {
        var y = _tape.CreateVariable(left);
        var x = _tape.CreateVariable(right);
        var u = Real.One / (x.Value * x.Value + y.Value * y.Value);

        var actual = _tape.CustomOperation(y, x, Real.Atan2, (y, x) => x * u, (y, x) => -y * u).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void CustomOperation_Unary_ReturnsGradient(double input, double expected)
    {
        var x = _tape.CreateVariable(input);
        _ = _tape.CustomOperation(x, Real.Sin, Real.Cos);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.942488801931697)]
    public void CustomOperation_Unary_ReturnsValue(double input, double expected)
    {
        var x = _tape.CreateVariable(input);

        var actual = _tape.CustomOperation(x, Real.Sin, Real.Cos).Value;

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
    [DataRow(1.23, 2.34, 0.5256410256410256)]
    public void Divide_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Divide, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.2246329169406093)]
    public void Divide_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Divide(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.5256410256410256)]
    public void Divide_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Divide(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274)]
    public void Divide_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Divide(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.5256410256410256)]
    public void Divide_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Divide(x, right).Value;

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
    [DataRow(1.23, 3.421229536289673)]
    public void Exp_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Exp, input);

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
    [DataRow(1.23, 2.345669898463758)]
    public void Exp2_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Exp2, input);

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
    [DataRow(1.23, 16.98243652461744)]
    public void Exp10_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Exp10, input);

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
    [DataRow(1.23, 0.2070141693843261)]
    public void Ln_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Ln, input);

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
    [DataRow(1.23, 2.34, 0.2435028442982799)]
    public void Log_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Log, left, right);

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
    [DataRow(1.23, 0.2986583155645151)]
    public void Log2_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Log2, input);

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
    [DataRow(1.23, 0.0899051114393979)]
    public void Log10_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Log10, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, 0)]
    public void Modulo_TwoVariables_ReturnsGradient(double left, double right, double expectedLeft, double expectedRight)
    {
        var x = _tape.CreateVariable(left);
        var y = _tape.CreateVariable(right);
        _ = _tape.Modulo(x, y);

        Real[] expected = [expectedLeft, expectedRight];

        _tape.ReverseAccumulate(out var actual);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(3.14, 2.34, 0.8)]
    public void Modulo_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        var y = _tape.CreateVariable(right);

        var actual = _tape.Modulo(x, y).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Modulo(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(3.14, 2.34, 0.8)]
    public void Modulo_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Modulo(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Modulo_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Modulo(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(3.14, 2.34, 0.8)]
    public void Modulo_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Modulo(x, right).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
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
    [DataRow(1.23, 2.34, 2.8782)]
    public void Multiply_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Multiply, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.23)]
    public void Multiply_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Multiply(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.8782)]
    public void Multiply_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Multiply(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34)]
    public void Multiply_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Multiply(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.8782)]
    public void Multiply_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Multiply(x, right).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -1)]
    public void Negate_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Negate, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, -1.23)]
    public void Negate_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Negate, input);

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
    [DataRow(1.23, 2.34, 1.623222151685371)]
    public void Pow_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Pow, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3360299854573856)]
    public void Pow_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Pow(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.623222151685371)]
    public void Pow_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Pow(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.088081166620949)]
    public void Pow_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Pow(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.623222151685371)]
    public void Pow_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Pow(x, right).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    public void ReverseAccumulate_WithCheckpointing_ReturnsGradients()
    {
        var vector = _tape.CreateAutoDiffVector(1.23, 2.34, 3.45, 4.56);

        var u = _tape.CreateCheckpoint(_tape.Cos(_tape.Multiply(_tape.Sin(_tape.Multiply(vector.X1, vector.X2)), vector.X4)));
        var v = _tape.CreateCheckpoint(_tape.Divide(_tape.Exp(_tape.Multiply(vector.X3, vector.X2)), u));
        var x = _tape.Exp(_tape.Divide(u, _tape.Sqrt(_tape.Multiply(_tape.Multiply(vector.X3, vector.X4), vector.X2))));
        var y = _tape.Add(u, _tape.Multiply(_tape.Ln(_tape.Divide(vector.X3, vector.X4)), _tape.Multiply(v, vector.X1)));

        Real[] expectedX = [1.67479908078448, 0.866325347771631, -0.00950769726936101, -0.04951809751601818];
        Real[] expectedY = [72675.93346228106, 29314.79027740105, -3824.679197396951, -4208.3704226546];

        _tape.ReverseAccumulate(out var actualX, x.Index);
        _tape.ReverseAccumulate(out var actualY, y.Index);

        Assert<Real>.AreApproximatelyEqual(expectedX, actualX, 1e-14);
        Assert<Real>.AreApproximatelyEqual(expectedY, actualY, 1e-14);
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
    [DataRow(1.23, 2.34, 1.092498848250374)]
    public void Root_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Root, left, right);

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
    [DataRow(1.23, 0.942488801931697)]
    public void Sin_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Sin, input);

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
    [DataRow(1.23, 1.564468479304407)]
    public void Sinh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Sinh, input);

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
    [DataRow(1.23, 1.109053650640942)]
    public void Sqrt_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Sqrt, input);

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
    [DataRow(1.23, 2.34, -1.11)]
    public void Subtract_TwoVariables_ReturnsValue(double left, double right, double expected)
    {
        var actual = ComputeValue(_tape.Subtract, left, right);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1)]
    public void Subtract_ConstantAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);
        _ = _tape.Subtract(left, x);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1.11)]
    public void Subtract_ConstantAndVariable_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(right);

        var actual = _tape.Subtract(left, x).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Subtract_VariableAndConstant_ReturnsGradient(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);
        _ = _tape.Subtract(x, right);
        _tape.ReverseAccumulate(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, Real.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1.11)]
    public void Subtract_VariableAndConstant_ReturnsValue(double left, double right, double expected)
    {
        var x = _tape.CreateVariable(left);

        var actual = _tape.Subtract(x, right).Value;

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 8.95136077522624)]
    public void Tan_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Tan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.819815734268152)]
    public void Tan_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Tan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2900600799721436)]
    public void Tanh_Variable_ReturnsGradient(double input, double expected)
    {
        var actual = ComputeGradient(_tape.Tanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.84257932565893)]
    public void Tanh_Variable_ReturnsValue(double input, double expected)
    {
        var actual = ComputeValue(_tape.Tanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    private Real ComputeGradient(Func<Variable<Real>, Variable<Real>> function, Real input)
    {
        var x = _tape.CreateVariable(input);
        _ = function(x);
        _tape.ReverseAccumulate(out var gradient);
        return gradient[0];
    }

    private Real[] ComputeGradient(Func<Variable<Real>, Variable<Real>, Variable<Real>> function, Real left, Real right)
    {
        var x = _tape.CreateVariable(left);
        var y = _tape.CreateVariable(right);
        _ = function(x, y);
        _tape.ReverseAccumulate(out var gradient);
        return gradient.ToArray();
    }

    private Real ComputeValue(Func<Variable<Real>, Variable<Real>> function, Real input)
    {
        var x = _tape.CreateVariable(input);
        return function(x).Value;
    }

    private Real ComputeValue(Func<Variable<Real>, Variable<Real>, Variable<Real>> function, Real left, Real right)
    {
        var x = _tape.CreateVariable(left);
        var y = _tape.CreateVariable(right);
        return function(x, y).Value;
    }
}
