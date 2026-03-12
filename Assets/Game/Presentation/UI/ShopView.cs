using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using System.Collections.Generic;

namespace Game.Presentation.UI
{
    public class ShopView : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private Transform Content;
        [SerializeField] private ShopItemView ItemPrefab;

        [Header("Header")]
        [SerializeField] private TMP_Text BalanceText;
        [SerializeField] private Button CloseButton;

        [Inject] private ShopViewModel _vm;

        private readonly CompositeDisposable _cd = new();
        private readonly List<(ShopItemView card, ShopItemVM item)> _cards = new();

        public event Action Closed;

        private void Awake()
        {
            foreach (var item in _vm.Items)
            {
                var card = Instantiate(ItemPrefab, Content);
                card.gameObject.SetActive(true);
                _cards.Add((card, item));
            }
        }

        private void OnEnable()
        {
            if (BalanceText)
                _vm.Balance
                    .Subscribe(v => BalanceText.text = $"COINS: {v}")
                    .AddTo(_cd);

            foreach (var (card, item) in _cards)
                card.Bind(_vm, item);

            if (CloseButton) CloseButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (CloseButton) CloseButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }
}