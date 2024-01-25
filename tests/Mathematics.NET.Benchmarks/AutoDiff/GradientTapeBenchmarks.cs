// <copyright file="GradientTapeBenchmarks.cs" company="Mathematics.NET">
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

using Mathematics.NET.AutoDiff;
using Mathematics.NET.Benchmarks.Implementations.AutoDiff;

namespace Mathematics.NET.Benchmarks.AutoDiff;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class GradientTapeBenchmarks
{
    public Real X { get; set; }
    public Real Y { get; set; }
    public Real Z { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        X = 1.23;
        Y = 2.34;
        Z = 3.45;
    }

    [Benchmark(Baseline = true)]
    public Real GradientTapeWithList()
    {
        GradientTape<Real> tape = new();

        var x = tape.CreateVariable(1.23);
        var y = tape.CreateVariable(2.34);
        var z = tape.CreateVariable(3.45);

        _ = tape.Sin(
            tape.Multiply(
                x,
                tape.Ln(
                    tape.Divide(
                        tape.Sqrt(y), z))));

        tape.ReverseAccumulate(out var gradient);
        return gradient[0] + gradient[1] + gradient[2];
    }

    [Benchmark]
    public Real GradientTapeWithLinkedList()
    {
        GradientTapeWithLinkedList<Real> tape = new();

        var x = tape.CreateVariable(1.23);
        var y = tape.CreateVariable(2.34);
        var z = tape.CreateVariable(3.45);

        _ = tape.Sin(
            tape.Multiply(
                x,
                tape.Ln(
                    tape.Divide(
                        tape.Sqrt(y), z))));

        tape.ReverseAccumulate(out var gradient);
        return gradient[0] + gradient[1] + gradient[2];
    }
}
