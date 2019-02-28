using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Noodlepop.GameEvents;
using Noodlepop.Systems;

namespace SumNeighbours
{
    public class NodeGraphBehaviour : MonoBehaviour
    {
        private NodeGraph _nodeGraph;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _numberNodePrefab;
        [SerializeField] private GameObject _ruleNodePrefab;
        [SerializeField] private GameObject _connectionPrefab;
        [SerializeField] private float _graphScale = 1;
        [SerializeField] private GameEvent _onPuzzleComplete;

        private List<NodeBehaviour> _nodeBehaviours = new List<NodeBehaviour>();
        private List<LineRenderer> _lines = new List<LineRenderer>();

        private void Start()
        {
            LoadLevel(GameManager.CurrentLevel);

            CenterCamera();
        }

        private void CenterCamera()
        {
            Node bottomLeft = _nodeGraph.GetNode(0);
            Node topRight = _nodeGraph.GetNode(_nodeGraph.Nodes.Count - 1);


            float x = (topRight.Position.x * _graphScale - bottomLeft.Position.x * _graphScale) / 2;
            float y = (topRight.Position.y * _graphScale - bottomLeft.Position.y * _graphScale) / 2;

            float nodeWidthCount = topRight.Position.x;
            float nodeHeightCount = topRight.Position.y;


            _camera.orthographicSize = Mathf.Min(nodeHeightCount, nodeWidthCount) * _graphScale;
            
            _camera.transform.position = new Vector3(x, y, _camera.transform.position.z);
            _camera.GetComponent<TouchDragCamera>().SetBounds(bottomLeft.Position * _graphScale, topRight.Position * _graphScale);

        }

        private void LoadLevel(LevelAsset levelAsset)
        {
            if (levelAsset == null) throw new System.NullReferenceException("cannot load a null level");

            _nodeGraph = new NodeGraph(levelAsset);
            _nodeGraph.LoadGraphFromData();

            DrawNodes();
            DrawConnections();

            _nodeGraph.EvaluateGraph();
            _nodeGraph.OnLevelComplete += _onPuzzleComplete.Raise;
        }

        public void EvaluateGraph()
        {
            _nodeGraph.EvaluateGraph();
        }

        public void ResetGraph()
        {
            for (int i = 0; i < _nodeBehaviours.Count; i++)
            {
                _nodeBehaviours[i].ResetNodeValue();
            }

            _nodeGraph.EvaluateGraph();
        }

        private NodeBehaviour CreateNodeGameObject(Transform graphParent, Node node)
        {
            NodeBehaviour nodeBehaviour = Instantiate(node.NodeType == NodeType.Rule ? _ruleNodePrefab : _numberNodePrefab).GetComponent<NodeBehaviour>();
            nodeBehaviour.transform.SetParent(graphParent);
            nodeBehaviour.transform.localPosition = node.Position * _graphScale;
            nodeBehaviour.transform.name = "Node ID: " + node.NodeId.ToString();
            nodeBehaviour.Init(_nodeGraph, node);
            return nodeBehaviour;
        }

        private void DrawNodes()
        {
            Transform graphParent = transform;

            foreach (Node node in _nodeGraph.Nodes)
            {
                NodeBehaviour nodeBehaviour = CreateNodeGameObject(graphParent, node);

                _nodeBehaviours.Add(nodeBehaviour);
            }
        }

        private void DrawConnections()
        {
            Transform graphParent = transform;

            foreach (Connection connection in _nodeGraph.Connections)
            {
                LineRenderer line = Instantiate(_connectionPrefab).GetComponent<LineRenderer>();
                line.transform.SetParent(graphParent, false);
                line.useWorldSpace = false;
                line.SetPosition(0, _nodeGraph.GetNode(connection.NodeA).Position * _graphScale);
                line.SetPosition(1, _nodeGraph.GetNode(connection.NodeB).Position * _graphScale);

                _lines.Add(line);
            }
        }

        public void Clear()
        {
            for (int i = _nodeBehaviours.Count - 1; i >= 0; i--)
            {
                Destroy(_nodeBehaviours[i].gameObject);
                _nodeBehaviours.RemoveAt(i);
            }

            for (int i = _lines.Count - 1; i >= 0; i--)
            {
                Destroy(_lines[i].gameObject);
            }

            _nodeBehaviours = new List<NodeBehaviour>();
            _lines = new List<LineRenderer>();
        }
    }

}
