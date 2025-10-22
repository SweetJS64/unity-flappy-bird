using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.UI
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject PanelRoot;
        [SerializeField] private Button ResumeButton;
        [SerializeField] private Button MenuButton;
        [SerializeField] private TMP_Text BestScoreText;

        [Inject] private PauseViewModel _vm;

        private readonly CompositeDisposable _cd = new();

        private void Awake()
        {
            if (PanelRoot == null)
                PanelRoot = gameObject;
        }

        private void OnEnable()
        {
            _vm.IsVisible
                .Subscribe(visible =>
                {
                    PanelRoot.SetActive(visible);
                    if (visible && BestScoreText != null)
                        BestScoreText.text = $"BEST: {_vm.BestScore}";
                })
                .AddTo(_cd);

            if (ResumeButton != null) ResumeButton.onClick.AddListener(_vm.Resume);
            if (MenuButton != null) MenuButton.onClick.AddListener(_vm.ToMenu);
        }

        private void OnDisable()
        {
            _cd.Clear();
            if (ResumeButton != null) ResumeButton.onClick.RemoveListener(_vm.Resume);
            if (MenuButton != null) MenuButton.onClick.RemoveListener(_vm.ToMenu);
        }
    }
}