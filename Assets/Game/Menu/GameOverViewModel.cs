using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class GameOverViewModel
    {
        private readonly IGameSession _session;
        
        public IReadOnlyReactiveProperty<bool> IsVisible { get; }

        public GameOverViewModel(IGameSession session)
        {
            _session = session;
            IsVisible = session.State
                .Select(s => s == GameState.GameOver)
                .ToReactiveProperty();
        }

        public void Restart() => _session.Restart();

        public void ToMenu() => _session.ToMenu();
    }
}