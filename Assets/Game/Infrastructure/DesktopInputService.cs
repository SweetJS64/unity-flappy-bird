using Game.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Infrastructure
{
    public class DesktopInputService : IInputService
    {
        private readonly IGameSession _session;

        public DesktopInputService(IGameSession session) => _session = session;

        public bool IsJumpPressed()
        {
            if (_session.State.Value != GameState.Playing)
                return false;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return false;
            
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }
    }
}