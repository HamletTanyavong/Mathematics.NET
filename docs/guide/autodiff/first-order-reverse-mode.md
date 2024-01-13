# First-Order, Reverse-Mode Automatic Differentiation

Support for first-order, reverse-mode automatic differentiation (autodiff) is provided by the `GradientTape` class.

## Gradient Tapes

Gradient tapes keep track of operations for autodiff; unlike forward-mode autodiff, tracking is required since gradients have to be calculated in reverse order. To begin using reverse-mode autodiff, we must create a gradient tape and assign it variables to track. These variables will be passed into and returned from methods that will compute the local gradients for us and record them on the tape.
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

GradientTape<Real> tape = new();
var x = tape.CreateVariable(1.23);
```
We also have to pass in an initial value—the point at which the gradients are calculated—to the variable creation method. If we want to track multiple variables, we can simply write
```csharp
GradientTape<Real> tape = new();
var x = tape.CreateVariable(1.23);
var y = tape.CreateVariable(0.66);
// Add more variables as needed
```
and so on. To simplify this process, we may choose to create a vector of variables.
```csharp
AutoDiffVector3 x = tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
```
Once we are satisfied, we may use these in our equations.

## Single-Variable Equations

Suppose we want to compute the derivative of the function
$$
    f(x) = \frac{\sin(x)\ln(x)}{e^{-x}}\quad\text{for }x>0
$$
at the point $ x=1.23 $. We can write
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

GradientTape<Real> tape = new();
var x = tape.CreateVariable(1.23);

var result = tape.Divide(
    tape.Multiply(tape.Sin(x), tape.Ln(x)),
    tape.Exp(
        tape.Multiply(-Real.One, x)));
```
which will give us the value of the function at our specified point. At this moment, the derivative has not been calculated, but we are, however, able to examine the nodes which have been added to our tape. We can use the method `PrintNodes` to examine our nodes.
```csharp
tape.PrintNodes(CancellationToken.None);
```
Here, we pass in a cancellation token in case the gradient tape is too large and we do not want to print all of the nodes onto the console. We can also set a limit on how many nodes are printed to the console (by default, this value is 100).
```csharp
tape.PrintNodes(CancellationToken.None, 25);
```
Using this on our gradient tape will give us the following output:
```
Root Node 0:
    Weights: [0, 0]
    Parents: [0, 0]

Node 1:
    Weights: [0.3342377271245026, 0]
    Parents: [0, 1]
Node 2:
    Weights: [0.8130081300813008, 0]
    Parents: [0, 2]
Node 3:
    Weights: [0.20701416938432612, 0.9424888019316975]
    Parents: [1, 2]
Node 4:
    Weights: [-1, 0]
    Parents: [0, 4]
Node 5:
    Weights: [0.2922925776808594, 0]
    Parents: [4, 5]
Node 6:
    Weights: [3.4212295362896734, -2.2837086494091605]
    Parents: [3, 5]
```
The root node represents the variable we are currently tracking. Nodes from unary operations will provide one weight and parent, while nodes from binary operations will provide two weights and parents. This may be helpful when we want to determine which node came from which operation. (For performance reasons, the names of these methods are not tracked.) Below is a graph representation of the nodes on our gradient tape:
```mermaid
graph BT
    x[w₀: x]
    sinx(w₁: sin)
    lnx(w₂: ln)
    sinxlnx(w₃: mul)
    negx(w₄: const mul)
    expnegx(w₅: exp)
    divide(w₆: divide)

    x -- adj(w₀ᵃ) = adj(w₁) ∂w₁/∂w₀ --> sinx
    x -- adj(w₀ᵇ) = adj(w₂) ∂w₂/∂w₀ --> lnx
    x -- adj(w₀ᶜ) = adj(w₄) ∂w₄/∂w₀ --> negx
    sinx -- adj(w₁) = adj(w₃) ∂w₃/∂w₁ --> sinxlnx
    lnx -- adj(w₂) = adj(w₃) ∂w₃/∂w₂ --> sinxlnx
    sinxlnx -- adj(w₃) = adj(w₆) ∂w₆/∂w₃ --> divide
    negx -- adj(w₄) = adj(w₅) ∂w₅/∂w₄ --> expnegx
    expnegx -- adj(w₅) = adj(w₆) ∂w₆/∂w₅ --> divide
    divide -- adj(f) = adj(w₆) = 1 (seed) --> function["f(x)"]
```
We can then calculate the gradient of our function by using the `ReverseAccumulation` method.
```csharp
tape.ReverseAccumulation(out var gradient);
```
Since this is a single variable equation, we can access the first element of `gradients` to get our result.
```csharp
Console.WriteLine(gradient[0]);
```
The correct value for the derivative should be `3.525753368769319`. The complete code looks as follows:
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

GradientTape<Real> tape = new();
var x = tape.CreateVariable(1.23);

