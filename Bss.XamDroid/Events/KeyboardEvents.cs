//
// KeyboardEvents.cs
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
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using ReactiveUI;
using Android.Runtime;

namespace Bss.XamDroid.Events
{
    public static class KeyboardEvents
    {
        private static readonly int DefaultKeyboardHeightDP = 100;
        private static readonly int EstimatedKeyboardDP = DefaultKeyboardHeightDP + (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop ? 48 : 0);

        private const string VisibilityContract = "android_keyboard_visibility";

        public static IObservable<bool> OnKeyboardVisiblity()
        {
            return MessageBus.Current.Listen<bool>(VisibilityContract);
        }

        public static void OnConfigurationChanged(Configuration newConfig)
        {
            if (newConfig.HardKeyboardHidden == HardKeyboardHidden.No)
                SendKeyboardVisibility(false);
            else if (newConfig.HardKeyboardHidden == HardKeyboardHidden.Yes)
                SendKeyboardVisibility(true);
        }

        public static IDisposable OnGlobalLayoutListener(Activity activity)
        {
            var parentView = activity.FindViewById(Android.Resource.Id.Content);
            return new GlobalLayoutListenerHelper(parentView);
        }

        public static IDisposable OnGlobalLayoutListener(View view)
        {
            return new GlobalLayoutListenerHelper(view);
        }

        public static int GetKeyboardSize(DisplayMetrics displayMetrics)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, EstimatedKeyboardDP, displayMetrics);
        }

        private static void SendKeyboardVisibility(bool isVisibile)
        {
            MessageBus.Current.SendMessage(isVisibile, VisibilityContract);
        }

        private class GlobalLayoutListenerHelper : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private readonly int _keyboardSize;

            private readonly Rect _rect = new Rect();
            private readonly View _view;

            private bool _alreadyOpen;

            private bool _isDisposed;

            public GlobalLayoutListenerHelper(View view)
            {
                _keyboardSize = GetKeyboardSize(view.Resources.DisplayMetrics);
                _view = view;
                var vto = view.ViewTreeObserver;
                vto.AddOnGlobalLayoutListener(this);
            }

            public GlobalLayoutListenerHelper(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
            {
                _isDisposed = true;
            }

            protected override void Dispose(bool disposing)
            {
                _isDisposed = true;
                var vto = _view.ViewTreeObserver;
                vto.RemoveOnGlobalLayoutListener(this);
                base.Dispose(disposing);
            }

            public void OnGlobalLayout()
            {
                if (_isDisposed)
                    return;
                _view.GetWindowVisibleDisplayFrame(_rect);
                var heightDiff = _view.RootView.Height - (_rect.Bottom - _rect.Top);
                var isShown = heightDiff >= _keyboardSize;

                if (isShown == _alreadyOpen)
                    return;

                _alreadyOpen = isShown;
                SendKeyboardVisibility(isShown);
            }
        }
    }
}
