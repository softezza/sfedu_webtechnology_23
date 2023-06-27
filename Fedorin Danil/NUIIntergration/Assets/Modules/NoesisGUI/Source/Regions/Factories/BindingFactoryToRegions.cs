using SmartTwin.NoesisGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Контракт привязки фабрики представлений к регионам для автоматического создания представления
	/// </summary>
	public interface IBindingFactoryToRegionsContract
	{
		/// <summary>
		/// Тип фабрики представлений
		/// </summary>
		Type FactoryType { get; }

		/// <summary>
		/// Имена регионов, для которых должно создаваться представление
		/// </summary>
		IEnumerable<string> RegionNames { get; }
	}

	/// <summary>
	/// Контракт привязки фабрики представлений к регионам для автоматического создания представления
	/// </summary>
	/// <typeparam name="FType">Тип фабрики представлений</typeparam>
	public class BindingFactoryToRegionsContract<FType> : IBindingFactoryToRegionsContract
		where FType : IViewFactory
	{
		public BindingFactoryToRegionsContract(IEnumerable<string> regionNames) => RegionNames = regionNames.Distinct();

		/// <summary>
		/// Тип фабрики представлений
		/// </summary>
		public Type FactoryType => typeof(FType);

		/// <summary>
		/// Имена регионов, для которых должно создаваться представление
		/// </summary>
		public IEnumerable<string> RegionNames { get; } 
	}
}
