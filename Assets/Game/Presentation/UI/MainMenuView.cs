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
            if (BestScoreText) BestScoreText.text = $"Best: {_vm.BestScore}";
            if (BalanceText) _vm.Balance.Subscribe(value => BalanceText.text = $"Coins: {value}")
                    .AddTo(_cd);
            if (ShopButton && ShopPanel) ShopButton.onClick.AddListener(OpenShop);
            if (ShopView) ShopView.Closed += OnShopClosed;
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (StartButton) StartButton.onClick.RemoveListener(_vm.StartGame);
            if (ShopButton) ShopButton.onClick.RemoveListener(OpenShop);
            if (ShopView) ShopView.Closed -= OnShopClosed;
        }
        
        private void OpenShop()
        {
            if (ShopPanel) ShopPanel.SetActive(true);
            if (MainBlockRoot) MainBlockRoot.SetActive(false);
        }

        private void OnShopClosed()
        {
            if (MainBlockRoot) MainBlockRoot.SetActive(true);
        }
    }
}