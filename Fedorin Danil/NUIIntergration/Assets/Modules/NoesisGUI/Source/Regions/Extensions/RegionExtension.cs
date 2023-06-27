#if UNITY_5_3_OR_NEWER
using Noesis;
using NoesisApp;
using SmartTwin.NoesisGUI.Tools;
using SmartTwin.NoesisGUI.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Расширения для работы с регионами
	/// </summary>
	public static class RegionExtension
	{
		/// <summary>
		/// Находит все регионы у указанного элемента
		/// </summary>
		/// <param name="element"></param>
		/// <returns>Возвращает все дочерние регионы</returns>
		public static IEnumerable<Region> FindRegions(this DependencyObject element)
		{
			var childs = element.GetChilds<DependencyObject>();
			var regions = new List<Region>();

			foreach (var child in childs)
			{
				var behaviors = Interaction.GetBehaviors(child);

				var regionBehavior = behaviors.FirstOrDefault(b => b is RegionBehavior) as RegionBehavior;

				if (regionBehavior != null)
				{
					if (regionBehavior.Region != null)
					{
						regions.Add(regionBehavior.Region);
					}
					else
					{
						var region = new Region(regionBehavior);
						regions.Add(region);
					}
				}
			}

			return regions;
		}
	}
}
#endif
