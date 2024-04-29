// <copyright file="MatrixMultiplyByScalarBenchmarks.cs" company="Mathematics.NET">
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

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Helpers;
using Mathematics.NET.Core.ParallelActions;

namespace Mathematics.NET.Benchmarks.LinearAlgebra;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class MatrixMultiplyByScalarBenchmarks
{
    public int Rows { get; set; }
    public int Cols { get; set; }
    public required Complex[,] MatrixOne { get; set; }
    public required Complex[,] MatrixTwo { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Rows = 100;
        Cols = 100;

        MatrixOne = new Complex[Rows, Cols];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                MatrixOne[i, j] = new(Random.Shared.NextDouble(), Random.Shared.NextDouble());
                MatrixTwo[i, j] = new(Random.Shared.NextDouble(), Random.Shared.NextDouble());
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void MultiplyByScalarNaive()
    {
        var matrixAsSpan = MatrixOne.AsMemory2D().Span;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                matrixAsSpan[i, j] *= Real.Pi;
            }
        }
    }

    [Benchmark]
    public void MultiplyByScalarParallel()
    {
        Memory2D<Complex> matrixAsMemory = MatrixTwo;
        ParallelHelper.ForEach(matrixAsMemory, new MultiplyByScalarAction<Complex>(Real.Pi));
    }
}
