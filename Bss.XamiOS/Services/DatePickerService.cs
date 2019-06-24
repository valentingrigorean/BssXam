//
// DatePickerService.cs
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
using System.Threading.Tasks;
using Bss.XamForms.Services;
using Bss.XamiOS.Services;
using Bss.XamiOS.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatePickerService))]
namespace Bss.XamiOS.Services
{
    public class DatePickerService : BaseContextService, IDatePickerService
    {
        private readonly DatePickerViewController _datePickerViewController = new DatePickerViewController();
        private TaskCompletionSource<DateTime?> _tcsDate;

        public DatePickerService()
        {
            _datePickerViewController.OnDatePicked += (sender, e) =>
            {
                _datePickerViewController.DismissViewController(true, null);
                if (_tcsDate != null)
                {
                    _tcsDate.TrySetResult(e);
                    _tcsDate = null;
                }
            };
        }

        public async Task<DateTime?> ShowDateAndTimePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (_tcsDate == null)
            {
                _tcsDate = new TaskCompletionSource<DateTime?>();

                _datePickerViewController.SetupDatePicked(title, UIKit.UIDatePickerMode.DateAndTime, defaultDate, minDate, maxDate);
                await TopViewController.PresentViewControllerAsync(_datePickerViewController, true);
            }
            return await _tcsDate.Task;
        }

        public async Task<DateTime?> ShowDatePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (_tcsDate == null)
            {
                _tcsDate = new TaskCompletionSource<DateTime?>();

                _datePickerViewController.SetupDatePicked(title, UIKit.UIDatePickerMode.Date, defaultDate, minDate, maxDate);
                await TopViewController.PresentViewControllerAsync(_datePickerViewController, true);
            }
            return await _tcsDate.Task;
        }

        public async Task<TimeSpan?> ShowTimePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (_tcsDate == null)
            {
                _tcsDate = new TaskCompletionSource<DateTime?>();

                _datePickerViewController.SetupDatePicked(title, UIKit.UIDatePickerMode.Time, defaultDate, minDate, maxDate);
                await TopViewController.PresentViewControllerAsync(_datePickerViewController, true);
            }
            var date = await _tcsDate.Task;

            if (date.HasValue)
                return date.Value.TimeOfDay;
            return null;
        }
    }

}
