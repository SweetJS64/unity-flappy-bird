using Zenject;
using Game.Menu;

namespace Game.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameViewModel>().AsSingle();
            Container.Bind<GameOverViewModel>().AsSingle();
        }
    }
}
