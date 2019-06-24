//
// DateTimeExtensions.cs
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
namespace Bss.XamCore.Extensions
{
    public enum DateTimePrecision
    {
        Day, Hour, Minute, Second
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime TrimDate(this DateTime date, DateTimePrecision precision)
        {
            switch (precision)
            {
                case DateTimePrecision.Day:
                    return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                case DateTimePrecision.Hour:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                case DateTimePrecision.Minute:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                case DateTimePrecision.Second:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            }
            return date;
        }

        public static long ToUnixTimeSeconds(this DateTime This)
        {
            var seconds = (long)(This.ToUniversalTime() - UnixEpoch).TotalSeconds;
            return seconds;
        }

        public static long ToUnixTimeMileseconds(this DateTime This)
        {
            return ToUnixTimeSeconds(This) * 1000;
        }

        public static DateTime UnixTimeSecondsToDate(this long This)
        {
            var timeSpan = TimeSpan.FromSeconds(This);
            var date = UnixEpoch.Add(timeSpan).ToLocalTime();
            return date;
        }

        public static DateTime UnixTimeMilesecondsToDate(this long This)
        {
            var timeSpan = TimeSpan.FromMilliseconds(This);
            var date = UnixEpoch.Add(timeSpan).ToLocalTime();
            return date;
        }       
    }

}
