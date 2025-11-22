// <copyright file="ContractionBenchmarks.cs" company="Mathematics.NET">
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

using Mathematics.NET.Benchmarks.Implementations.DifferentialGeometry;
using Mathematics.NET.Benchmarks.Implementations.DifferentialGeometry.Symbols;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.LinearAlgebra;

namespace Mathematics.NET.Benchmarks.DifferentialGeometry;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ContractionBenchmarks
{
    public Tensor<Vector4<Real>, Real, Index<Lower, Alpha>> RankOneTensorA;
    public Tensor<Vector4<Real>, Real, Index<Upper, Alpha>> RankOneTensorB;

    public Tensor<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Beta>, Index<Lower, Gamma>> RankThreeTensorA;
    public Tensor<Array4x4x4<Real>, Real, Index<Lower, Delta>, Index<Upper, Beta>, Index<Lower, Epsilon>> RankThreeTensorB;

    [GlobalSetup]
    public void GlobalSetup()
    {
        RankOneTensorA = new Vector4<Real>(1.23, 2.34, 3.45, 4.56);
        RankOneTensorB = new Vector4<Real>(0.44, -4.5, 2.13, -0.2);

        Tensor<Array4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Beta>, Index<Lower, Gamma>> tensorA = new();
        Tensor<Array4x4x4<Real>, Real, Index<Lower, Delta>, Index<Upper, Beta>, Index<Lower, Epsilon>> tensorB = new();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    tensorA[i, j, k] = 16 * i + 4 * j + k;
                    tensorB[i, j, k] = i + j + k;
                }
            }
        }

        RankThreeTensorA = tensorA;
        RankThreeTensorB = tensorB;
    }

    [Benchmark]
    public Real ContractRankOneTensorsWithNoInKeyword()
    {
        return DifGeoImpl.ContractRankOneTensorWithNoInKeyword(RankOneTensorA, RankOneTensorB);
    }

    [Benchmark]
    public Real ContractRankOneTensorsWithInKeyword()
    {
        return DifGeoImpl.ContractRankOneTensorWithInKeyword(RankOneTensorA, RankOneTensorB);
    }

    [Benchmark]
    public Tensor<Array4x4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Gamma>, Index<Lower, Delta>, Index<Lower, Epsilon>> ContractRankThreeTensorsWithNoInKeyword()
    {
        return DifGeoImpl.ContractRankThreeTensorWithNoInKeyword(RankThreeTensorA, RankThreeTensorB);
    }

    [Benchmark]
    public Tensor<Array4x4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Gamma>, Index<Lower, Delta>, Index<Lower, Epsilon>> ContractRankThreeTensorsWithInKeyword()
    {
        return DifGeoImpl.ContractRankThreeTensorWithInKeyword(RankThreeTensorA, RankThreeTensorB);
    }

    [Benchmark]
    public Tensor<Array4x4x4x4<Real>, Real, Index<Lower, Alpha>, Index<Lower, Gamma>, Index<Lower, Delta>, Index<Lower, Epsilon>> ContractRankThreeTensorsWithRefReadonlyKeyword()
    {
        return DifGeoImpl.ContractRankThreeTensorWithRefReadonlyKeyword(in RankThreeTensorA, in RankThreeTensorB);
    }
}
