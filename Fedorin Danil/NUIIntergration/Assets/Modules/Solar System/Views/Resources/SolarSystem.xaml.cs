#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
#endif

using SmartTwin.NoesisGUI.Views;

namespace SolarSystems
{
	/// <summary>
	/// Логика взаимодействия для SolarSystem.xaml
	/// </summary>
	public partial class SolarSystem : BaseView
	{
		static SolarSystem()
		{
			RegisterXamlPath<SolarSystem>();
		}

		public SolarSystem()
		{
			InitializeComponent();
		}

#if NOESIS
		
		private void InitializeComponent() => NoesisUnity.LoadComponent(this);
#endif
	}
}
