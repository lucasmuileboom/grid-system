using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool BuildingButtonDown()
    {
        return Input.GetKeyDown(KeyCode.B);
    }
    public bool LMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool RMouseDown()
    {
        return Input.GetMouseButtonDown(1);
    }
    public Vector3 MousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            print("Mouse Out Of Area");
            return Vector3.zero;
        }
    }
}