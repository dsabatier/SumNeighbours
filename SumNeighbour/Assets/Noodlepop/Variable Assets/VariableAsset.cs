using UnityEngine;

namespace Noodlepop.VariableAssets
{
    /// <summary>
    /// A variable that lives in your assets folder.  It can be marked as readonly or modified
    /// at runtime.  Runtime values are not saved. Default values are. (I think)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VariableAsset<T> : ScriptableObject
    {
        [SerializeField] protected bool _readOnly = false;
        [SerializeField] protected T _defaultValue;
        [SerializeField] protected T _value;
#if UNITY_EDITOR
        [SerializeField, TextArea] public string Notes;
#endif

        public T DefaultValue
        {
            get { return _defaultValue; }
        }

        public T Value
        {
            get { return _value; }
            set
            {
                if (!_readOnly)
                {
                    _value = value;
                }
                else
                {
                    LogReadOnlyWarning();
                }
            }
        }

        public void OnEnable()
        {
            _value = _defaultValue;
        }

        public void Init(T value)
        {
            _defaultValue = value;
            _value = value;
        }

        private void LogReadOnlyWarning()
        {
            Debug.LogWarningFormat("Tried changing read-only property {0}<{1}>", this.name, this.GetType().ToString());
        }
    }
}