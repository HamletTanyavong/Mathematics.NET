// <copyright file="RK4IntegrationBenchmarks.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023-present Hamlet Tanyavong
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

using Mathematics.NET.Solvers;

namespace Mathematics.NET.Benchmarks.Solvers;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class RK4IntegrationBenchmarks
{
    public SystemState<Real> SystemStateA;
    public SystemState<Real> SystemStateB;
    public RungeKutta4<Real> RK4 = new((t, x) => Real.Pow(Real.Sin(t), 2) * x);

    [GlobalSetup]
    public void GlobalSetup()
    {
        var arrayA = new Real[100_000];
        var arrayB = new Real[100_000];

        Array.Fill(arrayA, 2);
        Array.Fill(arrayB, 2);

        Memory<Real> memoryA = arrayA;
        Memory<Real> memoryB = arrayB;

        SystemStateA = new(memoryA, 0);
        SystemStateB = new(memoryB, 0);
    }

    [Benchmark(Baseline = true)]
    public void Integrate()
    {
        for (int i = 0; i < 10; i++)
        {
            RK4.Integrate(SystemStateA, 0.01);
        }
    }

    [Benchmark]
    public void IntegrateParallel()
    {
        for (int i = 0; i < 10; i++)
        {
            RK4.IntegrateParallel(SystemStateB, 0.01);
        }
    }
}
