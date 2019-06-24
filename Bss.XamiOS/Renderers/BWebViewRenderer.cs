//
// BWebViewRenderer.cs
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
using System.ComponentModel;
using Bss.XamForms.Controls;
using Bss.XamiOS.Renderers;
using Foundation;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BWebView), typeof(BWebViewRenderer))]
namespace Bss.XamiOS.Renderers
{
    //FIXME (vali): application is crashing check https://github.com/xamarin/xamarin-macios/issues/4130#issuecomment-398702385
    public class BWebViewRenderer : ViewRenderer<BWebView, WKWebView>, IWKNavigationDelegate
    {
        private IDisposable _estimateProgressDisposable;

        protected virtual void DecidePolicyInternaly(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            decisionHandler(WKNavigationActionPolicy.Allow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _estimateProgressDisposable?.Dispose();
                Control.NavigationDelegate = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new WKWebView(Frame, new WKWebViewConfiguration())
                {
                    NavigationDelegate = this,
                };

                _estimateProgressDisposable = webView.AddObserver("estimatedProgress", NSKeyValueObservingOptions.New, EstimateProgressChanged);

                webView.ScrollView.ScrollEnabled = true;

                SetNativeControl(webView);
            }

            if (e.NewElement != null)
            {
                LoadUrlInternal(GetUrlFromSource());
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == WebView.SourceProperty.PropertyName)
                LoadUrlInternal(GetUrlFromSource());
        }

        protected NSDictionary GetHeaders(IDictionary<string, string> headers, NSDictionary currentHeaders)
        {
            var dict = new NSMutableDictionary();

            if (currentHeaders != null)
            {
                foreach (var pair in currentHeaders)
                {
                    dict.Add(pair.Key, pair.Value);
                }
            }

            foreach (var pair in headers)
            {
                dict.Add(new NSString(pair.Key), new NSString(pair.Value));
            }

            return dict;
        }


        protected virtual void LoadUrlInternal(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            Control.LoadRequest(new NSUrlRequest(new NSUrl(url)));
        }

        protected string GetUrlFromSource()
        {
            if (Element?.Source is UrlWebViewSource source)
                return source.Url;

            return "";
        }

        [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        private void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            DecidePolicyInternaly(webView, navigationAction, decisionHandler);
        }


        private void EstimateProgressChanged(NSObservedChange observedChange)
        {
            Element.Progress = (float)Control.EstimatedProgress;
        }
    }
}