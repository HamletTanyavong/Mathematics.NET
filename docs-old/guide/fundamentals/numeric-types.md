# Numeric Types

There are three numeric types that can be used to represent complex, real, and rational numbers in Mathematics.NET.

All Mathematics.NET numbers implement the <xref href="Mathematics.NET.Core.IComplex`1"/> interface. Particularly useful is the fact that, unlike .NET runtime's [INumberBase\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/INumberBase.cs), `IComplex<T>` defines the [Conjugate](xref:Mathematics.NET.Core.IComplex`1.Conjugate*) method; this is helpful in avoiding code duplication for calculations involving complex and real numbers.

## Floating-Point Types

Floating-point Mathematics.NET numbers are backed by the @System.Double numeric type.

### Complex Numbers

The type that represents complex numbers in this package shares the same name with the one defined in @System.Numerics.Complex. Therefore, if we wish to use this library with `System.Numerics`, we need to create an alias to resolve the ambigious reference. Adding the line `using Complex = Mathematics.NET.Core.Complex;` should suffice.

To create a complex number, we can write
```csharp
Complex z = new(3, 4);
```
This represents the number $ z = 3+i4 $. We can also specify only one number to create a complex number with no imaginary part
```csharp
Complex z = 3;
```
which represents $ z = 3 $. To display only the real component of a complex number, use the format `RE`:
```csharp
Complex z = new(1, 2);
Console.WriteLine("{0:RE}", z);
Console.WriteLine(z.ToString("RE", null));
```
To display only the imaginary component of a complex number, use the format `IM` instead.

### Real Numbers

Likewise, to create a real number, write
```csharp
Real x = 1.23;
```
To get is backing value, write
```csharp
double backingValue = x.Value;
```

## Binary Types

Rational numbers are the only Mathematics.NET type in this category.

### Rational Numbers

Rational numbers require a type parameter constrained by both <xref href="System.Numerics.IBinaryInteger`1" /> and <xref href="System.Numerics.ISignedNumber`1" />. The type specified here is used to represent the numerator and denominator of the rational number.

With this information, we can create the following rational numbers:
```csharp
Rational<int> a = 2;
Rational<sbyte> b = new(2, 3);
Rational<BigInteger> c = new(3, 4);
```
which represent $ a = 2 $, $ b = 2/3 $, and $ c = 3/4 $.

> [!CAUTION]
> The floating-point representation of rational numbers may not be accurate in all cases.

We can also convert a double into a rational number with two different methods that do slightly different things:
```csharp
Console.WriteLine(Rational<int>.FromDouble(3.14));
```
which returns an exact rational representation of the number, `(157, 50)`, and
```csharp
var success = Rational<int>.TryConvertFromDouble(0.667, 0.01, 4, out var result);
Console.WriteLine(result);
```
which attempts to find the closest rational representation of the value within a certain threshold and with a maximum denominator size. The result here is `(2 / 3)`. To display only the numerator or denominator of a rational number, use the formats `N` and `D`, respectively.

> [!NOTE]
> The conversion conversion is not guaranteed to create the "best" fraction; for instance, the value $ 0.3333333333333333 $ will not always produce $ 1/3 $ but instead produce $ 8333333333333331 / 25000000000000000 $.

Be aware that there are performance penalties with converting rationals to and from real numbers. An <xref href="System.OverflowException" /> will also be thrown if a value being converted cannot be represented by the target type.
