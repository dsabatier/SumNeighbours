using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noodlepop.SingletonPattern
{
    /// <summary>
    /// Base class for all singletons.  For safety wrap
    /// access to the Instance in a static method.
    /// </summary>
    /// <typeparam name="T">Type of the inheriting method.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private bool _quitting = false;

        private static T _instance;
        protected static T Instance
        {
            get
            {
                Init();
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this as T;
        }

        private static void Init()
        {
            if (_instance == null || _instance.Equals(null))
            {
                _instance = new GameObject().AddComponent<T>();
            }
        }

        private void OnApplicationQuit()
        {
            _quitting = true;
        }

        protected virtual void OnDestroy()
        {
//            if (!_quitting)
//            {
//                _instance = null;
//                Init();
//            }
        }
    }
}