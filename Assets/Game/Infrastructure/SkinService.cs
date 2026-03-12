using System.Collections.Generic;
using Game.Core;
using Game.Skins;
using UniRx;
using UnityEngine;

namespace Game.Infrastructure
{
    public class SkinService : ISkinService
    {
        private const string OwnedKey    = "skins_owned_csv";
        private const string SelectedKey = "skin_selected_id";

        private readonly HashSet<string> _owned;
        private readonly ReactiveProperty<string> _selectedId;

        public IReadOnlyReactiveProperty<string> SelectedId => _selectedId;

        private readonly IBalanceService _balance;
        private readonly string _defaultSkinId;

        public SkinService(IBalanceService balance, SkinCatalog catalog)
        {
            _balance = balance;
            _defaultSkinId = catalog.GetDefault()?.Id ?? "";
            
            var csv = PlayerPrefs.GetString(OwnedKey, "");
            _owned = new HashSet<string>();
            if (!string.IsNullOrEmpty(csv))
            {
                foreach (var id in csv.Split(','))
                    if (!string.IsNullOrWhiteSpace(id))
                        _owned.Add(id.Trim());
            }

            if (!_owned.Contains(_defaultSkinId))
                _owned.Add(_defaultSkinId);

            SaveOwned();

            var sel = PlayerPrefs.GetString(SelectedKey, _defaultSkinId);
            if (!_owned.Contains(sel))
                sel = _defaultSkinId;

            _selectedId = new ReactiveProperty<string>(sel);
        }

        public bool IsOwned(string id) => !string.IsNullOrEmpty(id) && _owned.Contains(id);

        public bool TryBuy(string id, int price)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (IsOwned(id)) return true;
            if (!_balance.TrySpend(price)) return false;

            _owned.Add(id);
            SaveOwned();
            return true;
        }

        public void Select(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!IsOwned(id)) return;

            _selectedId.Value = id;
            PlayerPrefs.SetString(SelectedKey, id);
            PlayerPrefs.Save();
        }

        private void SaveOwned()
        {
            var csv = string.Join(",", _owned);
            PlayerPrefs.SetString(OwnedKey, csv);
            PlayerPrefs.Save();
        }
    }
}