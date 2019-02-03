using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SumNeighbours
{
    [System.Serializable]
    public class NodeGraph
    {
        #region Variables
        private const int STARTING_VALUE = 0;
        private const int MAX_RULE_NODE_COUNT = 100;

        public Action OnLevelComplete = () => { };

        [SerializeField] List<Node> _nodes = new List<Node>();
        public IReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();

        List<Connection> _connections = new List<Connection>();
        public IReadOnlyCollection<Connection> Connections => _connections.AsReadOnly();
        private LevelAsset _levelAsset;

        GameObject _nodePrefab;
        #endregion

        public NodeGraph()
        {

        }

        public NodeGraph(LevelAsset levelAsset)
        {
            _levelAsset = levelAsset;
        }

        public Node GetNode(int index)
        {
            return _nodes[index];
        }

        public Node GetNode(Vector3 position)
        {
            return _nodes.Where(n => n.Position.x == position.x && n.Position.y == position.y).FirstOr(Node.NoNode);
        }

        public void LoadGraphFromData()
        {
            Node[] nodes = _levelAsset.NodeGraph.Nodes.ToArray();
            for (int i = 0; i < _levelAsset.NodeGraph.Nodes.Count; i++)
            {
                Node node = nodes[i];
                AddNode(i, node.CurrentValue, node.NodeType, node.Position, node.NeighbourIds.ToArray());
            }

            GenerateAllConnections();
        }

        public void ResetGraph()
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].NodeType == NodeType.Number)
                    _nodes[i].SetValue(0);
            }
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        private void AddNode(int id, int startingValue, NodeType nodeType, Vector3 position, params int[] neighbourIds)
        {
            Node node = new Node(this, id, startingValue, nodeType, position, neighbourIds);
            AddNode(node);
        }

        private bool ConnectionExists(int id1, int id2)
        {
            return _connections
                    .Where(c => (c.NodeA == id1 && c.NodeB == id2) || (c.NodeB == id1 && c.NodeA == id2))
                    .ToList()
                    .Count > 0;
        }

        public void GenerateAllConnections()
        {
            foreach (Node node in _nodes)
            {
                foreach (int neighbourId in node.NeighbourIds)
                {
                    if (!ConnectionExists(node.NodeId, neighbourId))
                        _connections.Add(new Connection(node.NodeId, neighbourId));
                }
            }
        }

        public void EvaluateGraph()
        {
            bool complete = true;
            foreach (Node node in _nodes)
            {
                if (node.NodeType == NodeType.Number || node.Evaluate())
                {
                    // do nothing, we are just checking to see if any are false.
                    // We don't want to break out of the loop because we want to
                    // call Evaluate() on all nodes
                }
                else
                {
                    complete = false;
                }
            }

            if (complete)
                OnLevelComplete();
        }

        public float GetAverageNumberOfNeighbours()
        {
            return _nodes.Sum(n => (float)n.NeighbourIds.Count) / (float)_nodes.Count;
        }

        public int GetNumberOfStartingNodes()
        {
            return _nodes.Where(n => n.NeighbourIds.Count == 1 && n.NodeType == NodeType.Rule).ToList().Count;
        }

    }
}
