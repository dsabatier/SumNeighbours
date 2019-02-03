using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SumNeighbours
{
    public class Connection
    {
        internal readonly int NodeA;
        internal readonly int NodeB;

        internal Connection(int nodeA, int nodeB)
        {
            NodeA = nodeA;
            NodeB = nodeB;
        }
    }
}
