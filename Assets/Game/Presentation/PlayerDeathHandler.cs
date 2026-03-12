using UnityEngine;
using Zenject;
using Game.Common;
using Game.Core.Signals;

namespace Game.Presentation
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;

        private bool _isDead;
        private Collider2D _col;
        private int _obstacleLayer;
        private int _deathZoneLayer;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _obstacleLayer  = LayerMask.NameToLayer(Layers.Obstacle);
            _deathZoneLayer = LayerMask.NameToLayer(Layers.DeathZone);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isDead) return;

            if (other.collider.gameObject.layer == _obstacleLayer)
                Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDead) return;

            if (other.gameObject.layer == _deathZoneLayer)
                Die();
        }
        private void Die()
        {
            _isDead = true;
            _col.enabled = false;
            _signalBus.Fire<PlayerDiedSignal>();
        }
    }
}