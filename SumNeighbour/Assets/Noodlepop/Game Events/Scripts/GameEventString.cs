using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noodlepop.GameEvents
{
    [CreateAssetMenu(fileName = "New String Event", menuName = "Game Events/New String Event")]
    public class GameEventString : ScriptableObject
    {
        private List<GameEventStringListener> _registeredListeners = new List<GameEventStringListener>();

        public void Raise(string s)
        {
            foreach (GameEventStringListener listener in _registeredListeners)
            {
                listener.Raise(s);
            }
        }

        public void RegisterListener(GameEventStringListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventStringListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);

        }
    }
}