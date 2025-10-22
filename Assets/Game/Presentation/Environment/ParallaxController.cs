using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation.Environment
{
    public class ParallaxController : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float BaseSpeedX = 0.1f;
        [SerializeField] private float[] LayerFactors = { 0.1f, 0.3f, 0.5f };

        [Header("Advanced")]
        [SerializeField] private bool UseUnscaledTime = false;
        [SerializeField] private string TextureProperty = "_BaseMap";

        [Inject(Optional = true)] private IGameSession _session;

        private Renderer[] _layers;
        private MaterialPropertyBlock[] _mpb;
        private Vector2[] _offsets;
        private Vector2[] _tilings;

        private void Awake()
        {
            _layers = new Renderer[transform.childCount];
            for (var i = 0; i < _layers.Length; i++)
                _layers[i] = transform.GetChild(i).GetComponent<Renderer>();

            _layers = System.Array.FindAll(_layers, r => r != null);
            if (_layers.Length == 0)
            {
                Debug.LogWarning("[ParallaxController] _layers.Length == 0.", this);
                enabled = false;
                return;
            }

            _mpb = new MaterialPropertyBlock[_layers.Length];
            _offsets = new Vector2[_layers.Length];
            _tilings = new Vector2[_layers.Length];

            for (var i = 0; i < _layers.Length; i++)
            {
                _mpb[i] ??= new MaterialPropertyBlock();

                _layers[i].GetPropertyBlock(_mpb[i]);
                var st = _mpb[i].GetVector(TextureProperty + "_ST");
                if (st != Vector4.zero)
                {
                    _tilings[i] = new Vector2(st.x, st.y);
                    _offsets[i] = new Vector2(st.z, st.w);
                }
                else
                {
                    _tilings[i] = Vector2.one;
                    _offsets[i] = Vector2.zero;
                }

                if (_layers[i] is SpriteRenderer sr && sr.sprite != null)
                    sr.sprite.texture.wrapMode = TextureWrapMode.Repeat;
            }
        }

        private void Update()
        {
            if (_layers == null || _layers.Length == 0) return;

            if (_session != null && _session.State.Value == GameState.GameOver)
                return;

            var dt = UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            for (var i = 0; i < _layers.Length; i++)
            {
                var factor = GetFactor(i);
                _offsets[i].x += BaseSpeedX * factor * dt;

                _offsets[i].x -= Mathf.Floor(_offsets[i].x);

                _layers[i].GetPropertyBlock(_mpb[i]);
                _mpb[i].SetVector(TextureProperty + "_ST",
                    new Vector4(_tilings[i].x, _tilings[i].y, _offsets[i].x, _offsets[i].y));
                _layers[i].SetPropertyBlock(_mpb[i]);
            }
        }

        private float GetFactor(int index)
        {
            if (LayerFactors == null || LayerFactors.Length == 0) return 1f;
            if (index < LayerFactors.Length) return LayerFactors[index];
            return LayerFactors[LayerFactors.Length - 1];
        }
    }
}