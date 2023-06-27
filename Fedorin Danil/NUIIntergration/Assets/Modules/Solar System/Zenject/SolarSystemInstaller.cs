using Zenject;
using SmartTwin.NoesisGUI.Regions;
using DataBinding.ViewModels;

namespace SolarSystems
{
    /// <summary>
    /// Установщик ядра NoesisGUI для Unity
    /// </summary>
    public class SolarSystemInstaller : MonoInstaller
    {
		public override void InstallBindings()
        {
            Container.AttachViewToRegions<SolarSystem, SolarSystemViewModel>("SolarSystem");
        }

	}
}