using UniRx;

namespace Game.Core
{
    public interface IBalanceService
    {
        IReadOnlyReactiveProperty<int> Balance { get; }
        void Add(int amount);
        bool TrySpend(int amount);
    }
}