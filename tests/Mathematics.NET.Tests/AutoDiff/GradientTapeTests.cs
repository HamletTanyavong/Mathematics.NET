// <copyright file="GradientTapeTests.cs" company="Mathematics.NET">
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
public sealed class GradientTapeTests
{
    [TestMethod]
    [DataRow(0.123, -1.007651429146436)]
    public void Acos_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Acos, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.396315794095838)]
    public void Acosh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Acosh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, 1)]
    public void Add_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Add, left, right);

        Assert<Real>.ElementsAreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_RealAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(right);
        _ = tape.Add(left, x);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_VariableAndReal_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(left);
        _ = tape.Add(x, right);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(0.123, 1.007651429146436)]
    public void Asin_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Asin, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.6308300845448597)]
    public void Asinh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Asinh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3979465955668749)]
    public void Atan_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Atan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179, -0.1760034342133505)]
    public void Atan2_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Atan2, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -1.94969779684149)]
    public void Atanh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Atanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2903634877210767)]
    public void Cbrt_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Cbrt, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -0.942488801931697)]
    public void Cos_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Cos, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.564468479304407)]
    public void Cosh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Cosh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public void CreateVariable_Multiple_TracksCorrectNumberOfVariables(int amount)
    {
        GradientTape tape = new();
        for (int i = 0; i < amount; i++)
        {
            _ = tape.CreateVariable(0);
        }

        var actual = tape.VariableCount;

        Assert.AreEqual(amount, actual);
    }

    [TestMethod]
    [DataRow(1.23)]
    public void CreateVariable_WithSeedValue_ReturnsVariable(double value)
    {
        GradientTape tape = new();

        var actual = tape.CreateVariable(value).Value;

        Assert.AreEqual(value, actual);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274, -0.2246329169406093)]
    public void Divide_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Divide, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.2246329169406093)]
    public void Divide_RealAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(right);
        _ = tape.Divide(left, x);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274)]
    public void Divide_VariableAndReal_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(left);
        _ = tape.Divide(x, right);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 3.421229536289673)]
    public void Exp_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Exp, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.625894476644487)]
    public void Exp2_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Exp2, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 39.10350518430174)]
    public void Exp10_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Exp10, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.813008130081301)]
    public void Ln_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Ln, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.9563103467806, -0.1224030239537303)]
    public void Log_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Log, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.172922797470702)]
    public void Log2_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Log2, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.35308494463679)]
    public void Log10_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Log10, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, 0)]
    public void Modulo_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Modulo, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_RealAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(right);
        _ = tape.Modulo(left, x);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Modulo_VariableAndReal_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(left);
        _ = tape.Modulo(x, right);
        tape.ReverseAccumulation(out var gradient);

        var actual = gradient[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34, 1.23)]
    public void Multiply_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Multiply, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-16);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.23)]
    public void Multiply_RealAndVariable_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(right);
        _ = tape.Multiply(left, x);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34)]
    public void Multiply_VariableAndReal_ReturnsGradient(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(left);
        _ = tape.Multiply(x, right);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.088081166620949, 0.3360299854573856)]
    public void Pow_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Pow, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3795771135606888, -0.04130373687338086)]
    public void Root_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Root, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-15);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void Sin_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Sin, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.856761056985266)]
    public void Sinh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Sinh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.4508348173337161)]
    public void Sqrt_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Sqrt, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1, -1)]
    public void Subtract_TwoVariables_ReturnsGradients(double left, double right, double expectedLeft, double expectedRight)
    {
        GradientTape tape = new();
        var expected = new Real[2] { expectedLeft, expectedRight };

        var actual = ComputeGradients(tape, tape.Subtract, left, right);

        Assert<Real>.AreApproximatelyEqual(expectedLeft, actual[0], 1e-16);
        Assert<Real>.AreApproximatelyEqual(expectedRight, actual[1], 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1)]
    public void Subtract_RealAndVariable_ReturnsGradients(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(right);
        _ = tape.Subtract(left, x);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Subtract_VariableAndReal_ReturnsGradients(double left, double right, double expected)
    {
        GradientTape tape = new();
        var x = tape.CreateVariable(left);
        _ = tape.Subtract(x, right);
        tape.ReverseAccumulation(out var gradients);

        var actual = gradients[0];

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-16);
    }

    [TestMethod]
    [DataRow(1.23, 8.95136077522624)]
    public void Tan_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Tan, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2900600799721436)]
    public void Tanh_Variable_ReturnsGradient(double input, double expected)
    {
        GradientTape tape = new();

        var actual = ComputeGradient(tape, tape.Tanh, input);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    private static Real ComputeGradient(GradientTape tape, Func<Variable, Variable> function, Real input)
    {
        var x = tape.CreateVariable(input);
        _ = function(x);
        tape.ReverseAccumulation(out var gradients);
        return gradients[0];
    }

    private static Real[] ComputeGradients(GradientTape tape, Func<Variable, Variable, Variable> function, Real left, Real right)
    {
        var x = tape.CreateVariable(left);
        var y = tape.CreateVariable(right);
        _ = function(x, y);
        tape.ReverseAccumulation(out var gradients);
        return gradients.ToArray();
    }
}
