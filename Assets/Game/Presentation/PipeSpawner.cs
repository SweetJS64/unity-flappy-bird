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

        [Inject] private PipeObstacle.Pool _pool;

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
            var y = Random.Range(MinY, MaxY);
            var pos = new Vector2(SpawnPoint.position.x, y);
            _pool.Spawn(pos);
        }
    }
}
