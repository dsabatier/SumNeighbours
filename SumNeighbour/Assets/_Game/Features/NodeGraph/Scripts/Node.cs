using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SumNeighbours
{
    public enum NodeType
    {
        Rule = 0,
        Number = 1
    }

    [Serializable]
    public class Node
    {

        public class NullNode : Node
        {
            public override bool Evaluate()
            {
                return false;
            }

            public override void SetValue(int i)
            {

            }

            public override string ToString()
            {
                return "No Node";
            }
        }

        public static readonly Node NoNode = new NullNode();
        public event Action<bool> OnEvaluate = (b) => { };
        public event Action OnChanged = () => { };

        #region Variables
        [SerializeField] int _nodeId;
        public int NodeId => _nodeId;

        NodeGraph _nodeGraph;

        [SerializeField] int[] _neighbourIds = new int[0];
        public IReadOnlyCollection<int> NeighbourIds => Array.AsReadOnly(_neighbourIds);

        [SerializeField] int _currentValue;
        public int CurrentValue => _currentValue;

        [SerializeField] NodeType _nodeType;
        public NodeType NodeType => _nodeType;

        [SerializeField]
        Vector3 _position;
        public Vector3 Position => _position;
        #endregion

        public Node() { _nodeId = -1; _position = new Vector3(-100, -100, -100); }

        public Node(NodeGraph graph, int id, int startingValue, NodeType nodeType, Vector3 position, params int[] neighborIds)
        {
            _nodeGraph = graph;
            _nodeId = id;
            _currentValue = startingValue;
            _nodeType = nodeType;
            _position = position;
            _neighbourIds = neighborIds;
        }

        public virtual void SetValue(int i)
        {
            _currentValue = i;
            //            Evaluate();
            OnChanged();
        }

        public virtual bool Evaluate()
        {
            bool hasZeroNeighbour = _neighbourIds.Where(id => _nodeGraph.GetNode(id)._currentValue == 0).ToList().Count > 0;
            bool result = !hasZeroNeighbour && _neighbourIds.Sum(id => _nodeGraph.GetNode(id)._currentValue) == _currentValue;
            OnEvaluate(result);
            return result;
        }

        public int GetSumOfNeighbours()
        {
            return _neighbourIds.Sum(id => _nodeGraph.GetNode(id)._currentValue);
        }

        public void AddNeighbourById(int id)
        {
            if (_neighbourIds.Contains(id))
                return;

            List<int> neighbourIds = new List<int>(_neighbourIds)
            {
                id
            };

            _neighbourIds = neighbourIds.ToArray();
        }

        public void AssignNeighbours(List<int> neighbourIds)
        {
            neighbourIds.RemoveAll(id => id < 0);
            _neighbourIds = neighbourIds.ToArray();
        }

        public override string ToString()
        {
            return "Node: " + _nodeId;
        }

    }
}