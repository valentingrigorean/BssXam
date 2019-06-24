//
// AspectRatioDrawable.cs
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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Bss.XamDroid.Graphics
{
    public class AspectRatioDrawable : Drawable
    {
        private readonly Paint _paint = new Paint(PaintFlags.AntiAlias);

        private readonly Rect _rect = new Rect();

        private Color _color;

        public AspectRatioDrawable(Bitmap bmp)
        {
            Bitmap = bmp;
        }

        public Bitmap Bitmap { get; }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value)
                    return;
                _color = value;
                _paint.SetColorFilter(new PorterDuffColorFilter(value, PorterDuff.Mode.SrcIn));
                InvalidateSelf();
            }
        }

        public override int Opacity => _paint.Alpha;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Bitmap.Recycle();
            }
            base.Dispose(disposing);
        }

        public override void Draw(Canvas canvas)
        {
            var bounds = Bounds;
            var width = bounds.Width();
            var height = bounds.Height();

            if (width > 0 && height > 0)
            {
                var scale = Math.Min((float)bounds.Width() / Bitmap.Width, (float)bounds.Height() / Bitmap.Height);

                var scaledWidth = scale * Bitmap.Width;
                var scaledHeight = scale * Bitmap.Height;

                var centerXPadding = (int)((width - scaledWidth) / 2f);
                var centerYPadding = (int)((height - scaledHeight) / 2f);

                _rect.Set(bounds.Left + centerXPadding, bounds.Top + centerYPadding, bounds.Right - centerXPadding, bounds.Top + (int)scaledHeight +centerYPadding);
                canvas.DrawBitmap(Bitmap, null, _rect, _paint);
            }

        }

        public override void SetAlpha(int alpha)
        {
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
        }
    }

}
