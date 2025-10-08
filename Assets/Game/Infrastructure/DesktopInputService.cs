using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class DesktopInputService : IInputService
    {
        [Inject] private IGameSession _session;
        
        public bool IsJumpPressed()
        {
            if (_session.State.Value != GameState.Playing && _session.State.Value != GameState.Idle)
                return false;
            
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }
    }
}