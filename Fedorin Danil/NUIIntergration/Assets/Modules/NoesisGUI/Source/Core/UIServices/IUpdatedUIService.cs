

namespace SmartTwin.NoesisGUI.Services
{
	/// <summary>
	/// Обновляемый UI-сервис. Содержит метод, который вызывается ядром Noesis каждый кадр.
	/// </summary>
	public interface IUpdatedUIService
	{
		/// <summary>
		/// Метод обновления, вызываемый каждый кадр.
		/// </summary>
		void Update();
	}
}
