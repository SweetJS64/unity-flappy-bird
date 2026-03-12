using UnityEngine;
using Zenject;
using Game.Core.Signals;

namespace Game.Presentation
{
    [RequireComponent(typeof(Collider2D))]
    public class ScoreGate : MonoBehaviour
    {
        [SerializeField] private int Points = 1;

        [Inject] private SignalBus _bus;

        private void OnTriggerEnter2D(Collider2D other) => 
            _bus.Fire(new PlayerScoredSignal(Points));
    }
}