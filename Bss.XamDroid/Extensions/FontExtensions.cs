//
// FontExtensions.cs
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Application = Android.App.Application;

namespace Bss.XamDroid.Extensions
{
    public static class FontExtensions
    {
        private static readonly Dictionary<Tuple<string, FontAttributes>, Typeface> Typefaces = new Dictionary<Tuple<string, FontAttributes>, Typeface>();

        private static Typeface s_defaultTypeface = Typeface.Default;

        // We don't create and cache a Regex object here because we may not ever need it, and creating Regexes is surprisingly expensive (especially on older hardware)
        // Instead, we'll use the static Regex.IsMatch below, which will create and cache the regex internally as needed. It's the equivalent of Lazy<Regex> with less code.
        // See https://msdn.microsoft.com/en-us/library/sdx2bds0(v=vs.110).aspx#Anchor_2
        const string LoadFromAssetsRegex = @"\w+\.((ttf)|(otf))\#\w*";


        public static Typeface ToTypeface(this IFontElement self)
        {
            if (self.IsDefault())
                return s_defaultTypeface ?? (s_defaultTypeface = Typeface.Default);

            var key = new Tuple<string, FontAttributes>(self.FontFamily, self.FontAttributes);
            Typeface result;
            if (Typefaces.TryGetValue(key, out result))
                return result;

            if (self.FontFamily == null)
            {
                var style = ToTypefaceStyle(self.FontAttributes);
                result = Typeface.Create(Typeface.Default, style);
            }
            else if (Regex.IsMatch(self.FontFamily, LoadFromAssetsRegex))
            {
                result = Typeface.CreateFromAsset(Application.Context.Assets, FontNameToFontFile(self.FontFamily));
            }
            else
            {
                var style = ToTypefaceStyle(self.FontAttributes);
                result = Typeface.Create(self.FontFamily, style);
            }
            return (Typefaces[key] = result);
        }

        public static TypefaceStyle ToTypefaceStyle(FontAttributes attrs)
        {
            var style = TypefaceStyle.Normal;
            if ((attrs & (FontAttributes.Bold | FontAttributes.Italic)) == (FontAttributes.Bold | FontAttributes.Italic))
                style = TypefaceStyle.BoldItalic;
            else if ((attrs & FontAttributes.Bold) != 0)
                style = TypefaceStyle.Bold;
            else if ((attrs & FontAttributes.Italic) != 0)
                style = TypefaceStyle.Italic;
            return style;
        }

        internal static bool IsDefault(this IFontElement self)
        {
            return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
        }

        private static string FontNameToFontFile(string fontFamily)
        {
            int hashtagIndex = fontFamily.IndexOf('#');
            if (hashtagIndex >= 0)
                return fontFamily.Substring(0, hashtagIndex);

            throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
        }
    }
}
