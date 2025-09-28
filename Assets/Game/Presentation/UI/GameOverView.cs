using Game.Menu;
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

        [Inject] private GameOverViewModel _vm;

        private readonly CompositeDisposable _cd = new();

        private void OnEnable()
        {
            _vm.IsVisible
                .Subscribe(visible => PanelRoot.SetActive(visible))
                .AddTo(_cd);

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