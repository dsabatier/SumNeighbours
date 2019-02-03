using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noodlepop.GameEvents
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = "Game Events/New Int Event")]
    public class GameEventInt : ScriptableObject
    {
        private List<GameEventIntListener> _registeredListeners = new List<GameEventIntListener>();

        public void Raise(int i)
        {
            foreach (GameEventIntListener listener in _registeredListeners)
            {
                listener.Raise(i);
            }
        }

        public void RegisterListener(GameEventIntListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventIntListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);

        }
    }
}