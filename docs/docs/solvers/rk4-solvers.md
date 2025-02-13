---
sidebar_position: 1
description: Fourth-order Runge-Kutta solvers in Mathematics.NET.
keywords: [Runge-Kutta, RK4, solver, diff eq, differential equations, math, C#, csharp, .NET]
---

# RK4 Solvers

Mathematics.NET provides a number of fourth-order Runge-Kutta (RK4) solvers, some of which can be used for tensor equations.

## Second-Order Differential Equations

Suppose we want to solve the ordinary differential equation (ODE),

$$
\begin{align}
  \frac{1}{2}\frac{d^2x}{dt^2}+2\frac{dx}{dt}+\frac{1}{2}x=\sin{t}
\end{align}
$$

with the initial conditions $ x(0)=1.23 $ and $ x'(0)=2.34 $. We start by multiplying both sides by two and set $ u=dx/dt $ so that we obtain

$$
\begin{align}
  \frac{du}{dt}+4u+x=2\sin{t}
\end{align}
$$

This lets us write equation (1) as

$$
\begin{align}
  \frac{d}{dt}\begin{pmatrix}u \\ x\end{pmatrix}+\begin{pmatrix}4 & 1 \\ -1 & 0\end{pmatrix}\begin{pmatrix}u \\ x\end{pmatrix}=\begin{pmatrix}2\sin{t} \\ 0\end{pmatrix}
\end{align}
$$

To solve this with Mathematics.NET, use `RungeKutta4<Vector2<Real>, Real>` with `X1`=$ u $ and `X2`=$ x $. We write the following:

```csharp
using Mathematics.NET.Core;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.Solvers;
using Plotly.NET.CSharp;
using Chart = Plotly.NET.CSharp.Chart;

State<Vector2<Real>, Real> state = new(new Vector2<Real>[] { new(2.34, 1.23) }, 0);
RungeKutta4<Vector2<Real>, Real> rk4 = new((t, x) => new(2 * Real.Sin(t) - 4 * x.X1 - x.X2, x.X1));

var count = 200;
Vector2<Real>[] points = new Vector2<Real>[count];

for (int i = 0; i < count; i++)
{
    rk4.Integrate(state, 0.1);

    points[i].X1 = state.Time;
    points[i].X2 = state.System.Span[0].X2;
}

var chart = Chart.Line<double, double, string>(
    x: points.Select(x => x.X1.AsDouble()),
    y: points.Select(x => x.X2.AsDouble()),
    Name: "X(t)",
    ShowLegend: true);
chart.Show();
```

The state of the system is tracked by `State<Vector2<Real>, Real>`, which contains information about the intial conditions. We have also chosen $ \Delta t=0.1 $ and the number of points to be 200. The solution is then plotted using [Plotly.NET](https://github.com/plotly/Plotly.NET).
