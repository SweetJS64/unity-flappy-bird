using Game.Menu;
using Game.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.UI
{
    public class PauseButtonView : MonoBehaviour
    {
        [SerializeField] private Button PauseButton;

        [Inject] private PauseViewModel _vm;

        private readonly CompositeDisposable _cd = new();

        private void Awake()
        {
            if (PauseButton == null)
                PauseButton = GetComponentInChildren<Button>(true);
        }

        private void OnEnable()
        {
            _vm.Session.State
                .Subscribe(OnGameStateChanged)
                .AddTo(_cd);

            PauseButton.onClick.AddListener(_vm.Pause);
        }

        private void OnDisable()
        {
            _cd.Clear();
            PauseButton.onClick.RemoveListener(_vm.Pause);
        }

        private void OnGameStateChanged(GameState state)
        {
            var shouldShow = state is GameState.Playing or GameState.Idle;
            PauseButton.gameObject.SetActive(shouldShow);
        }
    }
}