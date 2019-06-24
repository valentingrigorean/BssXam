//
// asd.cs
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
using System.Reactive;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Bss.ReactiveMvvmCross.Cache;
using MvvmCross.ViewModels;
using ReactiveUI;
using INotifyPropertyChanging = ReactiveUI.INotifyPropertyChanging;
using PropertyChangingEventArgs = ReactiveUI.PropertyChangingEventArgs;
using PropertyChangingEventHandler = ReactiveUI.PropertyChangingEventHandler;

namespace Bss.ReactiveMvvmCross.ViewModels
{
    public abstract class MvxReactiveViewModel : MvxReactiveViewModel<Unit, Unit>
    {
        public override void Prepare(Unit parameter)
        {

        }
    }

    public abstract class MvxReactiveViewModel<TParam> : MvxReactiveViewModel<TParam, Unit>
    {
    }

    public abstract class MvxReactiveViewModel<TParam, TResult> : MvxViewModel<TParam, TResult>,
        IReactiveNotifyPropertyChanged<IReactiveObject>,
        IReactiveViewModel
    {
        private readonly MvxReactiveObject _reactiveObj = new MvxReactiveObject();
        private bool _isBusy;


        public virtual IDisposable SuppressChangeNotifications()
        {
            return _reactiveObj.SuppressChangeNotifications();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => _reactiveObj.Changing;
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => _reactiveObj.Changed;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add => ((IReactiveObject)_reactiveObj).PropertyChanging += value;
            remove => ((IReactiveObject)_reactiveObj).PropertyChanging -= value;
        }

        public new event PropertyChangedEventHandler PropertyChanged
        {
            add => ((INotifyPropertyChanged)_reactiveObj).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)_reactiveObj).PropertyChanged -= value;
        }

        public virtual void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            var newArgs = PropertyEventArgsCache.Instance.GetPropertyChangingEventArgs(args.PropertyName);
            if (InterceptRaisePropertyChanging(newArgs) == MvxInpcInterceptionResult.NotIntercepted)
                _reactiveObj.RaisePropertyChanging(args.PropertyName);
        }

        public IDisposable WireCommand<TRXParam, TRXResult>(ReactiveCommand<TRXParam, TRXResult> command, bool wireLoading = true, bool wireError = true)
        {
            var disp = new CompositeDisposable();

            if (wireLoading)
                command.IsExecuting
                    .BindTo(this, vm => vm.IsBusy)
                    .DisposeWith(disp);

            if (wireError)
                command.ThrownExceptions
                    .Subscribe(ex => OnError(ex, command))
                    .DisposeWith(disp);

            return disp;
        }

        public virtual void OnError(Exception ex, object context)
        {

        }


        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            if (InterceptRaisePropertyChanged(args) == MvxInpcInterceptionResult.NotIntercepted)
                ((IReactiveObject)_reactiveObj).RaisePropertyChanged(args);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var original = storage;

            if (ShouldRaisePropertyChanging())
                IReactiveObjectExtensions.RaiseAndSetIfChanged(this, ref storage, value, propertyName);
            else
                storage = value;
            return !EqualityComparer<T>.Default.Equals(original, value);
        }
    }
}