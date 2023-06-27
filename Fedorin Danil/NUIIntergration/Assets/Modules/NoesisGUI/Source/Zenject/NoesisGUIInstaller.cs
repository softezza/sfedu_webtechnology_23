#if UNITY_5_3_OR_NEWER
using SmartTwin.NoesisGUI.Physics;
using SmartTwin.NoesisGUI.Regions;
using Zenject;

namespace SmartTwin.NoesisGUI
{
	/// <summary>
	/// Установщик ядра NoesisGUI для Unity
	/// </summary>
	public class NoesisGUIInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<RegionManager>()
				.AsSingle()
				.NonLazy();

			Container.BindInterfacesAndSelfTo<NoesisEngine>()
				.AsSingle()
				.NonLazy();

            Container.BindInterfacesAndSelfTo<PhysicsNoesisSystem>()
                .AsSingle()
                .NonLazy();

			Container.BindInterfacesAndSelfTo<ViewFactoryRegistry>()
				.AsSingle()
				.NonLazy();

			Container.BindInterfacesAndSelfTo<ViewPipeline>()
				.AsSingle()
				.NonLazy();
        }
	}
}
#endif