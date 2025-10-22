using Zenject;
using Game.Infrastructure;
using Game.Core;
using Game.Skins;
using UnityEngine;

namespace Game.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SkinCatalog SkinCatalog;
        public override void InstallBindings()
        {
            Container.Bind<IBestScoreService>().To<BestScoreService>().AsSingle();
            Container.Bind<IBalanceService>().To<BalanceService>().AsSingle();
            Container.Bind<ISkinService>()
                .To<SkinService>()
                .AsSingle()
                .WithArguments("Red");
            Container.Bind<SkinCatalog>()
                .FromInstance(SkinCatalog)
                .AsSingle();
        }
    }
}