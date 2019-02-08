using System.Collections;
using System.Collections.Generic;
using Noodlepop.GameEvents;
using SumNeighbours;
using UnityEngine;
using UnityEngine.Events;

public class ConnectionBehaviour : MonoBehaviour
{
    [SerializeField] private Material _activeMaterial;
    [SerializeField] private Material _inactiveMaterial;
    [SerializeField] private UnityEvent _onConnectionToggled;
    
    private Node _nodeA;
    private Node _nodeB;
    private bool _isActive = false;

    public void SetNodes(Node nodeA, Node nodeB)
    {
        _nodeA = nodeA;
        _nodeB = nodeB;
    }

    public void Toggle()
    {
        if (_isActive)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }

        GetComponent<LineRenderer>().material = _isActive ? _activeMaterial : _inactiveMaterial;
        _onConnectionToggled.Invoke();
    }
    
    private void Activate()
    {
         _nodeA.AddNeighbourById(_nodeB.NodeId);
         _nodeB.AddNeighbourById(_nodeA.NodeId);
         _isActive = true;
    }

    private void Deactivate()
    {
        _nodeA.RemoveNeighbourById(_nodeB.NodeId);
        _nodeB.RemoveNeighbourById(_nodeA.NodeId);
        _isActive = false;
    }
    
}