var result = tape.Divide(
    tape.Multiply(tape.Sin(x), tape.Ln(x)),
    tape.Exp(
        tape.Multiply(-Real.One, x)));

// Optional: examine the nodes on the gradient tape
tape.PrintNodes();

tape.ReverseAccumulation(out var gradient);

// The value of the function at the point x = 1.23: 0.6675110878078776
Console.WriteLine("Value: {0}", result);
// The derivative of the function with respect to x at the point x = 1.23: 3.525753368769319
Console.WriteLine("Derivative: {0}", gradient[0]);
```

## Multivariable Equations

The multivariable case is as simple as the single variable case; we only need to track more variables on our gradient tape.
```csharp
using Mathematics.NET.AutoDiff;

GradientTape<Real> tape = new();
var x = tape.CreateVariable(1.23);
var y = tape.CreateVariable(0.66);
var z = tape.CreateVariable(2.34);
```
Now, if we wanted to compute the gradient of the function
$$
    f(x,y,z) = \frac{\cos(x)}{(x+y)\sin(z)}
$$
at the points we have chosen, $ x=1.23 $, $ y=0.66 $, and $ z=2.34 $, we can write
```csharp
var result = tape.Divide(
    tape.Cos(x.X1),
    tape.Multiply(
        tape.Add(x.X1, x.X2),
        tape.Sin(x.X3)));
```
If we want to examine the nodes, we can use the `PrintNodes` method that we have already encountered.
```
Root Node 0:
    Weights: [0, 0]
    Parents: [0, 0]
Root Node 1:
    Weights: [0, 0]
    Parents: [1, 1]
Root Node 2:
    Weights: [0, 0]
    Parents: [2, 2]

Node 3:
    Weights: [-0.9424888019316975, 0]
    Parents: [0, 3]
Node 4:
    Weights: [1, 1]
    Parents: [0, 1]
Node 5:
    Weights: [-0.695563326462902, 0]
    Parents: [2, 5]
Node 6:
    Weights: [0.7184647930691263, 1.8900000000000001]
    Parents: [4, 5]
Node 7:
    Weights: [0.7364320899293144, -0.18126788958785509]
    Parents: [3, 6]
```
Notice that there are now three root nodes, each representing the variables $ x $, $ y $, and $ z $, respectively. Here is a graph representation of our nodes:
```mermaid
graph BT
    x[w₀: x]
    y[w₁: y]
    z[w₂: z]
    cos(w₃: cos)
    add(w₄: add)
    sin(w₅: sin)
    mul(w₆: mul)
    div(w₇: div)

    x -- adj(w₀ᵃ) = adj(w₃) ∂w₃/∂w₀ --> cos
    x -- adj(w₀ᵇ) = adj(w₄) ∂w₄/∂w₀ --> add
    y -- adj(w₁) = adj(w₄) ∂w₄/∂w₁ --> add
    z -- adj(w₂) = adj(w₅) ∂w₅/∂w₂ --> sin
    cos -- adj(w₃) = adj(w₇) ∂w₇/∂w₃ --> div
    add -- adj(w₄) = adj(w₆) ∂w₆/∂w₄ --> mul
    sin -- adj(w₅) = adj(w₆) ∂w₆/∂w₅ --> mul
    mul -- adj(w₆) = adj(w₇) ∂w₇/∂w₆ --> div
    div -- adj(f) = adj(w₇) = 1 (seed) --> function["f(x, y, z)"]
```
As before, we can use `ReverseAccumulation` to get our gradients
```csharp
tape.ReverseAccumulation(out var gradient);
```
and print them to the console with
```csharp
using Mathematics.NET.LinearAlgebra;

// code

Console.WriteLine(gradient.ToDisplayString());
```
This will print the following to the console:
```
[-0.8243135949243512,  -0.13023459678281554, 0.2382974299363868    ]
```
which, for clarity, is
$$
\begin{align}
    \frac{\partial}{\partial x}f(x,y,z) &   =-\frac{\csc(z)}{x+y}\left(\frac{\cos(x)}{x+y}+\sin(x)\right) = -0.8243135949243512 \\
    \frac{\partial}{\partial y}f(x,y,z) &   =-\frac{\cos(x)\csc(x)}{(x+y)^2} = -0.13023459678281554 \\
    \frac{\partial}{\partial z}f(x,y,z) &   =-\frac{\cos(x)\cot(z)\csc(z)}{x+y} = 0.2382974299363868
\end{align}
$$
at our specified points.

### AutoDiff Vectors

Instead of tracking $ x $, $ y $, and $ z $ individually, we can create a vector of variables.
```csharp
tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
```
We can use this to calculate, for example, a Jacobian-vector product with the vector functions
$$
\begin{align}
    f_1(\textbf{x}) &   =\sin(x_1)(\cos(x_2)+\sqrt{x_3})    \\
    f_2(\textbf{x}) &   =\sqrt{x_1+x_2+x_3} \\
    f_3(\textbf{x}) &   =\sinh\left(\frac{e^xy}{z}\right)
\end{align}
$$
and the vector $ \textbf{v} = (0.23, 1.57, -1.71) $ for $ x_1,x_2,x_3>0 $.
```
using Mathematics.NET.AutoDiff;

