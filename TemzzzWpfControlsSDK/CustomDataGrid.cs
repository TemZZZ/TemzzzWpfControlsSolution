using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TemzzzWpfControlsSDK
{
    /// <summary>
    /// Класс элемента управления содержимым, представляющий таблицу
    /// <see cref="DataGrid"/>, но имеющий возможность привязки к выделенным
    /// элементам, чего нет в обычном <see cref="DataGrid"/>. Также имеется
    /// возможность задавать имена столбцов таблицы описаниями из атрибутов
    /// <see cref="DescriptionAttribute"/> свойств объектов коллекции,
    /// привязанной к таблице.
    /// </summary>
    public class CustomDataGrid : DataGrid
    {
        #region -- Dependency properties --

        /// <summary>
        /// Свойство зависимости, к которому можно привязаться,
        /// представляющее выделенные элементы таблицы.
        /// </summary>
        public static readonly DependencyProperty
            BindableSelectedItemsProperty;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Регистрирует свойства зависимостей.
        /// </summary>
        static CustomDataGrid()
        {
            BindableSelectedItemsProperty = DependencyProperty.Register(
                nameof(BindableSelectedItems), typeof(IList),
                typeof(CustomDataGrid), new PropertyMetadata(null));
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="CustomDataGrid"/>.
        /// </summary>
        public CustomDataGrid()
        {
            SelectionChanged += OnCustomDataGridSelectionChanged;
            AutoGeneratingColumn += OnCustomDataGridAutoGeneratingColumn;

            // Принудительно выделяет первый элемент таблицы при первом
            // добавлении новых элементов. Без этой строки кода привязанное к
            // BindableSelectedItems свойство вьюмодели будет равно null,
            // если пользователь еще ни разу не выделил ни одной строки в
            // таблице, а должно быть равно пустой коллекции объектов.
            SelectedIndex = 0;
        }

        #endregion

        #region -- Public properties --

        /// <summary>
        /// Позволяет получить или задать выделенные элементы таблицы.
        /// </summary>
        public IList BindableSelectedItems
        {
            get => (IList)GetValue(BindableSelectedItemsProperty);
            set => SetValue(BindableSelectedItemsProperty, value);
        }

        #endregion

        #region -- Auxiliary private methods --

        /// <summary>
        /// Срабатывает при изменении выделения таблицы. Присваивает свойству
        /// <see cref="BindableSelectedItems"/> выделенные элементы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomDataGridSelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            BindableSelectedItems = SelectedItems;
        }

        /// <summary>
        /// Изменяет заголовок создаваемого столбца таблицы с имени свойства
        /// объектов коллекции, привязанной к таблице, на описание из
        /// атрибута <see cref="DescriptionAttribute"/>, если этот атрибут
        /// назначен свойству, и описание в нем не равно пустой строке.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomDataGridAutoGeneratingColumn(object sender,
            DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = e.PropertyDescriptor
                as PropertyDescriptor;
            var description = propertyDescriptor?.Description;
            if (!string.IsNullOrEmpty(description))
            {
                e.Column.Header = description;
            }
        }

        #endregion
    }
}
