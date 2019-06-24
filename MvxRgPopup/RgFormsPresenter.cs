//
// RgFormsPresenter.cs
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
using MvvmCross.Forms.Presenters;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using Rg.Plugins.Popup.Contracts;

namespace MvxRgPopup
{
    public class RgFormsPresenter : MvxFormsPagePresenter
    {
        public RgFormsPresenter(IMvxFormsViewPresenter platformPresenter) : base(platformPresenter)
        {
        }

        private IPopupNavigation PopupNavigation => Rg.Plugins.Popup.Services.PopupNavigation.Instance;

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return base.Show(request);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Add(
                typeof(RgModalPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) =>
                        ShowModal(view, (RgModalPresentationAttribute) attribute, request),
                    CloseAction = (view, attribute) => CloseModal(view, (RgModalPresentationAttribute) attribute)
                });
        }

        public virtual async Task<bool> ShowModal(Type view, RgModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = CreatePage(view, request, attribute) as MvxPopupPage;

            await PopupNavigation.PushAsync(page, attribute.Animated);

            return true;
        }

        public virtual async Task<bool> CloseModal(IMvxViewModel viewModel, RgModalPresentationAttribute attribute)
        {
            await PopupNavigation.PopAsync(attribute.Animated);
            return true;
        }

        public override async Task CloseAllModals(bool animate = false)
        {
            await base.CloseAllModals(animate);
            await PopupNavigation.PopAllAsync(animate);
        }
    }
}