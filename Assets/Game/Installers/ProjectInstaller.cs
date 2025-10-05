using Zenject;
using Game.Infrastructure;
using Game.Core;

namespace Game.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IBestScoreService>().To<BestScoreService>().AsSingle();
        }
    }
}