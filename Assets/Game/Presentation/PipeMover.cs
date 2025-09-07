using Game.Core.Signals;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [RequireComponent(typeof(PipeObstacle))]
    public class PipeMover : MonoBehaviour
    {
        [SerializeField] float Speed = 2.5f;
        [SerializeField] float OffscreenX = -12f;

        private PipeObstacle _pair;
        [Inject] private SignalBus _bus;

        private void OnEnable()  => _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        private void OnDisable() => _bus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        
        private void Awake()
        {
            _pair = GetComponent<PipeObstacle>();
        } 

        private void Update()
        {
            transform.position += Vector3.left * (Speed * Time.deltaTime);
            if (transform.position.x < OffscreenX) _pair.Dispose();
        }
        
        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
