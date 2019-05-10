using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    private GridSystem _GridSystem;

    void Start()
    {
        _GridSystem = GetComponent<GridSystem>();
    }
    public List<Node> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Node startNode = _GridSystem.GetNodeFromWorldPosition(startPosition);
        Node endNode = _GridSystem.GetNodeFromWorldPosition(endPosition);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i]._fCost < currentNode._fCost || openSet[i]._fCost == currentNode._fCost && openSet[i]._hCost < currentNode._hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return retracePath(startNode, endNode); ;
            }
            List<Node> neighbours = _GridSystem.GetNodeNeibers(currentNode, 3, 3);
            foreach (Node neighbour in neighbours)
            {
                if(!neighbour._Walkable || closedSet.Contains(neighbour) || neighbour == currentNode)
                {
                    continue;
                }
                int newMoveCostToNeibers = currentNode._gCost + GetDistanceBetweenNodes(currentNode, neighbour);
                if (newMoveCostToNeibers < neighbour._gCost || !openSet.Contains(neighbour))
                {
                    neighbour._gCost = newMoveCostToNeibers;
                    neighbour._gCost = GetDistanceBetweenNodes(neighbour , endNode);
                    neighbour._parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

        }
    }
    private List<Node> retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode._parent;
        }
        path.Reverse();
        return path;
    }
    private int GetDistanceBetweenNodes(Node Node1,Node Node2)
    {
        int DistanceX = Mathf.Abs(Node1._nodePositionInGridX - Node2._nodePositionInGridX);
        int DistanceY = Mathf.Abs(Node1._nodePositionInGridY - Node2._nodePositionInGridY);

        if(DistanceX > DistanceY)
        {
            return 14 * DistanceY + 10 * (DistanceX - DistanceY);
        }
        return 14 * DistanceX + 10 * (DistanceY - DistanceX);
    }
}
