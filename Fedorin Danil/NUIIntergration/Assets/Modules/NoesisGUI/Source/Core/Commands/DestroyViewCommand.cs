#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
#else

#endif

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SmartTwin.NoesisGUI.Commands
{
	using SmartTwin.NoesisGUI.Views;

	/// <summary>
	/// Устарело по причине не использования. 
	/// ToDo: возможно в будущем его придется снова добавить
	/// </summary>
	[Obsolete]
	public class DestroyViewCommand : ICommand
	{
		public event EventHandler CanExecuteChanged = delegate { };

		Action<BaseView> _callback;

		Func<BaseView, bool> _verificationCallback;


		public DestroyViewCommand(Action<BaseView> callback)
		{
			if (callback == null)
				throw new ArgumentNullException("Callback");

			_callback = callback;
		}

		public DestroyViewCommand(Func<BaseView, bool> verificationCallback, Action<BaseView> callback)
			: this(callback)
		{
			if (verificationCallback == null)
				throw new ArgumentNullException("VerificationCallback");

			_verificationCallback = verificationCallback;
		}

		public bool CanExecute(object parameter)
		{
			if (_verificationCallback == null)
				return true;

			if (parameter is BaseView view)
				return _verificationCallback(view);

			return false;
		}

		public void Execute(object parameter)
		{
			if (parameter is BaseView view)
				_callback(view);
		}

		public void RaiseCanExecuteChanged() => CanExecuteChanged(this, EventArgs.Empty);
	}
}
