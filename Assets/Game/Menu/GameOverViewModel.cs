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

        private readonly ReactiveProperty<int> _bestScore = new();
        private readonly CompositeDisposable _cd = new();

        public IReadOnlyReactiveProperty<bool> IsVisible { get; }
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

            _bestScore.Value = _best.GetBestScore();

            IsVisible = _session.State
                .Select(s => s == GameState.GameOver)
                .ToReactiveProperty();

            IsVisible
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
        public void Dispose() => _cd.Dispose();
    }
}