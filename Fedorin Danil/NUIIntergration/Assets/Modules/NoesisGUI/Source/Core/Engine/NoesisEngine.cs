#if UNITY_5_3_OR_NEWER
using Cysharp.Threading.Tasks;
using System;

namespace SmartTwin.NoesisGUI
{
	using Noesis;
	using Regions;
	using SmartTwin.NoesisGUI.Physics;
	using SmartTwin.NoesisGUI.Services;
	using System.Collections.Generic;
	using System.Threading;
	using Tools;

	/// <summary>
	/// Ядро NoesisGUI для работы внутри Unity. 
	/// </summary>
	public class NoesisEngine : IDisposable
	{
		/// <summary>
		/// Компонент для работы с UI на сцене.
		/// </summary>
		private readonly NoesisView _noesisView;

		/// <summary>
		/// Менеджер регионов в UI.
		/// </summary>
        private readonly RegionManager _regionManager;

		/// <summary>
		/// Система взаимодействия между физическим миром Unity и Noesis (raycast и т.д.). 
		/// </summary>
        private readonly PhysicsNoesisSystem _physicsNoesisSystem;

		/// <summary>
		/// UI-сервисы для работы с интерфейсом.
		/// </summary>
        private readonly List<IUIService> _UIServices;

		/// <summary>
		/// UI-сервисы, которые необходимо обновлять каждый кадр.
		/// </summary>
		private readonly List<IUpdatedUIService> _updatedServices;

		/// <summary>
		/// Визуальный корень UI.
		/// </summary>
		private Visual _root;

		/// <summary>
		/// Главный экран интерфейса.
		/// </summary>
		private MainScreen _screen;

		/// <summary>
		/// Источник отмены для корутины <see cref="Update(CancellationToken)"/>
		/// </summary>
		private CancellationTokenSource _updateSource;

		/// <summary>
		/// Корутина <see cref="Update(CancellationToken)"/>
		/// </summary>
		private UniTask updateTask;


		public NoesisEngine(
			NoesisView noesis, 
			RegionManager regionManager, 
			PhysicsNoesisSystem physicsNoesisSystem,
			IEnumerable<IUIService> UIServices)
		{
			_noesisView = noesis;
			_regionManager = regionManager;
			_physicsNoesisSystem = physicsNoesisSystem;

			_UIServices = new List<IUIService>(UIServices);
			_updatedServices = new List<IUpdatedUIService>();
		}

		/// <summary>
		/// Проинициализировать Noesis
		/// <br>Важно! NoesisEngine ожидает загрузку главного экрана, которая выполняется только после метода Start в жизненном цикле Unity</br>
		/// </summary>
		/// <returns>Асинхронное ожидание</returns>
		/// <exception cref="NullReferenceException">Если в UI-документе вставлен не тип, наследуемый от MainScreen</exception>
        public async UniTask Initialize()
        {
            _screen = _noesisView.Content as MainScreen;

            if (_screen == null)
                throw new NullReferenceException(nameof(_screen));

			await _screen.WaitLoad();

			_screen.Initialize();

			_physicsNoesisSystem.Initialize(_screen);
            
            _regionManager.Initialize(_screen);

			InitializeUIServices();

			StartUpdate();
		}

		/// <summary>
		/// Очистить ядро Noesis
		/// </summary>
		public void Dispose()
		{
			StopUpdate();
			
			//ToDo: тут нужно вставить Dispose методы представлений и не только
		}


		/// <summary>
		/// Проинициализировать UI-сервисы <see cref="IUIService"/>
		/// </summary>
		private void InitializeUIServices()
		{
			_updatedServices.Clear();

			foreach (var service in _UIServices)
			{
				service.Initialize(_regionManager);

				if (service is IUpdatedUIService updatedService)
					_updatedServices.Add(updatedService);
			}
		}


		/// <summary>
		/// Запустить <see cref="Update(CancellationToken)"/>-корутину
		/// </summary>
		private void StartUpdate()
		{
			_updateSource = new CancellationTokenSource();

			updateTask = Update(_updateSource.Token);
		}

		/// <summary>
		/// Остановить <see cref="Update(CancellationToken)"/>-корутину
		/// </summary>
		private async void StopUpdate()
		{
			if (_updateSource != null)
			{
				_updateSource.Cancel();

				if (updateTask.Status == UniTaskStatus.Pending)
					await updateTask;

				_updateSource.Dispose();
			}
		}

		/// <summary>
		/// Корутина для покадрового обновления
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Асинхронное ожидание</returns>
		private async UniTask Update(CancellationToken token = default)
		{
			while (true)
			{
				foreach (var service in _updatedServices)
					service.Update();

				await UniTask.Yield(PlayerLoopTiming.LastUpdate);

				if (token.IsCancellationRequested) 
					return;
			}
		}
	}
}
#endif