---
sidebar_position: 1
---

# Numeric Types

All numeric types in Mathematics.NET implement the [`IComplex<T>`](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/IComplex.cs) interface. Unlike [`INumberBase<T>`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/INumberBase.cs), this interface defines the method `Conjugate()`, which makes operations that accept either complex or real numbers as parameters more robust.

## Complex Numbers

Because the [complex number type](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Complex.cs) in this library shares the name with .NET's own implementation, use the following statement to avoid conflicts:

```csharp
using Complex = Mathematics.NET.Core.Complex;
```
With that, to create any complex number, we can simply write

```csharp
Complex z = new(3, 4);
```

which represents $ z=3+i4 $ or

```csharp
Complex z = 2
```

which is equivalent to $ z=2 $.

### Important Methods

The following are some important methods related to complex numbers. Note that this method works for real and rational numbers as well.

#### Conjugate

As mentioned before, one important method is `Conjugate()`.

```csharp
z.Conjugate(); // 3-i4
```

#### Magnitude

The magnitude of a complex number is defined as $ |z| = \sqrt{x^2+y^2} $, where $ z=x+iy $. This is implemented as the property, `Magnitude`.

```csharp
z.Magnitude; // 5
```

#### Phase

The phase of a complex number is defined as $ \arg{z} = \arctan{y/x} $, where $ z=x+iy $. This is implemented as the property, `Phase`.

```csharp
z.Phase; // 0.9272952180016122
```


## Real Numbers

Real numbers are represented by the [real](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Real.cs) type and function as expected. This type implements the `IReal<T>` interface, which inherits from `IComplex<T>`, and contains additional methods such as `Max(x, y)` and `Min(x, y)`. To create one, simply write

```csharp
Real x = 1;
```

## Rational Numbers

Rational numbers are represented by the [rational](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Rational.cs) type. This type implements the `IRational<T>` interface, which inherits from `IReal<T>`, and contains addition mathods such as `Reciprocate()` and `GCD()`.
