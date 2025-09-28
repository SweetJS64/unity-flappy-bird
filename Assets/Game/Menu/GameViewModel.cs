using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class GameViewModel
    {
        public IReadOnlyReactiveProperty<int> Score => _scoreService.Score;

        private readonly IScoreService _scoreService;

        public GameViewModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
    }
}