using Game.Core;
using Game.Core.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class ScoreService : IScoreService, IInitializable, System.IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ReactiveProperty<int> _score = new(0);

        public IReadOnlyReactiveProperty<int> Score => _score;

        public ScoreService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerScoredSignal>(OnScored);
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<PlayerScoredSignal>(OnScored);
            _signalBus.TryUnsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Reset() => _score.Value = 0;

        public void Add(int value) => _score.Value += value;

        private void OnScored(PlayerScoredSignal signal)
        { 
            Add(signal.Value);
        } 

        private void OnGameStarted() => Reset();

        private void OnPlayerDied()
        {
            //TODO: BestScore
        }
    }
}