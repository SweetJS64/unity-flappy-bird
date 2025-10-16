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
        [SerializeField] private TMP_Text BestScoreText;
        [SerializeField] private TMP_Text BalanceText;

        [Inject] private MainMenuViewModel _vm;
        
        private readonly CompositeDisposable _cd = new();
        
        private void OnEnable()
        {
            if (StartButton)
                StartButton.onClick.AddListener(_vm.StartGame);
            
            if (BestScoreText)
                BestScoreText.text = $"Best: {_vm.BestScore}";
            
            if (BalanceText) 
                _vm.Balance.Subscribe(value => BalanceText.text = $"Coins: {value}")
                    .AddTo(_cd);
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (StartButton != null)
                StartButton.onClick.RemoveListener(_vm.StartGame);
        }
    }
}