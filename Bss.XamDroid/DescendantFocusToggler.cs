//
// DescendantFocusToggler.cs
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
using Android.Views;
using Android.Widget;

namespace Bss.XamDroid
{
    public class DescendantFocusToggler
    {
        public bool RequestFocus(global::Android.Views.View control, Func<bool> baseRequestFocus)
        {
            IViewParent ancestor = control.Parent;
            var previousFocusability = DescendantFocusability.BlockDescendants;


            LinearLayout cfl = null;
            // Work our way up through the tree until we find a ConditionalFocusLayout
            while (ancestor is ViewGroup)
            {
                cfl = ancestor as LinearLayout;

                var found = ancestor.GetType().Name == "ConditionalFocusLayout";

                if (cfl != null && found)
                {
                    previousFocusability = cfl.DescendantFocusability;
                    // Toggle DescendantFocusability to allow this control to get focus
                    cfl.DescendantFocusability = DescendantFocusability.AfterDescendants;
                    break;
                }

                ancestor = ancestor.Parent;
            }

            // Call the original RequestFocus implementation for the View
            bool result = baseRequestFocus();

            if (cfl != null)
            {
                // Toggle descendantfocusability back to whatever it was
                cfl.DescendantFocusability = previousFocusability;
            }

            return result;
        }
    }
}
