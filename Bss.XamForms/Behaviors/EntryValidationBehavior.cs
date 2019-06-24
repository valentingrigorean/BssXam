//
// EntryValidationBehavior.cs
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
using Bss.XamCore.Validations;
using Xamarin.Forms;

namespace Bss.XamForms.Behaviors
{
    public class EntryValidationBehavior : BindableBehavior<Entry>
    {

        public static readonly BindableProperty ValidatorProperty = BindableProperty.Create(nameof(Validator),
            typeof(ValidatableObject<string>), typeof(EntryValidationBehavior), default(ValidatableObject<string>));

        public ValidatableObject<string> Validator
        {
            get => (ValidatableObject<string>)GetValue(ValidatorProperty);
            set => SetValue(ValidatorProperty, value);
        }

        public static readonly BindableProperty ValidateOnUnfocusProperty = BindableProperty.Create(nameof(ValidateOnUnfocus), typeof(bool), typeof(EntryValidationBehavior), true);

        public bool ValidateOnUnfocus
        {
            get => (bool)GetValue(ValidateOnUnfocusProperty);
            set => SetValue(ValidateOnUnfocusProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += TextChanged;
            bindable.Unfocused += HandleFocused;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= TextChanged;
            bindable.Unfocused -= HandleFocused;
        }

        private void HandleFocused(object sender, FocusEventArgs e)
        {
            if (ValidateOnUnfocus)
                Validator?.Validate();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Validator == null)
                return;
            Validator.Validate();
        }
    }
}
