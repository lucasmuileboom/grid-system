using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool _walkable;
    [SerializeField] private bool _buildable;

    public int _gCost;
    public int _hCost;
    public Node _parent;

    public bool _Buildable
    {
        get
        {
            return _buildable;
        }
        set
        {
            _buildable = value;
        }
    }
    public bool _Walkable
    {
        get
        {
            return _walkable;
        }
        set
        {
            _walkable = value;
        }
    }
    public int _fCost
    {
        get
        {
            return _gCost + _hCost;
        }
    }

}
