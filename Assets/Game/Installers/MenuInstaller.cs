using Zenject;
using Game.Menu;
using Game.Skins;
using UnityEngine;

namespace Game.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.Bind<MainMenuViewModel>().AsSingle();
            Container.Bind<ShopViewModel>().AsSingle();
        }
    }
}