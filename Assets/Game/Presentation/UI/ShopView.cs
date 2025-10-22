using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;

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
        
        public event Action Closed;

        private void OnEnable()
        {
            if (BalanceText)
                _vm.Balance
                    .Subscribe(v => BalanceText.text = $"COINS: {v}")
                    .AddTo(_cd);

            Rebuild();

            if (CloseButton) CloseButton.onClick.AddListener(Close);;
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (CloseButton) CloseButton.onClick.RemoveListener(Close);
            ClearContent();
            Closed?.Invoke();
        }

        private void Rebuild()
        {
            ClearContent();

            foreach (var item in _vm.Items)
            {
                var card = Instantiate(ItemPrefab, Content);
                card.gameObject.SetActive(true);
                card.Bind(_vm, item);
            }
        }

        private void ClearContent()
        {
            if (!Content) return;
            for (int i = Content.childCount - 1; i >= 0; i--)
                Destroy(Content.GetChild(i).gameObject);
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }
}