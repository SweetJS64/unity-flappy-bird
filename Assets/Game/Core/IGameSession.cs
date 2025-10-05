using UniRx;

namespace Game.Core
{
    public enum GameState
    {
        Idle, 
        Playing, 
        Paused, 
        GameOver
    }

    public interface IGameSession
    {
        IReadOnlyReactiveProperty<GameState> State { get; }
        
        //void StartGame();
        void Pause();
        void Resume();
        void Restart();
        void ToMenu();
    }
}