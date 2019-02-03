using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SumNeighbours
{
    [CreateAssetMenu(fileName = "New Node Event", menuName = "Game Events/New Node Event")]
    public class GameEventNode : ScriptableObject
    {
        private List<GameEventNodeListener> _registeredListeners = new List<GameEventNodeListener>();

        public void Raise(Node node)
        {
            foreach (GameEventNodeListener listener in _registeredListeners)
            {
                listener.Raise(node);
            }
        }

        public void RegisterListener(GameEventNodeListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventNodeListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);

        }
    }
}