---
sidebar_position: 2
description: Use first and second-order, forward-mode automatic differentiation (autodiff) using gradients and Hessian tapes.
keywords: [forward-mode, autodiff, dual numbers, hyperdual, math, C#, csharp, .NET]
---

# Forward-Mode Automatic Differentiation

Support for first and second-order, forward-mode autodiff are provided by dual and hyperdual numbers.

## First Order

First-order, forward-mode autodiff can be performed using [dual numbers](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/AutoDiff/Dual.cs).

### Dual Numbers

Forward-mode autodiff can be performed through the use of dual numbers which keep track of derivatives from our calculations. To create a dual number in Mathematics.NET, we provide a *primal* and *tangent* part; if no tangent part is provided, it will automatically be set to zero.

```csharp
Dual<Real> x = new(1.23, 1.0);
Dual<Real> y = new(2.34);
```

The primal part represents the point at which we want to compute our derivative, while the tangent part holds the information about our derivative. When we create a dual number with a tangent part, we specify a value that will be used as the seed. (It is important to know that the value of the derivative changes proportionally with this value.) We can also write, equivalently,

```csharp
using static Mathematics.NET.AutoDiff.Dual<Mathematics.NET.Core.Real>;

var x = CreateVariable(1.23, 1.0);
var y = CreateVariable(2.34);
```

Suppose we want to compute the partial derivative of the function

$$
f(x,y) = \frac{\sin(x + y)e^{-y}}{x^2+y^2+1}
$$

at the points $ x=1.23 $ and $ y=2.34 $ with respect to $ x $. We write

```csharp
Dual<Real> x = CreateVariable(1.23, 1.0);
Dual<Real> y = CreateVariable(2.34);

var result = Sin(x + y) * Exp(-y) / (x * x + y * y + 1);

Console.WriteLine("∂f/∂x: {0}", result);
```

Notice that we set the seed for the variable of interest, $ x $, to 1 while the seed for the variable we do not care about, $ y $, was set to 0. If we had set both to 1.0, then we would have computed the total derivative of the function instead. To compute the partial derivative of our function with respect to $ y $, we write

```csharp
Dual<Real> x = CreateVariable(1.23);
Dual<Real> y = CreateVariable(2.34, 1.0);
```

with the tangent part of the variable of interest set to 1 and the other to 0. Doing this will print the following to the console:

```
∂f/∂x: (-0.005009285670379789, -0.009425990481108835)
∂f/∂y: (-0.005009285670379789, -0.003024626925238263)
```

#### Total Derivative

To get the total derivate of a function, set the seeds for each variable to `1.0`:

```csharp
Dual<Real> x = CreateVariable(1.23, 1.0);
Dual<Real> y = CreateVariable(2.34, 1.0);
// Repeat for each variable present
```

### AutoDiff Vectors

We can create autodiff vectors to help us keep track of multiple dual numbers.

```csharp
AutoDiffVector3<Real> x = new(CreateVariable(1.23), CreateVariable(0.66), CreateVariable(2.34));
```

We can use this to compute the vector-Jacobian product of the vector functions

$$
\begin{align}
  \begin{split}
    f_1(\textbf{x}) & =\sin(x_1)(\cos(x_2)+\sqrt{x_3})    \\
    f_2(\textbf{x}) & =\sqrt{x_1+x_2+x_3} \\
    f_3(\textbf{x}) & =\sinh\left(\frac{e^xy}{z}\right)
  \end{split}
\end{align}
$$

with the vector $ v=(0.23,1.57,-1.71) $ at our points $ x_1=1.23 $, $ x_2=0.66 $, and $ x_3=2.34 $ by writing

```csharp
AutoDiffVector3<Real> x = new(CreateVariable(1.23), CreateVariable(0.66), CreateVariable(2.34));
Vector3<Real> v = new(0.23, 1.57, -1.71);

var result = AutoDiffVector3<Real>.VJP(
    v,
    x => Sin(x.X1) * (Cos(x.X2) + Sqrt(x.X3)),
    x => Sqrt(x.X1 + x.X2 + x.X3),
    x => Sinh(Exp(x.X1) * x.X2 / x.X3),
    x);

Console.WriteLine(result); // (-1.9198130659708643, -3.508528536106042, 1.5122861260495055)
```

## Second Order

Second-order, forward-mode autodiff can be performed using [hperdual numbers](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/AutoDiff/HyperDual.cs).

### HyperDual Numbers

Suppose we wanted to find the second derivative of the complex function

$$
f(z,w) = \sin(\tan{z}\log{w})
$$

with respect to $ z $, at the points $ z=1.23+i0.66 $ and $ w=2.34-i0.25 $. We can do so by writing

```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;
using static Mathematics.NET.AutoDiff.HyperDual<Mathematics.NET.Core.Complex>;

var z = CreateVariable(new(1.23, 0.66), 1.0, 1.0);
var w = CreateVariable(new(2.34, -0.25));

var result = Sin(Tan(z) * Ln(w));

Console.WriteLine(result.D3); // (-6.158582087985498, 6.391603674636932)
```

Notice that we now have to provide two seed values. Each one, very loosely speaking, "represents" a first-order derivative with respect to the variable in which it appears. Since there are two seeds present with the variable $ z $, it means we want to take its derivative twice. Similarly, we can write the following if we wanted the second derivative of our function with respect to $ w $:

```csharp
var z = CreateVariable(new(1.23, 0.66));
var w = CreateVariable(new(2.34, -0.25), 1.0, 1.0);
```

This will give us `(0.30998196902728725, -0.11498565892578178)`. To get our mixed derivative, $ \partial f/\partial{z}\partial{w} $, we indicate we want one of each derivative

```csharp
var z = CreateVariable(new(1.23, 0.66), 1.0, 0.0);
var w = CreateVariable(new(2.34, -0.25), 0.0, 1.0);
```

keeping in mind that the seeds must not occupy the same "slot." This, for example, will not give us the correct answer:

```csharp
// Incorrect, seeds must not occupy the same "slot"
var z = CreateVariable(new(1.23, 0.66), 1.0, 0.0);
var w = CreateVariable(new(2.34, -0.25), 1.0, 0.0);
```

This will print `(0.6670456012622978, 2.2955143408553718)` to the console.

## Higher Order Derivatives

Higher order derivatives can be computed by nesting dual and hyperdual numbers. Though not officially supported, it is possible to create one yourself; take a look at the source code for [hyperdual numbers](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/AutoDiff/HyperDual.cs) to see how it can be done.
