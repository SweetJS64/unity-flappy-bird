using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BirdController : MonoBehaviour
    {
        [Inject] private IInputService _input;

        [Header("Flight")]
        [SerializeField] private float JumpSpeed = 5.5f;
        [SerializeField] private float DefaultGravity = 2.5f;
        [SerializeField] private float MaxFallSpeed = -7f;

        [Header("Tilt")]
        [SerializeField] private float TiltUp = 35f;
        [SerializeField] private float TiltDown = -40f;
        [SerializeField] private float TiltLerp = 8f;

        private Rigidbody2D _rb;
        private bool _started;
        private bool _alive = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
        }

        private void Update()
        {
            if (!_alive) return;

            if (_input.IsJumpPressed())
            {
                if (!_started)
                {
                    _started = true;
                    _rb.gravityScale = DefaultGravity;
                }
                Flap();
            }
            Tilt();
        }

        private void FixedUpdate()
        {
            if (!_alive) return;

            ClampFallSpeed();
        }

        private void Flap()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, JumpSpeed);
        }

        private void Tilt()
        {
            var t = Mathf.InverseLerp(MaxFallSpeed, JumpSpeed, _rb.linearVelocity.y);
            var targetAngle = Mathf.Lerp(TiltDown, TiltUp, t);
            var rot = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, TiltLerp * Time.deltaTime);
        }

        private void ClampFallSpeed()
        {
            if (_rb.linearVelocity.y < MaxFallSpeed)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, MaxFallSpeed);
        }
        
        public void Die()
        {
            _alive = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = DefaultGravity;
        }
    }
}