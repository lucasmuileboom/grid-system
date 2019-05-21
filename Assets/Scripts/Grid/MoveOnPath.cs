using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 stop;
    private AStarPathFinding _AStarPathFinding;
    private List<Node> path;

    void Start() 
    {
        _AStarPathFinding = GameObject.Find("GridSystem").GetComponent<AStarPathFinding>(); ;
        StartMoving(stop);//weg
    }
    public void ChangePath(Vector3 endPosition) 
    {
        path = _AStarPathFinding.FindPath(transform.position, endPosition);
    }
    public void StartMoving(Vector3 endPosition) 
    {
        ChangePath(endPosition);
        StartCoroutine(FollowPath());
    }
    public void StopMoving() 
    {
        StopCoroutine(FollowPath());
    }
    IEnumerator FollowPath() 
    {
        Vector3 currentTarget = new Vector3(path[0]._nodePositionX, path[0]._nodePositionY, -1);
        int tagetindex = 0;

        while (true) 
        { 
            if (transform.position == currentTarget) 
            {
                tagetindex++;
                if (tagetindex >= path.Count) 
                {
                    yield break;
                }
                currentTarget = new Vector3(path[tagetindex]._nodePositionX, path[tagetindex]._nodePositionY, -1);
            }
            transform.position = Vector3.MoveTowards(this.transform.position, currentTarget, _speed * Time.deltaTime);
            yield return null;
        }        
    }
}
