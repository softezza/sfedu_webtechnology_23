#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using System;
#else
using System.Windows;
#endif

using System.Threading;
using Cysharp.Threading.Tasks;
using System;

namespace SmartTwin.NoesisGUI.Tools
{
	[Obsolete]
	public static class NoesisGUIHelper
	{
		public static async UniTask WaitElementLoad(FrameworkElement element, CancellationToken token = default)
		{
			if (element.IsLoaded)
				return;

			var completion = new UniTaskCompletionSource();
			RoutedEventHandler loadHandler = (sender, args) => { completion.TrySetResult(); };
			element.Loaded += loadHandler;

			await completion.Task.AttachExternalCancellation(token);
		}

		public static async UniTask WaitElementUnload(FrameworkElement element, CancellationToken token = default)
		{
			if (!element.IsLoaded)
				return;

			var completion = new UniTaskCompletionSource();
			RoutedEventHandler loadHandler = (sender, args) => { completion.TrySetResult(); };
			element.Unloaded += loadHandler;

			await completion.Task.AttachExternalCancellation(token);
		}

		public static async void ExecuteWhenLoad(FrameworkElement element, Action<FrameworkElement> callback)
		{
			if (callback == null)
				throw new NullReferenceException(nameof(callback));

			if (element.IsLoaded)
				callback(element);

		}
	}
}
