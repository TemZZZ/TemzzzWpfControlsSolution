﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TemzzzWpfControlsSDK
{
    /// <summary>
    /// Класс комбинированного элемента разметки, состоящего из набора
    /// радиокнопок, текстового блока для отображения ассоциированного с
    /// выбранной радиокнопкой текста и текстового поля для ввода данных
    /// пользователем.
    /// </summary>
    public partial class RadioButtonSelector : UserControl
    {
        #region -- Private fields --

        private readonly Dictionary<RadioButton, string>
            _radioButtonToReadOnlyTextDictionary
                = new Dictionary<RadioButton, string>();

        #endregion

        #region -- Private methods --

        /// <summary>
        /// Обработчик выбора радиокнопки. Изменяет содержимое текстового
        /// блока на текст, ассоциированный с выбранной радиокнопкой.
        /// </summary>
        /// <param name="sender">Объект, сгенерировавший событие выбора
        /// радиокнопки</param>
        /// <param name="e">Дополнительные данные события</param>
        private void OnRadioButtonChecked(object sender, EventArgs e)
        {
            _readOnlyTextBlock.Text = _radioButtonToReadOnlyTextDictionary[
                (RadioButton)sender];

            for (int i = 0; i < _radioButtonsStackPanel.Children.Count; ++i)
            {
                var radioButton
                    = (RadioButton)_radioButtonsStackPanel.Children[i];
                if (radioButton.IsChecked == true)
                {
                    CheckedRadioButtonIndex = (uint?)i;
                    return;
                }
            }
        }

        /// <summary>
        /// Устанавливает видимость элемента-экземпляра класса
        /// <see cref="UIElement"/>. Если видимость устанавливается в false,
        /// то видимость устанавливается в
        /// <see cref="Visibility.Collapsed"/>, то есть элемент не будет
        /// отображаться и не будет участвовать в разметке.
        /// </summary>
        /// <param name="uiElement">Элемент разметки</param>
        /// <param name="isVisible">Статус видимости</param>
        private static void SetUIElementVisibility(UIElement uiElement,
            bool isVisible)
        {
            switch (isVisible)
            {
                case true:
                    uiElement.Visibility = Visibility.Visible;
                    break;
                case false:
                    uiElement.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #region -- Dependency properties --

        public static readonly
            DependencyProperty CheckedRadioButtonIndexProperty;

        public static readonly DependencyProperty EditableTextProperty;

        public static readonly
            DependencyProperty IsReadOnlyTextBlockVisibleProperty;

        public static readonly
            DependencyProperty IsEditableTextBoxVisibleProperty;

        public static readonly
            DependencyProperty RadioButtonTextToReadOnlyTextTuplesProperty;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Статический конструктор. В нем регистрируются свойства
        /// зависимостей элемента <see cref="RadioButtonSelector"/>.
        /// </summary>
        static RadioButtonSelector()
        {
            CheckedRadioButtonIndexProperty = DependencyProperty.Register(
                nameof(CheckedRadioButtonIndex), typeof(uint?),
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

            RadioButtonTextToReadOnlyTextTuplesProperty = DependencyProperty
                .Register(nameof(RadioButtonTextToReadOnlyTextTuples),
                    typeof(List<(string, string)>),
                    typeof(RadioButtonSelector),
                    new PropertyMetadata(new List<(string, string)>(),
                        RadioButtonTextToReadOnlyTextTupleChangedCallback));
        }

        /// <summary>
        /// Создает экземпляр элемента <see cref="RadioButtonSelector"/>.
        /// </summary>
        public RadioButtonSelector()
        {
            InitializeComponent();

            // Если пользователь вводит в текстовое поле строку, которую
            // невозможно преобразовать в тип данных свойства, привязанного
            // к свойству Text текстового поля, то подствечиваться красной
            // рамкой будет только текстовое поле, а не весь элемент
            // RadioButtonSelector.
            Validation.SetValidationAdornerSite(this, _editableTextBox);
        }

        #endregion

        #region -- Public properties --

        /// <summary>
        /// Позволяет задать или получить индекс выбранной радиокнопки (при
        /// задании индекса выбирается соответствующая радиокнопка).
        /// </summary>
        public uint? CheckedRadioButtonIndex
        {
            get => (uint?)GetValue(CheckedRadioButtonIndexProperty);
            set => SetValue(CheckedRadioButtonIndexProperty, value);
        }

        /// <summary>
        /// Позволяет задать или получить текст редактируемого текстового
        /// поля.
        /// </summary>
        public string EditableText
        {
            get => (string)GetValue(EditableTextProperty);
            set => SetValue(EditableTextProperty, value);
        }

        /// <summary>
        /// Позволяет установить видимость нередактируемого текстового блока
        /// или получить его текущее состояние видимости.
        /// </summary>
        public bool IsReadOnlyTextBlockVisible
        {
            get => (bool)GetValue(IsReadOnlyTextBlockVisibleProperty);
            set => SetValue(IsReadOnlyTextBlockVisibleProperty, value);
        }

        /// <summary>
        /// Позволяет установить видимость редактируемого текстового поля или
        /// получить его текущее состояние видимости.
        /// </summary>
        public bool IsEditableTextBoxVisible
        {
            get => (bool)GetValue(IsEditableTextBoxVisibleProperty);
            set => SetValue(IsEditableTextBoxVisibleProperty, value);
        }

        /// <summary>
        /// Позволяет задать список пар значений
        /// "текст радиокнопки-текст текстового блока". На основании этого
        /// списка добавляются радиокнопки с соответствующим текстом, и
        /// каждой радиокнопке назначается ассоциированный с ней текст,
        /// который будет отбражатся в нередактируемом текстовом блоке при ее
        /// выборе.
        /// </summary>
        public List<(string, string)> RadioButtonTextToReadOnlyTextTuples
        {
            set => SetValue(RadioButtonTextToReadOnlyTextTuplesProperty,
                value);
        }

        #endregion

        #region -- Dependency property changed callbacks --

        private static void
            RadioButtonTextToReadOnlyTextTupleChangedCallback(
                DependencyObject sender,
                DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var radioButtonSelector = (RadioButtonSelector)sender;
            var radioButtonsStackPanelChildren
                = radioButtonSelector._radioButtonsStackPanel.Children;

            radioButtonsStackPanelChildren.Clear();
            radioButtonSelector._radioButtonToReadOnlyTextDictionary.Clear();
            radioButtonSelector.CheckedRadioButtonIndex = null;

            var newRadioButtonTextToReadOnlyTextTuples
                = (List<(string, string)>)e.NewValue;
            if (newRadioButtonTextToReadOnlyTextTuples is null
                || newRadioButtonTextToReadOnlyTextTuples.Count == 0)
            {
                return;
            }

            foreach (var (radioButtonText, readOnlyText)
                in newRadioButtonTextToReadOnlyTextTuples)
            {
                var radioButton = new RadioButton
                {
                    Content = radioButtonText
                };
                radioButtonSelector._radioButtonToReadOnlyTextDictionary[
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
            var newCheckedRadioButtonIndex = (uint?)e.NewValue;

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
            if (e.OldValue != e.NewValue)
            {
                SetUIElementVisibility(
                    ((RadioButtonSelector)sender)._readOnlyTextBlock,
                    (bool)e.NewValue);
            }
        }

        private static void EditableTextBoxVisibilityChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                SetUIElementVisibility(
                    ((RadioButtonSelector)sender)._editableTextBox,
                    (bool)e.NewValue);
            }
        }

        #endregion
    }
}
