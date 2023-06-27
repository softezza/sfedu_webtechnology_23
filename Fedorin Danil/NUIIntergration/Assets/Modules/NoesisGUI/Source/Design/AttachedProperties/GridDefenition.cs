#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
#endif
using System.Linq;
using System.Collections.Generic;

namespace SmartTwin.NoesisGUI.AttachedProperties
{
    /// <summary>
    /// Класс с прикрепляемыми свойствами для удобства разметки <see cref="Grid"/>
    /// </summary>
    public class GridDefenition
    {
        /// <summary>
        /// Свойство зависимости для привязки к колоннам
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridDefenition),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, OnDefenisionChanged));

        /// <summary>
        /// Свойство зависимости для привязки к рядам
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(GridDefenition),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, OnDefenisionChanged));

        /// <summary>
        /// Получить колонны
        /// </summary>
        /// <param name="target">Элемент, у которого нужно получить колонны</param>
        /// <returns></returns>
        public static string GetColumns(UIElement target) =>
            (string)target.GetValue(ColumnsProperty);

        /// <summary>
        /// Установить колонны
        /// </summary>
        /// <param name="target">Элемент, которому устанавливаются колонны</param>
        /// <param name="value">Новое значение колонн</param>
        public static void SetColumns(UIElement target, string value) =>
            target.SetValue(ColumnsProperty, value);

		/// <summary>
		/// Получить ряды
		/// </summary>
		/// <param name="target">Элемент, у которого нужно получить ряды</param>
		/// <returns></returns>
		public static string GetRows(UIElement target) =>
           (string)target.GetValue(RowsProperty);

		/// <summary>
		/// Установить ряды
		/// </summary>
		/// <param name="target">Элемент, которому устанавливаются ряды</param>
		/// <param name="value">Новое значение рядов</param>
		public static void SetRows(UIElement target, string value) =>
            target.SetValue(RowsProperty, value);


        /// <summary>
        /// Вызывается при изменении описания колонн или рядов.
        /// </summary>
        /// <param name="d">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private static void OnDefenisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
                return;

            var value = e.NewValue as string;
            if (string.IsNullOrEmpty(value))
                return;

            if (e.Property == ColumnsProperty)
            {
                grid.ColumnDefinitions.Clear();
                var defenitions = ParseString(value);
                foreach (var defenition in defenitions)
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = defenition });
            }

            if (e.Property == RowsProperty)
            {
                grid.RowDefinitions.Clear();
                var defenitions = ParseString(value);
                foreach (var defenition in defenitions)
                    grid.RowDefinitions.Add(new RowDefinition() { Height = defenition });
            }
        }

        /// <summary>
        /// Распарсить строковое представление колонн или рядов в эквивалентное значение их размеров
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static IEnumerable<GridLength> ParseString(string s)
        {
#if NOESIS
            return s.Split(',').Select(p => GridLength.Parse(p.Trim())); 
#else
            var converter = new GridLengthConverter();
            return s.Split(',').Select(p => (GridLength)converter.ConvertFromString(p.Trim()));
#endif
        }
    }
}