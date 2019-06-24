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
using Android.App;
using Android.Content;
using Xamarin.Forms;
using Bss.XamCore.Extensions;
using DatePicker = Android.Widget.DatePicker;
using TimePicker = Android.Widget.TimePicker;
using Bss.XamForms.Services;
using Bss.XamDroid.Services;

[assembly: Dependency(typeof(DatePickerService))]
namespace Bss.XamDroid.Services
{
    public class DatePickerService: Java.Lang.Object, IDatePickerService, DatePickerDialog.IOnDateSetListener, IDialogInterfaceOnCancelListener, TimePickerDialog.IOnTimeSetListener
    {
        private readonly DatePickerDialog _datePickerDialog;
        private readonly TimePickerDialog _timePickerDialog;
        private TaskCompletionSource<DateTime?> _tcsDate;
        private TaskCompletionSource<TimeSpan?> _tcsTime;

        public DatePickerService()
        {
            var date = DateTime.Now;

            _datePickerDialog = new DatePickerDialog(Context, this, date.Year, date.Month - 1, date.Day);
            _datePickerDialog.SetOnCancelListener(this);

            _timePickerDialog = new TimePickerDialog(Context, this, date.Hour, date.Minute, true);
            _timePickerDialog.SetOnCancelListener(this);
        }

        public Context Context => DependencyService.Get<ICurrentActivity>().Current;

        public void OnCancel(IDialogInterface dialog)
        {
            if (_tcsDate != null)
                SetResultDate(null);
            else if (_tcsTime != null)
                SetResultTime(null);
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            SetResultDate(new DateTime(year, month + 1, dayOfMonth));
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            SetResultTime(new TimeSpan(hourOfDay, minute, 0));
        }

        public Task<DateTime?> ShowDatePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (_datePickerDialog.IsShowing && _tcsDate != null)
                return _tcsDate.Task;

            if (minDate.HasValue)
                _datePickerDialog.DatePicker.MinDate = minDate.Value.ToUnixTimeMileseconds();

            if (maxDate.HasValue)
                _datePickerDialog.DatePicker.MaxDate = maxDate.Value.ToUnixTimeMileseconds();

            if (defaultDate.HasValue && defaultDate.Value >= _datePickerDialog.DatePicker.MinDateTime &&
                defaultDate.Value <= _datePickerDialog.DatePicker.MaxDateTime)
            {
                _datePickerDialog.DatePicker.DateTime = defaultDate.Value;
            }

            _tcsDate = new TaskCompletionSource<DateTime?>();

            _datePickerDialog.SetTitle(title);
            _datePickerDialog.Show();

            return _tcsDate.Task;
        }

        public Task<TimeSpan?> ShowTimePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (_timePickerDialog.IsShowing && _tcsTime != null)
                return _tcsTime.Task;


            if (defaultDate.HasValue)
                _timePickerDialog.UpdateTime(defaultDate.Value.Hour, defaultDate.Value.Minute);

            _tcsTime = new TaskCompletionSource<TimeSpan?>();

            _timePickerDialog.SetTitle(title);
            _timePickerDialog.Show();

            return _tcsTime.Task;
        }

        public async Task<DateTime?> ShowDateAndTimePickerAsync(string title, DateTime? defaultDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            var date = await ShowDatePickerAsync(title, defaultDate, minDate, maxDate);
            if (date != null)
            {
                var time = await ShowTimePickerAsync(title, defaultDate, minDate, minDate);
                if (time != null)
                    return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
                return await ShowDateAndTimePickerAsync(title, defaultDate, minDate, maxDate);
            }
            return null;
        }

        private void SetResultDate(DateTime? date)
        {
            if (_datePickerDialog.IsShowing)
                _datePickerDialog.Dismiss();

            if (_tcsDate == null)
                return;

            _tcsDate.TrySetResult(date);
            _tcsDate = null;
        }

        private void SetResultTime(TimeSpan? timespan)
        {
            if (_timePickerDialog.IsShowing)
                _timePickerDialog.Dismiss();

            if (_tcsTime == null)
                return;

            _tcsTime.TrySetResult(timespan);
            _tcsTime = null;
        }
    }
}
