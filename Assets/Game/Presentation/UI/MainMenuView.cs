using UnityEngine;
using Zenject;
using Game.Menu;

namespace Game.Presentation.UI
{
    public class MainMenuView : MonoBehaviour
    {
        private MainMenuViewModel _vm;

        [Inject]
        public void Construct(MainMenuViewModel vm)
        {
            _vm = vm;
        }

        public void OnStartButtonClicked() => _vm.StartGame();
    }
}