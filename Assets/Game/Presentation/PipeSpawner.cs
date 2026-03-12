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
        [SerializeField] private float MinStepY = 1.6f;

        [Header("Timing")]
        [SerializeField] private float Interval = 1.4f;

        private float _timer;
        private float _lastY;
        private bool  _hasLastY;
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
            _pool.Spawn(new Vector2(x, NextY()));
        }

        private float NextY()
        {
            if (!_hasLastY)
            {
                _lastY    = Random.Range(MinY, MaxY);
                _hasLastY = true;
                return _lastY;
            }

            var leftWidth  = Mathf.Max(0f, _lastY - MinStepY - MinY);
            var rightWidth = Mathf.Max(0f, MaxY - (_lastY + MinStepY));
            var total      = leftWidth + rightWidth;

            if (total <= 0f)
            {
                _lastY = Random.Range(MinY, MaxY);
                return _lastY;
            }

            var r = Random.Range(0f, total);
            _lastY = r < leftWidth
                ? MinY + r
                : _lastY + MinStepY + (r - leftWidth);

            return _lastY;
        }
        
        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
