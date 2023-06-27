#if UNITY_5_3_OR_NEWER
#define NOESIS
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
#else
#endif

using Cysharp.Threading.Tasks;
using System;
using System.Windows.Input;

namespace SmartTwin.NoesisGUI.Commands
{
	/// <summary>
	/// Асинхронная команда. Не вызывается, пока предыдущий вызов не завершится.
	/// ToDo: под сомнением.
	/// </summary>
	public class AsyncCommand : ICommand
	{
		/// <summary>
		/// Событие об изменении возможности выполниться
		/// </summary>
		public event EventHandler CanExecuteChanged = delegate { };

		/// <summary>
		/// Обратный вызов с UniTask-задачей
		/// </summary>
		private readonly Func<object, UniTask> _callback;


		/// <summary>
		/// Обратный вызов проверки возможности выполниться
		/// </summary>
		private Func<object, bool> _verificationCallback;

		/// <summary>
		/// Флаг, показывающий, выполняется ли текущая команда или нет
		/// ToDo: не получится кешировать UniTask, т.к. это структура
		/// </summary>
		private bool isExecuted; 


		public AsyncCommand(Func<object, UniTask> callback)
		{
			if (callback == null)
				throw new ArgumentNullException("Callback");

			_callback = callback;

			isExecuted = false;
		}

		public AsyncCommand(Func<object, UniTask> callback, Func<object, bool> verificationCallback)
			: this(callback)
		{
			if (verificationCallback == null)
				throw new ArgumentNullException("VerificationCallback");

			_verificationCallback = verificationCallback;
		}


		/// <summary>
		/// Проверить возможность выполнения команды
		/// </summary>
		/// <param name="parameter">Параметр для команды</param>
		/// <returns>True, если команда может выполниться. False, если нет</returns>
		public bool CanExecute(object parameter = null)
		{
			if (isExecuted)
				return false;

			return _verificationCallback == null || _verificationCallback(parameter);
		}

		/// <summary>
		/// Выполнить команду
		/// </summary>
		/// <param name="parameter">Параметр для команды</param>
		public async void Execute(object parameter = null)
		{
			if (isExecuted)
				return;

			//ToDo: Нужно ли дополнить или все гениальное простое? 
			isExecuted = true;

			RaiseCanExecuteChanged();

			await _callback(parameter);

			isExecuted = false;

			RaiseCanExecuteChanged();
		}

		/// <summary>
		/// Вызывать событие об изменении возможности выполнения команды
		/// </summary>
		public void RaiseCanExecuteChanged() => CanExecuteChanged(this, EventArgs.Empty);
	}
}
