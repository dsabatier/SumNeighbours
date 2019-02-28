using System.Collections;
using System.Collections.Generic;
using Noodlepop.GameEvents;
using UnityEngine;

namespace SumNeighbours
{
    public class RuleNodeBehaviour : NodeBehaviour
    {
        [Space(4)] 

        [SerializeField] protected Material _correctMaterial;

        public override void Init(NodeGraph graph, Node node)
        {
            base.Init(graph, node);
            _node.OnEvaluate += UpdateColor;
        }
        
        private void UpdateColor(bool correct)
        {
           // _text.text = (_node.CurrentValue - _node.GetSumOfNeighbours()).ToString();
            GetComponent<MeshRenderer>().material = correct ? _correctMaterial : _normalMaterial;
        }
        
        public override void ChangeNodeValue(int newValue)
        {
            // do nothing
        }
        public override void ResetNodeValue()
        {
            
        }

        public override void SelectNode(Node node)
        {
            
        }
    }
}