// <copyright file="ComplexDivisionBenchmarks.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Benchmarks.Core.ComplexNumberBenchmarks;

[MemoryDiagnoser]
public class ComplexDivisionBenchmarks
{
    public ComplexNumber Z { get; set; }
    public ComplexNumber W { get; set; }

    public Complex X { get; set; }
    public Complex Y { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Z = new(1.23, 2.34);
        W = new(4.56, 3.45);

        X = new(1.23, 2.34);
        Y = new(4.56, 3.45);
    }

    [Benchmark(Baseline = true)]
    public Complex SystemDivision()
    {
        return X / Y;
    }

    [Benchmark]
    public ComplexNumber ComplexDivision_WithAggressiveInlining()
    {
        return Z / W;
    }
}
