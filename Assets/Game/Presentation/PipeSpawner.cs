using Game.Core.Signals;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PipeSpawner : MonoBehaviour
    {
        [Header("Placement")]
        [SerializeField] private Transform SpawnPoint;
        [SerializeField] private float MinY = -1.5f;
        [SerializeField] private float MaxY =  1.5f;

        [Header("Timing")]
        [SerializeField] private float Interval = 1.4f;

        private float _timer;
        private bool _started; 

        [Inject] private PipeObstacle.Pool _pool;
        [Inject] private SignalBus _bus;

        private void OnEnable()
        {
            _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _bus.Subscribe<GameStartedSignal>(OnGameStarted);
        }

        private void OnDisable()
        {
            _bus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _bus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        }

        private void Update()
        {
            if(!_started) return;
            Debug.Log("Update");
            _timer += Time.deltaTime;
            if (_timer >= Interval)
            {
                _timer = 0f;
                Spawn();
            }
        }

        private void Spawn()
        {
            var y = Random.Range(MinY, MaxY);
            var pos = new Vector2(SpawnPoint.position.x, y);
            _pool.Spawn(pos);
        }
        
        private void OnGameStarted()
        {
            Debug.Log("Game Started");
            _started = true;
        }
        
        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
