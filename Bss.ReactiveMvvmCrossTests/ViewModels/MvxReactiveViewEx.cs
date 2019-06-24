//
// MvxReactiveViewModelTest.cs
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

using System.Reactive;
using Bss.ReactiveMvvmCross.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bss.ReactiveMvvmCrossTests.ViewModels
{
    public class MvxReactiveViewEx : MvxReactiveViewModel
    {
        private string _textWithoutFody;
        private int _intWithoutFody;

        public MvxReactiveViewEx()
        {
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(false);
        }

        [Reactive]
        public string TextFody { get; set; }    

        public string TextWithoutFody
        {
            get => _textWithoutFody;
            set => SetProperty(ref _textWithoutFody, value);
        }

        [Reactive]
        public int IntFody { get; set; }

        public int IntWithoutFody
        {
            get => _intWithoutFody;
            set => SetProperty(ref _intWithoutFody, value);
        }

        public override void Prepare(Unit parameter)
        {
        }
    }


    public class ReactiveObjectEx : ReactiveObject
    {
        [Reactive] 
        public string TextFody { get; set; }
    }
}