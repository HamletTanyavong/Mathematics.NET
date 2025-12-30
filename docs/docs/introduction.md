---
sidebar_position: 1
description: Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems.
keywords: [math, C#, csharp, .NET]
---

# Introduction

Welcome to Mathematics.NET.

## Overview

Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems. It contains custom types for complex, real, and rational numbers as well as other objects such as vectors, matrices, and tensors. The library also supports first and second-order, forward and reverse-mode automatic differentiation.

One focus of this library is in the field of differential geometry, where support for operations such as index raising, lowering, and contractions are provided. These features work seemlessly with the autodiff capabilities of the library.

## Get Started

The package can be used in the following environments:

### Package Reference

To use Mathematics.NET in your C# project, add the following line to your project's `.csproj` file:

```xml
<PackageReference Include="Physics.NET.Mathematics" Version="*" />
```

### Polyglot Notebooks

To use Mathematics.NET in a polyglot notebook, add the following line to the first cell:

```
#r "nuget: Physics.NET.Mathematics, *",
```

### File-Based Apps

To use Mathematics.NET in a file-based application, add the following line to the top of the file:

```csharp
#:package Mathematics.NET@*
```

:::note

Use `*` to get the latest stable version of the package or `*-*` to get the latest pre-release version.

:::
