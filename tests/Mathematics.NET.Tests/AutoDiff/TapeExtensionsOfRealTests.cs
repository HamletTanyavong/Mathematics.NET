// <copyright file="TapeExtensionsOfRealTests.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Tests.AutoDiff;

[TestClass]
[TestCategory("AutoDiff"), TestCategory("Real Number")]
public sealed class TapeExtensionsOfRealTests
{
    private GradientTape<Real> _tape;

    public TapeExtensionsOfRealTests()
    {
        _tape = new();
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(1.23, 0.66, 2.34, 1.954144178335244, -1.142124546272508, 0.820964086423733)]
    public void Curl_VectorField_ReturnsCurl(double x, double y, double z, double expectedX, double expectedY, double expectedZ)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);
        Vector3<Real> expected = new(expectedX, expectedY, expectedZ);

        var actual = _tape.Curl(FX, FY, FZ, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(0.23, 1.57, -1.71, 1.23, 0.66, 2.34, -0.801549048972843)]
    public void DirectionalDerivative_ScalarFunctionAndDirection_ReturnsDirectionalDerivative(double vx, double vy, double vz, double x, double y, double z, double expected)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);
        Vector3<Real> v = new(vx, vy, vz);

        var actual = _tape.DirectionalDerivative(v, F, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(1.23, 0.66, 2.34, 0.3987010509910668)]
    public void Divergence_VectorField_ReturnsDivergence(double x, double y, double z, double expected)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);

        var actual = _tape.Divergence(FX, FY, FZ, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(1.23, 0.66, 2.34, -0.824313594924351, -0.1302345967828155, 0.2382974299363869)]
    public void Gradient_ScalarFunction_ReturnsGradient(double x, double y, double z, double expectedX, double expectedY, double expectedZ)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);
        Vector3<Real> expected = new(expectedX, expectedY, expectedZ);

        var actual = _tape.Gradient(F, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    public void Hessian_ScalarFunction_ReturnsHessian()
    {
        HessianTape<Real> tape = new();
        var u = tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
        Matrix3x3<Real> expected = new(
            0.6261461305189455, 0.5050519532842152, -0.7980381386329245,
            0.5050519532842152, 0.1378143881299635, -0.1260832962626385,
            -0.7980381386329245, -0.1260832962626385, 0.707546520412796);

        var actual = tape.Hessian(F, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    public void Jacobian_R3VectorFunction_ReturnsJacobian()
    {
        var u = _tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
        Matrix3x3<Real> expected = new(
            0.775330615737715, -0.5778557672605755, 0.3080621020764366,
            0.2431083191631576, 0.2431083191631576, 0.2431083191631576,
            1.450186648348945, 2.197252497498402, -0.6197378839098056);

        var actual = _tape.Jacobian(FX, FY, FZ, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(1.23, 0.66, 2.34, 0.23, 1.57, -1.71, -1.255693707530136, 0.0218797487246842, 4.842981131678516)]
    public void JVP_R3VectorFunctionAndVector_ReturnsJVP(double x, double y, double z, double vx, double vy, double vz, double expectedX, double expectedY, double expectedZ)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);
        Vector3<Real> v = new(vx, vy, vz);
        Vector3<Real> expected = new(expectedX, expectedY, expectedZ);

        var actual = _tape.JVP(FX, FY, FZ, u, v);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(1.23, 0.66, 2.34, 1.471507039061705)]
    public void Laplacian_ScalarFunction_ReturnsLaplacian(double x, double y, double z, double expected)
    {
        HessianTape<Real> tape = new();
        var u = tape.CreateAutoDiffVector(x, y, z);

        var actual = tape.Laplacian(F, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    [TestMethod]
    [TestCategory("Vector Calculus")]
    [DataRow(0.23, 1.57, -1.71, 1.23, 0.66, 2.34, -1.919813065970865, -3.508528536106042, 1.512286126049506)]
    public void VJP_VectorAndR3VectorFunction_ReturnsVJP(double vx, double vy, double vz, double x, double y, double z, double expectedX, double expectedY, double expectedZ)
    {
        var u = _tape.CreateAutoDiffVector(x, y, z);
        Vector3<Real> v = new(vx, vy, vz);
        Vector3<Real> expected = new(expectedX, expectedY, expectedZ);

        var actual = _tape.VJP(v, FX, FY, FZ, u);

        Assert<Real>.AreApproximatelyEqual(expected, actual, 1e-15);
    }

    //
    // Helpers
    //

    // f(x, y, z) = Cos(x) / ((x + y) * Sin(z))
    private static Variable<Real> F(ITape<Real> tape, AutoDiffVector3<Real> x)
    {
        return tape.Divide(
            tape.Cos(x.X1),
            tape.Multiply(
                tape.Add(x.X1, x.X2),
                tape.Sin(x.X3)));
    }

    // f(x, y, z) = Sin(x) * (Cos(y) + Sqrt(z))
    private static Variable<Real> FX(ITape<Real> tape, AutoDiffVector3<Real> x)
    {
        return tape.Multiply(
            tape.Sin(x.X1),
            tape.Add(
                tape.Cos(x.X2),
                tape.Sqrt(x.X3)));
    }

    // f(x, y, z) = Sqrt(x + y + z)
    private static Variable<Real> FY(ITape<Real> tape, AutoDiffVector3<Real> x)
    {
        return tape.Sqrt(
            tape.Add(
                tape.Add(
                    x.X1,
                    x.X2),
                x.X3));
    }

    // f(x, y, z) = Sinh(Exp(x) * y / z)
    private static Variable<Real> FZ(ITape<Real> tape, AutoDiffVector3<Real> x)
    {
        return tape.Sinh(
            tape.Multiply(
                tape.Exp(x.X1),
                tape.Divide(
                    x.X2,
                    x.X3)));
    }
}
