#:package Mathematics.NET
#:package Microsoft.Extensions.Logging

#pragma warning disable EXP0001

using CommunityToolkit.HighPerformance;
using Mathematics.NET.Core;
using Mathematics.NET.GPU.OpenCL;
using Mathematics.NET.LinearAlgebra;
using Microsoft.Extensions.Logging;

MatMul.Compute();

// [[(14, 42),  (14, 54)   ]
//  [(56, 78),  (68, 110)  ]]

public static class MatMul
{
    public static void Compute()
    {
        var logger = CreateLogger();

        // Change the vendor and name to those being used on the machine.
        using var openCL = new OpenCLService(logger, OpenCLService.Vendors.NVIDIA, x => x.Name == "NVIDIA GeForce RTX 2080 Ti");

        var matA = GetMatA();
        var matB = GetMatB();

        var result = openCL.CompMatMul(openCL.Devices[0], new(2, 2), new(2, 2), matA, matB);
        Console.WriteLine(result.ToDisplayString());
    }

    private static ILogger<OpenCLService> CreateLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        return loggerFactory.CreateLogger<OpenCLService>();
    }

    private static Span2D<Complex> GetMatA()
    {
        Span2D<Complex> mat = new Complex[2, 4];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mat[i, j] = new(4 * i + j, i + j);
            }
        }
        return mat;
    }

    private static Span2D<Complex> GetMatB()
    {
        Span2D<Complex> mat = new Complex[4, 2];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                mat[i, j] = new(2 * i + j, i + j);
            }
        }
        return mat;
    }
}
