using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class MobileInputService : IInputService
    {
        public bool IsJumpPressed()
        {
            if (Input.touchCount <= 0) return false;
            var t = Input.GetTouch(0);
            return t.phase == TouchPhase.Began;
        }
    }
}