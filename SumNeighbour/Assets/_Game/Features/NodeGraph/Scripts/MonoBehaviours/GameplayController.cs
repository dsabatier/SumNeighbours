using System.Collections;
using System.Collections.Generic;
using Noodlepop.GameEvents;
using Noodlepop.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SumNeighbours
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameEvent _loadCompleteEvent;
        [SerializeField] private NodeVariable _selectedNode;
        [SerializeField] private GameEventNode _selectedNodeEvent;

        MoveHistory _moveHistory = new MoveHistory();

        private void Start()
        {
            _loadCompleteEvent.Raise();
        }
        
        public void SelectNode(Node node)
        {
            _selectedNode.Value = node;
        }

        public void ClearSelectedNode()
        {
            _selectedNode.Value = Node.NoNode;
        }

        // Maybe create a move event, when a player makes a move add it to the queue
        // so it can be undone later
        public void AddMove(Node node, int oldValue, int newValue)
        {
            _moveHistory.AddMove(node, oldValue, newValue);
        }

        public void UndoMove()
        {
            Node newSelectedNode = _moveHistory.Undo().Node;
            if (newSelectedNode == Node.NoNode)
                return;

            _selectedNodeEvent.Raise(newSelectedNode);
        }

        public void ResetMoveHistory()
        {
            _moveHistory = new MoveHistory();
        }

        public void GoToMainMenu()
        {
            GameManager.RestartGame();
        }
    }
}

