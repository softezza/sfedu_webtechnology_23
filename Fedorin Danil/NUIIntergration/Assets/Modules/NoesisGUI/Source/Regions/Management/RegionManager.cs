#if UNITY_5_3_OR_NEWER
using Cysharp.Threading.Tasks;
using Noesis;
using NoesisApp;
using SmartTwin.NoesisGUI.Tools;
using SmartTwin.NoesisGUI.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Менеджер регионов
	/// </summary>
	public class RegionManager : IEnumerable<Region>, IDisposable
	{
		/// <summary>
		/// Регионы
		/// </summary>
		private readonly Dictionary<string, Region> _regions;

		/// <summary>
		/// Конвеер представлений
		/// </summary>
		private readonly ViewPipeline _viewPipeline;


		public RegionManager(ViewPipeline pipeline)
		{
			_viewPipeline = pipeline;

			_regions = new Dictionary<string, Region>();
		}

		/// <summary>
		/// Проинициализировать
		/// </summary>
		/// <param name="screen"></param>
		public void Initialize(MainScreen screen)
		{
			InitializeRootRegions(screen);
		}

		/// <summary>
		/// Очистить
		/// </summary>
		public void Dispose()
		{
			foreach (var region in _regions.Values)
				region.Dispose();

			_regions.Clear();
		}

		
		/// <summary>
		/// Получить регион по имени
		/// </summary>
		/// <param name="regionName"></param>
		/// <returns></returns>
		public Region GetRegion(string regionName)
		{
			if (_regions.TryGetValue(regionName, out var region))
				return region;

			return null;
		}

		/// <summary>
		/// Получить перечислитель регионов
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Region> GetEnumerator() => _regions.Values.GetEnumerator();

		/// <summary>
		/// Получить перечислитель регионов
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() => _regions.Values.GetEnumerator();

		/// <summary>
		/// Запросить все представления указанного типа из всех регионов
		/// </summary>
		/// <typeparam name="TView"></typeparam>
		/// <returns></returns>
		public List<TView> ResolveViews<TView>()
			where TView : BaseView
		{
			var views = new List<TView>();

			foreach (var region in _regions.Values)
				views.AddRange(region.FindAll<TView>());

			return views;
		}


		/// <summary>
		/// Заполнить главные регионы
		/// </summary>
		/// <param name="screen"></param>
		private void InitializeRootRegions(MainScreen screen)
		{
			var regions = screen.FindRegions();

			foreach (var region in regions)
			{
				var regionName = region.Name;

				if (_regions.ContainsKey(regionName))
				{
					Debug.LogError($"A region with this name already exists: {regionName}");
					continue;
				}

				_regions.Add(regionName, region);

				_viewPipeline.RegisterViewsOnRegion(region);
			}
		}
	}
}
#endif