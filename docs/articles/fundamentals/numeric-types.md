# Numeric Types

There are three numeric types that can be used to represent complex, real, and rational numbers in Mathematics.NET.

All Mathematics.NET numbers implement the [IComplex<T, U>](https://mathematics.hamlettanyavong.com/api/Mathematics.NET.Core.IComplex-2.html) interface. Particularly useful is the fact that, unlike .NET runtime's [INumberBase\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/INumberBase.cs), `IComplex<T, U>` defines the `Conjugate` method; this is incredibly helpful in avoiding code duplication for calculations involving complex and real numbers.

## Floating-Point Types

Floating-point Mathematics.NET numbers may be backed any number that implements [IFloatingPointIee754\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/IFloatingPointIeee754.cs) and [IMinMaxValue\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/IMinMaxValue.cs), or more specifically, [float](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Single.cs), [double](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Double.cs), and [decimal](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Decimal.cs).

### Complex Numbers

To create a complex number, we choose a backing type, in this case `double`, and write
```csharp
Complex<double> z = new(3, 4);
```
This represents the number $ z = 3+i4 $. We can also specify only one number to create a complex number with no imaginary part
```csharp
Complex<double> z = 3;
```
which represents $ z = 3 $.

### Real Numbers

Likewise, to create a real number, write
```csharp
Real<double> z = 1;
```
With real numbers, we can also get maximum and minimum values which will depend on the backing type.
```csharp
Console.WriteLine("Max value with float backing type: {0}", Real<float>.MaxValue);
Console.WriteLine("Max value with double backing type: {0}", Real<double>.MaxValue);
```
This will output `Max value with float backing type: 3.4028235E+38`
and `Max value with double backing type: 1.7976931348623157E+308`.

## Binary Types

Rational numbers are the only Mathematics.NET type in this category.

### Rational Numbers

Rational numbers require two backing types, one that implements [IBinaryInteger\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/IBinaryInteger.cs) and one that implements both [IFloatingPointIee754\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/IFloatingPointIeee754.cs) and [IMinMaxValue\<T\>](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/IMinMaxValue.cs).

With this information, we can create the following rational numbers:
```csharp
Rational<int, double> a = 2;
Rational<byte, float> b = new(2, 3);
Rational<BigInteger, double> c = new(3, 4);
```
which represent $ a = 2 $, $ b = 2/3 $, and $ c = 3/4 $.

The first type parameter indicates that the constructor only accepts values of that type. In these cases, `a` must be an int, `b` must be a byte, and `c` must be a BigInteger. The second parameter indicates the desired floating-point type with which we want to represent the rational number. We can get this value in two ways, e.g.
```csharp
Console.WriteLine(b.Value);
Console.WriteLine((float)b);
```
which will both output `0.6666667`.

> [!CAUTION]
> The floating-point representation of rational numbers may not be accurate in all cases.

We can also convert a floating-point number into a rational number with an explicit cast
```csharp
Console.WriteLine((Rational<int, double>)3.14);
```
> [!NOTE]
> The conversion conversion is not guaranteed to create the "best" fraction; for instance, the value $ 0.3333333333333333 $ will not produce $ 1/3 $ but instead produce $ 8333333333333331 / 25000000000000000 $.

Be aware that there are performance penalties with converting rationals to and from real numbers. An overflow exception will also be thrown if a value being converted cannot be represented by the target type.