GradientTape<Real> tape = new();
var x = tape.CreateAutoDiffVector(1.23, 0.66, 2.34);
Vector3<Real> v = new(0.23, 1.57, -1.71);

var result = tape.JVP(F1, F2, F3, x, v);

Console.WriteLine(result);

// f(x, y, z) = Sin(x) * (Cos(y) + Sqrt(z))
static Variable F1(GradientTape tape, AutoDiffVector3 x)
{
    return tape.Multiply(
        tape.Sin(x.X1),
        tape.Add(tape.Cos(x.X2), tape.Sqrt(x.X3)));
}

// f(x, y, z) = Sqrt(x + y + z)
static Variable F2(GradientTape tape, AutoDiffVector3 x)
{
    return tape.Sqrt(
        tape.Add(
            tape.Add(x.X1, x.X2),
            x.X3));
}

// f(x, y, z) = Sinh(Exp(x) * y / z)
static Variable F3(GradientTape tape, AutoDiffVector3 x)
{
    return tape.Sinh(
        tape.Multiply(
            tape.Exp(x.X1),
            tape.Divide(x.X2, x.X3)));
}
```
Note that this time, we do not call the method `ReverseAccumulation`. This should give us the following result: `(-1.2556937075301358, 0.021879748724684178, 4.842981131678516)`.

## Complex Variables

We can also work with complex numbers and complex derivatives by specifying `Complex` as a type parameter when we create our gradient tape. Suppose we want to find the gradient of the function:
$$
    f(z,w)  =   \cos(\sin(z)\sqrt{w})
$$
at the points $ z=1.23+i2.34 $ and $ w=-0.66+i0.23 $. We can write
```csharp
using Mathematics.NET.AutoDiff;
using Mathematics.NET.Core;

GradientTape<Complex> tape = new();
var z = tape.CreateVariable(new(1.23, 2.34));
var w = tape.CreateVariable(new(-0.66, 0.23));

var result = tape.Cos(
    tape.Multiply(
        tape.Sin(z),
        tape.Sqrt(w)));

// Optional: examine the nodes on the gradient tape
tape.PrintNodes(CancellationToken.None);
Console.WriteLine();

tape.ReverseAccumulation(out var gradient);

// The value of the function at the point z = 1.23 + i2.34 and w = -0.66 + i0.23
Console.WriteLine("Value: {0}", result);
// The gradient of the function: ∂f/∂z and ∂f/∂w, respectively
Console.WriteLine("Gradient: {0}", gradient.ToDisplayString());
```
which is almost the exact same code we would have written in the real case. (Note that some methods such as `Atan2` are not available for complex gradient tapes.) This should output the following to the console:
```
Root Node 0:
    Weights: [(0, 0), (0, 0)]
    Parents: [0, 0]
Root Node 1:
    Weights: [(0, 0), (0, 0)]
    Parents: [1, 1]

Node 2:
    Weights: [(1.7509986221653533, -4.84670574511495), (0, 0)]
    Parents: [0, 2]
Node 3:
    Weights: [(0.09980501743235655, -0.5896861208610882), (0, 0)]
    Parents: [1, 3]
Node 4:
    Weights: [(0.13951299258538988, 0.8242959875555208), (4.937493465463717, 1.7188022913039218)]
    Parents: [2, 3]
Node 5:
    Weights: [(24.762656886395174, -27.774291395591305), (0, 0)]
    Parents: [4, 5]

Value: (27.784322505370138, 24.753716703326287)
Gradient: [(126.28638563049401, -98.74954259806483),  (-38.801295827094066, -109.6878698782088)  ]
```

## Custom Operations

If there is a function we need that is not provided in the class, we are still able to use it for our gradient tape provided we know its derivative. Suppose, for example, we did not have the `Sin` method. Since we know its derivative is `Cos`, we could write the following:
```csharp
GradientTape tape = new();
var x = tape.CreateVariable(1.23);

var result = tape.CustomOperation(
    x,                 // Our variable
    x => Real.Sin(x),  // The function
    x => Real.Cos(x)); // The derivative of the function

tape.ReverseAccumulation(out var gradient);
Console.WriteLine("Value: {0}", result);
Console.WriteLine("Gradient: {0}", gradient.ToDisplayString());
```
For custom binary operations, we can write
```csharp
_ = tape.CustomOperation(
    x,
    y,
    (x, y) => // f(x, y)
    (x, y) => // ∂f/∂x
    (x, y) => // ∂f/∂y
);
```
> [!NOTE]
> Using variables in loops is not recommended since each iteration will add a node to the tape. If the derivative of the operation is known ahead of time, it may be possible to avoid this problem by using custom operations.
