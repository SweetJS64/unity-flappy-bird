using Game.Core.Signals;
using UnityEngine;
using Zenject;
using Game.Common;

namespace Game.Presentation
{
    [RequireComponent(typeof(PipeObstacle))]
    public class PipeMover : MonoBehaviour
    {
        [SerializeField] float Speed = 2.5f;
        [SerializeField] private float DespawnPaddingX = 0.8f;

        private PipeObstacle _pair;
        private Camera _cam;
        private float _offscreenLeftX;
        
        [Inject] private SignalBus _bus;

        private void Awake()
        {
            _pair = GetComponent<PipeObstacle>();
            _cam = Camera.main;
            _offscreenLeftX = CameraBounds.LeftX(_cam, DespawnPaddingX);
        }
        
        private void OnEnable()  => _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        private void OnDisable() => _bus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        
        private void Update()
        {
            transform.position += Vector3.left * (Speed * Time.deltaTime);
            if (transform.position.x < _offscreenLeftX)
                _pair.Dispose();
        }
        
        private void OnPlayerDied() => enabled = false;
    }
}
