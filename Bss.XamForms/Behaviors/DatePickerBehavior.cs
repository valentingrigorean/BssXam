//
// DatePickerBehavior.cs
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
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using ReactiveUI;
using Xamarin.Forms;
using Bss.XamForms.Services;
using Bss.XamCore.Extensions;

namespace Bss.XamForms.Behaviors
{
    public enum DatePickerType
    {
        Date,
        Time,
        DateAndTime
    }

    public class DatePickerBehavior : BindableBehavior<View>
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private PropertyInfo _textPropertyInfo;

        private bool IsDateType => DatePickerType == DatePickerType.Date || DatePickerType == DatePickerType.DateAndTime;

        public static readonly BindableProperty SelectedTimerProperty = BindableProperty.Create(nameof(SelectedTimer), typeof(TimeSpan?), typeof(DatePickerBehavior), default(TimeSpan?));

        public TimeSpan? SelectedTimer
        {
            get => (TimeSpan?)GetValue(SelectedTimerProperty);
            set => SetValue(SelectedTimerProperty, value);
        }

        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DatePickerBehavior), null, BindingMode.TwoWay);

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime?), typeof(DatePickerBehavior), null);

        public DateTime? MinimumDate
        {
            get => (DateTime?)GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
        }

        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime?), typeof(DatePickerBehavior), null);

        public DateTime? MaximumDate
        {
            get => (DateTime?)GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(DatePickerBehavior), default(string));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty DateFormatProperty = BindableProperty.Create(nameof(DateFormat), typeof(string), typeof(DatePickerBehavior), "dd/MM/yyyy HH:mm");

        public string DateFormat
        {
            get => (string)GetValue(DateFormatProperty);
            set => SetValue(DateFormatProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DatePickerBehavior), default(string));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty DatePickerTypeProperty = BindableProperty.Create(nameof(DatePickerType), typeof(DatePickerType), typeof(DatePickerBehavior), DatePickerType.DateAndTime);

        public DatePickerType DatePickerType
        {
            get => (DatePickerType)GetValue(DatePickerTypeProperty);
            set => SetValue(DatePickerTypeProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            var properties = AssociatedObject.GetType().GetProperties();

            _textPropertyInfo = properties.FirstOrDefault(p => p.Name == "Text");

            var datePickerService = DependencyService.Get<IDatePickerService>(DependencyFetchTarget.NewInstance);

            if (datePickerService == null)
                throw new Exception("You need to impl/add to container IDatePickerService");

            var command = ReactiveCommand.CreateFromTask(async () =>
            {
                switch (DatePickerType)
                {
                    case DatePickerType.Time:
                        SelectedTimer = await datePickerService.ShowTimePickerAsync(Title, SelectedDate, MinimumDate, MaximumDate);
                        break;
                    case DatePickerType.Date:
                        var selectedDate = await datePickerService.ShowDatePickerAsync(Title, SelectedDate, MinimumDate, MaximumDate);
                        if (selectedDate.HasValue)
                            SelectedDate = selectedDate.Value.TrimDate(DateTimePrecision.Day);
                        break;
                    case DatePickerType.DateAndTime:
                        SelectedDate = await datePickerService.ShowDateAndTimePickerAsync(Title, SelectedDate, MinimumDate, MaximumDate);
                        break;
                }

                Update();
            });

            var commandPropertyInfo = properties.FirstOrDefault(p => p.Name == "Command");

            if (commandPropertyInfo != null)
            {
                commandPropertyInfo.SetValue(AssociatedObject, command);
            }
            else
            {
                var tapGesture = new TapGestureRecognizer
                {
                    Command = command
                };

                bindable.GestureRecognizers.Add(tapGesture);

                Disposable.Create(() => bindable.GestureRecognizers.Remove(tapGesture))
                          .DisposeWith(_disposables);
            }
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            _disposables?.Dispose();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                Update();
            }
        }

        private void Update()
        {
            if (IsDateType)
                SetDate();
            else
                throw new NotImplementedException();
        }

        private void SetDate()
        {
            if (SelectedDate.HasValue)
            {
                var format = DateFormat;
                if (string.IsNullOrEmpty(format))
                    format = "dd/MM/yyyy HH:mm";

                _textPropertyInfo?.SetValue(AssociatedObject, SelectedDate.Value.ToString(format));
            }
            else
            {
                _textPropertyInfo?.SetValue(AssociatedObject, Placeholder);
            }
        }
    }

}
