using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PipeObstacle : MonoBehaviour
    {
        private IMemoryPool _pool;
        
        public void Init(Vector2 startPos, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = startPos;
            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        public class Pool : MonoMemoryPool<Vector2, PipeObstacle>
        {
            protected override void Reinitialize(Vector2 pos, PipeObstacle item)
            {
                item.Init(pos, this);
            }

            protected override void OnDespawned(PipeObstacle item)
            {
                item.gameObject.SetActive(false);
                base.OnDespawned(item);
            }
        }
    }
}