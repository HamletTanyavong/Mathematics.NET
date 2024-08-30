# First-Order, Forward Mode Automatic Differentiation

Support for first-order, forward-mode autodiff is provided by <xref href="Mathematics.NET.AutoDiff.Dual`1" />.

## Dual Numbers

Forward-mode autodiff can be performed through the use of dual numbers which keep track of derivatives from our calculations. To create a dual number, we may provide a *primal* and *tangent* part. If no tangent part is provided, it will automatically be set to zero.
```csharp
Dual<Real> x = new(1.23, 1.0);
Dual<Real> y = new(2.34);
```
The primal part represents the point at which we want to compute our derivative while the tangent part holds the information about our derivative. When we create a dual number with a tangent part, we specify a value that will be used as the seed, and it is important to know that the value of the derivative changes proportionally with this value. We could also write, equivalently,
```csharp
using static Mathematics.NET.AutoDiff.Dual<Mathematics.NET.Core.Real>;

var x = CreateVariable(1.23, 1.0);
var y = CreateVariable(2.34);
```
Suppose we want to compute the partial derivative of the function
$$
    f(x,y) = \frac{\sin(x + y)e^{-y}}{x^2+y^2+1}
$$
at the points $ x=1.23 $ and $ y=2.34 $.
with respect to $ x $. We must write
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;
// Add the following line to avoid having to type Dual<Real> every time we want to use a function.
using static Mathematics.NET.AutoDiff.Dual<Mathematics.NET.Core.Real>;

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

### Total Derivative

To get the total derivate of a function, set the seeds for each variable to `1.0`;
```csharp
Dual<Real> x = CreateVariable(1.23, 1.0);
Dual<Real> y = CreateVariable(2.34, 1.0);
// Repeat for each variable present
```

## AutoDiff Vectors

We can create autodiff vectors to help us keep track of multiple dual numbers.
```csharp
AutoDiffVector3<Real> x = new(CreateVariable(1.23), CreateVariable(0.66), CreateVariable(2.34));
```
We can use this to compute the vector-Jacobian product of the vector functions
$$
\begin{align}
    f_1(\textbf{x}) &   =\sin(x_1)(\cos(x_2)+\sqrt{x_3})    \\
    f_2(\textbf{x}) &   =\sqrt{x_1+x_2+x_3} \\
    f_3(\textbf{x}) &   =\sinh\left(\frac{e^xy}{z}\right)
\end{align}
$$
with the vector $ v=(0.23,1.57,-1.71) $ at our points $ x_1=1.23 $, $ x_2=0.66 $, and $ x_3=2.34 $ by writing
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;
using static Mathematics.NET.AutoDiff.Dual<Mathematics.NET.Core.Real>;

AutoDiffVector3<Real> x = new(CreateVariable(1.23), CreateVariable(0.66), CreateVariable(2.34));
Vector3<Real> v = new(0.23, 1.57, -1.71);

var result = AutoDiffVector3<Real>.VJP(
    v,
    x => Sin(x.X1) * (Cos(x.X2) + Sqrt(x.X3)),
    x => Sqrt(x.X1 + x.X2 + x.X3),
    x => Sinh(Exp(x.X1) * x.X2 / x.X3),
    x);

Console.WriteLine(result);
```
This prints `(-1.9198130659708643, -3.508528536106042, 1.5122861260495055)` to the console.
