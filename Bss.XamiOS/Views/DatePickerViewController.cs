//
// DatePickerViewController.cs
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
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Bss.XamiOS.Views
{
    public partial class DatePickerViewController : UIViewController
    {
        [Outlet]
        UIKit.UIBarButtonItem CancelButton { get; set; }

        [Outlet]
        UIKit.UIDatePicker DatePicker { get; set; }

        [Outlet]
        UIKit.UIBarButtonItem DoneButton { get; set; }

        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        private string _title;
        private UIDatePickerMode _datePickerMode;
        private DateTime? _defaultDate;
        private DateTime? _minDate;
        private DateTime? _maxDate;

        private string DatePickerTitle
        {
            get => _title;
            set
            {
                _title = value;
                if (TitleLabel != null)
                    TitleLabel.Text = value;
            }
        }

        private UIDatePickerMode DatePickerMode
        {
            get => _datePickerMode;
            set
            {
                _datePickerMode = value;
                if (DatePicker != null)
                    DatePicker.Mode = value;
            }
        }

        private DateTime? DefaultDate
        {
            get => _defaultDate;
            set
            {
                _defaultDate = value;
                if (DatePicker != null)
                    SetupDefaultDate();
            }
        }

        private DateTime? MinDate
        {
            get => _minDate;
            set
            {
                _minDate = value;
                if (DatePicker != null)
                    SetupMinDate();
            }
        }

        private DateTime? MaxDate
        {
            get => _maxDate;
            set
            {
                _maxDate = value;
                if (DatePicker != null)
                    SetupMaxDate();
            }
        }


        public DatePickerViewController() : base("DatePickerViewController", null)
        {
            ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
        }

        public event EventHandler<DateTime?> OnDatePicked;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Text = DatePickerTitle;

            SetupDefaultDate();
            SetupMinDate();
            SetupMaxDate();

            DatePicker.Mode = DatePickerMode;
        }

        public override void ViewDidAppear(bool animated)
        {
            DoneButton.Clicked += DoneButtonClicked;
            CancelButton.Clicked += CancelButtonClicked;
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            DoneButton.Clicked -= DoneButtonClicked;
            CancelButton.Clicked -= CancelButtonClicked;
            base.ViewDidDisappear(animated);
        }

        public void SetupDatePicked(string title, UIDatePickerMode datePickerMode, DateTime? defaultDate, DateTime? minDate, DateTime? maxDate)
        {
            DatePickerTitle = title;
            DefaultDate = defaultDate;
            MinDate = minDate;
            MaxDate = maxDate;
            DatePickerMode = datePickerMode;
        }

        private void SetupDefaultDate()
        {
            if (_defaultDate.HasValue)
                DatePicker.SetDate(_defaultDate.Value.ToNSDate(), false);
        }

        private void SetupMinDate()
        {
            if (_minDate.HasValue)
                DatePicker.MinimumDate = _minDate.Value.ToNSDate();
        }

        private void SetupMaxDate()
        {
            if (_maxDate.HasValue)
                DatePicker.MaximumDate = _maxDate.Value.ToNSDate();
        }

        private void DoneButtonClicked(object sender, EventArgs e)
        {
            OnDatePicked?.Invoke(this, DatePicker.Date.ToDateTime());
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            OnDatePicked?.Invoke(this, null);
        }

        void ReleaseDesignerOutlets()
        {
            if (DoneButton != null)
            {
                DoneButton.Dispose();
                DoneButton = null;
            }

            if (CancelButton != null)
            {
                CancelButton.Dispose();
                CancelButton = null;
            }

            if (DatePicker != null)
            {
                DatePicker.Dispose();
                DatePicker = null;
            }

            if (TitleLabel != null)
            {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }

    }
}

