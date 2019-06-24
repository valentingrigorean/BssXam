//
//  RadioButton.xaml.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2018 Songurov Fiodor
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace Bss.XamForms.Input
{
    public class RadioButtonGroupView: StackLayout
    {
        public RadioButtonGroupView()
        {
            this.ChildAdded += RadioButtonGroupView_ChildAdded;
            this.ChildrenReordered += RadioButtonGroupView_ChildrenReordered;
        }

        #region BindableProperties
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItem = nv);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioButtonGroupView), -1, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedIndex = (int)nv);
        public static readonly BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItemChangedCommand = (ICommand)nv);
        #endregion

        public event EventHandler SelectedItemChanged;

        public event EventHandler ValidationChanged;


        public ICommand SelectedItemChangedCommand { get; set; }

        public object CommandParameter { get; set; }
        private void RadioButtonGroupView_ChildrenReordered(object sender, EventArgs e)
        {
            UpdateAllEvent();
        }
        private void UpdateAllEvent()
        {
            foreach (var item in Children)
            {
                if (item is RadioButton)
                {
                    (item as RadioButton).Clicked -= UpdateSelected;
                    (item as RadioButton).Clicked += UpdateSelected;
                }
            }
        }
        private void RadioButtonGroupView_ChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is RadioButton)
            {
                (e.Element as RadioButton).Clicked -= UpdateSelected;
                (e.Element as RadioButton).Clicked += UpdateSelected;
            }
        }
        private void UpdateSelected(object selected, EventArgs e)
        {
            foreach (var item in Children)
            {
                if (item is RadioButton)
                    (item as RadioButton).IsChecked = item == selected;
            }

            SetValue(SelectedItemProperty, SelectedItem);
            OnPropertyChanged(nameof(SelectedItem));
            SetValue(SelectedIndexProperty, SelectedIndex);
            OnPropertyChanged(nameof(SelectedIndex));
            SelectedItemChanged?.Invoke(this, new EventArgs());
            if (SelectedItemChangedCommand?.CanExecute(CommandParameter ?? this) ?? false) ;
            SelectedItemChangedCommand?.Execute(CommandParameter ?? this);
            ValidationChanged?.Invoke(this, new EventArgs());
        }

        public int SelectedIndex
        {
            get
            {
                int index = 0;
                foreach (var item in Children)
                {
                    if (item is RadioButton)
                    {
                        if ((item as RadioButton).IsChecked)
                            return index;
                        index++;
                    }
                }
                return -1;
            }
            set
            {
                int index = 0;
                foreach (var item in Children)
                {
                    if (item is RadioButton)
                    {
                        (item as RadioButton).IsChecked = index == value;
                        index++;
                    }
                }
            }
        }

        public object SelectedItem
        {
            get
            {
                foreach (var item in Children)
                {
                    if (item is RadioButton && (item as RadioButton).IsChecked)
                        return (item as RadioButton).Value;
                }
                return null;
            }
            set
            {
                foreach (var item in Children)
                {
                    if (item is RadioButton)
                        (item as RadioButton).IsChecked = (item as RadioButton).Value == value;
                }
            }
        }

        public bool IsRequired { get; set; }

        public bool IsValidated { get => !IsRequired || SelectedIndex >= 0; }

        public string ValidationMessage { get; set; }
    }

    public partial class RadioButton: ContentView
    {
        public RadioButton()
        {
            InitializeComponent();

            controlLabel.BindingContext = this;

            checkedBackground.BindingContext = this;
            checkedImage.BindingContext = this;
            borderImage.BindingContext = this;

            mainContainer.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(Tapped)
            });
        }

        #region BindableProperties

        public static readonly BindableProperty UncheckResourceProperty = BindableProperty.Create(nameof(UncheckResource), typeof(string), typeof(RadioButton), "", BindingMode.TwoWay);
        public static readonly BindableProperty CheckedResourceProperty = BindableProperty.Create(nameof(CheckedResource), typeof(string), typeof(RadioButton), "", BindingMode.TwoWay);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButton), false, BindingMode.TwoWay, propertyChanged: CheckedPropertyChanged);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioButton), "", BindingMode.TwoWay);
        public static readonly BindableProperty CheckedChangedCommandProperty = BindableProperty.Create(nameof(CheckedChangedCommand), typeof(Command), typeof(RadioButton));

        #endregion

        public string UncheckResource
        {
            get { return (string)GetValue(UncheckResourceProperty); }
            set { SetValue(UncheckResourceProperty, value); }
        }

        public string CheckedResource
        {
            get { return (string)GetValue(CheckedResourceProperty); }
            set { SetValue(CheckedResourceProperty, value); }
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Command CheckedChangedCommand
        {
            get { return (Command)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }

        public event EventHandler Clicked;

        public object Value { get; set; }

        private static void CheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((RadioButton)bindable).ApplyCheckedState();
        }

        private void Tapped()
        {
            IsChecked = !IsChecked;
            Clicked?.Invoke(this, new EventArgs());
            SetCheckedState(IsChecked);
            if (CheckedChangedCommand != null && CheckedChangedCommand.CanExecute(this))
                CheckedChangedCommand.Execute(this ?? Value);
        }

        private void SetCheckedState(bool isChecked)
        {
            var storyboard = new Animation();
            double fadeStartVal;
            double fadeEndVal;
            double scaleStartVal;
            double scaleEndVal;
            Easing checkEasing;

            if (isChecked)
            {
                checkedImage.Scale = 0;
                fadeStartVal = 0;
                fadeEndVal = 1;
                scaleStartVal = 0;
                scaleEndVal = 1;
                checkEasing = Easing.CubicIn;
            }
            else
            {
                fadeStartVal = 1;
                fadeEndVal = 0;
                scaleStartVal = 1;
                scaleEndVal = 0;
                checkEasing = Easing.CubicOut;
            }
            var fadeAnim = new Animation(
                callback: d => checkedBackground.Opacity = d,
                start: fadeStartVal,
                end: fadeEndVal,
                easing: Easing.CubicOut
            );
            var checkFadeAnim = new Animation(
                callback: d => checkedImage.Opacity = d,
                start: fadeStartVal,
                end: fadeEndVal,
                easing: checkEasing
            );
            var checkBounceAnim = new Animation(
                callback: d => checkedImage.Scale = d,
                start: scaleStartVal,
                end: scaleEndVal,
                easing: checkEasing
            );

            storyboard.Add(0, 0.6, fadeAnim);
            storyboard.Add(0, 0.6, checkFadeAnim);
            storyboard.Add(0.4, 1, checkBounceAnim);
            storyboard.Commit(this, "checkAnimation", length: 400);
        }

        private void ApplyCheckedState()
        {
            SetCheckedState(IsChecked);
        }
    }
}
