using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noodlepop.GameEvents
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Game Events/New Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _registeredListeners = new List<GameEventListener>();

        public void Raise()
        {
            foreach (GameEventListener listener in _registeredListeners)
            {
                listener.Raise();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);
        }
    }
}