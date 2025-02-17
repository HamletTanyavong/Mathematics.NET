// <copyright file="RealvsDouble.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Benchmarks.Core.RealNumberBenchmarks;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class RealvsDouble
{
    public Real XReal { get; set; }
    public double XDouble { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        XReal = Math.PI;
        XDouble = Math.PI;
    }

    [Benchmark(Baseline = true)]
    public double AdditionsWithDouble()
    {
        double result = 0;
        for (int i = 0; i < 100_000; i++)
        {
            result += XDouble;
        }
        return result;
    }

    [Benchmark]
    public Real AdditionsWithReal()
    {
        Real result = 0.0;
        for (int i = 0; i < 100_000; i++)
        {
            result += XReal;
        }
        return result;
    }
}
