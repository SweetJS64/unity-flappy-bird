using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class PauseViewModel
    {
        public IGameSession Session { get; }
        private readonly IBestScoreService _bestScore;

        public IReadOnlyReactiveProperty<bool> IsVisible { get; }

        public PauseViewModel(IGameSession session, IBestScoreService bestScore)
        {
            Session = session;
            _bestScore = bestScore;

            IsVisible = session.State
                .Select(s => s == GameState.Paused)
                .ToReactiveProperty();
        }

        public int BestScore => _bestScore.GetBestScore();

        public void Pause()  => Session.Pause();
        public void Resume() => Session.Resume();
        public void ToMenu() => Session.ToMenu();
    }
}