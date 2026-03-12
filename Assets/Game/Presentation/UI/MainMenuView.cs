using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button StartButton;
        [SerializeField] private Button ShopButton;
        [SerializeField] private TMP_Text BestScoreText;
        [SerializeField] private TMP_Text BalanceText;
        
        [Header("Panels")]
        [SerializeField] private GameObject MainBlockRoot;
        [SerializeField] private GameObject ShopPanel;
        [SerializeField] private ShopView ShopView;

        [Inject] private MainMenuViewModel _vm;
        
        private readonly CompositeDisposable _cd = new();
        
        private void OnEnable()
        {
            if (StartButton) StartButton.onClick.AddListener(_vm.StartGame);
            if (BestScoreText) BestScoreText.text = $"BEST: {_vm.BestScore}";
            if (BalanceText) _vm.Balance
                .Subscribe(value => BalanceText.text = $"COINS: {value}")
                .AddTo(_cd);

            _vm.IsShopOpen
                .Subscribe(isOpen =>
                {
                    if (ShopPanel) ShopPanel.SetActive(isOpen);
                    if (MainBlockRoot) MainBlockRoot.SetActive(!isOpen);
                })
                .AddTo(_cd);

            if (ShopButton) ShopButton.onClick.AddListener(_vm.OpenShop);
            if (ShopView) ShopView.Closed += _vm.CloseShop;
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (StartButton) StartButton.onClick.RemoveListener(_vm.StartGame);
            if (ShopButton) ShopButton.onClick.RemoveListener(_vm.OpenShop);
            if (ShopView) ShopView.Closed -= _vm.CloseShop;
        }
    }
}