#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;
#endif

using System;

namespace SmartTwin.NoesisGUI.Regions
{
	using Views;

	/// <summary>
	/// Поведение региона для <see cref="Panel"/>
	/// </summary>
	public class PanelRegionBehavior : RegionBehavior<Panel>
	{
		/// <summary>
		/// Свойство включения/выключения приоритетной отрисовки при нажатии
		/// </summary>
		//ToDo: свойство пока что будет реагировать на нажатие, но в будущем надо переделать под фокус
		public static DependencyProperty IsAutoDisplayViewProperty;

		
		/// <summary>
		/// Выделенное представление
		/// </summary>
		private BaseView _focusedView;


		static PanelRegionBehavior()
		{
			IsAutoDisplayViewProperty = DependencyProperty.Register(
				nameof(IsAutoDisplayView),
				typeof(bool),
				typeof(PanelRegionBehavior),
				new PropertyMetadata(false));
		}


		/// <summary>
		/// Свойства включения/выключения выделения при нажатии.
		/// True, если нужно, чтобы любое представление в регионе при нажатии становилось на передний план
		/// </summary>
		public bool IsAutoDisplayView
		{
			get => (bool)GetValue(IsAutoDisplayViewProperty);
			set => SetValue(IsAutoDisplayViewProperty, value);
		}


		public override bool TryAdd(BaseView view)
		{
			var container = Container;

			if (container == null)
				return false;

			var children = container.Children;

			if (children.Contains(view))
				return false;

			children.Add(view);

			if (IsAutoDisplayView)
				view.PreviewMouseDown += OnViewMouseDown;

			return true;
		}

		public override bool TryRemove(BaseView view)
		{
			var container = Container;

			if (container == null)
				return false;

			var children = container.Children;

			if (!children.Contains(view))
				return false;

			children.Remove(view);
			
			view.PreviewMouseDown -= OnViewMouseDown;

			if (children.Count == 0)
				ResetFocusView();

			return true;
		}


		public override bool TryClear()
		{
			var container = Container;

			if (container == null)
				return false;

			var children = container.Children;

			children.Clear();

			return true;
		}


		protected override Exception ValidateContainer(Panel container)
		{
			if (container.Children.Count > 0)
				return new InvalidOperationException("Invalid Region - Panel is not Empty");
			return null;
		}

		
		/// <summary>
		/// Вызывается при событии <see cref="UIElement.PreviewMouseDown"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnViewMouseDown(object sender, MouseButtonEventArgs args)
		{
			if (sender is BaseView view)
				FocusView(view);
		}

		/// <summary>
		/// Выделить представление на передний план
		/// </summary>
		/// <param name="view">Представление, которое нужно выделить</param>
		private void FocusView(BaseView view)
		{
			ResetFocusView();

			_focusedView = view;
			Panel.SetZIndex(_focusedView, 1);
		}

		/// <summary>
		/// Сбросить выделенное представление с переднего плана
		/// </summary>
		private void ResetFocusView()
		{
			if (_focusedView != null)
			{
				Panel.SetZIndex(_focusedView, 0);
				_focusedView = null;
			}
		}
	}
}
