#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endif


namespace SmartTwin.NoesisGUI.Behaviors
{
	/// <summary>
	/// Поведение для выставления свойства Margin за пределами контейнера
	/// ToDo: подумать над переносом в Attached Property
	/// </summary>
	public class OutsideMarginBehavior : Behavior<FrameworkElement>
	{
		/// <summary>
		/// Горизонтальная привязка к границе
		/// </summary>
		public readonly static DependencyProperty HorizontalOutsideProperty;

		/// <summary>
		/// Вертикальная привязка к границе
		/// </summary>
		public readonly static DependencyProperty VerticalOutsideProperty;

		/// <summary>
		/// Горизонтальное смещение
		/// </summary>
		public readonly static DependencyProperty HorizontalOffsetProperty;

		/// <summary>
		/// Вертикальное смещение
		/// </summary>
		public readonly static DependencyProperty VerticalOffsetProperty;


		static OutsideMarginBehavior()
		{
			HorizontalOutsideProperty = DependencyProperty.Register(
				nameof(HorizontalOutside),
				typeof(HorizontalOutside),
				typeof(OutsideMarginBehavior),
				new PropertyMetadata(HorizontalOutside.None, OnHorizontalOutsideChanged));


			VerticalOutsideProperty = DependencyProperty.Register(
				nameof(VerticalOutside),
				typeof(VerticalOutside),
				typeof(OutsideMarginBehavior),
				new PropertyMetadata(VerticalOutside.None, OnVerticalOutsideChanged));

			HorizontalOffsetProperty = DependencyProperty.Register(
				nameof(HorizontalOffset),
				typeof(float),
				typeof(OutsideMarginBehavior),
				new PropertyMetadata(0f, OnHorizontalOffsetChanged));

			VerticalOffsetProperty = DependencyProperty.Register(
                nameof(VerticalOffset),
                typeof(float),
                typeof(OutsideMarginBehavior),
                new PropertyMetadata(0f, OnVerticalOffsetChanged));
		}


		/// <summary>
		/// Горизонтальная привязка к границе
		/// </summary>
		public HorizontalOutside HorizontalOutside
		{
			get => (HorizontalOutside)GetValue(HorizontalOutsideProperty);
			set => SetValue(HorizontalOutsideProperty, value);
		}

		/// <summary>
		/// Вертикальная привязка к границе
		/// </summary>
		public VerticalOutside VerticalOutside
		{
			get => (VerticalOutside)GetValue(VerticalOutsideProperty);
			set => SetValue(VerticalOutsideProperty, value);
		}

		/// <summary>
		/// Горизонтальное смещение
		/// </summary>
		public float HorizontalOffset
		{
			get => (float)GetValue(HorizontalOffsetProperty);
			set => SetValue(HorizontalOffsetProperty, value);
		}

		/// <summary>
		/// Вертикальное смещение
		/// </summary>
		public float VerticalOffset
		{
			get => (float)GetValue(VerticalOffsetProperty);
			set => SetValue(VerticalOffsetProperty, value);
		}

		/// <summary>
		/// Вызывается при прикреплении к объекту
		/// </summary>
		protected override void OnAttached()
		{
			var element = AssociatedObject;

			element.Loaded += OnElementLoaded;
			element.SizeChanged += OnElementSizeChanged;
		}

		/// <summary>
		/// Вызывается при открепления от элемента
		/// </summary>
		protected override void OnDetaching()
		{
			var element = AssociatedObject;

			element.Loaded -= OnElementLoaded;
			element.SizeChanged -= OnElementSizeChanged;
		}

		
		/// <summary>
		/// Вызывается при событии <see cref="FrameworkElement.Loaded"/>
		/// </summary>
		/// <param name="sender">Источник события</param>
		/// <param name="e"></param>
		private void OnElementLoaded(object sender, RoutedEventArgs e)
		{
			var element = AssociatedObject;

			if (element == sender)
				UpdateMargin(element, this);

			element.Loaded -= OnElementLoaded;
		}

		/// <summary>
		/// Вызывается при событии <see cref="FrameworkElement.SizeChanged"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var element = AssociatedObject;

			if (element == sender)
				UpdateMargin(element, this);
		}


