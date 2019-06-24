//
// HtmlLabelConverter.cs
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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bss.XamForms.Converters
{
    public class HtmlLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatted = new FormattedString();

            foreach (var item in ProcessString((string)value))
                formatted.Spans.Add(CreateSpan(item));

            return formatted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public IList<StringSection> ProcessString(string rawText)
        {
            const string spanPattern = @"(<a.*?>.*?</a>)";
            MatchCollection collection = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline);

            var sections = new List<StringSection>();

            var lastIndex = 0;

            foreach (Match item in collection)
            {
                var foundText = item.Value;
                sections.Add(new StringSection { Text = rawText.Substring(lastIndex, item.Index) });
                lastIndex += item.Index + item.Length;

                // Get HTML href 
                var html = CreateUrlSection(item.Value);
                sections.Add(html);
            }
            var text = rawText.Substring(lastIndex);
            if (!string.IsNullOrEmpty(text))
                sections.Add(new StringSection { Text = text });
            return sections;
        }

        public class StringSection
        {
            public string Text { get; set; }
            public string Link { get; set; }
        }

        private Span CreateSpan(StringSection section)
        {
            var span = new Span()
            {
                Text = section.Text
            };

            if (!string.IsNullOrEmpty(section.Link))
            {
                span.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = _navigationCommand,
                    CommandParameter = section.Link
                });
                span.TextColor = Color.Blue;
                span.TextDecorations = TextDecorations.Underline;
            }

            return span;
        }

        private StringSection CreateUrlSection(string str)
        {
            var link = Regex.Match(str, "(?<=href=\\\")[\\S]+(?=\\\")").Value?.ToLowerInvariant();
            var text = Regex.Replace(str, "<.*?>", string.Empty);

            if (!link.StartsWith("http://", StringComparison.InvariantCulture) && !link.StartsWith("https://", StringComparison.InvariantCulture))
                link = "http://" + link;

            return new StringSection { Link = link, Text = text };
        }

        private ICommand _navigationCommand = new Command<string>((url) =>
        {
            Device.OpenUri(new Uri(url));
        });
    }
}
