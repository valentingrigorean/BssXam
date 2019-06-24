//
//  BDatePicker.cs
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
using Bss.XamForms.Extensions;
using Xamarin.Forms;

namespace Bss.XamForms.Input
{
    public enum IntervalDate
    {
        Now,
        Befor,
        After
    }

    public class BDatePicker: DatePicker
    {
        public static readonly BindableProperty NullableDateProperty =
            BindableProperty.Create(nameof(NullableDate), typeof(string), typeof(BDatePicker), "", BindingMode.TwoWay);

        public static readonly BindableProperty IntervalValidDateProperty =
            BindableProperty.Create(nameof(IntervalValidDate), typeof(IntervalDate), typeof(BDatePicker),
                                    IntervalDate.Now, BindingMode.TwoWay);

        public string OriginalFormat;

        public BDatePicker()
        {
            Format = "dd/MM/yyyy";
        }

        public string NullableDate
        {
            get => (string)GetValue(NullableDateProperty);
            set
            {
                SetValue(NullableDateProperty, value);
                UpdateDate();
            }
        }

        public IntervalDate IntervalValidDate
        {
            get => (IntervalDate)GetValue(IntervalValidDateProperty);
            set
            {
                SetValue(IntervalValidDateProperty, value);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null) return;
            OriginalFormat = Format;
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName || propertyName == IsFocusedProperty.PropertyName &&
                !IsFocused && Date.ToString("d") == DateTime.Now.ToString("d"))
                AssignValue();

            if (propertyName != NullableDateProperty.PropertyName) return;
            if (!NullableDate.IsNullOrWhiteSpace())
                Date = Convert.ToDateTime(NullableDate);
            else if (Date.ToString(OriginalFormat) == DateTime.Now.ToString(OriginalFormat))
                UpdateDate();
        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }

        public void AssignValue()
        {
            NullableDate = Date.ToString("d");
            UpdateDate();
        }

        private void UpdateDate()
        {
            if (NullableDate == null) return;
            if (OriginalFormat != null)
                Format = OriginalFormat;
        }
    }
}