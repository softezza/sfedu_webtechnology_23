using SmartTwin.NoesisGUI.Views;
using System;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Фабрика создания Singleton-представлений через DiContainer
	/// </summary>
	/// <typeparam name="VType">Тип создаваемого представления</typeparam>
	/// <typeparam name="VMType">Тип модели для представления</typeparam>
	public class SingletonViewFactory<VType, VMType> : IViewFactory
		where VType : BaseView, new()
		where VMType : class
	{
		private readonly VMType _viewModel;


		private VType _view;


		public SingletonViewFactory(VMType viewModel) => _viewModel = viewModel;


		public Type ViewType => typeof(VMType);

		public Type ViewModelType => typeof(VMType);


		//ToDo - черновой вариант - переработать
		public virtual BaseView Create(params object[] args)
		{
			if (_view == null)
			{
				if (args == null)
					throw new ArgumentNullException(nameof(args));

				_view = ViewProvider.Create<VType>();

				if (_view == null)
					return null;

				_view.DataContext = _viewModel;
			}

			return _view;
		}
	}
}
