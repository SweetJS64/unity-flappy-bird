using Game.Core;
using UniRx;

namespace Game.Menu
{
    public class ScoreViewModel
    {
        public IReadOnlyReactiveProperty<int> Score => _scoreService.Score;

        private readonly IScoreService _scoreService;

        public ScoreViewModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
    }
}