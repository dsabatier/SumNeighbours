﻿using System.Collections;
using System.Collections.Generic;
using SumNeighbours;
using UnityEngine;
using UnityEngine.Events;

public class NumberNodeBehaviour : NodeBehaviour
{
    [Space(10)]
    [SerializeField] protected Material _selectedMaterial;
    [SerializeField] protected UnityEvent _onSelected;
    public override void ChangeNodeValue(int newValue)
    {
        if (_selectedNode.Value != _node || _selectedNode.Value.CurrentValue == newValue)
            return;

        _moveMadeEvent.Raise(_node, _node.CurrentValue, newValue);
        _node.SetValue(newValue);
        _nodeGraph.EvaluateGraph();
    }

    public override void SelectNode(Node node)
    {
        if (node == _node)
        {
            _onSelected.Invoke();
        }
        
        GetComponent<Renderer>().material = node == _node ? _selectedMaterial : _normalMaterial;
    }

    public override void ResetNodeValue()
    {
        _node.SetValue(0);
    }
}