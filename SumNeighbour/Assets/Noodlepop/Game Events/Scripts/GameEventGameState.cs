using System.Collections.Generic;
using UnityEngine;

namespace Noodlepop.GameEvents
{
    [CreateAssetMenu(fileName = "New Game State Change Event", menuName = "Game Events/New Game State Change Event")]
    public class GameEventGameState : ScriptableObject
    {
        private List<GameEventGameStateListener> _registeredListeners = new List<GameEventGameStateListener>();

        public void Raise(GameState oldState, GameState newState)
        {
            foreach (GameEventGameStateListener listener in _registeredListeners)
            {
                listener.Raise(oldState, newState);
            }
        }

        public void RegisterListener(GameEventGameStateListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventGameStateListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);
        }

    }
}