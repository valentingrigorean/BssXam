//
// TextColorSwitcher.cs
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
using Android.Content.Res;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Bss.XamDroid
{
    public class TextColorSwitcher
    {
        private static readonly int[][] s_colorStates = { new[] { Android.Resource.Attribute.StateEnabled }, new[] { -Android.Resource.Attribute.StateEnabled } };

        private readonly ColorStateList _defaultTextColors;
        private readonly bool _useLegacyColorManagement;
        private Color _currentTextColor;

        public TextColorSwitcher(ColorStateList textColors, bool useLegacyColorManagement = true)
        {
            _defaultTextColors = textColors;
            _useLegacyColorManagement = useLegacyColorManagement;
        }

        public void UpdateTextColor(TextView control, Color color, Action<ColorStateList> setColor = null)
        {
            if (color == _currentTextColor)
                return;

            if (setColor == null)
            {
                setColor = control.SetTextColor;
            }

            _currentTextColor = color;

            if (color.IsDefault)
            {
                setColor(_defaultTextColors);
            }
            else
            {
                if (_useLegacyColorManagement)
                {
                    // Set the new enabled state color, preserving the default disabled state color
                    int defaultDisabledColor = _defaultTextColors.GetColorForState(s_colorStates[1], color.ToAndroid());
                    setColor(new ColorStateList(s_colorStates, new[] { color.ToAndroid().ToArgb(), defaultDisabledColor }));
                }
                else
                {
                    var acolor = color.ToAndroid().ToArgb();
                    setColor(new ColorStateList(s_colorStates, new[] { acolor, acolor }));
                }
            }
        }
    }
}
