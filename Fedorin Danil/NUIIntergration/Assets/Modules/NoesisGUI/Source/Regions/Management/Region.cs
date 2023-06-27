#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using System.Windows;
using Microsoft.Xaml.Behaviors;
#endif

using System;
using System.Collections.Generic;
using SmartTwin.NoesisGUI.Views;
using System.Collections;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Регион представлений
	/// </summary>
	public class Region : IDisposable, IEnumerable<BaseView>
	{
		/// <summary>
		/// Представления
		/// </summary>
		private readonly List<BaseView> _views;

		/// <summary>
		/// Поведениер региона
		/// </summary>
		private readonly RegionBehavior _behavior;
 
		
		public Region(RegionBehavior behavior)
		{
			_views = new List<BaseView>();

			_behavior = behavior;
			_behavior.BindRegion(this);

			ActiveView = null;
		}

		/// <summary>
		/// Очистить регион
		/// </summary>
		public void Dispose()
		{
			Clear();
			_behavior.UnbindRegion();
		}


		/// <summary>
		/// Имя региона
		/// </summary>
		public string Name
		{
			get => _behavior.RegionName;

			set => _behavior.RegionName = value;
		}

		/// <summary>
		/// Контекст региона
		/// </summary>
		public object RegionContext => _behavior.RegionContext;

		/// <summary>
		/// Текущее активное представление
		/// ToDo: Доработать, функционал не работает!
		/// </summary>
		public BaseView ActiveView { get; private set; }

		/// <summary>
		/// Представления региона
		/// </summary>
		public IReadOnlyCollection<BaseView> Views => _views;


		/// <summary>
		/// Добавить новое представление
		/// </summary>
		/// <param name="view"></param>
		/// <returns>True, если удачно</returns>
		public bool Add(BaseView view)
		{
			if (!_behavior.TryAdd(view))
				return false;

			_views.Add(view);

			//интерфейсы должны проверяться здесь ?
			CheckBehaviorInterfaces(view);

			return true;
		}

		/// <summary>
		/// Удалить представление
		/// </summary>
		/// <param name="view"></param>
		/// <returns>True, если удачно</returns>
		public bool Remove(BaseView view)
		{
			if (!_behavior.TryRemove(view))
				return false;

			_views.Remove(view);

			return true;
		}

		/// <summary>
		/// Очистить регион
		/// </summary>
		/// <returns>True, если удачно</returns>
		public bool Clear()
		{
			if (!_behavior.TryClear())
				return false;

			_views.Clear();

			return true;
		}

		/// <summary>
		/// Найти регион по имени
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public BaseView Find(string name) => _views.Find(view => view.Name == name);

		/// <summary>
		/// Найти регион по типу
		/// </summary>
		/// <typeparam name="TView"></typeparam>
		/// <returns></returns>
		public BaseView Find<TView>() => _views.Find(view => view.GetType() == typeof(TView));

		/// <summary>
		/// Найти регион по типу
		/// </summary>
		/// <param name="typeView"></param>
		/// <returns></returns>
		public BaseView Find(Type typeView) => _views.Find(view => view.GetType() == typeView);

		/// <summary>
		/// Найти все регионы конкретного типа
		/// </summary>
		/// <typeparam name="TView"></typeparam>
		/// <returns></returns>
		public List<TView> FindAll<TView>()
			where TView : BaseView
		{
			var views = new List<TView>();

			foreach (var view in _views)
			{
				if (view is TView concreteView)
					views.Add(concreteView);
			}

			return views;
		}

		/// <summary>
		/// Получить контейнер региона, где будут размещаться представления
		/// </summary>
		/// <typeparam name="TContainer"></typeparam>
		/// <returns></returns>
		public TContainer GetContainer<TContainer>() where TContainer : DependencyObject 
			=> _behavior.Container as TContainer;

		/// <summary>
		/// Перечислить представлений
		/// </summary>
		/// <returns></returns>
		public IEnumerator<BaseView> GetEnumerator() => _views.GetEnumerator();

		/// <summary>
		/// Перечислить представлений
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() => _views.GetEnumerator();


		/// <summary>
		/// Проверить интерфейсы-контракты представления
		/// </summary>
		/// <param name="view"></param>
		private void CheckBehaviorInterfaces(BaseView view)
		{
			var regionContext = RegionContext;

			if (regionContext != null)
			{
				var accepted = view as IRegionContextAccepted ?? view.DataContext as IRegionContextAccepted;
				accepted?.AcceptRegionContext(regionContext);
			}
		}
	}
}
