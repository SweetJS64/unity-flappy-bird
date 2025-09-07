using System;
using UnityEngine;

namespace Game.Presentation
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        public event Action Died;

        [SerializeField] private LayerMask ObstacleMask;
        [SerializeField] private LayerMask DeathZoneMask;

        private bool _isDead;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isDead) return;

            if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDead) return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("DeathZone"))
                Die();
        }
        private void Die()
        {
            _isDead = true;
            var ctrl = GetComponent<PlayerController>();
            ctrl.enabled = false;
        }
    }
}