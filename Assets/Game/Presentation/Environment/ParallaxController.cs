using Game.Core;
using UnityEngine;
using Zenject;
using System;

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
        [SerializeField] private Camera TargetCamera;

        [Header("Fit To Camera")]
        [SerializeField] private bool FitHeightToCamera = true;

        [Inject(Optional = true)] private IGameSession _session;

        private Renderer[] _layers;
        private MaterialPropertyBlock[] _mpb;
        private Vector2[] _offsets;
        private Vector2[] _tilings;

        private int _propIdST;
        private int _lastW, _lastH;
        private float _lastAspect = -1f;

        private void Awake()
        {
            _propIdST = Shader.PropertyToID(TextureProperty + "_ST");
            if (TargetCamera == null) TargetCamera = Camera.main;

            _layers = new Renderer[transform.childCount];
            for (var i = 0; i < _layers.Length; i++)
                _layers[i] = transform.GetChild(i).GetComponent<Renderer>();
            _layers = Array.FindAll(_layers, r => r != null);

            if (_layers.Length == 0)
            {
                Debug.LogWarning("[ParallaxController] No child renderers.", this);
                enabled = false;
                return;
            }

            _mpb     = new MaterialPropertyBlock[_layers.Length];
            _offsets = new Vector2[_layers.Length];
            _tilings = new Vector2[_layers.Length];

            for (int i = 0; i < _layers.Length; i++)
            {
                _mpb[i] = new MaterialPropertyBlock();

                _layers[i].GetPropertyBlock(_mpb[i]);
                var st = _mpb[i].GetVector(_propIdST);
                _tilings[i] = st != Vector4.zero ? new Vector2(st.x, st.y) : Vector2.one;
                _offsets[i] = st != Vector4.zero ? new Vector2(st.z, st.w) : Vector2.zero;

                if (_layers[i] is SpriteRenderer sr && sr.sprite != null)
                    sr.sprite.texture.wrapMode = TextureWrapMode.Repeat;
            }
        }

        private void Start()
        {
            ApplyFit();
            RecalculateTilingToFitWidth();

            _lastW = Screen.width;
            _lastH = Screen.height;
            _lastAspect = TargetCamera != null ? TargetCamera.aspect : (float)Screen.width / Screen.height;
        }

        private void Update()
        {
            if (_layers == null || _layers.Length == 0) return;
            if (_session != null)
            {
                var state = _session.State.Value;
                if (state == GameState.Paused || state == GameState.GameOver)
                    return;
            }
            var currentAspect = TargetCamera != null ? TargetCamera.aspect : (float)Screen.width / Screen.height;
            if (_lastW != Screen.width || _lastH != Screen.height || !Mathf.Approximately(_lastAspect, currentAspect))
            {
                _lastW = Screen.width; _lastH = Screen.height; _lastAspect = currentAspect;
                ApplyFit();
                RecalculateTilingToFitWidth();
            }

            var dt = UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            for (var i = 0; i < _layers.Length; i++)
            {
                var factor = GetFactor(i);

                _offsets[i].x += BaseSpeedX * factor * dt;
                _offsets[i].x -= Mathf.Floor(_offsets[i].x);

                _layers[i].GetPropertyBlock(_mpb[i]);
                _mpb[i].SetVector(_propIdST, new Vector4(_tilings[i].x, _tilings[i].y, _offsets[i].x, _offsets[i].y));
                _layers[i].SetPropertyBlock(_mpb[i]);
            }
        }

        private void ApplyFit()
        {
            if (!FitHeightToCamera) return;
            if (TargetCamera == null) TargetCamera = Camera.main;
            if (TargetCamera == null) return;

            var camH = 2f * TargetCamera.orthographicSize;
            var camW = camH * TargetCamera.aspect;

            var targetH = Mathf.Max(0.0001f, camH);
            var targetW = camW;

            for (var i = 0; i < _layers.Length; i++)
            {
                var t = _layers[i].transform;
                t.localScale = new Vector3(targetW, targetH, 1f);
            }
        }

        private void RecalculateTilingToFitWidth()
        {
            for (var i = 0; i < _layers.Length; i++)
            {
                var r = _layers[i];
                var size = r.bounds.size;
                var layerW = Mathf.Max(0.0001f, size.x);
                var layerH = Mathf.Max(0.0001f, size.y);

                Texture tex = null;
                if (r is SpriteRenderer sr && sr.sprite != null) tex = sr.sprite.texture;
                else if (r.sharedMaterial != null) tex = r.sharedMaterial.GetTexture(TextureProperty);

                if (tex == null || tex.height == 0)
                {
                    _tilings[i] = Vector2.one;
                }
                else
                {
                    var texAspect = (float)tex.width / tex.height;
                    var worldWidthPerTile = layerH * texAspect;
                    var repeatsX = Mathf.Max(1f, layerW / worldWidthPerTile);
                    _tilings[i] = new Vector2(repeatsX, 1f);
                }

                r.GetPropertyBlock(_mpb[i]);
                _mpb[i].SetVector(_propIdST, new Vector4(_tilings[i].x, _tilings[i].y, _offsets[i].x, _offsets[i].y));
                r.SetPropertyBlock(_mpb[i]);
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