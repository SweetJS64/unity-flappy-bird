using System.Collections.Generic;
using Game.Core;
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

        private readonly string _defaultSkinId;
        
        public SkinService(string defaultSkinId = "B1_Red")
        {
            _defaultSkinId = defaultSkinId;
            
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

        public bool TryBuy(string id, int price, IBalanceService balance)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (IsOwned(id)) return true;
            if (!balance.TrySpend(price)) return false;

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
        }

        private void SaveOwned()
        {
            var csv = string.Join(",", _owned);
            PlayerPrefs.SetString(OwnedKey, csv);
        }
    }
}