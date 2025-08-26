using Zenject;
using Game.Core;
using Game.Infrastructure;
using Game.Presentation;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputService>().To<DesktopInputService>().AsSingle();
            
            // #if UNITY_EDITOR || UNITY_STANDALONE
            //     Container.Bind<IInputService>().To<DesktopInputService>().AsSingle();
            // #else
            //     Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
            // #endif

            Container.Bind<BirdController>().FromComponentInHierarchy().AsSingle();
        }
    }
}