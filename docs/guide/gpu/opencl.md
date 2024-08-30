# OpenCL

OpenCL via Mathematics.NET is available on Windows and Linux machines with compatible GPUs. Please note that support is limited, and its features are subject to change drastically until an official release of the package has been made.

## Matrix Multiplication Example

Here is an example of matrix multiplication on an [NVIDIA GeForce RTX 2080 Ti](https://compubench.com/device.jsp?benchmark=compu20d&os=Windows&api=cl&D=NVIDIA+GeForce+RTX+2080+Ti&testgroup=info). More methods will be added in the future that will allow users to get more information about their GPUs.

### Setup

One way to set up OpenCL for Mathematics.NET is with the following:
```csharp
using CommunityToolkit.HighPerformance;
using Mathematics.NET.Core;
using Mathematics.NET.GPU;
using Mathematics.NET.GPU.OpenCL;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

// Configure the service.
builder.Services.AddSingleton<IComputeService, OpenCLService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<OpenCLService>>();
    return new OpenCLService(
        logger,
        OpenCLService.Vendors.NVIDIA,
        x => x.Name == "NVIDIA GeForce RTX 2080 Ti");
});

// Add logging filters.
#if DEBUG
builder.Logging.AddFilter(logLevel => logLevel >= LogLevel.Debug);
#elif RELEASE
builder.Logging.AddFilter(logLevel => logLevel >= LogLevel.Warning);
#endif

// Build the application.
var app = builder.Build();
```
We can also instantiate [OpenCLService](xref:Mathematics.NET.GPU.OpenCL.OpenCLService) directly, but for this example, we will use the former. To get the service, use
```csharp
// Get the OpenCL service.
using var openCL = app.Services.GetRequiredService<IComputeService>();
```

### Matrix Multiplication Without Padding
Suppose we want to compute $ M=AB $, where
$$
A = \begin{pmatrix}
0  & 1  & 2  & 3  & 4  & 5  & 6  & 7  & 8  & 9  & 10 & 11 & 12 & 13 & 14 & 15 \\
16 & 17 & 18 & 19 & 20 & 21 & 22 & 23 & 24 & 25 & 26 & 27 & 28 & 29 & 30 & 31 \\
32 & 33 & 34 & 35 & 36 & 37 & 38 & 39 & 40 & 41 & 42 & 43 & 44 & 45 & 46 & 47 \\
48 & 49 & 50 & 51 & 52 & 53 & 54 & 55 & 56 & 57 & 58 & 59 & 60 & 61 & 62 & 63
\end{pmatrix}
$$
and
$$
B = \begin{pmatrix}
0   & 1   & 2   & 3   & 4   & 5   & 6   & 7   \\
8   & 9   & 10  & 11  & 12  & 13  & 14  & 15  \\
16  & 17  & 18  & 19  & 20  & 21  & 22  & 23  \\
24  & 25  & 26  & 27  & 28  & 29  & 30  & 31  \\
32  & 33  & 34  & 35  & 36  & 37  & 38  & 39  \\
40  & 41  & 42  & 43  & 44  & 45  & 46  & 47  \\
48  & 49  & 50  & 51  & 52  & 53  & 54  & 55  \\
56  & 57  & 58  & 59  & 60  & 61  & 62  & 63  \\
64  & 65  & 66  & 67  & 68  & 69  & 70  & 71  \\
72  & 73  & 74  & 75  & 76  & 77  & 78  & 79  \\
80  & 81  & 82  & 83  & 84  & 85  & 86  & 87  \\
88  & 89  & 90  & 91  & 92  & 93  & 94  & 95  \\
96  & 97  & 98  & 99  & 100 & 101 & 102 & 103 \\
104 & 105 & 106 & 107 & 108 & 109 & 110 & 111 \\
112 & 113 & 114 & 115 & 116 & 117 & 118 & 119 \\
120 & 121 & 122 & 123 & 124 & 125 & 126 & 127
\end{pmatrix}
$$
We can use the method [MatMul](xref:Mathematics.NET.GPU.OpenCL.OpenCLService.MatMul*) to perform matrix multiplication on the GPU.
```csharp
// Create and fill the left matrix.
Span2D<Real> matA = new Real[4, 16];
for (int i = 0; i < 4; i++)
{
    for (int j = 0; j < 16; j++)
    {
        matA[i, j] = 16 * i + j;
    }
}

// Create and fill the right matrix.
Span2D<Real> matB = new Real[16, 8];
for (int i = 0; i < 16; i++)
{
    for (int j = 0; j < 8; j++)
    {
        matB[i, j] = 8 * i + j;
    }
}

// Perform the computation on a chosen device with global and local work sizes of { 4, 8 }.
var result = openCL.MatMul(openCL.Devices[0], new(4, 8), new(4, 8), matA, matB);
Console.WriteLine(result.ToDisplayString());
```
This gives us the following result:
$$
M = \begin{pmatrix}
9920  & 10040 & 10160 & 10280 & 10400 & 10520 & 10640 & 10760 \\
25280 & 25656 & 26032 & 26408 & 26784 & 27160 & 27536 & 27912 \\
40640 & 41272 & 41904 & 42536 & 43168 & 43800 & 44432 & 45064 \\
56000 & 56888 & 57776 & 58664 & 59552 & 60440 & 61328 & 62216
\end{pmatrix}
$$

### Matrix Multiplication with Padding

Sometimes, the matrices we want to multiply my not have ideal dimensions; more specifically, the dimensions of the matrices are such that they cannot be evenly divided by our local work size. In such a case, we have to pad our matrices to get them to the proper dimensions. Now, suppose
$$
A = \begin{pmatrix}
0  & 1  & 2  & 3  & 4  & 5  & 6  \\
7  & 8  & 9  & 10 & 11 & 12 & 13 \\
14 & 15 & 16 & 17 & 18 & 19 & 20
\end{pmatrix}
$$
and
$$
B = \begin{pmatrix}
0  & 1  & 2  & 3  & 4  \\
5  & 6  & 7  & 8  & 9  \\
10 & 11 & 12 & 13 & 14 \\
15 & 16 & 17 & 18 & 19 \\
20 & 21 & 22 & 23 & 24 \\
25 & 26 & 27 & 28 & 29 \\
30 & 31 & 32 & 33 & 34
\end{pmatrix}
$$
We can use [Pad](xref:Mathematics.NET.LinearAlgebra.LinAlgExtensions.Pad``1(CommunityToolkit.HighPerformance.Span2D{``0},System.Int32,System.Int32)) to add zeros to the right and bottom of each of these matrices so that $ A $ and $ B $ have dimensions `4 x 8` and `8 x 8`, respectively.
```csharp
var pMatA = matA.Pad(4, 8);
var pMatB = matB.Pad(8, 8);
```
Then using the same method above, we can perform matrix multiplication with the two matrices.
```csharp
var result = openCL.MatMul(openCL.Devices[0], new(4, 8), new(4, 4), pMatA, pMatB);
Console.WriteLine(result.Slice(0, 0, 3, 5).ToDisplayString());
```
Here, we chose a local work size of `{ 4, 4 }`, which is a multiple of our global work size.

> [!NOTE]
> For performance reasons, matrices should only be padded once. When finished, use [Slice](xref:System.Span`1.Slice*), or the 2D version from [CommunityToolkit](https://learn.microsoft.com/en-us/dotnet/api/microsoft.toolkit.highperformance.span2d-1.slice), to get the unpadded result.

With that, we now have our result:
$$
M = \begin{pmatrix}
455  & 476  & 497  & 518  & 539  \\
1190 & 1260 & 1330 & 1400 & 1470 \\
1925 & 2044 & 2163 & 2282 & 2401
\end{pmatrix}
$$
