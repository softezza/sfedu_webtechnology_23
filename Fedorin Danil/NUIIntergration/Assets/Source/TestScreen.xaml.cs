#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
#endif

using SmartTwin.NoesisGUI;

namespace NUIIntergration
{
	/// <summary>
	/// Логика взаимодействия для TestScreen.xaml
	/// </summary>
	public partial class TestScreen : MainScreen
	{
		public TestScreen()
		{
			InitializeComponent();
		}

#if NOESIS
		
		private void InitializeComponent() => NoesisUnity.LoadComponent(this);
#endif
	}
}
