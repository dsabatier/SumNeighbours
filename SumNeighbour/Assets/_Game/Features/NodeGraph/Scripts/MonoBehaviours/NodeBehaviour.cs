using System;
using UnityEngine;
using UnityEngine.UI;
using Noodlepop.VariableAssets;
using UnityEngine.Analytics;
using UnityEngine.Events;

namespace SumNeighbours
{
    /// <summary>
    /// NodeBehaviour is used to connect the player to node data and
    /// to control the UI components representing nodes.
    /// </summary>
    [SelectionBase]
    public abstract class NodeBehaviour : MonoBehaviour
    {
        protected Node _node = null;
        protected NodeGraph _nodeGraph = null;
        public int NodeId => _node.NodeId;
        
        [SerializeField] protected NodeVariable _selectedNode;
        [SerializeField] protected UnityEventNode _onTap;
        [SerializeField] protected Text _text;
        
        [SerializeField] protected Material _normalMaterial;
        
        [Space(4)]
        [SerializeField] protected GameEventMoveMade _moveMadeEvent;

        protected bool _dragging = false;

        private void Reset()
        {
            _text = GetComponentInChildren<Text>();
        }

        private void OnDisable()
        {
            _node.OnChanged -= UpdateText;
        }

        public virtual void Init(NodeGraph graph, Node node)
        {
            _nodeGraph = graph;
            _node = node;
            _node.OnChanged += UpdateText;
            _text.text = _node.CurrentValue == 0 ? string.Empty : _node.CurrentValue.ToString();
        }

        public void OnTap()
        {
            if(!_dragging)
                _onTap.Invoke(_node);
        }

        public void ToggleDrag(bool dragging)
        {
            _dragging = dragging;
        }
        

        /// <summary>
        /// Change the current value of a node
        /// </summary>
        /// <param name="newValue"></param>
        public abstract void ChangeNodeValue(int newValue);

        private void UpdateText()
        {
            _text.text = _node.CurrentValue == 0 ? string.Empty : _node.CurrentValue.ToString();
        }

        public abstract void ResetNodeValue();

        public abstract void SelectNode(Node node);

        public void ResetNodeColor()
        {
            GetComponent<MeshRenderer>().material = _normalMaterial;
        }

    }
}
