using System;
using UnityEngine;
using Zenject;
using Game.Common;
using Game.Core.Signals;

namespace Game.Presentation
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] private LayerMask ObstacleMask;
        [SerializeField] private LayerMask DeathZoneMask;

        private bool _isDead;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isDead) return;

            if (other.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Obstacle))
                Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDead) return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer(Layers.DeathZone))
                Die();
        }
        private void Die()
        {
            _isDead = true;
            _signalBus.Fire<PlayerDiedSignal>();
        }
    }
}