using Game.Core;
using UniRx;
using UnityEngine;

namespace Game.Infrastructure
{
    public class BalanceService : IBalanceService
    {
        private const string Key = "balance";
        private readonly ReactiveProperty<int> _balance = new(PlayerPrefs.GetInt(Key, 0));

        public IReadOnlyReactiveProperty<int> Balance => _balance;

        public void Add(int amount)
        {
            if (amount <= 0) return;
            _balance.Value += amount;
            PlayerPrefs.SetInt(Key, _balance.Value);
            PlayerPrefs.Save();
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0 || _balance.Value < amount) return false;
            _balance.Value -= amount;
            PlayerPrefs.SetInt(Key, _balance.Value);
            PlayerPrefs.Save();
            return true;
        }
    }
}