using Game.Core;
using Game.Skins;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class SkinController : MonoBehaviour
    {
        [Inject] private ISkinService _skins;
        [Inject] private SkinCatalog _catalog;

        private SpriteRenderer _sr;
        private Animator _anim;
        private CapsuleCollider2D _collider;
        private readonly CompositeDisposable _cd = new();
        
        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void OnEnable()
        {
            Apply(_skins.SelectedId.Value);

            _skins.SelectedId
                .Skip(1)
                .Subscribe(Apply)
                .AddTo(_cd);
        }

        private void OnDisable()
        {
            _cd.Clear();
        }

        private void Apply(string id)
        {
            var def = _catalog.GetById(id);
            if (def == null)
            {
                Debug.LogWarning($"[SkinController] SkinDef '{id}' not found");
                return;
            }

            if (def.Icon != null) _sr.sprite = def.Icon;
            
            if (_collider != null)
            {
                _collider.offset = def.ColliderOffset;
                _collider.size   = def.ColliderSize;
            }
            
            if (def.AnimatorController != null && _anim.runtimeAnimatorController != def.AnimatorController)
            {
                _anim.runtimeAnimatorController = def.AnimatorController;
                _anim.Rebind();
                _anim.Update(0f);
            }
        }
    }
}