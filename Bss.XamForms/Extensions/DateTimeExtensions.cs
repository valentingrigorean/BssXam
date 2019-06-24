//
//  DateTimeExtensions.cs
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

using System;

namespace Bss.XamForms.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Ises the in period.
        /// </summary>
        /// <returns><c>true</c>, if in period was ised, <c>false</c> otherwise.</returns>
        /// <param name="dateTime">Date time.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public static bool IsInPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            return dateTime >= startDate && dateTime <= endDate;
        }

        /// <summary>
        ///     Ises the out of period.
        /// </summary>
        /// <returns><c>true</c>, if out of period was ised, <c>false</c> otherwise.</returns>
        /// <param name="dateTime">Date time.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public static bool IsOutOfPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            return dateTime < startDate || dateTime > endDate;
        }

        public static bool IsBeforDateNow(this DateTime dateTime)
        {
            return DateTime.Now.Date > dateTime.Date;
        }

        public static bool IsAfterDateNow(this DateTime dateTime)
        {
            return DateTime.Now.Year <= dateTime.Year &&
                   DateTime.Now.Month <= dateTime.Month &&
                   DateTime.Now.Day <= dateTime.Day;
        }

        public static bool IsBetween(this DateTime dt, DateTime startDate, DateTime endDate, bool compareTime = false)
        {
            return compareTime
                ? dt >= startDate && dt <= endDate
                : dt.Date >= startDate.Date && dt.Date <= endDate.Date;
        }
    }
}