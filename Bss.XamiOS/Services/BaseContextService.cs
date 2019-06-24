//
// BaseContextService.cs
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
using Foundation;
using UIKit;

namespace Bss.XamiOS.Services
{
    public abstract class BaseContextService : NSObject
    {
        public UIViewController TopViewController
        {
            get
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                return GetTopViewController(vc);
            }
        }

        protected UINavigationController CreateNavigationController(UIColor color)
        {
            var navigationController = new CustomNavigationController(color);

            return navigationController;
        }


        private UIViewController GetTopViewController(UIViewController viewController)
        {
            switch (viewController)
            {
                case UINavigationController navigationController:
                    return GetTopViewController(navigationController.VisibleViewController);
                case UITabBarController tabBarController:
                    return GetTopViewController(tabBarController.SelectedViewController);
            }
            if (viewController.PresentedViewController != null)
                return GetTopViewController(viewController.PresentedViewController);
            return viewController;
        }

        private class CustomNavigationController : UINavigationController
        {
            private readonly UIViewController _emptyViewController = new UIViewController();

            private readonly UIColor _navigationBarColor;

            public CustomNavigationController(UIColor color)
            {
                _navigationBarColor = color;
                Initialiaze();
            }

            public override UIViewController PopViewController(bool animated)
            {
                if (ViewControllers.Length <= 2)
                {
                    var vc = ViewControllers.LastOrDefault();
                    DismissViewController(animated, () => base.PopViewController(false));
                    return vc;
                }
                return base.PopViewController(animated);
            }

            private void Initialiaze()
            {
                NavigationBar.SetBackgroundImage(null, UIBarMetrics.Default);
                NavigationBar.BackgroundColor = _navigationBarColor;
                NavigationBar.ShadowImage = null;
                NavigationBar.Translucent = false;

                View.BackgroundColor = UIColor.Clear;
                _emptyViewController.View.BackgroundColor = UIColor.Clear;

                ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;

                PushViewController(_emptyViewController, false);
            }
        }
    }

}
