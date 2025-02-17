// <copyright file="ComplexTrigonometryBenchmarks.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Benchmarks.Core.ComplexNumberBenchmarks;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ComplexTrigonometryBenchmarks
{
    public Complex Z { get; set; }
    public Complex ImOverTwo { get; set; }

    public SystemComplex W { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Z = new(1.23, 2.34);
        ImOverTwo = Mathematics.Im / 2.0;

        W = new(1.23, 2.34);
    }

    [Benchmark(Baseline = true)]
    public SystemComplex Atan_System()
    {
        return SystemComplex.Atan(W);
    }

    [Benchmark]
    public Complex Atan_MathNET()
    {
        return Complex.Atan(Z);
    }

    //[Benchmark]
    public Complex Atan_WithoutConstImOverTwo()
    {
        return Mathematics.Im / 2.0 * Complex.Ln((Mathematics.Im + Z) / (Mathematics.Im - Z));
    }

    //[Benchmark]
    public Complex Atan_WithConstImOverTwo()
    {
        return ImOverTwo * Complex.Ln((Mathematics.Im + Z) / (Mathematics.Im - Z));
    }
}
