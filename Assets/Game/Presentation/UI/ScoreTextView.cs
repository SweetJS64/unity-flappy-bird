using Game.Menu;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Presentation.UI
{
    public class ScoreTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text ScoreText;

        [Inject] private ScoreViewModel _vm;

        private readonly CompositeDisposable _cd = new();

        private void OnEnable()
        {
            if (ScoreText == null)
                ScoreText = GetComponent<TMP_Text>();

            _vm.Score
                .Subscribe(value => ScoreText.text = value.ToString())
                .AddTo(_cd);
        }

        private void OnDisable()
        {
            _cd.Clear();
        }
    }
}