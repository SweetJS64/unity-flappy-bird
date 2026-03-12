using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class PauseViewModel
    {
        private readonly IGameSession _session;
        private readonly IBestScoreService _bestScore;

        public IReadOnlyReactiveProperty<bool> IsVisible { get; }
        public IReadOnlyReactiveProperty<bool> IsPauseButtonVisible { get; }

        public PauseViewModel(IGameSession session, IBestScoreService bestScore)
        {
            _session = session;
            _bestScore = bestScore;

            IsVisible = session.State
                .Select(s => s == GameState.Paused)
                .ToReactiveProperty();

            IsPauseButtonVisible = session.State
                .Select(s => s == GameState.Playing)
                .ToReactiveProperty();
        }

        public int BestScore => _bestScore.GetBestScore();

        public void Pause()  => _session.Pause();
        public void Resume() => _session.Resume();
        public void ToMenu() => _session.ToMenu();
    }
}