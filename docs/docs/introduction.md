---
sidebar_position: 1
description: Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems.
keywords: math, C#, csharp, .NET
---

# Introduction

Welcome to Mathematics.NET.

## Overview

Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems. It contains custom types for complex, real, and rational numbers as well as other objects such as vectors, matrices, and tensors. The library also supports first and second-order, forward and reverse-mode automatic differentiation.

One focus of this library is in the field of differential geometry, where support for operations such as index raising, lowering, and contractions are provided. These features work seemlessly with the autodiff capabilities of the library.

## Get Started

### Package Reference

To use Mathematics.NET in your C# project, add the following line to your project's `.csproj` file:

```xml
<PackageReference Include="Physics.NET.Mathematics" Version="0.2.0-alpha.12" />
```

### Polyglot Notebooks

To use Mathematics.NET in a polyglot notebook, add the following line to the first cell:

```
#r "nuget: Physics.NET.Mathematics"
```
