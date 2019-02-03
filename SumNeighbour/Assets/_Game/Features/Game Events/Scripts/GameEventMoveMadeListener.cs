using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SumNeighbours
{
    public class GameEventMoveMadeListener : MonoBehaviour
    {
        [SerializeField] private GameEventMoveMade _gameEvent;
        [SerializeField] private UnityEventMoveMade _onInvoke;

        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void Raise(Node node, int oldValue, int newValue)
        {
            if (_onInvoke != null)
                _onInvoke.Invoke(node, oldValue, newValue);
        }
    }

}
