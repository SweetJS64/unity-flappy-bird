using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Skins;
using UniRx;
using UnityEngine;

namespace Game.Menu
{
    public sealed class ShopItemVM
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int Price { get; }
        public Sprite Icon { get; }

        public readonly ReactiveProperty<bool> Owned = new(false);
        public readonly ReactiveProperty<bool> Selected = new(false);

        public ShopItemVM(SkinDef def)
        {
            Id = def.Id;
            DisplayName = def.DisplayName;
            Price = def.Price;
            Icon = def.Icon;
        }
    }

    public sealed class ShopViewModel : System.IDisposable
    {
        public IReadOnlyReactiveProperty<int> Balance => _balance.Balance;
        public IReadOnlyList<ShopItemVM> Items => _items;

        private readonly IBalanceService _balance;
        private readonly ISkinService _skins;
        private readonly List<ShopItemVM> _items = new();
        private readonly CompositeDisposable _cd = new();

        public ShopViewModel(IBalanceService balance, ISkinService skins, SkinCatalog catalog)
        {
            _balance = balance;
            _skins   = skins;

            if (catalog != null && catalog.Items != null)
            {
                foreach (var def in catalog.Items.Where(d => d != null))
                    _items.Add(new ShopItemVM(def));
            }

            SyncAll();

            _skins.SelectedId
                  .Subscribe(_ => SyncSelected())
                  .AddTo(_cd);
        }

        public void Dispose() => _cd.Dispose();

        public void Buy(string id)
        {
            var item = Find(id);
            if (item == null) return;

            if (_skins.TryBuy(item.Id, item.Price))
                SyncOwned();
        }

        public void Select(string id)
        {
            var item = Find(id);
            if (item == null) return;
            _skins.Select(item.Id);
        }

        public bool CanBuy(string id)
        {
            var item = Find(id);
            if (item == null) return false;
            return !_skins.IsOwned(item.Id) && _balance.Balance.Value >= item.Price;
        }

        private ShopItemVM Find(string id) => _items.FirstOrDefault(i => i.Id == id);

        private void SyncAll()
        {
            SyncOwned();
            SyncSelected();
        }

        private void SyncOwned()
        {
            foreach (var it in _items)
                it.Owned.Value = _skins.IsOwned(it.Id);
        }

        private void SyncSelected()
        {
            var selected = _skins.SelectedId.Value;
            foreach (var it in _items)
                it.Selected.Value = (it.Id == selected);
        }
    }
}