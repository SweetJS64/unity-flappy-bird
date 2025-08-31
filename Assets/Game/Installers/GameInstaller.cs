using Zenject;
using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using UnityEngine;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Pipes")]
        [SerializeField] private PipeObstacle PipePrefab;
        [SerializeField] private Transform PipesRoot; 
        
        public override void InstallBindings()
        {
            Container.Bind<IInputService>().To<DesktopInputService>().AsSingle();
            
            // #if UNITY_EDITOR || UNITY_STANDALONE
            //     Container.Bind<IInputService>().To<DesktopInputService>().AsSingle();
            // #else
            //     Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
            // #endif

            Container.Bind<BirdController>().FromComponentInHierarchy().AsSingle();
            
            
            Container.BindMemoryPool<PipeObstacle, PipeObstacle.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(PipePrefab)
                .UnderTransform(PipesRoot);

        }
    }
}