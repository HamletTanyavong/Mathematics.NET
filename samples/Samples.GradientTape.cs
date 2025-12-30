#:package Mathematics.NET

using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

GradientTape<Real> tape = new();

var x = tape.CreateVariable(1.0);
var y = tape.CreateVariable(2.0);
var z = tape.CreateVariable(3.0);

// f(x, y, z) = sin(x + y) * cos(exp(y + z))
var result = tape.Multiply(
    tape.Sin(
        tape.Add(x, y)),
    tape.Cos(
        tape.Exp(
            tape.Add(y, z))));

tape.ReverseAccumulate(out var gradient);

Console.WriteLine($"The value of f(x, y, z) is {result}.");
Console.WriteLine($"Gradient of f(x, y, z) with respect to x: {gradient[0]}.");
Console.WriteLine($"Gradient of f(x, y, z) with respect to y: {gradient[1]}.");
Console.WriteLine($"Gradient of f(x, y, z) with respect to z: {gradient[2]}.");
