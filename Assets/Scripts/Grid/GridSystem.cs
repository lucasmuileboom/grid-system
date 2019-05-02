using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject _nodeprefab;
    [SerializeField] private int _gridRows;
    [SerializeField] private int _gridColumns;
    [SerializeField] private float _nodeHeight;
    [SerializeField] private float _nodeWidth;
    [SerializeField] private float _NodeGap;
    private Node[,] _grid;

    private void Start()
    {
        SpawnGrid();
    }
    private void SpawnGrid()
    {
        _grid = new Node[_gridRows, _gridColumns];
        Vector3 _spawnPosition;
        GameObject _spawnedObject;
        for (int i = 0; i < _gridRows; i++)
        {
            for (int j = 0; j < _gridColumns; j++)
            {
                _spawnPosition = new Vector3((_nodeWidth + _NodeGap) * i, (_nodeHeight + _NodeGap) * j, 0) + transform.position;
                _spawnedObject = Instantiate(_nodeprefab, _spawnPosition, Quaternion.identity);
                _spawnedObject.transform.parent = transform;
                _spawnedObject.transform.localScale = new Vector3(_nodeWidth, _nodeHeight, 1);
                _grid[i, j] = _spawnedObject.GetComponent<Node>();
            }
        }
    }
    public int GetXCount(float position)
    {
        position -= transform.position.x;
        return Mathf.RoundToInt(position / (_nodeWidth + _NodeGap));
    }
    public int GetYCount(float position)
    {
        position -= transform.position.y;
        return Mathf.RoundToInt(position / (_nodeHeight + _NodeGap));
    }
    public Vector3 NearestPointInWorldSpace(Vector3 position)
    {
        Vector3 _nodePosition;
        _nodePosition.x = GetXCount(position.x) * (_nodeWidth + _NodeGap);
        _nodePosition.y = GetYCount(position.y) * (_nodeHeight + _NodeGap);
        _nodePosition.z = -1;
        return _nodePosition + transform.position;
    }
    public List<Node> GetNodeNeibers(Vector2 nodePosition,int amoutOfNeibersX, int amoutOfNeibersY)
    {
        List<Node> result = new List<Node>();

        for (int i = (int)-Mathf.Floor(amoutOfNeibersX / 2); i < -Mathf.Floor(amoutOfNeibersX / 2) + amoutOfNeibersX; i++)
        {
            for (int j = (int)-Mathf.Floor(amoutOfNeibersY / 2); j < -Mathf.Floor(amoutOfNeibersY / 2) + amoutOfNeibersY; j++)
            {
                if (nodePosition.x + i >= 0 && nodePosition.x + i < _grid.GetLength(0) && nodePosition.y + j >= 0 && nodePosition.y + j < _grid.GetLength(1))
                {
                    result.Add(_grid[(int)nodePosition.x + i, (int)nodePosition.y + j]);
                }
            }
        }
        return result;
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        return _grid[GetXCount(worldPosition.x), GetYCount(worldPosition.y)];
    }
}
