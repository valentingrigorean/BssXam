//
// MvxPopupPage.cs
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

using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using MvvmCross.Forms.Bindings;

namespace MvxRgPopup
{
    public class MvxPopupPage : PopupPage, IMvxPage
    {
        private IMvxBindingContext _bindingContext;

        public MvxPopupPage()
        {
            MvxDesignTimeChecker.Check();
        }

        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel),
            typeof(IMvxViewModel), typeof(IMvxElement), default(MvxViewModel), BindingMode.Default, null,
            ViewModelChanged, null, null);


        public object DataContext
        {
            get => BindingContext.DataContext;
            set
            {
                if (value != null && !(_bindingContext != null && ReferenceEquals(DataContext, value)))
                    BindingContext = new MvxBindingContext(value);
            }
        }

        public new IMvxBindingContext BindingContext
        {
            get
            {
                if (_bindingContext == null)
                    BindingContext = new MvxBindingContext(base.BindingContext);
                return _bindingContext;
            }
            set
            {
                _bindingContext = value;
                base.BindingContext = _bindingContext.DataContext;
            }
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                SetValue(ViewModelProperty, value);
                OnViewModelSet();
            }
        }

        internal static void ViewModelChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue != null)
            {
                if (bindable is IMvxElement element)
                    element.DataContext = newvalue;
                else
                    bindable.BindingContext = newvalue;
            }
        }

        protected virtual void OnViewModelSet()
        {
            ViewModel?.ViewCreated();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.ViewAppearing();
            ViewModel?.ViewAppeared();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.ViewDisappearing();
            ViewModel?.ViewDisappeared();
            ViewModel?.ViewDestroy();
        }
    }
}