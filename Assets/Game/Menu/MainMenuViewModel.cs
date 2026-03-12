using Game.Core;
using Game.Common;
using UniRx;
using UnityEngine.SceneManagement;

namespace Game.Menu
{
    public class MainMenuViewModel
    {
        private readonly IBestScoreService _best;
        private readonly IBalanceService _balance;
        private readonly ReactiveProperty<bool> _isShopOpen = new(false);

        public int BestScore => _best.GetBestScore();
        public IReadOnlyReactiveProperty<int> Balance => _balance.Balance;
        public IReadOnlyReactiveProperty<bool> IsShopOpen => _isShopOpen;

        public MainMenuViewModel(IBestScoreService best, IBalanceService balance)
        {
            _best = best;
            _balance = balance;
        }

        public void StartGame()  => SceneManager.LoadScene(Scenes.Game);
        public void OpenShop()  => _isShopOpen.Value = true;
        public void CloseShop() => _isShopOpen.Value = false;
    }
}