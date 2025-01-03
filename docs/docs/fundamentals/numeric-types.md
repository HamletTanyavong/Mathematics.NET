---
sidebar_position: 1
description: Mathematics.NET contains custom types for complex, real, and rational numbers.
keywords: [complex, real, rational, numbers, math, C#, csharp, .NET]
---

# Numeric Types

All numeric types in Mathematics.NET implement the [`IComplex`](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/IComplex.cs) interface. Unlike [`INumberBase`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/INumberBase.cs), this interface defines the method `Conjugate`, which makes operations that accept either complex or real numbers as parameters more robust.

## Complex Numbers

Because the [complex number type](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Complex.cs) in this library shares the name with .NET's own implementation, use the following statement to avoid conflicts:

```csharp
using Complex = Mathematics.NET.Core.Complex;
```

With that, to create complex numbers, we can simply write

```csharp
Complex z = new(3, 4); // 3+i4
Complex w = 2 // 2
```

To get the real or imaginary part of a complex number, we can use the properties `Re` and `Im`, respectively.


### Important Methods

The following are some important methods related to complex numbers. Note that this method works for real and rational numbers as well.

#### Conjugate

As mentioned before, one important method is `Conjugate`.

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

Real numbers are represented by the [real](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Real.cs) type and function as expected. This type implements the `IReal` interface, which inherits from `IComplex`, and contains additional methods such as `Max` and `Min`. To create one, simply write

```csharp
Real x = 1;
```

## Rational Numbers

Rational numbers are represented by the [rational](https://github.com/HamletTanyavong/Mathematics.NET/blob/main/src/Mathematics.NET/Core/Rational.cs) type. This type implements the `IRational` interface, which inherits from `IReal`, and contains addition mathods such as `Reciprocate` and `GCD`.

To create rational numbers, we can write

```csharp
Rational<int> p = new(3, 5); // 3/5
Rational<int> q = 2; // 2
```

Rationals accept any backing type that implements the `IBinaryInteger` and `ISignedNumber` interfaces. This includes `BigInteger`, which should be used carefully due to its performance implications.

To get the numerator and denominator of a rational number, we can use the properties `Num` and `Den`, respectively.

### Important Methods

These methods are specific to rational numbers.

#### Reduce

The `Reduce` method simplifies rational numbers by dividing the numerator and denominator by their greatest common divisor.

```csharp
Rational<int> p = new(6, 9);
Rational<int>.Reduce(p); // 2/3
```

:::note

Operations involving rational numbers automatically reduce their results.

:::

#### Reciprocate

The `Reciprocate` method returns the reciprocal of a rational number.

```csharp
Rational<int> p = new(3, 5);
Rational<int>.Reciprocate(p); // 5/3
```

### Fields

For rational numbers in Mathematics.NET, `Nan`, `PositiveInfinity`, and `NegativeInfinity` are represented in the following ways:

```csharp
Rational<int>.Nan; // 0/0
Rational<int>.PositiveInfinity; // 1/0
Rational<int>.NegativeInfinity; // -1/0
```
