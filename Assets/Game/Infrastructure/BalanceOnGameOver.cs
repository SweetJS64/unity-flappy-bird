using Game.Core;
using Game.Core.Signals;
using Zenject;

namespace Game.Infrastructure
{
    public class BalanceOnGameOver : IInitializable, System.IDisposable
    {
        private readonly SignalBus _bus;
        private readonly IScoreService _score;
        private readonly IBalanceService _balance;

        public BalanceOnGameOver(SignalBus bus, IScoreService score, IBalanceService balance)
        {
            _bus = bus; 
            _score = score; 
            _balance = balance;
        }

        public void Initialize() => _bus.Subscribe<PlayerDiedSignal>(OnDied);
        public void Dispose() => _bus.TryUnsubscribe<PlayerDiedSignal>(OnDied);

        private void OnDied()
        {
            var gained = _score.Score.Value;
            if (gained > 0)
                _balance.Add(gained);
        }
    }
}