//
// ValidatableObject.cs
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
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;

namespace Bss.XamCore.Validations
{
    //TODO(vali):make a way to give an array of ValidableObject and return IObservable<bool> combine of all IsValid
    public class ValidatableObject<T> : ReactiveObject, IValidity
    {
        private bool _autoValidate;
        private bool _notify = true;
        private IDisposable _autoValidateDisposable;

        public ValidatableObject()
        {
            Validations = new List<IValidationRule<T>>();
            IsValid = true;
            Errors = new List<string>();

            ValueAsObservable = this
                .ObservableForProperty(vm => vm.Value)
                .Value();

            IsValidAsObservable = ValueAsObservable
                .DistinctUntilChanged()
                .Select(_ => !Validations.Any(v => !v.Check(_)));
        }

        public List<IValidationRule<T>> Validations { get; }

        [Reactive]
        public T Value { get; set; }

        public IObservable<T> ValueAsObservable { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="T:Bss.XamForms.Validations.ValidatableObject`1" />
        ///     auto validate.(Will set IsValid/Errors everytime Value change)
        /// </summary>
        /// <value><c>true</c> if auto validate; otherwise, <c>false</c>.</value>
        public bool AutoValidate
        {
            get => _autoValidate;
            set
            {
                if (_autoValidate == value)
                    return;
                _autoValidate = value;
                _autoValidateDisposable?.Dispose();
                _autoValidateDisposable = null;
                if (value)
                    _autoValidateDisposable = ValueAsObservable
                        .Select(_ => Validate())
                        .Subscribe();
            }
        }

        /// <summary>
        ///     Gets the is valid as observable.
        ///     Will not set IsValid or Errors
        /// </summary>
        /// <value>The is valid as observable.</value>
        public IObservable<bool> IsValidAsObservable { get; }

        [Reactive]
        public IList<string> Errors { get; private set; }

        [Reactive]
        public string ErrorMessage { get; private set; }

        [Reactive]
        public bool IsValid { get; set; }

        IDisposable IValidity.SuppressChangeNotifications()
        {
            _notify = false;
            return Disposable.Create(() => _notify = true);
        }

        public ValidatableObject<T> AddRule(IValidationRule<T> validationRule)
        {
            Validations.Add(validationRule);
            return this;
        }

        public bool Validate()
        {
            Errors.Clear();

            var errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage).ToList();

            var isValid = errors.Count == 0;

            if (_notify)
                Errors = errors;
            if (_notify)
                IsValid = isValid;
            if (_notify)
                ErrorMessage = Errors.FirstOrDefault();

            return isValid;
        }

        public ValidatableObject<T> RuleFor(Func<T, bool> func)
        {
            Validations.Add(new CompareRule<T>(func));
            return this;
        }
    }
}