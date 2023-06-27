#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTwin.NoesisGUI.Views
{
	/// <summary>
	/// Поставщик представлений
	/// </summary>
	public static class ViewProvider
	{
		/// <summary>
		/// Таблица типов представлений и их сооотвествующих путей к xaml-документам
		/// </summary>
		private static Dictionary<Type, NoesisXaml> Xamls = new Dictionary<Type, NoesisXaml>();

		/// <summary>
		/// Создать представление
		/// </summary>
		/// <typeparam name="V">Тип представления</typeparam>
		/// <returns>null, если не удалось создать представление</returns>
		public static V Create<V>()
			where V : BaseView
		{
			var type = typeof(V);

			return Create(type) as V;
		}

		/// <summary>
		/// Создать представление указанного типа
		/// </summary>
		/// <returns>null, если не удалось создать представление</returns>
		public static BaseView Create(Type type)
		{
			if (!typeof(BaseView).IsAssignableFrom(type))
			{
				Debug.LogError($"{typeof(BaseView)} is not assignable from {type}");
				return null;        //ToDo Добавить исключение? 
			}

			if (Xamls.TryGetValue(type, out var xaml))
				return xaml.Load() as BaseView;

			var path = BaseView.GetXamlPath(type);

			if (path == null)
			{
				Debug.LogError($"The path to the location of the xml document of the view {type} could not be found. Perhaps the view is not registered via BaseView.RegisterXamlPath");
				return null;
			}

			xaml = Resources.Load<NoesisXaml>(path);

			if (xaml == null)
			{
				Debug.LogError($"Failed to load Xml documents via Resources");
				return null;
			}

			Xamls.Add(type, xaml);
			return xaml.Load() as BaseView;
		}
	}
}
#endif