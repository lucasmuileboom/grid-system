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
    public void FindPath(Vector3 startPosition, Vector3 endPosition)
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
                retracePath(startNode, endNode);
                return;
            }

            Vector2 currentNodePositionInGrid = new Vector2(_GridSystem.GetXCount(currentNode.transform.position.x), _GridSystem.GetYCount(currentNode.transform.position.y));
            List<Node> neighbours = _GridSystem.GetNodeNeibers(currentNodePositionInGrid, 3, 3);
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
    private void retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode._parent;
        }
        path.Reverse();//moet nog iets met het pad doen
    }
    private int GetDistanceBetweenNodes(Node Node1,Node Node2)
    {
        int DistanceX = Mathf.Abs(_GridSystem.GetXCount(Node1.transform.position.x) - _GridSystem.GetXCount(Node2.transform.position.x));
        int DistanceY = Mathf.Abs(_GridSystem.GetYCount(Node1.transform.position.y) - _GridSystem.GetYCount(Node2.transform.position.y));

        if(DistanceX > DistanceY)
        {
            return 14 * DistanceY + 10 * (DistanceX - DistanceY);
        }
        return 14 * DistanceX + 10 * (DistanceY - DistanceX);
    }
}
