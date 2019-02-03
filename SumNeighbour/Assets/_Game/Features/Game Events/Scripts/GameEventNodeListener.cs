using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SumNeighbours
{
    public class GameEventNodeListener : MonoBehaviour
    {
        [SerializeField] private GameEventNode _gameEvent;
        [SerializeField] private UnityEventNode _onInvoke;

        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void Raise(Node node)
        {
            if (_onInvoke != null)
                _onInvoke.Invoke(node);
        }
    }

}
