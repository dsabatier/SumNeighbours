using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noodlepop.Systems;

namespace SumNeighbours
{
    [CreateAssetMenu(fileName = "New Move Made Event", menuName = "Game Events/New Move Made Event")]
    public class GameEventMoveMade : ScriptableObject
    {
        private List<GameEventMoveMadeListener> _registeredListeners = new List<GameEventMoveMadeListener>();

        public void Raise(Node node, int oldValue, int newValue)
        {
            foreach (GameEventMoveMadeListener listener in _registeredListeners)
            {
                listener.Raise(node, oldValue, newValue);
            }
        }

        public void RegisterListener(GameEventMoveMadeListener listener)
        {
            if (!_registeredListeners.Contains(listener))
                _registeredListeners.Add(listener);
        }

        public void UnregisterListener(GameEventMoveMadeListener listener)
        {
            if (_registeredListeners.Contains(listener))
                _registeredListeners.Remove(listener);

        }
    }
}