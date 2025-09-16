using Zenject;
using Game.Menu;

namespace Game.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuViewModel>().AsSingle();
        }
    }
}