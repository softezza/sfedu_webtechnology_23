#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using UnityEngine;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endif

namespace SmartTwin.NoesisGUI.Controls
{
	/// <summary>
	/// Кнопка в виде иконки (без текстовое описания)
	/// </summary>
	public class IconButton : Button
	{
		/// <summary>
		/// Геометрия иконки
		/// </summary>
		public static readonly DependencyProperty DataProperty;

		/// <summary>
		/// Формат заполнения геометрией пространства кнопки
		/// </summary>
		public static readonly DependencyProperty StretchProperty;


		static IconButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));

			DataProperty = DependencyProperty.Register(nameof(Data), typeof(Geometry), typeof(IconButton), new PropertyMetadata());
            StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(IconButton), new PropertyMetadata());
		}


		/// <summary>
		/// Геометрия иконки
		/// </summary>
		public Geometry Data
		{
			get => (Geometry)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		/// <summary>
		/// Формат заполнения геометрией пространства кнопки
		/// </summary>
		public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }
    }
}
