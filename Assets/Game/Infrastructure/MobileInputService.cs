using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class MobileInputService : IInputService
    {
        [Inject] private IGameSession _session;
        public bool IsJumpPressed()
        {
            if (_session.State.Value != GameState.Playing && _session.State.Value != GameState.Idle)
                return false;
            if (Input.touchCount <= 0) return false;
            var t = Input.GetTouch(0);
            return t.phase == TouchPhase.Began;
        }
    }
}