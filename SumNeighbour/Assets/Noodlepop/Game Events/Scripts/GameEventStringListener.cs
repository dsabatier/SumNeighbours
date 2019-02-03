using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Noodlepop.GameEvents
{
    public class GameEventStringListener : MonoBehaviour
    {
        [SerializeField] private GameEventString _gameEvent;
        [SerializeField] private UnityEventString _onInvoke;

        private void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void Raise(string i)
        {
            if (_onInvoke != null)
                _onInvoke.Invoke(i);
        }
    }
}