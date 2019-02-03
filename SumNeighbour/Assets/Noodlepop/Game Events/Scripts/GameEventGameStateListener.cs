using UnityEngine;
using Noodlepop;

namespace Noodlepop.GameEvents
{
    public class GameEventGameStateListener : MonoBehaviour
    {
        [SerializeField] private GameEventGameState _gameEvent;
        [SerializeField] private UnityEventGameState _onInvoke;

        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void Raise(GameState oldState, GameState newState)
        {
            if (_onInvoke != null)
                _onInvoke.Invoke(oldState, newState);
        }

    }
}