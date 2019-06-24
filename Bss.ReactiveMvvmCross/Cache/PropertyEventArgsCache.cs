//
// PropertyChangedEventArgsCache.cs
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

using System.Collections.Concurrent;
using System.ComponentModel;

namespace Bss.ReactiveMvvmCross.Cache
{
    /// <summary>
    ///     Provides a cache for <see cref="PropertyChangedEventArgs" /> instances.
    /// </summary>
    public sealed class PropertyEventArgsCache
    {
        /// <summary>
        ///     The underlying dictionary. This instance is its own mutex.
        /// </summary>
        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> _cachePropertyChanged =
            new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        private static readonly ConcurrentDictionary<string, PropertyChangingEventArgs> _cachePropertyChanging =
            new ConcurrentDictionary<string, PropertyChangingEventArgs>();

        /// <summary>
        ///     Private constructor to prevent other instances.
        /// </summary>
        private PropertyEventArgsCache()
        {
        }

        /// <summary>
        ///     The global instance of the cache.
        /// </summary>
        public static PropertyEventArgsCache Instance { get; } = new PropertyEventArgsCache();

        /// <summary>
        ///     Retrieves a <see cref="PropertyChangedEventArgs" /> instance for the specified property, creating it and adding it
        ///     to the cache if necessary.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            PropertyChangedEventArgs result;
            if (_cachePropertyChanged.TryGetValue(propertyName, out result))
                return result;
            result = new PropertyChangedEventArgs(propertyName);
            _cachePropertyChanged.TryAdd(propertyName, result);
            return result;
        }

        public PropertyChangingEventArgs GetPropertyChangingEventArgs(string propertyName)
        {
            PropertyChangingEventArgs result;
            if (_cachePropertyChanging.TryGetValue(propertyName, out result))
                return result;
            result = new PropertyChangingEventArgs(propertyName);
            _cachePropertyChanging.TryAdd(propertyName, result);
            return result;
        }
    }
}