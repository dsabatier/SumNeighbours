using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SumNeighbours
{
    [CreateAssetMenu(fileName = "New Level Asset")]
    public class LevelAsset : ScriptableObject
    {
        public float AverageNumberOfNeighbours;
        public int NumberOfStartingNodes;

        public NodeGraph NodeGraph;
    }
}