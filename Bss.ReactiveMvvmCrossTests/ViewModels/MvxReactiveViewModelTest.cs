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

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;
using Xunit;
using ReactiveUI.Legacy;

namespace Bss.ReactiveMvvmCrossTests.ViewModels
{
    public class MvxReactiveViewModelTest
    {
        [Fact]
        public void NotifyPropertyChangedTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = new List<string>();

            obj.PropertyChanged += (sender, e) => list.Add(e.PropertyName);

            Assert.True(list.Count == 0, "Should be 0");

            obj.TextFody = "abc";

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextFody = "";

            Assert.True(list.Count == 2, "Should be 2");
        }

        [Fact]
        public void NotifyPropertyChangedWithoutFodyTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = new List<string>();

            obj.PropertyChanged += (sender, e) => { list.Add(e.PropertyName); };

            Assert.True(list.Count == 0, "Should be 0");

            obj.TextWithoutFody = "abc";

            Task.Delay(200).Wait();

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextWithoutFody = "";

            Assert.True(list.Count == 2, "Should be 2");
        }

        [Fact]
        public void SupressNotificationTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = obj.WhenAnyValue(vm => vm.TextWithoutFody)
                          .CreateCollection(ImmediateScheduler.Instance);

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextWithoutFody = "abc";

            using (obj.SuppressChangeNotifications())
            {
                obj.TextWithoutFody = "";
            }

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextWithoutFody = "abc";

            Assert.True(list.Count == 2, "Should be 1");
        }

        [Fact]
        public void WhenAnyValueTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = obj.WhenAnyValue(vm => vm.TextFody)
                .CreateCollection(ImmediateScheduler.Instance);

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextFody = "abc";

            Assert.True(list.Count == 2, "Should be 2");

            obj.TextFody = "";

            Assert.True(list.Count == 3, "Should be 3");
        }

        [Fact]
        public void WhenAnyValueWithoutFodyTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = obj.WhenAnyValue(vm => vm.TextWithoutFody)
                .CreateCollection(ImmediateScheduler.Instance);

            Assert.True(list.Count == 1, "Should be 1");

            obj.TextWithoutFody = "abc";

            Assert.True(list.Count == 2, "Should be 2");

            obj.TextWithoutFody = "";

            Assert.True(list.Count == 3, "Should be 3");
        }

        [Fact]
        public void WhenAnyValueIntTest()
        {
            var obj = new MvxReactiveViewEx();

            var list = obj.WhenAnyValue(vm => vm.IntFody)
                .CreateCollection(ImmediateScheduler.Instance);

            Assert.True(list.Count == 1, "Should be 1");

            obj.IntFody = 1;

            Assert.True(list.Count == 2, "Should be 2");

            obj.IntFody = 2;

            Assert.True(list.Count == 3, "Should be 3");
        }
    }
}