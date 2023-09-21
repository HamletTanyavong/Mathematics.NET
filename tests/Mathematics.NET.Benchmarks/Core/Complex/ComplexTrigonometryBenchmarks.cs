// <copyright file="ComplexTrigonometryBenchmarks.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Benchmarks.Core.Complex;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ComplexTrigonometryBenchmarks
{
    public Complex<double> Z { get; set; }
    public Complex<double> ImOverTwo { get; set; }

    public System.Numerics.Complex W { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Z = new(1.23, 2.34);
        ImOverTwo = Math<double>.Im / Complex<double>.Two;

        W = new(1.23, 2.34);
    }

    [Benchmark(Baseline = true)]
    public System.Numerics.Complex Atan_System()
    {
        return System.Numerics.Complex.Atan(W);
    }

    [Benchmark]
    public Complex<double> Atan_MathNET()
    {
        return Complex<double>.Atan(Z);
    }

    //[Benchmark]
    public Complex<double> Atan_WithoutConstImOverTwo()
    {
        return Math<double>.Im / Complex<double>.Two * Complex<double>.Ln((Math<double>.Im + Z) / (Math<double>.Im - Z));
    }

    //[Benchmark]
    public Complex<double> Atan_WithConstImOverTwo()
    {
        return ImOverTwo * Complex<double>.Ln((Math<double>.Im + Z) / (Math<double>.Im - Z));
    }
}
