using Game.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button StartButton;
        [SerializeField] private TMP_Text BestScoreText;

        [Inject] private MainMenuViewModel _vm;

        private void Awake()
        {
            if (StartButton == null)
                StartButton = GetComponentInChildren<Button>(true);

            if (BestScoreText == null)
                BestScoreText = GetComponentInChildren<TMP_Text>(true);
        }

        private void OnEnable()
        {
            if (BestScoreText != null)
                BestScoreText.text = $"Best: {_vm.BestScore}";

            if (StartButton != null)
                StartButton.onClick.AddListener(_vm.StartGame);
        }

        private void OnDisable()
        {
            if (StartButton != null)
                StartButton.onClick.RemoveListener(_vm.StartGame);
        }
    }
}