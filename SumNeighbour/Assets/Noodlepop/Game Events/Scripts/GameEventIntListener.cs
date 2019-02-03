using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Noodlepop.GameEvents
{
    public class GameEventIntListener : MonoBehaviour
    {
        [SerializeField] private GameEventInt _gameEvent;
        [SerializeField] private UnityEventInt _onInvoke;

        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void Raise(int i)
        {
            if (_onInvoke != null)
                _onInvoke.Invoke(i);
        }
    }
}