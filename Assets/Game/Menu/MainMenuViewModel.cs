using UnityEngine.SceneManagement;

namespace Game.Menu
{
    public class MainMenuViewModel
    {
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}