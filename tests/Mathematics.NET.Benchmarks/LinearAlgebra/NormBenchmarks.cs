// <copyright file="NormBenchmarks.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023 Hamlet Tanyavong
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>

using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Benchmarks.LinearAlgebra;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class NormBenchmarks
{
    public Vector4<Real> Vector { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Vector = new(1.23, 2.34, 3.45, 4.56);
    }

    [Benchmark(Baseline = true)]
    public Real NormWithoutLoop()
    {
        Span<Real> components = stackalloc Real[4];

        components[0] = (Real.Conjugate(Vector.X1) * Vector.X1).Re;
        components[1] = (Real.Conjugate(Vector.X2) * Vector.X2).Re;
        components[2] = (Real.Conjugate(Vector.X3) * Vector.X3).Re;
        components[3] = (Real.Conjugate(Vector.X4) * Vector.X4).Re;

        Real max = components[0];
        for (int i = 1; i < 4; i++)
        {
            if (components[i] > max)
            {
                max = components[i];
            }
        }

        Real partialSum = Real.Zero;
        var maxSquared = max * max;
        partialSum += components[0] / maxSquared;
        partialSum += components[1] / maxSquared;
        partialSum += components[2] / maxSquared;
        partialSum += components[3] / maxSquared;

        return max * Real.Sqrt(partialSum);
    }

    [Benchmark]
    public Real NormWithLoop()
    {
        Span<Real> components = stackalloc Real[4];

        components[0] = (Real.Conjugate(Vector.X1) * Vector.X1).Re;
        components[1] = (Real.Conjugate(Vector.X2) * Vector.X2).Re;
        components[2] = (Real.Conjugate(Vector.X3) * Vector.X3).Re;
        components[3] = (Real.Conjugate(Vector.X4) * Vector.X4).Re;

        Real max = components[0];
        for (int i = 1; i < 4; i++)
        {
            if (components[i] > max)
            {
                max = components[i];
            }
        }

        Real partialSum = Real.Zero;
        var maxSquared = max * max;
        for (int i = 0; i < 4; i++)
        {
            partialSum += components[i] / maxSquared;
        }

        return max * Real.Sqrt(partialSum);
    }
}
