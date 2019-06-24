//
// ViewExtensions.cs
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
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using AView = Android.Views.View;

namespace Bss.XamDroid.Extensions
{
    public static class ViewExtensions
    {
        public static void HideKeyboard(this AView inputView, bool overrideValidation = false)
        {
            if (inputView == null)
                throw new ArgumentNullException(nameof(inputView) + " must be set before the keyboard can be hidden.");

            using (var inputMethodManager = (InputMethodManager)inputView.Context.GetSystemService(Context.InputMethodService))
            {
                if (!overrideValidation && !(inputView is EditText || inputView is TextView || inputView is SearchView))
                    throw new ArgumentException("inputView should be of type EditText, SearchView, or TextView");

                IBinder windowToken = inputView.WindowToken;
                if (windowToken != null && inputMethodManager != null)
                    inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
            }
        }

        public static void ShowKeyboard(this AView inputView)
        {
            if (inputView == null)
                throw new ArgumentNullException(nameof(inputView) + " must be set before the keyboard can be shown.");

            using (var inputMethodManager = (InputMethodManager)inputView.Context.GetSystemService(Context.InputMethodService))
            {
                if (inputView is EditText || inputView is TextView || inputView is SearchView)
                {
                    if (inputMethodManager != null)
                    {
                        inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
                        inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
                    }
                }
                else
                    throw new ArgumentException("inputView should be of type EditText, SearchView, or TextView");
            }
        }
    }
}
