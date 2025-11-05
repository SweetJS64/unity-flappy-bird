using UnityEngine;

namespace Game.Common
{
    public static class CameraBounds
    {
        public static float RightX(Camera cam, float padding = 0f)
            => cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0f)).x + padding;

        public static float LeftX(Camera cam, float padding = 0f)
            => cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - padding;
    }
}