#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
#endif

using System;
using SmartTwin.NoesisGUI.Tools;
using Cysharp.Threading.Tasks;

namespace SmartTwin.NoesisGUI.Behaviors
{
	/// <summary>
	/// Поведение бесконечной прокрутки для элементов с функционалом <see cref="ItemsControl"/>
	/// <br>Поведение должно получить обратный вызов в <see cref="InfiniteScrollBehavior.ModificationAsyncCallback"/>, который вызывается при достижении любого порога и добавляет необходимые элементы в коллекцию для создания эффекта бесконечной прокрутки</br>
	/// </summary>
	public class InfiniteScrollBehavior : Behavior<ItemsControl>
	{
		/// <summary>
		/// Порог верхней границы при котором нужно добавлять новые элементы сверху
		/// </summary>
		public static readonly DependencyProperty UpperThresholdProperty;

		/// <summary>
		/// Порог нижней границы при котором нужно добавлять новые элементы снизу
		/// </summary>
		public static readonly DependencyProperty LowerThresholdProperty;

		/// <summary>
		/// Порог левой границы при котором нужно добавлять новые элементы слева
		/// </summary>
		public static readonly DependencyProperty LeftThresholdProperty;

		/// <summary>
		/// Порог правой границы при котором нужно добавлять новые элементы справа
		/// </summary>
		public static readonly DependencyProperty RightThresholdProperty;

		/// <summary>
		/// Обратный вызов проверки возможности изменения коллекции.
		/// <br>Установите его, если хотите делать предварительную проверку, прежде, чем модифицировать коллекцию</br>
		/// </summary>
		public static readonly DependencyProperty ValidationCallbackProperty;

		/// <summary>
		/// Обратный вызов изменения коллекции.
		/// <br>Установите его, чтобы добавлять необходимые элементы в коллекцию и создать эффект бесконечной прокрутки</br>
		/// </summary>
		public static readonly DependencyProperty ModificationAsyncCallbackProperty;


		static InfiniteScrollBehavior()
		{
			UpperThresholdProperty = DependencyProperty.Register
				(nameof(UpperThreshold),
				typeof(double),
				typeof(InfiniteScrollBehavior));


			LowerThresholdProperty = DependencyProperty.Register
				(nameof(LowerThreshold),
				typeof(double),
				typeof(InfiniteScrollBehavior));


			LeftThresholdProperty = DependencyProperty.Register
				(nameof(LeftThreshold),
				typeof(double),
				typeof(InfiniteScrollBehavior));


			RightThresholdProperty = DependencyProperty.Register
				(nameof(RightThreshold),
				typeof(double),
				typeof(InfiniteScrollBehavior));

			ValidationCallbackProperty = DependencyProperty.Register
				(nameof(ValidationCallback),
				typeof(Func<bool>),
				typeof(InfiniteScrollBehavior));


			ModificationAsyncCallbackProperty = DependencyProperty.Register
				(nameof(ModificationAsyncCallback),
				typeof(Func<ThresholdType, UniTask>),
				typeof(InfiniteScrollBehavior));
		}


		/// <summary>
		/// Порог верхней границы при котором нужно добавлять новые элементы сверху
		/// </summary>
		public double UpperThreshold
		{
			get => (double)GetValue(UpperThresholdProperty);
			set => SetValue(UpperThresholdProperty, value);
		}

		/// <summary>
		/// Порог нижней границы при котором нужно добавлять новые элементы снизу
		/// </summary>
		public double LowerThreshold
		{
			get => (double)GetValue(LowerThresholdProperty);
			set => SetValue(LowerThresholdProperty, value);
		}

		/// <summary>
		/// Порог левой границы при котором нужно добавлять новые элементы слева
		/// </summary>
		public double LeftThreshold
		{
			get => (double)GetValue(LeftThresholdProperty);
			set => SetValue(LeftThresholdProperty, value);
		}

		/// <summary>
		/// Порог правой границы при котором нужно добавлять новые элементы справа
		/// </summary>
		public double RightThreshold
		{
			get => (double)GetValue(RightThresholdProperty);
			set => SetValue(RightThresholdProperty, value);
		}

		/// <summary>
		/// Обратный вызов проверки возможности изменения коллекции
		/// </summary>
		public Func<bool> ValidationCallback
		{
			get => (Func<bool>)GetValue(ValidationCallbackProperty);
			set => SetValue(ValidationCallbackProperty, value);
		}

		/// <summary>
		/// Обратный вызов изменения коллекции
		/// </summary>
		public Func<ThresholdType, UniTask> ModificationAsyncCallback
		{
			get => (Func<ThresholdType, UniTask>)GetValue(ModificationAsyncCallbackProperty);
			set => SetValue(ModificationAsyncCallbackProperty, value);
		}


