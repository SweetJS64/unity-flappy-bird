namespace Game.Core
{
    public enum GameState { Idle, Playing, Paused, GameOver }

    public interface IGameSession
    {
        GameState State { get; }
        void StartGame();
        void Pause();
        void Resume();
        void Restart();
    }
}