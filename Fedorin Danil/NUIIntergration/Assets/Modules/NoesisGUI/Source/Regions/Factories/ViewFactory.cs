using SmartTwin.NoesisGUI.Views;
using System;
using Zenject;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Фабрика создания представления через DiContainer
	/// </summary>
	/// <typeparam name="VType">Тип представления</typeparam>
	/// <typeparam name="VMType">Тип модели представления</typeparam>
	public class ViewFactory<VType, VMType> : IViewFactory
		where VType : BaseView, new()
		where VMType : class
	{
		/// <summary>
		/// DiContainer
		/// </summary>
		protected readonly DiContainer _container;

		public ViewFactory(DiContainer container) => _container = container;


		public Type ViewType => typeof(VType);

		public Type ViewModelType => typeof(VMType);


		public virtual BaseView Create(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException(nameof(args));

			if (_container.HasBinding<VMType>())
				throw new InvalidOperationException($"{typeof(VMType)} already embedded in DiContainer and cannot be used as a dynamic element.");

			var view = ViewProvider.Create<VType>();

			if (view == null)
				return null;

			view.DataContext = _container.Instantiate<VMType>(args);

			return view;
		}
	}
}
