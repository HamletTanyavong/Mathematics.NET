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
[![Discord](https://img.shields.io/discord/1079097990939148308?style=flat-square&label=Discord&logo=discord&logoColor=white&color=5865F2)](https://discord.gg/haqS9TVK8B)

## About

Mathematics.NET provides custom types for complex, real, and rational numbers as well as other mathematical objects such as vectors, matrices, and tensors.[^1] Mathematics.NET also supports first and second-order, forward and reverse-mode automatic differentiation.

## Features

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

Support for automatic differentiation (autodiff) is provided by gradient tapes, Hessian tapes, dual numbers, and hyper-dual numbers.

#### First-Order, Reverse-Mode AutoDiff

Reverse-mode automatic differentiation can be performed using gradient tapes. To create a gradient tape, use

```csharp
GradientTape<Real> tape = new();
```

Variables can be created using the `.CreateVariable()` method. To work with functions of three variables with intial values $x=1.23$, $y=0.66$, and $z=2.34$, write

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

#### Second-Order, Reverse-Mode AutoDiff

We can use Hessian tapes to perform second-order, reverse-mode automatic differentiation. To create a Hessian tape, use

```csharp
HessianTape<Real> tape = new();
```

Then, we can use the `.CreateAutoDiffVector()` method to create a vector with initial values. Write

```csharp
var x = tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
```

to create a vector with the initial values $x=1.23$, $y=0.66$, and $z=2.34$. Now suppose we want to find the Laplacian of the function

$$
f(r,\theta,\phi) = \frac{\cos r}{(r+\theta)\sin\phi}.
$$

We can use the formula

$$
\begin{align*}
  \nabla^2f &= \frac{1}{r^2}\frac{\partial}{\partial r}\left(r^2\frac{\partial f}{\partial r}\right)+\frac{1}{r^2\sin\theta}\frac{\partial}{\partial\theta}\left(\sin\theta\frac{\partial f}{\partial\theta}\right)+\frac{1}{r^2\sin^2\theta}\frac{\partial^2f}{\partial\phi^2} \\
  &=\frac{2}{r}\frac{\partial f}{\partial r}+\frac{\partial^2f}{\partial r^2}+\frac{1}{r^2\sin\theta}\left(\cos\theta\frac{\partial f}{\partial\theta}+\sin\theta\frac{\partial^2f}{\partial\theta^2}\right)+\frac{1}{r^2\sin^2\theta}\frac{\partial^2f}{\partial\phi^2}
\end{align*}
$$

and write

```csharp
// f(r, θ, ϕ) = cos(r) / ((r + θ) * sin(ϕ))
_ = tape.Divide(
  tape.Cos(x.X1),
  tape.Multiply(
    tape.Add(x.X1, x.X2),
    tape.Sin(x.X3)));
```

Use the `.ReverseAccumulate()` method to get our gradient and Hessian.

```csharp
tape.ReverseAccumulate(out var gradient, our var Hessian);
```

Finally, use those values to compute our Laplacian.

```csharp
var u = Real.One / (x.X1.Value * Real.Sin(x.X2.Value)); // 1 / (r * Sin(θ))
var laplacian =
  2.0 * gradient[0] / x.X1.Value +
  hessian[0, 0] +
  u * Real.Cos(x.X2.Value) * gradient[1] / x.X1.Value +
  hessian[1, 1] / (x.X1.Value * x.X1.Value) +
  u * u * hessian[2, 2];

Console.Writeline(laplacian);
// 48.80966092022821
```

## Contributing

Contributions are welcome and appreciated; filing issues is a good way to do so. However, please start a discussion before starting any work as not all implementations or features will be accepted.

## License

Mathematics.NET falls under the [MIT](LICENSE) license.

[^1]: Please visit the [documentation site](https://mathematics.hamlettanyavong.com) for detailed information.