		/// <summary>
		/// Вызывается при прикреплении поведения к элементу
		/// </summary>
		/// <exception cref="InvalidOperationException">Если элемент не содержит <see cref="ScrollViewer"/></exception>
		protected override async void OnAttached()
		{
			var itemsControl = AssociatedObject;

			await NoesisGUIHelper.WaitElementLoad(itemsControl);

			var scrollViewer = itemsControl.GetChild<ScrollViewer>();

			if (scrollViewer == null)
				throw new InvalidOperationException("ItemsControl must contain ScrollViewer!");

			scrollViewer.ScrollChanged += OnScrollChanged;
		}

		/// <summary>
		/// Вызывается при открепления поведения от элемента
		/// </summary>
		protected override void OnDetaching()
		{
			var scrollViewer = AssociatedObject.GetChild<ScrollViewer>();

			if (scrollViewer != null)
				scrollViewer.ScrollChanged -= OnScrollChanged;
		}

		/// <summary>
		/// Вызывается при изменении значения скролла в <see cref="ScrollViewer"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{	
			var scrollViewer = sender as ScrollViewer;

			if (scrollViewer == null)
				return;


			var modificationCallback = ModificationAsyncCallback;

			if (modificationCallback == null)
				return;


			var validationCallback = ValidationCallback;

			await ValidateVerticalChange(scrollViewer, e, modificationCallback, validationCallback);
		}

		/// <summary>
		/// Проверить вертикальный скролл
		/// </summary>
		/// <param name="scrollViewer">Проверяемый <see cref="ScrollViewer"/></param>
		/// <param name="args">Параметры события</param>
		/// <param name="modificationCallback">Обратный вызов модификации коллекции</param>
		/// <param name="validationCallback">Обратный вызов проверки возможности модификации коллекции</param>
		/// <returns>Асинхронное ожидание</returns>
		private async UniTask ValidateVerticalChange(
			ScrollViewer scrollViewer, 
			ScrollChangedEventArgs args, 
			Func<ThresholdType, UniTask> modificationCallback,
			Func<bool> validationCallback)
		{
			var verticalOffset = args.VerticalOffset;
			var extendHeight = args.ExtentHeight;
			var verticalChange = args.VerticalChange;

			if (verticalChange > 0)
			{
				var verticalThreshold = extendHeight - verticalOffset;

				if (verticalThreshold <= LowerThreshold)
				{
					if (validationCallback != null && !validationCallback())
						return;

					await modificationCallback(ThresholdType.VerticalDown);
				}
			}
			else if (verticalChange < 0)
			{
				//ToDo: придобавлении элементов вниз ItemsPanel и ScrollViewer автоматически оставляют ScrollBar на том же положении
				//Тем самым создавая эффект бесконечной прокрутки. В случае с верхней границы менять положение нужно в ручную 
				if (verticalOffset <= UpperThreshold)
				{
					if (validationCallback != null && !validationCallback())
						return;


					await modificationCallback(ThresholdType.VerticalUp);

					scrollViewer.UpdateLayout();

					verticalOffset += scrollViewer.ExtentHeight - extendHeight;

					scrollViewer.ScrollToVerticalOffset(verticalOffset);
					scrollViewer.UpdateLayout();
				}
			}
		}

		/// <summary>
		/// Проверить горизонтальный скролл
		/// </summary>
		/// <param name="scrollViewer">Проверяемый <see cref="ScrollViewer"/></param>
		/// <param name="args">Параметры события</param>
		/// <param name="modificationCallback">Обратный вызов модификации коллекции</param>
		/// <param name="validationCallback">Обратный вызов проверки возможности модификации коллекции</param>
		/// <returns>Асинхронное ожидание</returns>
		private async UniTask ValidateHorizontalChange(
			ScrollViewer scrollViewer,
			ScrollChangedEventArgs args,
			Func<ThresholdType, UniTask> modificationCallback,
			Func<bool> validationCallback)
		{
			var horizontalOffset = args.HorizontalOffset;
			var extendWidth = args.ExtentWidth;
			var horizontalChange = args.HorizontalChange;

			if (horizontalChange > 0)
			{
				var verticalThreshold = extendWidth - horizontalOffset;

				if (verticalThreshold <= RightThreshold)
				{
					if (validationCallback != null && !validationCallback())
						return;

					await modificationCallback(ThresholdType.HorizontalRight);
				}
			}
			else if (horizontalChange < 0)
			{
				//ToDo по аналогии с вертикальной прокруткой
				if (horizontalOffset <= LeftThreshold)
				{
					if (validationCallback != null && !validationCallback())
						return;

					await modificationCallback(ThresholdType.HorizontalLeft);

					scrollViewer.UpdateLayout();

					horizontalOffset += scrollViewer.ExtentHeight - extendWidth;

					scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
					scrollViewer.UpdateLayout();
				}
			}
		}
	}

	public enum ThresholdType
	{
		HorizontalLeft,
		HorizontalRight,
		VerticalDown,
		VerticalUp,
	}
}
