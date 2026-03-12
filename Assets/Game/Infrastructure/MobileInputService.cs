using Game.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Infrastructure
{
    public class MobileInputService : IInputService
    {
        private readonly IGameSession _session;

        public MobileInputService(IGameSession session) => _session = session;

        public bool IsJumpPressed()
        {
            if (_session.State.Value != GameState.Playing)
                return false;

            if (Input.touchCount == 0)
                return false;

            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Began)
                return false;

            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return false;

            return true;
        }
    }
}