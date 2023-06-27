#if UNITY_5_3_OR_NEWER
using SmartTwin.NoesisGUI.Services;
using SmartTwin.NoesisGUI.Views;
using System;
using Zenject;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Класс расширений для регистраций представлений и их фабрик
	/// </summary>
	public static class ViewRegistrationDiExtensions
	{
		/// <summary>
		/// Зарегестрировать UIService
		/// </summary>
		/// <typeparam name="TService">Тип сервиса</typeparam>
		/// <param name="container"></param>
		/// <param name="args">Набор параметров для регистрации</param>
		/// <returns></returns>
		public static DiContainer RegisterUIService<TService>(this DiContainer container, params object[] args)
			where TService : IUIService
		{
			container.BindInterfacesAndSelfTo<TService>()
				.AsSingle()
				.WithArguments(args);

			return container;
		}


		/// <summary>
		/// Зарегестрировать стандартную фабрику представлений
		/// </summary>
		/// <typeparam name="VType">Тип представления</typeparam>
		/// <typeparam name="VMType">Тип модели представления</typeparam>
		/// <returns></returns>
		public static DiContainer RegisterViewFactory<VType, VMType>(this DiContainer container)
			where VType : BaseView, new()
			where VMType : class
		{
			container.BindInterfacesAndSelfTo<ViewFactory<VType, VMType>>()
				.AsSingle()
				.NonLazy();

			return container;
		}

		/// <summary>
		/// Зарегестрировать фабрику для создания Singleton представления
		/// </summary>
		/// <typeparam name="VType">Тип представления</typeparam>
		/// <typeparam name="VMType">Тип модели представления</typeparam>
		/// <returns></returns>
		public static DiContainer RegisterSingletonViewFactory<VType, VMType>(this DiContainer container)
			where VType : BaseView, new()
			where VMType : class
		{
			
			container.BindInterfacesAndSelfTo<SingletonViewFactory<VType, VMType>>()
				.AsSingle()
				.NonLazy();

			return container;
		}

		/// <summary>
		/// Зарегестрировать пользовательскую фабрику
		/// </summary>
		/// <typeparam name="FType">Тип фабрики</typeparam>
		/// <param name="container"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static DiContainer RegisterCustomFactory<FType>(this DiContainer container, params object[] args)
			where FType : IViewFactory
		{
			container.BindInterfacesAndSelfTo<FType>()
				.AsSingle()
				.WithArguments(args);

			return container;
		}

		/// <summary>
		/// Привязать представления к регионам через стандартную фабрику 
		/// </summary>
		/// <typeparam name="VType">Тип представления</typeparam>
		/// <typeparam name="VMType">Тип модели представления</typeparam>
		/// <param name="container"></param>
		/// <param name="regionNames">Имена регионов, на которых нужно автоматически создавать представление</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Если regionNames is null</exception>
		public static DiContainer AttachViewToRegions<VType, VMType>(this DiContainer container, params string[] regionNames)
			where VType : BaseView, new()
			where VMType : class
		{
			if (regionNames == null)
				throw new NullReferenceException(nameof(regionNames));

			if (!container.HasBinding<ViewFactory<VType, VMType>>())
				container.RegisterViewFactory<VType, VMType>();

			container.Bind<IBindingFactoryToRegionsContract>().To<BindingFactoryToRegionsContract<ViewFactory<VType, VMType>>>()
				.AsSingle()
				.WithArguments(regionNames);

			return container;
		}

		/// <summary>
		/// Привязать представления к регионам через singleton фабрику 
		/// </summary>
		/// <typeparam name="VType">Тип представления</typeparam>
		/// <typeparam name="VMType">Тип модели представления</typeparam>
		/// <param name="container"></param>
		/// <param name="regionNames">Имена регионов, на которых нужно автоматически создавать представление</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Если regionNames is null</exception>
		public static DiContainer AttachSingletonViewToRegions<VType, VMType>(this DiContainer container, params string[] regionNames)
			where VType : BaseView, new()
			where VMType : class
		{
			if (regionNames == null)
				throw new NullReferenceException(nameof(regionNames));

			container.BindInterfacesAndSelfTo<VMType>()
				.AsSingle();

			if (!container.HasBinding<SingletonViewFactory<VType, VMType>>())
				container.RegisterSingletonViewFactory<VType, VMType>();

			container.Bind<IBindingFactoryToRegionsContract>().To<BindingFactoryToRegionsContract<SingletonViewFactory<VType, VMType>>>()
				.AsSingle()
				.WithArguments(regionNames);

			return container;
		}

		/// <summary>
		/// Привязать кастомную фабрику представлений к регионам 
		/// </summary>
		/// <param name="container"></param>
		/// <param name="regionNames">Имена регионов, на которых нужно автоматически создавать представление</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Если regionNames is null</exception>
		public static DiContainer AttachCustomFactoryToRegions<FType>(this DiContainer container, params string[] regionNames)
			where FType : IViewFactory
		{
			if (regionNames == null)
				throw new NullReferenceException(nameof(regionNames));

			if (!container.HasBinding<FType>())
				container.RegisterCustomFactory<FType>();

			container.Bind<IBindingFactoryToRegionsContract>().To<BindingFactoryToRegionsContract<FType>>()
				.AsSingle()
				.WithArguments(regionNames);

			return container;
		}
	}
}
#endif
