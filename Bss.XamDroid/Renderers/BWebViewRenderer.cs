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
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Webkit;
using Bss.XamDroid.Renderers;
using Bss.XamForms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BWebView), typeof(BWebViewRenderer))]
namespace Bss.XamDroid.Renderers
{
    public class BWebViewRenderer : WebViewRenderer
    {
        public BWebViewRenderer(Context context) : base(context)
        {
        }

        protected virtual bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            return false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                LoadUrlInternal(GetUrlFromSource());
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.WebView.SourceProperty.PropertyName && !IgnoreSourceChanges)
            {
                LoadUrlInternal(GetUrlFromSource());
                return;
            }
            base.OnElementPropertyChanged(sender, e);
        }

        protected override WebViewClient GetWebViewClient()
        {
            return new BWebViewClient(this);
        }

        protected override FormsWebChromeClient GetFormsWebChromeClient()
        {
            return new BWebChromeClient(this);
        }

        protected virtual void LoadUrlInternal(string url)
        {
            Control.LoadUrl(url);
        }

        protected string GetUrlFromSource()
        {
            if (Element?.Source is UrlWebViewSource source)
                return source.Url;

            return "";
        }

        protected class BWebViewClient : FormsWebViewClient
        {
            [Weak]
            private readonly BWebViewRenderer _renderer;

            public BWebViewClient(BWebViewRenderer renderer) : base(renderer)
            {
                _renderer = renderer;
            }

            protected BWebViewClient(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
            {
            }

            protected BWebView BAWebView => _renderer?.Element as BWebView;           

            [Obsolete]
            public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
            {
                return _renderer?.ShouldOverrideUrlLoading(view, url) ?? base.ShouldOverrideUrlLoading(view, url);
            }

            public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
            {
                return _renderer?.ShouldOverrideUrlLoading(view, request.Url.ToString()) ?? base.ShouldOverrideUrlLoading(view, request);
            }
        }

        protected class BWebChromeClient : FormsWebChromeClient
        {
            [Weak]
            private readonly BWebViewRenderer _renderer;


            public BWebChromeClient(BWebViewRenderer renderer)
            {
                _renderer = renderer;
            }

            protected BWebView BAWebView => _renderer?.Element as BWebView;


            public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
            {
                base.OnProgressChanged(view, newProgress);

                var element = BAWebView;
                if (element != null)
                    element.Progress = newProgress / 100f;
            }
        }

    }
}
