#if UNITY_5_3_OR_NEWER
#define NOESIS
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
#else
#endif

using System;
using System.Windows.Input;

namespace SmartTwin.NoesisGUI.Commands
{
	/// <summary>
	/// Стандартная команда в контексте MVVM
	/// </summary>
	public class DelegateCommand : ICommand
	{
		/// <summary>
		/// Событие изменения возможности выполнения команды
		/// </summary>
		public event EventHandler CanExecuteChanged = delegate { };

		/// <summary>
		/// Обратный вызов, который вызывается командой
		/// </summary>
		private readonly Action<object> _callback;

		/// <summary>
		/// Обратный вызов проверки возможности вызова команды
		/// </summary>
		private Func<object, bool> _verificationCallback;
		

		public DelegateCommand(Action<object> callback)
		{
			if (callback == null)
				throw new ArgumentNullException("Callback");

			_callback = callback;
		}

		public DelegateCommand(Action<object> callback, Func<object, bool> verificationCallback)
			: this(callback)
		{
			if (verificationCallback == null)
				throw new ArgumentNullException("VerificationCallback");

			_verificationCallback = verificationCallback;
		}

		/// <summary>
		/// Проверить возможность вызова команды
		/// </summary>
		/// <param name="parameter">Параметр для команды</param>
		/// <returns>True, если можно. False, если нельзя.</returns>
		public bool CanExecute(object parameter) => _verificationCallback == null || _verificationCallback(parameter);

		/// <summary>
		/// Вызвать команду
		/// </summary>
		/// <param name="parameter">Параметр для команды</param>
		public void Execute(object parameter) => _callback(parameter);

		/// <summary>
		/// Вызвать событие изменения возможности вызова команды
		/// </summary>
		public void RaiseCanExecuteChanged() => CanExecuteChanged(this, EventArgs.Empty);
	}
}
