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
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Bss.XamiOS.Extensions
{
    public static class FontExtensions
    {
        private static readonly string DefaultFontName = UIFont.SystemFontOfSize(12).Name;

        public static UIFont ToUIFont(this IFontElement self)
        {
            var family = self.FontFamily;
            var attributes = self.FontAttributes;
            var size = (float)self.FontSize;

            var bold = (attributes & FontAttributes.Bold) != 0;
            var italic = (attributes & FontAttributes.Italic) != 0;

            if (family != null && family != DefaultFontName)
            {
                try
                {
                    UIFont result;
                    if (UIFont.FamilyNames.Contains(family))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(family);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            if (italic)
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;

                            descriptor = descriptor.CreateWithTraits(traits);
                            result = UIFont.FromDescriptor(descriptor, size);
                            if (result != null)
                                return result;
                        }
                    }

                    result = UIFont.FromName(family, size);

                    if (result != null)
                        return result;
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", family);
                }
            }

            if (bold && italic)
            {
                var defaultFont = UIFont.SystemFontOfSize(size);

                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }
            if (italic)
                return UIFont.ItalicSystemFontOfSize(size);

            if (bold)
                return UIFont.BoldSystemFontOfSize(size);

            return UIFont.SystemFontOfSize(size);
        }
    }
}
