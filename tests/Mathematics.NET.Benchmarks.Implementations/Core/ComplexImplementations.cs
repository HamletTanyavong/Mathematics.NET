// <copyright file="ComplexImplementations.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023–present Hamlet Tanyavong
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

using System.Runtime.Intrinsics.X86;

namespace Mathematics.NET.Benchmarks.Implementations.Core;

public static class ComplexImplementations
{
    public static Complex MultiplyNaive(Complex left, Complex right)
        => new(left.Re * right.Re - left.Im * right.Im, left.Re * right.Im + right.Re * left.Im);

    public static Complex MultiplySimd(Complex left, Complex right)
    {
        if (Avx.IsSupported)
        {
            var vecL = left.AsVector128();
            var vecR = right.AsVector128();

            var mulStraight = Avx.Multiply(vecL, vecR);
            var mulCross = Avx.Multiply(vecL, Avx.Permute(vecR, 0b00011001));

            return Avx.AddSubtract(Avx.UnpackLow(mulStraight, mulCross), Avx.UnpackHigh(mulStraight, mulCross)).AsComplex();
        }
        else
        {
            return new(left.Re * right.Re - left.Im * right.Im, left.Re * right.Im + right.Re * left.Im);
        }
    }
}
