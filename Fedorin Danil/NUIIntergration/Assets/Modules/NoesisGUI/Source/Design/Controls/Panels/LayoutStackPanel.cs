#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using System;

using Float = System.Single;
#else
using SmartTwin.NoesisGUI.Tools;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Float = System.Double;
#endif

namespace SmartTwin.NoesisGUI.Controls
{
	/// <summary>
	/// Расширенный <see cref="StackPanel"/> c механизмом дочерней разметки
	/// <br>ToDo: Наследуемый от <see cref="Canvas"/>, так как в контексте Noesis нет возможности переопределить метод GetLayoutClip, который позволял элементу отрисовываться за пределами контейнера.</br>
	/// </summary>
	public class LayoutStackPanel : Canvas
	{
		/// <summary>
		/// Ориентация размещения дочерних элементов.
		/// <br><see cref="Orientation.Vertical"/>, если нужно размещать вертикально.</br>
		/// <br><see cref="Orientation.Horizontal"/>, если нужно размещать горизонтально.</br>
		/// </summary>
		public static DependencyProperty OrientationProperty;

		/// <summary>
		/// Длина пространства между дочерними элементам.
		/// </summary>
		public static DependencyProperty SpacingProperty;


		static LayoutStackPanel()
		{
			OrientationProperty = DependencyProperty.Register(
				nameof(Orientation),
				typeof(Orientation),
				typeof(LayoutStackPanel), 
				new FrameworkPropertyMetadata(
					Orientation.Vertical, 
					FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsMeasure));

			SpacingProperty = DependencyProperty.Register(
				nameof(Spacing),
				typeof(float),
				typeof(LayoutStackPanel),
				 new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
		}


		/// <summary>
		/// Ориентация размещения дочерних элементов.
		/// <br><see cref="Orientation.Vertical"/>, если нужно размещать вертикально.</br>
		/// <br><see cref="Orientation.Horizontal"/>, если нужно размещать горизонтально.</br>
		/// </summary>
		public Orientation Orientation
		{
			get => (Orientation)GetValue(OrientationProperty);
			set => SetValue(OrientationProperty, value);
		}

		/// <summary>
		/// Длина пространства между дочерними элементам.
		/// </summary>
		public float Spacing
		{
			get => (float)GetValue(SpacingProperty);
			set => SetValue(SpacingProperty, value);
		}

		/// <summary>
		/// Метод вычисляет собственный размер и размеры дочерних элементов
		/// </summary>
		/// <param name="constraint">Исходная ограничивающая рамка</param>
		/// <returns>Новый размер рамки</returns>
		protected override Size MeasureOverride(Size constraint)
		{
			if (Orientation == Orientation.Vertical)
				return MeasureVertical(constraint);
			else
				return MeasureHorizontal(constraint);
		}

		/// <summary>
		/// Предоставляет финальный размер контейнера и рассчитывает размеры и позиции для дочерних элементов
		/// </summary>
		/// <param name="finalSize">Исходный размер контейнера</param>
		/// <returns>Новый размер контейнера</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (Orientation == Orientation.Vertical)
				ArrangeVertical(finalSize);
			else
				ArrangeHorizontal(finalSize);

			return finalSize;
		}


		/// <summary>
		/// Вычислить собственный и дочерние размеры при вертикальной ориентации
		/// </summary>
		/// <param name="constraint">Текущая ограничивающая рамка</param>
		/// <returns>Новая ограничивающая рамка</returns>
		private Size MeasureVertical(Size constraint)
		{
			var newSize = default(Size);
			var avialibleElementSize = new Size(float.PositiveInfinity, float.PositiveInfinity);
			var spacing = Spacing;
			var width = Width;
			var newWidth = default(Float);

			var childrens = Children;

			var count = childrens.Count;
			for (int i = 0; i < count; i++)
			{
				var element = childrens[i];
				if (element == null)
					continue; 

				element.Measure(avialibleElementSize);
				var desiredSize = element.DesiredSize;

				newWidth = Math.Max(newWidth, desiredSize.Width);
				newSize.Height += desiredSize.Height;

				if (i != count - 1)
					newSize.Height += spacing;
			}

			if (Float.IsNaN(width) || Float.IsInfinity(width))
				newSize.Width = newWidth;
			else
				newSize.Width = width;

			return newSize;
		}

		/// <summary>
		/// Вычислить собственный и дочерние размеры при горизонтальной ориентации
		/// </summary>
		/// <param name="constraint">Текущая ограничивающая рамка</param>
		/// <returns>Новая ограничивающая рамка</returns>
		private Size MeasureHorizontal(Size constraint)
		{
			var newSize = default(Size);
			var avialibleElementSize = new Size(float.PositiveInfinity, float.PositiveInfinity);
			var spacing = Spacing;
			var height = Height;
			var newHeight = default(Float);

			var childrens = Children;

			var count = childrens.Count;
			for (int i = 0; i < count; i++)
			{
				var element = childrens[i];
				if (element == null)
					continue;

				element.Measure(avialibleElementSize);
				var desiredSize = element.DesiredSize;

				newHeight = Math.Max(newHeight, desiredSize.Height);
				newSize.Width += desiredSize.Width;

				if (i != count - 1)
					newSize.Width += spacing;
			}

			if (Float.IsNaN(height) || Float.IsInfinity(height))
				newSize.Height = newHeight;
			else
				newSize.Height = height;

			return newSize;
		}

		/// <summary>
		/// Предоставляет финальный размер контейнера и рассчитывает размеры и позиции для дочерних элементов при вертикальной ориентации
		/// </summary>
		/// <param name="finalSize">Исходный размер контейнера</param>
		private void ArrangeVertical(Size finalSize)
		{
			var childrens = Children;
			var spacing = Spacing;
			var currentY = default(Float);
			var elementRect = new Rect();

			var count = childrens.Count;
			for (int i = 0; i < count; i++)
			{
				var element = childrens[i];

				if (element == null)
					continue;

				var desiredSize = element.DesiredSize;

				elementRect.Y = currentY;
				elementRect.Height = desiredSize.Height;
				elementRect.Width = Math.Max(finalSize.Width, desiredSize.Width);

				element.Arrange(elementRect);

				currentY += elementRect.Height + spacing;
			}
		}

		/// <summary>
		/// Предоставляет финальный размер контейнера и рассчитывает размеры и позиции для дочерних элементов при горизонтальной ориентации
		/// </summary>
		/// <param name="finalSize">Исходный размер контейнера</param>
		private void ArrangeHorizontal(Size finalSize)
		{
			var childrens = Children;
			var spacing = Spacing;
			var currentX = default(Float);
			var elementRect = new Rect();

			var count = childrens.Count;
			for (int i = 0; i < count; i++)
			{
				var element = childrens[i];

				if (element == null)
					continue;

				var desiredSize = element.DesiredSize;

				elementRect.X = currentX;
				elementRect.Height = Math.Max(finalSize.Height, desiredSize.Height);
				elementRect.Width = desiredSize.Width;

				element.Arrange(elementRect);

				currentX += elementRect.Width + spacing;
			}
		}
	}
}
