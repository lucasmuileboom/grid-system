using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private GameObject[] _buildings;

    private GameObject _spawnedBuilding;
    private Node _previousNode;
    private int _buildingHeightInNodes;
    private int _buildingWidthInNodes;
    private bool _previousCanBuild = false;
    private bool _building = false;

    //moet een ui input maken die StartBuilding aanroept met het goede gebouw
    //moet aangeroepen met de goede buildsize
    //test moet weg
    [SerializeField] private int testSizeX;
    [SerializeField] private int testSizeY;

    private bool CanBuildHere(Node[] _nodeNeibers, int xNeibers, int yNeibers)
    {
        if(xNeibers * yNeibers != _nodeNeibers.Length)
        {
            return false;
        }
        for (int i = 0; i < _nodeNeibers.Length; i++)
        {
            if (_nodeNeibers[i]._Buildable == false)
            {
                return false;
            }
        }
        return true;
    }
    private void Update()
    {
        if (_inputManager.BuildingButtonDown())//moet weg als de input er is
        {
            StartBuilding(0);
            print("start");
        }
        if (_building)
        {
            BuildingEnabled(_buildingHeightInNodes, _buildingWidthInNodes);
            if (_inputManager.RMouseDown())
            {
                StopBuilding();
            }
        }
    }
    private void BuildingEnabled(int buildingSizeX, int buildingSizeY)
    {
        Node currentnode = _gridSystem.GetNodeFromWorldPosition(_inputManager.MousePosition()); ;
        Vector3 _nodePositionWordSpace = new Vector3(currentnode._nodePositionX, currentnode._nodePositionY,-1);
        Vector2 _nodePositionInGrid = new Vector2(currentnode._nodePositionInGridX, currentnode._nodePositionInGridX);
        bool _canbuild = false;

        _spawnedBuilding.transform.position = _nodePositionWordSpace;

        if (_previousCanBuild && _previousNode == currentnode)
        {
            CanPlaceBuilding();
        }
        else
        {
            Node[] _nodeNeibers = _gridSystem.GetNodeNeibers(currentnode, buildingSizeX, buildingSizeY).ToArray();
            _canbuild = CanBuildHere(_nodeNeibers, buildingSizeX, buildingSizeY);
            if (_canbuild)
            {
                CanPlaceBuilding();
                if (_inputManager.LMouseDown())
                {
                    PlaceBuilding(_nodeNeibers);
                }
            }
            else
            {
                CantPlaceBuilding();
            }
        }
        _previousNode = currentnode;
        _previousCanBuild = _canbuild;
    }
    public void StartBuilding(int buildingNumber)
    {
        if (!_building)
        {
            _spawnedBuilding = Instantiate(_buildings[buildingNumber], _inputManager.MousePosition(), Quaternion.identity);
            _buildingHeightInNodes = testSizeX;
            _buildingWidthInNodes = testSizeY;
            _previousCanBuild = false;
            _building = true;
        }
    }
    private void StopBuilding()
    {
        Destroy(_spawnedBuilding);
        _previousCanBuild = false;
        _building = false;
    }
    private void PlaceBuilding(Node[] _nodeNeibers)
    {
        _spawnedBuilding.transform.position += new Vector3(0, 0, 0.5f);
        _spawnedBuilding = null;
        _building = false;
        for (int i = 0; i < _nodeNeibers.Length; i++)
        {
            _nodeNeibers[i]._Buildable = false;
        }
    }
    private void CanPlaceBuilding()
    {
        _spawnedBuilding.GetComponent<Renderer>().material.color = Color.green;
        _previousCanBuild = true;
    }
    private void CantPlaceBuilding()
    {
        _spawnedBuilding.GetComponent<Renderer>().material.color = Color.red;
        _previousCanBuild = false;
    }
}