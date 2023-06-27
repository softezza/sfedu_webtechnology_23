#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
#endif


namespace SmartTwin.NoesisGUI
{
	/// <summary>
	/// Главный экран UI в Noesis.
	/// <br>Чтобы добавить UI на сцену, необходимо создать xaml-документ, наследуемый от этого типа.</br>
	/// <br>ToDo: Возможно будущее дополнение функционалом</br>
	/// </summary>
	public abstract class MainScreen : UserControl
	{
		public MainScreen()
		{

		}


		public virtual void Initialize() { /* */ }
	}
}
