#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using System.Windows;
using Microsoft.Xaml.Behaviors;
#endif

using System;
using SmartTwin.NoesisGUI.Views;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Поведение региона
	/// </summary>
	public abstract class RegionBehavior : Behavior<DependencyObject>
	{
		/// <summary>
		/// Свойство зависимости имени региона
		/// </summary>
		public static readonly DependencyProperty RegionNameProperty;

		/// <summary>
		/// Свойство зависимости контекста региона
		/// </summary>
		public static readonly DependencyProperty RegionContextProperty;


		static RegionBehavior()
		{
			RegionNameProperty = DependencyProperty.Register(
				nameof(RegionName),
				typeof(string),
				typeof(RegionBehavior),
				new PropertyMetadata(string.Empty));

			RegionContextProperty = DependencyProperty.Register(
				nameof(RegionContext),
				typeof(object),
				typeof(RegionBehavior),
				new PropertyMetadata(null));
		}


		/// <summary>
		/// Имя региона
		/// </summary>
		public string RegionName
		{
			get => (string)GetValue(RegionNameProperty);
			set => SetValue(RegionNameProperty, value);
		}

		/// <summary>
		/// Контекст региона
		/// </summary>
		public object RegionContext
		{
			get => (object)GetValue(RegionContextProperty);
			set => SetValue(RegionContextProperty, value);
		}

		/// <summary>
		/// Контейнер представлений
		/// </summary>
		public DependencyObject Container => AssociatedObject;

		/// <summary>
		/// Регион
		/// </summary>
		public Region Region { get; private set; }


		/// <summary>
		/// Попытаться добавить представление
		/// </summary>
		/// <param name="view"></param>
		/// <returns>True, если удачно</returns>
		public abstract bool TryAdd(BaseView view);

		/// <summary>
		/// Попытаться удалить представление
		/// </summary>
		/// <param name="view"></param>
		/// <returns>True, если успешно</returns>
		public abstract bool TryRemove(BaseView view);

		/// <summary>
		/// Попытаться очистить регион от представлений
		/// </summary>
		/// <returns>True, если успешно</returns>
		public abstract bool TryClear();

		/// <summary>
		/// Привязать поведение к региону
		/// </summary>
		/// <param name="region"></param>
		public void BindRegion(Region region) => Region = region;

		/// <summary>
		/// Отвязать поведение от региона
		/// </summary>
		public void UnbindRegion() => Region = null;
	}
	

	/// <summary>
	/// Поведение региона для конкретного типа контейнера
	/// </summary>
	/// <typeparam name="TContainer">Тип контейнера</typeparam>
	public abstract class RegionBehavior<TContainer> : RegionBehavior
		where TContainer : DependencyObject
	{
		/// <summary>
		/// Контейнер
		/// </summary>
		public new TContainer Container => AssociatedObject as TContainer;

		/// <summary>
		/// Вызывается при присоединении поведения к элементу
		/// </summary>
		/// <exception cref="InvalidOperationException">Если элемент не указанного типа</exception>
		protected sealed override void OnAttached()
		{
			if (AssociatedObject is TContainer container)
			{
				var exception = ValidateContainer(container);

				if (exception != null)
					throw exception;
			}
			else
			{
				throw new InvalidOperationException("AssociatedObject is not cast to type: " + typeof(TContainer));
			}


			OnRegionAttached();
		}

		/// <summary>
		/// Вызывается при отсоединении поведения от элемента
		/// </summary>
		protected sealed override void OnDetaching() => OnRegionDetaching();

		/// <summary>
		/// Вызывается при присоединении поведения к региону
		/// </summary>
		protected virtual void OnRegionAttached() { }

		/// <summary>
		/// Вызывается при отсоединении поведения от региона
		/// </summary>
		protected virtual void OnRegionDetaching() { }

		/// <summary>
		/// Вызывается для проверки контейнера
		/// </summary>
		/// <param name="container"></param>
		/// <returns>Возвращает <see cref="Exception"/>, если возникло исключение или null, если контейнер удовлетворяет всем условиям</returns>
		protected virtual Exception ValidateContainer(TContainer container) => null;
	}
}
