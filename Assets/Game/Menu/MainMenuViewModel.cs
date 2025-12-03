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

        public int BestScore => _best.GetBestScore();
        public IReadOnlyReactiveProperty<int> Balance => _balance.Balance;
        
        public MainMenuViewModel(IBestScoreService best, IBalanceService balance)
        {
            _best = best;
            _balance = balance;
        }

        public void StartGame() => SceneManager.LoadScene(Scenes.Game);
        public void OpenShop() {}
    }
}