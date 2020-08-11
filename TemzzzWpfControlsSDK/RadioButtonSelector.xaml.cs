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
        #region -- Private dependency properties --

        private static readonly
            DependencyProperty CheckedRadioButtonIndexProperty;
        public static readonly DependencyProperty EditableTextProperty;

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

        #endregion
    }
}
