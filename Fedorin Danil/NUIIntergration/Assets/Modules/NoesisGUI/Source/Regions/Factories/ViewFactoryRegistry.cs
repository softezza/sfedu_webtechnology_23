using SmartTwin.NoesisGUI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Реестр фабрик представлений
	/// </summary>
	public class ViewFactoryRegistry
	{
		/// <summary>
		/// Фабрики представлений
		/// </summary>
		private readonly List<IViewFactory> _factories;

		/// <summary>
		/// Таблица привязки фабрик представлений к регионам
		/// <br>При создания региона с конкретным именем автоматически будут создаваться представления с помощью привязанных фабрик</br>
		/// <br>ToDo: подумать о Scope регионов</br>
		/// </summary>
		private readonly Dictionary<string, List<IViewFactory>> _regionsBindingTable;


		public ViewFactoryRegistry(IEnumerable<IViewFactory> factories, IEnumerable<IBindingFactoryToRegionsContract> contracts)
		{
			_regionsBindingTable = new Dictionary<string, List<IViewFactory>>();
			_factories = new List<IViewFactory>(factories);

			foreach (var contract in contracts)
			{
				var factory = _factories.Find(factory => factory.GetType() == contract.FactoryType);

				if (factory == null)
				{
					//ToDo - нужно ли здесь исключение? 
					Debug.LogWarning($"A request was received for automatic creation of a representation by the factory, but the factory is not registered: {contract.FactoryType}");
					continue;
				}

				foreach (var regionName in contract.RegionNames)
				{
					if (_regionsBindingTable.TryGetValue(regionName, out var list))
						list.Add(factory);
					else
						_regionsBindingTable.Add(regionName, new List<IViewFactory>() { factory });
				}
			}
		}


		/// <summary>
		/// Получить все фабрики, привязанные к этому региону
		/// </summary>
		/// <param name="regionName">Имя региона</param>
		/// <returns>Коллекцию найденных фабрик. Empty - если не нашлись</returns>
		public IEnumerable<IViewFactory> GetFactoriesFromRegion(string regionName)
		{
			if (_regionsBindingTable.TryGetValue(regionName, out var factories))
				return factories;
			else
				return new List<IViewFactory>();
		}

		/// <summary>
		/// Получить фабрику, который создает представление указанного типа
		/// </summary>
		/// <typeparam name="TView"></typeparam>
		/// <returns></returns>
		public IViewFactory GetFactory<TView>()
			where TView : BaseView
		{
			var factory = _factories.Find(factory => factory.ViewType == typeof(TView));

			return factory;
		}
	}
}