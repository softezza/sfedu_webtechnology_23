using SmartTwin.NoesisGUI.Views;
using System;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Интерфейс фабрики представлений
	/// </summary>
	public interface IViewFactory
	{
		/// <summary>
		/// Создает новое представление <see cref="BaseView"/>
		/// </summary>
		/// <param name="args">Набор аргументов для создания</param>
		/// <returns>Построенное представление</returns>
		BaseView Create(params object[] args);

		/// <summary>
		/// Тип представления, наследуемого от <see cref="BaseView"/>
		/// </summary>
		Type ViewType { get; }

		/// <summary>
		/// Тип модели представления
		/// <br>ToDo: на удаление</br>
		/// </summary>
		Type ViewModelType { get; }
	}
}
