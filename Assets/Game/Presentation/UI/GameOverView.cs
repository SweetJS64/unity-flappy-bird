using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject PanelRoot;
        [SerializeField] private Button RestartButton;
        [SerializeField] private Button MenuButton;
        [SerializeField] private TMP_Text BestScoreText;
        [SerializeField] private TMP_Text GainedText;
        [SerializeField] private TMP_Text BalanceText; 

        [Inject] private GameOverViewModel _vm;

        private readonly CompositeDisposable _cd = new();

        private void Awake()
        {
            if (PanelRoot == null) PanelRoot = gameObject;
        }

        private void OnEnable()
        {
            _vm.IsVisible
                .Subscribe(visible =>
                {
                    PanelRoot.SetActive(visible);
                    if (visible)
                    {
                        if (GainedText)  GainedText.text  = $"+{_vm.GainedThisRun}";
                    }
                })
                .AddTo(_cd);

            if (BestScoreText != null)
            {
                _vm.BestScore
                    .Subscribe(v => BestScoreText.text = $"BEST: {v}")
                    .AddTo(_cd);
            }
            
            if (BalanceText)
            {
                _vm.TotalBalance
                    .Subscribe(v => BalanceText.text = $"COINS: {v}")
                    .AddTo(_cd);
            }


            RestartButton.onClick.AddListener(_vm.Restart);
            MenuButton.onClick.AddListener(_vm.ToMenu);
        }

        private void OnDisable()
        {
            _cd.Clear();
            RestartButton.onClick.RemoveListener(_vm.Restart);
            MenuButton.onClick.RemoveListener(_vm.ToMenu);
        }
    }
}