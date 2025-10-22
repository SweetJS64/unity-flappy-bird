using UniRx;

namespace Game.Core
{
    public interface ISkinService
    {
        IReadOnlyReactiveProperty<string> SelectedId { get; }

        bool IsOwned(string id);
        bool TryBuy(string id, int price, IBalanceService balance);
        void Select(string id);
    }
}