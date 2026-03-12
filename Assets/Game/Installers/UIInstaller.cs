using Zenject;
using Game.Menu;

namespace Game.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ScoreViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseViewModel>().AsSingle();
        }
    }
}
