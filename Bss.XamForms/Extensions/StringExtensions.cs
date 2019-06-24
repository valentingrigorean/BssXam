//
//  StringExtensions.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2018 Songurov Fiodor
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System.Text;
using System.Text.RegularExpressions;

namespace Bss.XamForms.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string ToUpperFirstInvariant(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var sb = new StringBuilder(value);
            sb[0] = char.ToUpperInvariant(sb[0]);
            return sb.ToString();
        }

        public static string ToUpperLast(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var sb = new StringBuilder(value);
            sb[value.Length - 1] = char.ToUpper(sb[value.Length - 1]);
            return sb.ToString();
        }

        public static string UpperCaseWords(this string text)
        {
            var sb = new StringBuilder(text.Length);
            var capitalize = true;

            foreach (var c in text.Trim())
            {
                sb.Append(capitalize ? char.ToUpper(c) : char.ToLower(c));
                capitalize = !char.IsLetter(c);
            }

            return Regex.Replace(sb.ToString().Replace(" ", string.Empty), "([A-Z])", " $1").Trim();
        }
    }
}