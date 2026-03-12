using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class GameOverViewModel : System.IDisposable
    {
        private readonly IGameSession _session;
        private readonly IScoreService _score;
        private readonly IBestScoreService _best;
        private readonly IBalanceService _balance;

        private readonly ReactiveProperty<bool> _isVisible = new(false);
        private readonly ReactiveProperty<int> _bestScore;
        private readonly CompositeDisposable _cd = new();

        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public IReadOnlyReactiveProperty<int> BestScore => _bestScore;

        public int GainedThisRun => _score.Score.Value;
        public IReadOnlyReactiveProperty<int> TotalBalance => _balance.Balance;

        public GameOverViewModel(
            IGameSession session,
            IScoreService scoreService,
            IBestScoreService bestScoreService,
            IBalanceService balanceService)
        {
            _session = session;
            _score = scoreService;
            _best = bestScoreService;
            _balance = balanceService;

            _bestScore = new(_best.GetBestScore());

            _session.State
                .Subscribe(s => _isVisible.Value = s == GameState.GameOver)
                .AddTo(_cd);

            _isVisible
                .Where(v => v)
                .Subscribe(_ => TryUpdateBest())
                .AddTo(_cd);
        }

        private void TryUpdateBest()
        {
            var current = _score.Score.Value;
            var best    = _best.GetBestScore();

            if (current > best)
            {
                _best.SetBestScore(current);
                _bestScore.Value = current;
            }
        }

        public void Restart() => _session.Restart();
        public void ToMenu()  => _session.ToMenu();
        public void Dispose()
        {
            _cd.Dispose();
            _isVisible.Dispose();
            _bestScore.Dispose();
        }
    }
}