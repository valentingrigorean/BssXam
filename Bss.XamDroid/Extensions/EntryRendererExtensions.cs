//
// EntryRendererExtensions.cs
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
using Android.Views.InputMethods;
using Xamarin.Forms;

namespace Bss.XamDroid.Extensions
{
    public static class EntryRendererExtensions
    {

        public static ImeAction ToAndroidImeAction(this ReturnType returnType)
        {
            switch (returnType)
            {
                case ReturnType.Go:
                    return ImeAction.Go;
                case ReturnType.Next:
                    return ImeAction.Next;
                case ReturnType.Send:
                    return ImeAction.Send;
                case ReturnType.Search:
                    return ImeAction.Search;
                case ReturnType.Done:
                    return ImeAction.Done;
                case ReturnType.Default:
                    return ImeAction.Done;
                default:
                    throw new System.NotImplementedException($"ReturnType {returnType} not supported");
            }
        }

        public static ImeAction ToAndroidImeOptions(this Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags flags)
        {
            switch (flags)
            {
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Previous:
                    return ImeAction.Previous;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Next:
                    return ImeAction.Next;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Search:
                    return ImeAction.Search;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Send:
                    return ImeAction.Send;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Go:
                    return ImeAction.Go;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.None:
                    return ImeAction.None;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.ImeMaskAction:
                    return ImeAction.ImeMaskAction;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.NoPersonalizedLearning:
                    return (ImeAction)ImeFlags.NoPersonalizedLearning;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.NoExtractUi:
                    return (ImeAction)ImeFlags.NoExtractUi;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.NoAccessoryAction:
                    return (ImeAction)ImeFlags.NoAccessoryAction;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.NoFullscreen:
                    return (ImeAction)ImeFlags.NoFullscreen;
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Default:
                case Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ImeFlags.Done:
                default:
                    return ImeAction.Done;
            }
        }
    }
}
