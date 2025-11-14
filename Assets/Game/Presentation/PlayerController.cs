using Game.Core;
using Game.Core.Signals;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Inject] private IInputService _input;
        [Inject] private SignalBus _bus;

        [Header("Flight")]
        [SerializeField] private float JumpSpeed = 5.5f;
        [SerializeField] private float DefaultGravity = 2.5f;
        [SerializeField] private float MaxFallSpeed = -7f;

        [Header("Tilt")]
        [SerializeField] private float TiltUp = 35f;
        [SerializeField] private float TiltDown = -40f;
        [SerializeField] private float TiltLerp = 8f;

        private Rigidbody2D _rb;
        private Animator _animator;
        private bool _started;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_input.IsJumpPressed())
            {
                if (!_started)
                {
                    _started = true;
                    _rb.gravityScale = DefaultGravity;
                    _animator.SetBool("GameStarted", true);
                }
                Flap();
            }
            Tilt();
        }

        private void FixedUpdate()
        {
            ClampFallSpeed();
        }
        
        private void OnEnable() => _bus.Subscribe<PlayerDiedSignal>(OnPlayerDied);

        private void OnDisable() => _bus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);

        private void Flap()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, JumpSpeed);
            if (_animator != null)
                _animator.SetTrigger("Flap");
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
        
        private void OnPlayerDied()
        {
            if (_rb.gravityScale == 0) _rb.gravityScale = DefaultGravity;
            _started = true;
            _animator.enabled = false;
        }
    }
}