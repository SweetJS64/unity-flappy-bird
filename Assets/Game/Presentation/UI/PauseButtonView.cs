using Game.Menu;
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
            _vm.IsPauseButtonVisible
                .Subscribe(visible => PauseButton.gameObject.SetActive(visible))
                .AddTo(_cd);

            PauseButton.onClick.AddListener(_vm.Pause);
        }

        private void OnDisable()
        {
            _cd.Clear();
            PauseButton.onClick.RemoveListener(_vm.Pause);
        }
    }
}