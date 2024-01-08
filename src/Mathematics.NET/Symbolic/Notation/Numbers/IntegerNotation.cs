// <copyright file="IntegerNotation.cs" company="Mathematics.NET">
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

using Mathematics.NET.Symbolic.Notation.Numbers;

namespace Mathematics.NET.Symbolic.Notation.Numbers
{
    /// <summary>An integer number notation node</summary>
    public class IntegerNotation : NumberNotation
    {
        /// <summary>The value of the integer</summary>
        public string Value;

        public IntegerNotation(string value) : base(NumberKind.Integer)
        {
            Value = value;
        }
    }
}

namespace Mathematics.NET.Symbolic
{
    public static partial class NotationFactory
    {
        /// <summary>Create an <see cref="IntegerNotation"/> instance.</summary>
        /// <param name="value">The value of the integer</param>
        /// <returns>An <see cref="IntegerNotation"/> instance</returns>
        public static IntegerNotation Integer(string value) => new(value);
    }
}
