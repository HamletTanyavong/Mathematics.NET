# Second-Order, Reverse Mode Automatic Differentiation

Support for first-order, reverse-mode automatic differentiation (autodiff) is provided by the `HessianTape` class.

## Hessian Tapes

The steps needed to perform second-order, reverse-mode autodiff is similar to the steps needed to perform the first-order case. This time, however, we have access to the following overloads and/or versions of `ReverseAccumulation`:
```csharp
HessianTape<Complex> tape = new();

// Do some math...

// Use when we are only interested in the gradient
tape.ReverseAccumulation(out ReadOnlySpan<Complex> gradient);
// Use when we are only interested in the Hessian
tape.ReverseAccumulation(out ReadOnlySpan2D<Complex> hessian);
// Use when we are interested in both the gradient and Hessian
tape.ReverseAccumulation(out var gradient, out var hessian);
```
The last version may be useful for calculations such as finding the Laplacian of a scalar function in spherical coordinates which involves derivatives of first and second orders:
$$
\begin{align}
    \nabla^2f(r,\theta,\phi)    &   =\frac{1}{r^2}\frac{\partial}{\partial r}\left(r^2\frac{\partial f}{\partial r}\right)+\frac{1}{r^2\sin{\theta}}\frac{\partial}{\partial\theta}\left(\sin{\theta}\frac{\partial f}{\partial\theta}\right)+\frac{1}{r^2\sin^2{\theta}}\frac{\partial^2f}{\partial\phi^2}   \\
    &   =\frac{2}{r}\frac{\partial f}{\partial r}+\frac{\partial^2f}{\partial r^2}+\frac{1}{r^2\sin{\theta}}\left(\cos{\theta}\frac{\partial f}{\partial\theta}+\sin{\theta}\frac{\partial^2f}{\partial\theta^2}\right)+\frac{1}{r^2\sin^2{\theta}}\frac{\partial^2f}{\partial\phi^2}
\end{align}
$$
Note that, in the future, we will not have to do this manually since there will be a method made specifically to compute Laplacians in spherical coordinates. For now, if we wanted to compute the Laplacian of the function
$$
    f(x,y,z) = \frac{\cos(x)}{(x+y)\sin(z)}
$$
we can write
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

HessianTape<Real> tape = new();
var x = tape.CreateVariableVector(1.23, 0.66, 0.23);

// f(r, θ, ϕ) = cos(r) / ((r + θ) * sin(ϕ))
_ = tape.Divide(
        tape.Cos(x.X1),
        tape.Multiply(
            tape.Add(x.X1, x.X2),
            tape.Sin(x.X3)));

tape.ReverseAccumulation(out var gradient, out var hessian);

// Manual Laplacian computation
var u = Real.One / (x.X1.Value * Real.Sin(x.X2.Value)); // 1 / (r * sin(θ))
var laplacian = 2.0 * gradient[0] / x.X1.Value +
                hessian[0, 0] +
                u * Real.Cos(x.X2.Value) * gradient[1] / x.X1.Value +
                hessian[1, 1] / (x.X1.Value * x.X1.Value) +
                u * u * hessian[2, 2];

Console.WriteLine(laplacian);
```
which should give us `48.80966092022821`.
