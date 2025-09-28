using Game.Core;
using Game.Core.Signals;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Infrastructure
{
    public class GameSessionService : IGameSession, IInitializable, System.IDisposable
    {
        private readonly SignalBus _bus;
        private readonly ReactiveProperty<GameState> _state = new(GameState.Idle);

        public IReadOnlyReactiveProperty<GameState> State => _state;

        public GameSessionService(SignalBus bus)
        {
            _bus = bus;
        }

        public void Initialize()
        {
            _bus.Subscribe<GameStartedSignal>(() => Set(GameState.Playing));
            _bus.Subscribe<PlayerDiedSignal>(() => Set(GameState.GameOver));
        }

        public void Dispose()
        {
            _bus.TryUnsubscribe<GameStartedSignal>(() => Set(GameState.Playing));
            _bus.TryUnsubscribe<PlayerDiedSignal>(() => Set(GameState.GameOver));
        }

        public void Set(GameState state) => _state.Value = state;

        public void Restart()
        {
            var name = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(name);
        }

        public void ToMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}