#if UNITY_5_3_OR_NEWER
using System;

namespace SmartTwin.NoesisGUI.Services
{
	using Regions;

	/// <summary>
	/// Интерфейс UI-сервиса.
	/// При реализации получает доступ к UI и регионам.
	/// </summary>
	public interface IUIService : IDisposable
	{
		/// <summary>
		/// Инициализация UI-сервиса. Вызывается после инициализации всего UI
		/// </summary>
		/// <param name="regionManager">Заполненный регионами менеджер для доступа к интерфейсу</param>
		void Initialize(RegionManager regionManager);
	}
}
#endif