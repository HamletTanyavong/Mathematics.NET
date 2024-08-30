# Second-Order, Forward Mode Automatic Differentiation

Support for first-order, forward-mode autodiff is provided by <xref href="Mathematics.NET.AutoDiff.HyperDual`1" />. Because this type is used in a similar manner to <xref href="Mathematics.NET.AutoDiff.Dual`1" />, please refer to that section for help.

### Second-Order Derivatives

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

Console.WriteLine(result.D3);
```
which will give us `(-6.158582087985498, 6.391603674636932)`. Notice that we now have to provide two seed values. Each one, very loosely speaking, "represents" a first-order derivative with respect to the variable in which it appears. Since there are two seeds present with the variable $ z $, it means we want to take its derivative twice. Similarly, we can write the following if we wanted the second derivative of our function with respect to $ w $:
```csharp
var z = CreateVariable(new(1.23, 0.66));
var w = CreateVariable(new(2.34, -0.25), 1.0, 1.0);
```
This will give us `(0.30998196902728725, -0.11498565892578178)`. To get our mixed derivative, $ \partial f/\partial{z}\partial{w} $, we must indicate with we want one of each derivative
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
