using UniRx;

namespace Game.Core
{
    public interface IScoreService
    {
        IReadOnlyReactiveProperty<int> Score { get; }
        void Reset();
        void Add(int value);
    }
}