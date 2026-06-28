// <copyright file="DualOfRealTests.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.UnitTests.AutoDiff;

[TestClass]
[TestCategory("AutoDiff"), TestCategory("Dual Number")]
public sealed class DualOfRealTests
{
    [TestMethod]
    [DataRow(0.123, -1.007651429146436)]
    public void Acos_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Acos(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.396315794095838)]
    public void Acosh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Acosh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = (x + y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = (x + y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Add_ConstantAndDualNumber_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (left + new Dual<Real<double>, double>(right, Real<double>.Zero)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (left + new Dual<Real<double>, double>(right, Real<double>.One)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Add_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.One) + right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Add_DualNumberAndConstant_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.Zero) + right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(0.123, 1.007651429146436)]
    public void Asin_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Asin(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.6308300845448597)]
    public void Asinh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Asinh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3979465955668749)]
    public void Atan_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Atan(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179)]
    public void Atan2_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> y = new(left, Real<double>.One);
        Dual<Real<double>, double> x = new(right, Real<double>.Zero);

        var actual = Dual<Real<double>, double>.Atan2(y, x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.1760034342133505)]
    public void Atan2_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> y = new(left, Real<double>.Zero);
        Dual<Real<double>, double> x = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Atan2(y, x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, -1.94969779684149)]
    public void Atanh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Atanh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2903634877210767)]
    public void Cbrt_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Cbrt(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0)]
    public void Ceiling_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Ceiling(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 0);
    }

    [TestMethod]
    [DataRow(1.23, -0.942488801931697)]
    public void Cos_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Cos(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.564468479304407)]
    public void Cosh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Cosh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274)]
    public void Divide_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = (x / y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.2246329169406093)]
    public void Divide_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = (x / y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Divide_ConstantAndDualNumber_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (left / new Dual<Real<double>, double>(right, Real<double>.Zero)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.2246329169406093)]
    public void Divide_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (left / new Dual<Real<double>, double>(right, Real<double>.One)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.4273504273504274)]
    public void Divide_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.One) / right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Divide_DualNumberAndConstant_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.Zero) / right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 3.421229536289673)]
    public void Exp_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Exp(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.625894476644487)]
    public void Exp2_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Exp2(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 39.10350518430174)]
    public void Exp10_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Exp10(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0)]
    public void Floor_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Floor(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 0);
    }

    [TestMethod]
    [DataRow(1.23)]
    public void ImplicitOperator_Number_ReturnsDualNumber(double value)
    {
        Real<double> input = value;

        Dual<Real<double>, double> expected = new(input, 0);

        Dual<Real<double>, double> actual = input;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(1.23, 0.813008130081301)]
    public void Ln_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Ln(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.9563103467806)]
    public void Log_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> b = new(right, Real<double>.Zero);

        var actual = Dual<Real<double>, double>.Log(x, b).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.1224030239537303)]
    public void Log_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> b = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Log(x, b).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.172922797470702)]
    public void Log2_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Log2(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.35308494463679)]
    public void Log10_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Log10(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Modulo_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = Dual<Real<double>, double>.Modulo(x, y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Modulo(x, y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_ConstantAndDualNumber_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = Dual<Real<double>, double>.Modulo(left, new Dual<Real<double>, double>(right, Real<double>.Zero)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = Dual<Real<double>, double>.Modulo(left, new Dual<Real<double>, double>(right, Real<double>.One)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Modulo_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = Dual<Real<double>, double>.Modulo(new Dual<Real<double>, double>(left, Real<double>.One), right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Modulo_DualNumberAndConstant_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = Dual<Real<double>, double>.Modulo(new Dual<Real<double>, double>(left, Real<double>.Zero), right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34)]
    public void Multiply_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = (x * y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.23)]
    public void Multiply_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = (x * y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Multiply_ConstantAndDualNumber_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (left * new Dual<Real<double>, double>(right, Real<double>.Zero)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1.23)]
    public void Multiply_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (left * new Dual<Real<double>, double>(right, Real<double>.One)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 2.34)]
    public void Multiply_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.One) * right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Multiply_DualNumberAndConstant_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.Zero) * right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, -1)]
    public void Negate_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = (-x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.334835801674179)]
    public void Operation_Binary_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> y = new(left, Real<double>.One);
        Dual<Real<double>, double> x = new(right, Real<double>.Zero);
        var u = Real<double>.One / (x.D0 * x.D0 + y.D0 * y.D0);

        var actual = Dual<Real<double>, double>.Operation(x, y, Real<double>.Atan2, (x, y) => x * u, (x, y) => -y * u).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.1760034342133505)]
    public void Operation_Binary_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> y = new(left, Real<double>.Zero);
        Dual<Real<double>, double> x = new(right, Real<double>.One);
        var u = Real<double>.One / (x.D0 * x.D0 + y.D0 * y.D0);

        var actual = Dual<Real<double>, double>.Operation(x, y, Real<double>.Atan2, (x, y) => x * u, (x, y) => -y * u).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void Operation_Unary_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Operation(x, Real<double>.Sin, Real<double>.Cos).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.088081166620949)]
    public void Pow_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = Dual<Real<double>, double>.Pow(x, y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3360299854573856)]
    public void Pow_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Pow(x, y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3360299854573856)]
    public void Pow_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Pow(left, x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 3.088081166620949)]
    public void Pow_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);

        var actual = Dual<Real<double>, double>.Pow(x, right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0.3795771135606888)]
    public void Root_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> n = new(right, Real<double>.Zero);

        var actual = Dual<Real<double>, double>.Root(x, n).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -0.04130373687338086)]
    public void Root_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> n = new(right, Real<double>.One);

        var actual = Dual<Real<double>, double>.Root(x, n).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.3342377271245026)]
    public void Sin_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Sin(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 1.856761056985266)]
    public void Sinh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Sinh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.4508348173337161)]
    public void Sqrt_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Sqrt(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Subtract_TwoDualNumbers_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.One);
        Dual<Real<double>, double> y = new(right, Real<double>.Zero);

        var actual = (x - y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1)]
    public void Subtract_TwoDualNumbers_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        Dual<Real<double>, double> x = new(left, Real<double>.Zero);
        Dual<Real<double>, double> y = new(right, Real<double>.One);

        var actual = (x - y).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Subtract_ConstantAndDualNumber_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (left - new Dual<Real<double>, double>(right, Real<double>.Zero)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, -1)]
    public void Subtract_ConstantAndDualNumber_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (left - new Dual<Real<double>, double>(right, Real<double>.One)).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 1)]
    public void Subtract_DualNumberAndConstant_ReturnsDerivativeWithRespectToLeft(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.One) - right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 2.34, 0)]
    public void Subtract_DualNumberAndConstant_ReturnsDerivativeWithRespectToRight(double left, double right, double expected)
    {
        var actual = (new Dual<Real<double>, double>(left, Real<double>.Zero) - right).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, Real<double>.Zero);
    }

    [TestMethod]
    [DataRow(1.23, 8.95136077522624)]
    public void Tan_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Tan(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [DataRow(1.23, 0.2900600799721436)]
    public void Tanh_DualNumber_ReturnsDerivative(double input, double expected)
    {
        Dual<Real<double>, double> x = new(input, Real<double>.One);

        var actual = Dual<Real<double>, double>.Tanh(x).D1;

        Assert<Real<double>, double>.AreApproximatelyEqual(expected, actual, 1e-15);
    }
}
