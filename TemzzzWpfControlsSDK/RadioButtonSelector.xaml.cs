using System;
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
                typeof(RadioButtonSelector), new PropertyMetadata(null,
                    CheckedRadioButtonIndexChangedCallback));

            EditableTextProperty = DependencyProperty.Register(
                nameof(EditableText), typeof(string),
                typeof(RadioButtonSelector),
                new PropertyMetadata(string.Empty));

            IsReadOnlyTextBlockVisibleProperty = DependencyProperty.Register(
                nameof(IsReadOnlyTextBlockVisible), typeof(bool),
                typeof(RadioButtonSelector), new PropertyMetadata(true,
                    ReadOnlyTextBlockVisibilityChangedCallback));

            IsEditableTextBoxVisibleProperty = DependencyProperty.Register(
                nameof(IsEditableTextBoxVisible), typeof(bool),
                typeof(RadioButtonSelector), new PropertyMetadata(true,
                    EditableTextBoxVisibilityChangedCallback));

            RadioButtonTextToReadOnlyTextMapProperty = DependencyProperty
                .Register(nameof(RadioButtonTextToReadOnlyTextMap),
                    typeof(List<(string, string)>),
                    typeof(RadioButtonSelector),
                    new PropertyMetadata(new List<(string, string)>(),
                        RadioButtonTextToReadOnlyTextMapChangedCallback));
        }

        public RadioButtonSelector()
        {
            InitializeComponent();
            Validation.SetValidationAdornerSite(this, _editableTextBox);
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

        #region -- Dependency property changed callbacks --

        private static void RadioButtonTextToReadOnlyTextMapChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var radioButtonSelector = (RadioButtonSelector)sender;
            var radioButtonsStackPanelChildren
                = radioButtonSelector._radioButtonsStackPanel.Children;

            foreach (RadioButton radioButton
                in radioButtonsStackPanelChildren)
            {
                radioButton.Checked
                    -= radioButtonSelector.OnRadioButtonChecked;
            }
            radioButtonsStackPanelChildren.Clear();
            radioButtonSelector._radioButtonToReadOnlyTextMap.Clear();
            radioButtonSelector.CheckedRadioButtonIndex = null;

            var newRadioButtonTextToReadOnlyTextMap
                = (List<(string, string)>)e.NewValue;
            if (newRadioButtonTextToReadOnlyTextMap is null
                || newRadioButtonTextToReadOnlyTextMap.Count == 0)
            {
                return;
            }

            foreach (var (radioButtonText, readOnlyText)
                in newRadioButtonTextToReadOnlyTextMap)
            {
                var radioButton = new RadioButton
                {
                    Content = radioButtonText
                };
                radioButton.Checked
                    += radioButtonSelector.OnRadioButtonChecked;
                radioButtonSelector._radioButtonToReadOnlyTextMap[
                    radioButton] = readOnlyText;
                radioButtonsStackPanelChildren.Add(radioButton);
            }
        }

        private static void CheckedRadioButtonIndexChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var radioButtonSelector = (RadioButtonSelector)sender;
            var radioButtonsStackPanelChildren
                = radioButtonSelector._radioButtonsStackPanel.Children;
            var newCheckedRadioButtonIndex = (int?)e.NewValue;

            if (newCheckedRadioButtonIndex == null)
            {
                foreach (RadioButton radioButton
                    in radioButtonsStackPanelChildren)
                {
                    if (radioButton.IsChecked == true)
                    {
                        radioButton.IsChecked = false;
                        break;
                    }
                }
                return;
            }

            if (radioButtonsStackPanelChildren.Count == 0)
            {
                throw new ArgumentException(
                    "There are no radiobuttons yet. Radiobuttons count " +
                    "must be more or equal to 1.",
                    nameof(newCheckedRadioButtonIndex));
            }

            if (newCheckedRadioButtonIndex < 0
                || (newCheckedRadioButtonIndex
                    >= radioButtonsStackPanelChildren.Count))
            {
                throw new ArgumentException(
                    "The value of checked radiobutton index must be null " +
                    "or in range from 0 to " +
                    $"{radioButtonsStackPanelChildren.Count - 1}, but it " +
                    $"is {newCheckedRadioButtonIndex}.",
                    nameof(newCheckedRadioButtonIndex));
            }

            var newCheckedRadioButton
                = (RadioButton)radioButtonsStackPanelChildren[
                    (int)newCheckedRadioButtonIndex];
            if (newCheckedRadioButton.IsChecked != true)
            {
                newCheckedRadioButton.IsChecked = true;
            }
        }

        private static void ReadOnlyTextBlockVisibilityChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var radioButtonSelector = (RadioButtonSelector)sender;
            switch (e.NewValue)
            {
                case true:
                    radioButtonSelector._readOnlyTextBlock.Visibility
                        = Visibility.Visible;
                    break;
                case false:
                    radioButtonSelector._readOnlyTextBlock.Visibility
                        = Visibility.Collapsed;
                    break;
            }
        }

        private static void EditableTextBoxVisibilityChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var radioButtonSelector = (RadioButtonSelector)sender;
            switch (e.NewValue)
            {
                case true:
                    radioButtonSelector._editableTextBox.Visibility
                        = Visibility.Visible;
                    break;
                case false:
                    radioButtonSelector._editableTextBox.Visibility
                        = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        private void OnRadioButtonChecked(object sender, EventArgs e)
        {
            _readOnlyTextBlock.Text
                = _radioButtonToReadOnlyTextMap[(RadioButton)sender];

            for (int i = 0; i < _radioButtonsStackPanel.Children.Count; ++i)
            {
                var radioButton
                    = (RadioButton)_radioButtonsStackPanel.Children[i];
                if (radioButton.IsChecked == true)
                {
                    CheckedRadioButtonIndex = i;
                    return;
                }
            }
        }

        private Dictionary<RadioButton, string> _radioButtonToReadOnlyTextMap
            = new Dictionary<RadioButton, string>();
    }
}
