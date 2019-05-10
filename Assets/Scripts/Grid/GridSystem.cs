using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject _nodeprefab;
    [SerializeField] private int _gridRows;
    [SerializeField] private int _gridColumns;
    [SerializeField] private float _nodeHeight;//miss in node opslaan?
    [SerializeField] private float _nodeWidth;//miss in node opslaan?
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

                _grid[i, j]._nodePositionX = _spawnPosition.x;
                _grid[i, j]._nodePositionY = _spawnPosition.y;
                _grid[i, j]._nodePositionInGridX = i;
                _grid[i, j]._nodePositionInGridY = j;
            }
        }
    }
    public int GetXInGridFromWorldPosition(float position)
    {
        position -= transform.position.x;
        return Mathf.RoundToInt(position / (_nodeWidth + _NodeGap));
    }
    public int GetYInGridFromWorldPosition(float position)
    {
        position -= transform.position.y;
        return Mathf.RoundToInt(position / (_nodeHeight + _NodeGap));
    }
    public Node GetNodeFromWorldPosition(Vector3 worldPosition) 
    {
        return _grid[GetXInGridFromWorldPosition(worldPosition.x), GetYInGridFromWorldPosition(worldPosition.y)];
    }
    public List<Node> GetNodeNeibers(Node currentnode,int amoutOfNeibersX, int amoutOfNeibersY)
    {
        List<Node> result = new List<Node>();

        for (int i = (int)-Mathf.Floor(amoutOfNeibersX / 2); i < -Mathf.Floor(amoutOfNeibersX / 2) + amoutOfNeibersX; i++)
        {
            for (int j = (int)-Mathf.Floor(amoutOfNeibersY / 2); j < -Mathf.Floor(amoutOfNeibersY / 2) + amoutOfNeibersY; j++)
            {
                if (currentnode._nodePositionInGridX + i >= 0 && currentnode._nodePositionInGridX + i < _grid.GetLength(0) && currentnode._nodePositionInGridY + j >= 0 && currentnode._nodePositionInGridY + j < _grid.GetLength(1))
                {
                    result.Add(_grid[(int)currentnode._nodePositionInGridX + i, (int)currentnode._nodePositionInGridY + j]);
                }
            }
        }
        return result;
    }
}
