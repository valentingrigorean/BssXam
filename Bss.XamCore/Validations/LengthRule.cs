//
// LengthRule.cs
//
// Author:
//       valentingrigorean <valentin.grigorean1@gmail.com>
//
// Copyright (c) 2018 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace Bss.XamCore.Validations
{
    public class LengthRule : IValidationRule<string>
    {
        private readonly int _minSize;
        private readonly int _maxSize;

        public LengthRule(int minSize, int maxSize = -1)
        {
            _minSize = minSize;

            if (maxSize > 0 && maxSize < minSize)
                throw new InvalidOperationException($"{nameof(maxSize)} should be greater then {nameof(minSize)}");
            _maxSize = maxSize;
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            var textSize = value?.Length ?? 0;
            if (_minSize >= 0 && _maxSize >= 0)
                return textSize >= _minSize || textSize <= _maxSize;
            return textSize >= _minSize;
        }
    }
}
