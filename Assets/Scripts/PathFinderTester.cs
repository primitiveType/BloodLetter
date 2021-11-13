using System;
using UnityEngine;

public class PathFinderTester : MonoBehaviour
{
    private AstarPath path;
    private void Start()
    {
        path = GetComponent<AstarPath>();
    }

    private void Update()
    {
        
        Debug.Log($"AStarPath : {path}");
        Debug.Log($"AStarPath data : {path.data}");
        Debug.Log($"AStarPath graph count: {path.graphs.Length}");
        Debug.Log($"AStarPath scan on startup: {path.scanOnStartup}");
        Debug.Log($"AStarPath update in progress: {path.IsAnyGraphUpdateInProgress}");
    }
}