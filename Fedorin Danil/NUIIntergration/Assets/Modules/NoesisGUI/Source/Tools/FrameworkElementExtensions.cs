#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
#endif

using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace SmartTwin.NoesisGUI.Tools
{
	/// <summary>
	/// Расширения для <see cref="FrameworkElement"/>
	/// </summary>
	public static class FrameworkElementExtensions
	{
		/// <summary>
		/// Асинхронное ожидание события <see cref="FrameworkElement.Loaded"/>
		/// </summary>
		/// <param name="element">Элемент, который нужно ожидать</param>
		/// <param name="token">Токен отмены</param>
		/// <returns></returns>
		public static async UniTask WaitLoad(this FrameworkElement element, CancellationToken token = default)
		{
			if (element.IsLoaded)
				return;

			var completion = new UniTaskCompletionSource();
			RoutedEventHandler loadHandler = (sender, args) => { completion.TrySetResult(); };
			element.Loaded += loadHandler;

			await completion.Task.AttachExternalCancellation(token);
		}

		/// <summary>
		/// Асинхронное ожидание события <see cref="FrameworkElement.Unloaded"/>
		/// </summary>
		/// <param name="element">Элемент, который нужно ожидать</param>
		/// <param name="token">Токен отмены</param>
		/// <returns></returns>
		public static async UniTask WaitUnload(this FrameworkElement element, CancellationToken token = default)
		{
			if (!element.IsLoaded)
				return;

			var completion = new UniTaskCompletionSource();
			RoutedEventHandler loadHandler = (sender, args) => { completion.TrySetResult(); };
			element.Unloaded += loadHandler;

			await completion.Task.AttachExternalCancellation(token);
		}

		/// <summary>
		/// Выполняет обратный вызов, когда элемент полностью загружен (событие <see cref="FrameworkElement.Loaded"/>)
		/// ToDo: добавить возможность отмены или проверки из-за возможной бесконечной загрузки (Loaded никогда не сработает)
		/// </summary>
		/// <param name="element">Элемент, к которому прикрепляется обратный вызов</param>
		/// <param name="callback">Обратный вызов с этим элементом в параметре</param>
		/// <exception cref="NullReferenceException">Если обратный вызов null</exception>
		public static async void ExecuteWhenLoad(this FrameworkElement element, Action<FrameworkElement> callback)
		{
			if (callback == null)
				throw new NullReferenceException(nameof(callback));

			if (element.IsLoaded)
				callback(element);

			await element.WaitLoad();

			callback(element);
		}

		/// <summary>
		/// Выполняет обратный вызов, когда элемент полностью загружен (событие <see cref="FrameworkElement.Unloaded"/>)
		/// ToDo: добавить возможность отмены или проверки из-за возможной бесконечной загрузки (Unloaded никогда не сработает)
		/// </summary>
		/// <param name="element">Элемент, к которому прикрепляется обратный вызов</param>
		/// <param name="callback">Обратный вызов с этим элементом в параметре</param>
		/// <exception cref="NullReferenceException">Если обратный вызов null</exception>
		public static async void ExecuteWhenUnload(this FrameworkElement element, Action<FrameworkElement> callback)
		{
			if (callback == null)
				throw new NullReferenceException(nameof(callback));

			if (!element.IsLoaded)
				callback(element);

			await element.WaitUnload();

			callback(element);
		}
	}
}
