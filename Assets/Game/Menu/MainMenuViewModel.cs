using Game.Core;
using UnityEngine.SceneManagement;

namespace Game.Menu
{
    public class MainMenuViewModel
    {
        private readonly IBestScoreService _best;

        public int BestScore => _best.GetBestScore();

        public MainMenuViewModel(IBestScoreService best)
        {
            _best = best;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}