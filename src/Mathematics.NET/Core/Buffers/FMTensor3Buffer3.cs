// <copyright file="FMTensor3Buffer3.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0051

using System.Runtime.CompilerServices;
using Mathematics.NET.AutoDiff;
using Mathematics.NET.DifferentialGeometry;
using Mathematics.NET.DifferentialGeometry.Abstractions;

namespace Mathematics.NET.Core.Buffers;

/// <summary>Represents a buffer of 3 AutoDiffTensor3 delegates.</summary>
/// <typeparam name="TDN">A type that implements <see cref="IDual{TDN, TN}"/>.</typeparam>
/// <typeparam name="TN">A type that implements <see cref="IComplex{T}"/> and <see cref="IDifferentiableFunctions{T}"/>.</typeparam>
/// <typeparam name="TPI">The index of the point on the manifold.</typeparam>
[InlineArray(3)]
internal struct FMTensor3Buffer3<TDN, TN, TPI>
    where TDN : IDual<TDN, TN>
    where TN : IComplex<TN>, IDifferentiableFunctions<TN>
    where TPI : IIndex
{
    private Func<AutoDiffTensor3<TDN, TN, TPI>, TDN> _element0;
}
