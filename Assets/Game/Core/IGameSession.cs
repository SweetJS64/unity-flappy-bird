using UniRx;

namespace Game.Core
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public interface IGameSession
    {
        IReadOnlyReactiveProperty<GameState> State { get; }

        void Pause();
        void Resume();
        void Restart();
        void ToMenu();
    }
}