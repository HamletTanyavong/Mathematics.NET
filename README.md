<div align="center">
  <a href="https://mathematics.hamlettanyavong.com">
      <img src="docs/static/img/mathematics.net.svg" width="128" height="128" alt="Mathematics.NET Logo">
  </a>
  <h1>Mathematics.NET</h1>
  <p>Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems.</p>
</div>

[![GitHub](https://img.shields.io/github/license/HamletTanyavong/Mathematics.NET?style=flat-square&logo=github&labelColor=87cefa&color=ffd700)](https://github.com/HamletTanyavong/Mathematics.NET)
[![GitHub Repo Stars](https://img.shields.io/github/stars/HamletTanyavong/Mathematics.NET?color=87cefa&style=flat-square&logo=github)](https://github.com/HamletTanyavong/Mathematics.NET/stargazers)
[![NuGet Package](https://img.shields.io/nuget/v/Mathematics.NET?style=flat-square&logo=nuget&color=green)](https://www.nuget.org/packages/Mathematics.NET)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Mathematics.NET?style=flat-square&logo=nuget&color=green)](https://www.nuget.org/packages/Mathematics.NET)
[![Unit Tests](https://img.shields.io/github/actions/workflow/status/HamletTanyavong/Mathematics.NET/unit-tests.yml?label=Unit%20Tests&style=flat-square&logo=github&color=87cefa)](https://github.com/HamletTanyavong/Mathematics.NET/actions/workflows/unit-tests.yml)

## About

Mathematics.NET provides custom types for complex, real, and rational numbers as well as other mathematical objects such as vectors, matrices, and tensors.[^1] Mathematics.NET also supports first and second-order, forward and reverse-mode automatic differentiation.

## Examples

The examples below highlight some of the features of Mathematics.NET.

### Rational Numbers

Rational numbers can be created using

```csharp
Rational<int> number = new(4, 6);
```

Besides the basic arithmetic operations, operations on rational numbers include, but are not limited to, the following:

```csharp
var reduced = number.Reduce();
var reciprocal = number.Reciprocate();
```

### Automatic Differentiation

Reverse-mode automatic differentiation can be performed using gradient tapes. To create a gradient tape, use

```csharp
GradientTape<Real> tape = new();
```

Variables can be created using the `.CreateVarible()` method. To work with functions of three variables with intial values $x=1.23$, $y=0.66$, and $z=2.34$, write

```csharp
var x = tape.CreateVariable(1.23);
var y = tape.CreateVariable(0.66);
var z = tape.CreateVariable(2.34);
```

Now suppose we want to find the gradient of the function

$$
  f(x,y,z) = \frac{\cos x}{(x+y)\sin z}
$$

at the specified points. We write

```csharp
var result = tape.Divide(
  tape.Cos(x),
  tape.Multiply(
    tape.Add(x, y),
    tape.Sin(z)));
```

and use the `.ReverseAccumulate()` method to compute the gradient.

```csharp
tape.ReverseAccumulate(out var gradient);

Console.WriteLine("At the points x = 1.23, y = 0.66, and z = 2.34:");
Console.WriteLine($"f = {result.Value}");
Console.WriteLine($"df/dx = {gradient[0]}");
Console.WriteLine($"df/dy = {gradient[1]}");
Console.WriteLine($"df/dz = {gradient[2]}");

// At the points x = 1.23, y = 0.66, and z = 2.34:
// f = 0.24614338791952137
// df/dx = -0.8243135949243512
// df/dy = -0.13023459678281554
// df/dz = 0.2382974299363868

```

[^1]: Please visit the [documentation site](https://mathematics.hamlettanyavong.com) for detailed information.
