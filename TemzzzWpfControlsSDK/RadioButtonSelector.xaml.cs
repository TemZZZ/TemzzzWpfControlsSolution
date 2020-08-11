﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TemzzzWpfControlsSDK
{
    /// <summary>
    /// Interaction logic for RadioButtonSelector.xaml
    /// </summary>
    public partial class RadioButtonSelector : UserControl
    {
        #region -- Dependency properties --

        public static readonly
            DependencyProperty CheckedRadioButtonIndexProperty;

        public static readonly DependencyProperty EditableTextProperty;

        public static readonly
            DependencyProperty IsReadOnlyTextBlockVisibleProperty;

        public static readonly
            DependencyProperty IsEditableTextBoxVisibleProperty;

        public static readonly
            DependencyProperty RadioButtonTextToReadOnlyTextMapProperty;

        #endregion

        #region -- Constructors --

        static RadioButtonSelector()
        {
            CheckedRadioButtonIndexProperty = DependencyProperty.Register(
                nameof(CheckedRadioButtonIndex), typeof(int?),
                typeof(RadioButtonSelector), new PropertyMetadata(null));

            EditableTextProperty = DependencyProperty.Register(
                nameof(EditableText), typeof(string),
                typeof(RadioButtonSelector),
                new PropertyMetadata(string.Empty));

            IsReadOnlyTextBlockVisibleProperty = DependencyProperty.Register(
                nameof(IsReadOnlyTextBlockVisible), typeof(bool),
                typeof(RadioButtonSelector), new PropertyMetadata(true));

            IsEditableTextBoxVisibleProperty = DependencyProperty.Register(
                nameof(IsEditableTextBoxVisible), typeof(bool),
                typeof(RadioButtonSelector), new PropertyMetadata(true));

            RadioButtonTextToReadOnlyTextMapProperty = DependencyProperty
                .Register(nameof(RadioButtonTextToReadOnlyTextMap),
                    typeof(List<(string, string)>),
                    typeof(RadioButtonSelector),
                    new PropertyMetadata(new List<(string, string)>()));
        }

        public RadioButtonSelector()
        {
            InitializeComponent();
        }

        #endregion

        #region -- Public properties --

        public int? CheckedRadioButtonIndex
        {
            get => (int?)GetValue(CheckedRadioButtonIndexProperty);
            set => SetValue(CheckedRadioButtonIndexProperty, value);
        }

        public string EditableText
        {
            get => (string)GetValue(EditableTextProperty);
            set => SetValue(EditableTextProperty, value);
        }

        public bool IsReadOnlyTextBlockVisible
        {
            get => (bool)GetValue(IsReadOnlyTextBlockVisibleProperty);
            set => SetValue(IsReadOnlyTextBlockVisibleProperty, value);
        }

        public bool IsEditableTextBoxVisible
        {
            get => (bool)GetValue(IsEditableTextBoxVisibleProperty);
            set => SetValue(IsEditableTextBoxVisibleProperty, value);
        }

        public List<(string, string)> RadioButtonTextToReadOnlyTextMap
        {
            set => SetValue(RadioButtonTextToReadOnlyTextMapProperty, value);
        }

        #endregion
    }
}
