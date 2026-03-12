using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class PauseViewModel : System.IDisposable
    {
        private readonly IGameSession _session;
        private readonly IBestScoreService _bestScore;
        private readonly CompositeDisposable _cd = new();
        private readonly ReactiveProperty<bool> _isVisible            = new(false);
        private readonly ReactiveProperty<bool> _isPauseButtonVisible = new(false);

        public IReadOnlyReactiveProperty<bool> IsVisible            => _isVisible;
        public IReadOnlyReactiveProperty<bool> IsPauseButtonVisible => _isPauseButtonVisible;

        public PauseViewModel(IGameSession session, IBestScoreService bestScore)
        {
            _session = session;
            _bestScore = bestScore;

            session.State
                .Subscribe(s => _isVisible.Value = s == GameState.Paused)
                .AddTo(_cd);

            session.State
                .Subscribe(s => _isPauseButtonVisible.Value = s == GameState.Playing)
                .AddTo(_cd);
        }

        public int BestScore => _bestScore.GetBestScore();

        public void Pause()  => _session.Pause();
        public void Resume() => _session.Resume();
        public void ToMenu() => _session.ToMenu();

        public void Dispose()
        {
            _cd.Dispose();
            _isVisible.Dispose();
            _isPauseButtonVisible.Dispose();
        }
    }
}