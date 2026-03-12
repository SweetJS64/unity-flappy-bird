using UniRx;

namespace Game.Core
{
    public interface IScoreService
    {
        IReadOnlyReactiveProperty<int> Score { get; }
    }
}