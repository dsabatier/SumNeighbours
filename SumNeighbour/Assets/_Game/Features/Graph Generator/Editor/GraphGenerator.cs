using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SumNeighbours
{
    public class GraphGenerator
    {
        public string Filename;
        public readonly int Height;
        public readonly int Width;
        public readonly int HeightVariance;
        public readonly int WidthVariance;


        public GraphGenerator(string filename, int height, int width, int heightVariance, int widthVariance)
        {
            Filename = filename;
            Height = height;
            Width = width;
            HeightVariance = heightVariance;
            WidthVariance = widthVariance;
        }

        public NodeGraph GenerateGraph()
        {
            NodeGraph graph = new NodeGraph();

            int nodeIndex = 0;
            int heightWithVariance = Height + Random.Range(0, HeightVariance);
            int widthWithVariance = Width + Random.Range(0, WidthVariance);
            
            // Generate nodes
            for (int y = 0; y < heightWithVariance; y++)
            {
                for (int x = 0; x < widthWithVariance; x++)
                {
                    // Create offset in grid.
                    NodeType nodeType = ((y % 2) + x) % 2 == 0 ? NodeType.Number : NodeType.Rule;
                    Vector3 position = new Vector3(x, y, 0);

                    Node node = new Node(
                        graph: graph,
                        id: nodeIndex,
                        startingValue: 0,
                        nodeType: nodeType,
                        position: new Vector3(x, y));

                    graph.AddNode(node);
                    nodeIndex++;
                }
            }

            // Give nodes some neighbours
            foreach (Node node in graph.Nodes)
            {
                List<int> neighbourIds;
                neighbourIds = GenerateNeighbourIds(graph, node).ToList();
                node.AssignNeighbours(neighbourIds);
            }

            // make sure each node has a reference to any other node that is referencing it
            foreach (Node node in graph.Nodes)
            {
                List<int> neighbourIds = new List<int>(node.NeighbourIds);

                foreach (int neighbourId in node.NeighbourIds)
                {
                    Node neighbour = graph.GetNode(neighbourId);

                    if (!neighbourIds.Contains(node.NodeId))
                        neighbour.AddNeighbourById(node.NodeId);
                }
            }

            // Check if graph is connected
            if (CheckIfGraphIsConnected(graph) == false)
            {
                Debug.LogError("Graph generated was not connected, discarding it!");
                return null;
            }
            
            // fill in all the number nodes with 1-9
            foreach (Node node in graph.Nodes)
            {
                if (node.NodeType == NodeType.Number)
                    node.SetValue(Random.Range(1, 9));
            }

            // get sum of all the rule nodes neighbours
            foreach (Node node in graph.Nodes)
            {
                if (node.NodeType == NodeType.Rule)
                    node.SetValue(node.GetSumOfNeighbours());
            }

            // delete the values of all the number nodes
            foreach (Node node in graph.Nodes)
            {
//                if (node.NodeType == NodeType.Number)
//                    node.SetValue(0);
            }

            // clear all neighbours
            foreach (Node node in graph.Nodes)
            {
                node.AssignNeighbours(new List<int>());
            }

            return graph;
        }

        private static bool CheckIfGraphIsConnected(NodeGraph graph)
        {
            Node startingNode = graph.Nodes[0];
            List<Node> checkedNodes = GetAllNeighboursAndAddToList(graph, startingNode, new List<Node>());
            return checkedNodes.Count == graph.Nodes.Count;
        }
        private static List<Node> GetAllNeighboursAndAddToList(NodeGraph graph, Node node, List<Node> nodesChecked)
        {
            nodesChecked.Add(node);
            List<Node> neighbours = new List<Node>();
            foreach (int neighbourId in node.NeighbourIds)
            {
                neighbours.Add(graph.Nodes[neighbourId]);
            }

            foreach (Node neighbour in neighbours)
            {
                if (!nodesChecked.Contains(neighbour))
                    GetAllNeighboursAndAddToList(graph, neighbour, nodesChecked);
            }

            return nodesChecked;

        }

        private static int[] GenerateNeighbourIds(NodeGraph graph, Node node)
        {
            // All possible directions
            List<Vector3> neighbourDirections = new List<Vector3>(NodeGraph.NeighbourDirections);

            // make sure we aren't going to grab a null node
            for (int i = neighbourDirections.Count - 1; i >= 0; i--)
            {
                Vector3 direction = neighbourDirections[i];
                if (graph.GetNode(node.Position + direction) == Node.NoNode)
                {
                    neighbourDirections.Remove(direction);
                }
            }

            int[] neighbourIds = new int[Random.Range(1, neighbourDirections.Count - 1)];
            for (int i = 0; i < neighbourIds.Length; i++)
            {
                int randomIndex = Random.Range(1, neighbourDirections.Count);
                Vector3 randomDirection = neighbourDirections[randomIndex];

                Node neighbour = graph.GetNode(node.Position + randomDirection);
                neighbourDirections.Remove(randomDirection);
                neighbourIds[i] = neighbour.NodeId;
            }

            return neighbourIds;
        }
    }
}