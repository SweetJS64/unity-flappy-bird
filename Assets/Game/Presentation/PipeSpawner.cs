using Game.Core.Signals;
using Game.Common;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PipeSpawner : MonoBehaviour
    {
        [Header("Placement")]
        [SerializeField] private float MinY = -1.5f;
        [SerializeField] private float MaxY =  1.5f;
        [SerializeField] private float SpawnPaddingX = 0.8f;
        
        [Header("Timing")]
        [SerializeField] private float Interval = 1.4f;

        private float _timer;
        private Camera _cam;

        [Inject] private PipeObstacle.Pool _pool;
        [Inject] private SignalBus _bus;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void OnEnable()   => _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        private void OnDisable()  => _bus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Interval)
            {
                _timer = 0f;
                Spawn();
            }
        }

        private void Spawn()
        {
            var x = CameraBounds.RightX(_cam, SpawnPaddingX);
            var y = Random.Range(MinY, MaxY);
            _pool.Spawn(new Vector2(x, y));
        }
        
        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
