using Zenject;
using Game.Menu;

namespace Game.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ScoreViewModel>().AsSingle();
            Container.Bind<GameOverViewModel>().AsSingle();
            Container.Bind<PauseViewModel>().AsSingle();
        }
    }
}
