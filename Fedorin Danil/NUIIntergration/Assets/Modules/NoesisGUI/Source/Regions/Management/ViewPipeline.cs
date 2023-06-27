#if UNITY_5_3_OR_NEWER
using Cysharp.Threading.Tasks;
using SmartTwin.NoesisGUI.Tools;
using SmartTwin.NoesisGUI.Views;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Конвейер представлений. Необходим для создания представления через MVVM подход и региональное взаимодействие
	/// </summary>
	public class ViewPipeline
	{
		/// <summary>
		/// Реестр фабрик представлений
		/// </summary>
		private readonly ViewFactoryRegistry _registry;


		public ViewPipeline(ViewFactoryRegistry registry)
		{
			_registry = registry;
		}


		/// <summary>
		/// Создать представление на регионе с последующей пост-обработкой.
		/// </summary>
		/// <typeparam name="TView">Тип представления</typeparam>
		/// <param name="region">Регион</param>
		/// <param name="args">Дополнительные аргументы при построении</param>
		/// <returns>Асинхронное ожидание представления</returns>
		public async UniTask<TView> CreateViewOnRegion<TView>(Region region, params object[] args)
			where TView : BaseView
		{
			var factory = _registry.GetFactory<TView>();

			if (factory == null)
				return null;

			var view = factory.Create(args) as TView;

			if (!region.Add(view))
				return null;

			await view.WaitLoad();

			ProcessAdditionView(region, view);

			return view;
		}

		/// <summary>
		/// Удалить представление из региона с последующей пост-обработкой.
		/// Необходим для зеркального процесса - Создал через конвеер - удалил также через конвеер
		/// </summary>
		/// <param name="region">Регион</param>
		/// <param name="view">Удаляемое представление</param>
		/// <returns>Асинхронное ожидание. True, если успешно</returns>
		public async UniTask<bool> RemoveViewFromRegion(Region region, BaseView view)
		{
			if (region.Remove(view))
			{
				await view.WaitUnload();
				//Тут место для постобработки
				return true;
			}

			return false;
		}

		/// <summary>
		/// Зарегестрировать представления на регионе. Автоматически создает все представления, которые привязаны к имени региона.
		/// </summary>
		/// <param name="region">Регион, который нужно автоматически заполнить</param>
		public void RegisterViewsOnRegion(Region region)
		{
			var factories = _registry.GetFactoriesFromRegion(region.Name);

			foreach (var factory in factories)
			{
				var view = factory.Create();

				if (!region.Add(view))
					continue;

				view.ExecuteWhenLoad((_) => ProcessAdditionView(region, view));
			}
		}


		/// <summary>
		/// Постаобработка добавленного в регион представления
		/// </summary>
		private void ProcessAdditionView(Region region, BaseView view)
		{
			//ToDo: Будут добавлены другие этапы
			CheckChildRegions(view);
		}

		/// <summary>
		/// Обработать все дочерине регионы представления
		/// </summary>
		private void CheckChildRegions(BaseView view)
		{
			var regions = view.FindRegions();

			foreach (var region in regions)
			{
				var factories = _registry.GetFactoriesFromRegion(region.Name);

				foreach (var factory in factories)
				{
					var childView = factory.Create();
					if (!region.Add(childView))
						continue;

					childView.ExecuteWhenLoad((_) => ProcessAdditionView(region, childView));
				}
			}
		}
	}
}
#endif