using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation.UI
{
    public class ShopItemView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image Icon;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text PriceText;
        [SerializeField] private Button BuyButton;
        [SerializeField] private Button SelectButton;
        [SerializeField] private GameObject SelectedBadge;

        [Header("Background")]
        [SerializeField] private Image Background;
        [SerializeField] private Sprite NotOwnedSprite;
        [SerializeField] private Sprite OwnedSprite; 
        
        private readonly CompositeDisposable _cd = new();

        public void Bind(ShopViewModel vm, ShopItemVM item)
        {
            if (Icon) Icon.sprite = item.Icon;
            if (NameText) NameText.text = item.DisplayName;
            if (PriceText) PriceText.text = item.Price.ToString();

            item.Owned
                .CombineLatest(item.Selected, (owned, selected) => (owned, selected))
                .Subscribe(state =>
                {
                    var (owned, selected) = state;

                    if (BuyButton) BuyButton.gameObject.SetActive(!owned);
                    if (SelectButton) SelectButton.gameObject.SetActive(owned && !selected);
                    if (SelectedBadge) SelectedBadge.SetActive(selected);
                    if (PriceText) PriceText.gameObject.SetActive(!owned);
                    if (Background) Background.sprite = owned ? OwnedSprite : NotOwnedSprite;
                })
                .AddTo(_cd);

            vm.Balance
              .CombineLatest(item.Owned, (bal, owned) => !owned && bal >= item.Price)
              .Subscribe(canBuy =>
              {
                  if (BuyButton) BuyButton.interactable = canBuy;
              })
              .AddTo(_cd);

            if (BuyButton) BuyButton.onClick.AddListener(() => vm.Buy(item.Id));
            if (SelectButton) SelectButton.onClick.AddListener(() => vm.Select(item.Id));
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (BuyButton) BuyButton.onClick.RemoveAllListeners();
            if (SelectButton) SelectButton.onClick.RemoveAllListeners();
        }
    }
}