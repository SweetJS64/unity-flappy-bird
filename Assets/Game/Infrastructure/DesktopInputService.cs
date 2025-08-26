using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class DesktopInputService : IInputService
    {
        public bool IsJumpPressed()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }
    }
}