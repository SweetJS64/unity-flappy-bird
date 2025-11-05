using UnityEngine;

namespace Game.Presentation.Environment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundFitToCamera : MonoBehaviour
    {
        [SerializeField] private Camera TargetCamera;

        private void Start()
        {
            FitToCamera();
        }

        private void FitToCamera()
        {
            if (TargetCamera == null) TargetCamera = Camera.main;
            if (TargetCamera == null) return;

            var sr = GetComponent<SpriteRenderer>();
            if (sr.sprite == null) return;

            var camHeight = 2f * TargetCamera.orthographicSize;
            var camWidth = camHeight * TargetCamera.aspect;

            Vector2 spriteSize = sr.sprite.bounds.size;

            Vector3 scale = transform.localScale;
            scale.x = camWidth / spriteSize.x;
            scale.y = camHeight / spriteSize.y;
            transform.localScale = scale;
        }
    }
}