using Game.Core;
using Game.Core.Signals;
using Game.Common;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Infrastructure
{
    public class GameSessionService : IGameSession, IInitializable, System.IDisposable
    {
        private readonly SignalBus _bus;
        private readonly ReactiveProperty<GameState> _state = new(GameState.Playing);

        public IReadOnlyReactiveProperty<GameState> State => _state;

        public GameSessionService(SignalBus bus) => _bus = bus;

        public void Initialize()
        {
            Time.timeScale = 1f;

            _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Dispose()
        {
            _bus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied() => Set(GameState.GameOver);

        private void Set(GameState state)
        {
            // При повторном входе в то же состояние принудительно уведомляем подписчиков,
            // чтобы View корректно переотрисовался (например, повторный GameOver при рестарте).
            if (_state.Value == state) _state.SetValueAndForceNotify(state);
            else _state.Value = state;

            switch (state)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.Playing:
                case GameState.GameOver:
                    Time.timeScale = 1f;
                    break;
            }
        }

        public void Pause()  => Set(GameState.Paused);
        public void Resume() => Set(GameState.Playing);
        
        public void Restart()
        {
            Time.timeScale = 1f;
            var name = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(name);
        }

        public void ToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(Scenes.Menu);
        }
    }
}