		/// <summary>
		/// Вызывается при изменении свойства <see cref="HorizontalOutside"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnHorizontalOutsideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = d as OutsideMarginBehavior;
			var element = behavior.AssociatedObject;

			if (element == null)
				return;

			UpdateMargin(element, behavior);
		}

		/// <summary>
		/// Вызывается при изменении свойства <see cref="VerticalOutside"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnVerticalOutsideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = d as OutsideMarginBehavior;
			var element = behavior.AssociatedObject;

			if (element == null)
				return;

			UpdateMargin(element, behavior);
		}

		/// <summary>
		/// Вызывается при изменении свойства <see cref="HorizontalOffset"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = d as OutsideMarginBehavior;
			var element = behavior.AssociatedObject;

			if (element == null)
				return;

			UpdateMargin(element, behavior);
		}

		/// <summary>
		/// Вызывается при изменении свойства <see cref="VerticalOffset"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = d as OutsideMarginBehavior;
			var element = behavior.AssociatedObject;

			if (element == null)
				return;

			UpdateMargin(element, behavior);
		}

		/// <summary>
		/// Обновить Margin
		/// </summary>
		/// <param name="element">Обновляемый элемент</param>
		/// <param name="behavior">Его поведение</param>
		private static void UpdateMargin(FrameworkElement element, OutsideMarginBehavior behavior)
		{
			var horizontalOutside = behavior.HorizontalOutside;
			var verticalOutside = behavior.VerticalOutside;

			var horizontalOffset = -(element.ActualWidth + behavior.HorizontalOffset);
			var verticalOffset = -(element.ActualHeight + behavior.VerticalOffset);

			switch (horizontalOutside)
			{
				case HorizontalOutside.Left:
					element.HorizontalAlignment = HorizontalAlignment.Left;
					UpdateHorizontalMargin(element, horizontalOffset, 0);
					break;
				case HorizontalOutside.Right:
					element.HorizontalAlignment = HorizontalAlignment.Right;
					UpdateHorizontalMargin(element, 0, horizontalOffset);
					break;
				case HorizontalOutside.None:
					break;
			}

			switch (verticalOutside)
			{
				case VerticalOutside.Top:
					element.VerticalAlignment = VerticalAlignment.Top;
					UpdateVerticalMargin(element, verticalOffset, 0);
					break;
				case VerticalOutside.Bottom:
					element.VerticalAlignment = VerticalAlignment.Bottom;
					UpdateVerticalMargin(element, 0, verticalOffset);
					break;
				case VerticalOutside.None:
					break;
			}
		}

#if NOESIS
		/// <summary>
		/// Обновить горизонтальный margin
		/// </summary>
		/// <param name="element">Обновляемый элемент</param>
		/// <param name="left">Смещение влево</param>
		/// <param name="right">Смещение вправо</param>
		private static void UpdateHorizontalMargin(FrameworkElement element, float left, float right)
		{
			var margin = element.Margin;

			margin.Left = left;
			margin.Right = right;

			element.Margin = margin;
		}

		/// <summary>
		/// Обновить вертикальный margin
		/// </summary>
		/// <param name="element">Обновляемый элемент</param>
		/// <param name="top">Смещение вверх</param>
		/// <param name="bottom">Смещение вниз</param>
		private static void UpdateVerticalMargin(FrameworkElement element, float top, float bottom)
		{
			var margin = element.Margin;

			margin.Top = top;
			margin.Bottom = bottom;

			element.Margin = margin;
		}
#else
		private static void UpdateHorizontalMargin(FrameworkElement element, double left, double right)
		{
			var margin = element.Margin;

			margin.Left = left;
			margin.Right = right;

			element.Margin = margin;
		}

		private static void UpdateVerticalMargin(FrameworkElement element, double top, double bottom)
		{
			var margin = element.Margin;

			margin.Top = top;
			margin.Bottom = bottom;

			element.Margin = margin;
		}
#endif


	}

	/// <summary>
	/// Горизонтальная привязка к границе
	/// </summary>
	public enum HorizontalOutside
	{
		None,
		Left,
		Right
	}

	/// <summary>
	/// Вертикальная привязка к границе
	/// </summary>
	public enum VerticalOutside
	{
		None,
		Top,
		Bottom,
	}
}
