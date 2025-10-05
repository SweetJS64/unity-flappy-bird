using Game.Core;
using UniRx;
using System;

namespace Game.Menu
{
    public class GameOverViewModel : IDisposable
    {
        private readonly IGameSession _session;
        private readonly IScoreService _scoreService;
        private readonly IBestScoreService _bestScoreService;

        private readonly ReactiveProperty<int> _bestScore;
        private readonly CompositeDisposable _cd = new();

        public IReadOnlyReactiveProperty<bool> IsVisible { get; }

        public IReadOnlyReactiveProperty<int> BestScore => _bestScore;

        public GameOverViewModel(
            IGameSession session,
            IScoreService scoreService,
            IBestScoreService bestScoreService)
        {
            _session = session;
            _scoreService = scoreService;
            _bestScoreService = bestScoreService;

            _bestScore = new ReactiveProperty<int>(_bestScoreService.GetBestScore());

            IsVisible = _session.State
                                .Select(s => s == GameState.GameOver)
                                .ToReactiveProperty();

            IsVisible
                .Where(visible => visible)
                .Subscribe(_ => TryUpdateBest())
                .AddTo(_cd);
        }

        private void TryUpdateBest()
        {
            var current = _scoreService.Score.Value;
            var best = _bestScoreService.GetBestScore();

            if (current > best)
            {
                _bestScoreService.SetBestScore(current);
                _bestScore.Value = current;
            }
        }

        public void Restart() => _session.Restart();
        public void ToMenu()  => _session.ToMenu();

        public void Dispose() => _cd.Dispose();
    }
}