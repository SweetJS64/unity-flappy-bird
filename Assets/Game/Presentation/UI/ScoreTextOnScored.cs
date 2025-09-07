using UnityEngine;
using Zenject;
using Game.Core.Signals;
using TMPro;

namespace Game.Presentation.UI
{
    public class ScoreTextOnScored : MonoBehaviour
    {
        [SerializeField] private TMP_Text  ScoreText;

        [Inject] private SignalBus _bus;
        private int _score;

        private void OnEnable() => _bus.Subscribe<PlayerScoredSignal>(OnScored);

        private void OnDisable() => _bus.Unsubscribe<PlayerScoredSignal>(OnScored);

        private void OnScored(PlayerScoredSignal sig)
        {
            _score += sig.Value;
            ScoreText.text = _score.ToString();
        }
    }
}