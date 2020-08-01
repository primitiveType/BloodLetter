using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodingEssentials.Trees;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class OctreeManager : MonoBehaviourSingleton<OctreeManager>
{
    // private NonSparseOctree<IOctreeObject> Octree { get; set; }

    [SerializeField] private bool debug;

    public GameObject Scene;
    [field: SerializeField] public float UpdateInterval { get; } = 1f;

    [FormerlySerializedAs("desiredSize")] public float desiredUnitSize = 1; //meters

    private World staticWorld;
    private World dynamicWorld;

    private void Awake()
    {
        BoxCollider col = Scene.GetComponent<BoxCollider>();
        var colSize = col.size;
        float size = Math.Max(Math.Max(colSize.x, colSize.y), colSize.z);
        var maxLevel = Mathf.Log(size / desiredUnitSize, 2);
        var maxLevelInt = Mathf.CeilToInt(maxLevel);
        staticWorld = new World(Scene, size, this.transform.position, maxLevelInt, 0, true, Graph.GraphType.CENTER,
            true);


        if (debug)
        {
            staticWorld.DisplayVoxels();
        }

    }

    private void Start()
    {
        DefaultPathFindingTarget = Toolbox.Instance.PlayerHeadTransform;
        StartCoroutine(PathfindingTick());

    }

    // public void TestPathFinding(Vector3 start, List<Vector3> dest)
    // {
    //     Graph.PathFindingMethod method = staticWorld.spaceGraph.LazyThetaStar;
    //     float totalLength = 0;
    //     float startTime = Time.realtimeSinceStartup;
    //     var paths = staticWorld
    //         .spaceGraph //TODO: consider using the one that takes a list of source-dest. check performance
    //         .FindPath(method, start, dest, staticWorld.space);
    //
    //     if (paths == null)
    //     {
    //         return;
    //     }
    //
    //     foreach (var path in paths)
    //     {
    //         for (int i = 0; i < path.Count; i++)
    //         {
    //             if (i == 0)
    //             {
    //                 continue;
    //             }
    //
    //             Debug.DrawLine(path[i].center, path[i - 1].center, Color.magenta, 10);
    //         }
    //     }
    // }

    List<PathfindingHandle> HandlesToUpdate = new List<PathfindingHandle>();
    private IEnumerator PathfindingTick()
    {
        int count = 0;
        while (true)
        {
            count = 0;
            HandlesToUpdate.Clear();
            CachedDestinationsList.Clear();

            //currently assumes everything is pathfinding toward the player
            foreach (var pathfinder in currentPathfinders)
            {
                if (pathfinder.NeedsUpdate)
                {
                    pathfinder._pathIndex = count++;
                    CachedDestinationsList.Add(pathfinder.Finder.transform.position);
                }

                if (debug)
                {
                    pathfinder.DrawPath();
                }
            }
            UpdatePathfindingHandles();
            foreach (var pathfinder in currentPathfinders)
            {
                if (pathfinder.NeedsUpdate)
                {
                    pathfinder.IsValid = true;
                    pathfinder.Updated();
                }
            }

            yield return new WaitForSeconds(UpdateInterval);
        }
    }

    private List<PathfindingHandle> currentPathfinders = new List<PathfindingHandle>();

    private List<Vector3> CachedDestinationsList = new List<Vector3>();
    private List<List<Node>> Paths;

    public List<Node> GetPathForIndex(int index)
    {
        return Paths?[index];
    }

    private void UpdatePathfindingHandles()
    {
        Graph.PathFindingMethod method = staticWorld.spaceGraph.LazyThetaStar;
        // handle.CurrentPath
        Paths = staticWorld
            .spaceGraph //TODO: consider using the one that takes a list of source-dest. check performance
            .FindPath(method, DefaultPathFindingTarget.transform.position, CachedDestinationsList, staticWorld.space);
    }

    public Transform DefaultPathFindingTarget { get; private set; }

    public PathfindingHandle StartPathfindingToPlayer(Transform pathfinder)
    {
        var handle = new PathfindingHandle(pathfinder, currentPathfinders.Count);
        currentPathfinders.Add(handle);
        return handle;
    }

    public void RemovePathfinder(PathfindingHandle handle)
    {
        currentPathfinders.Remove(handle);
    }
}

public class PathfindingHandle : IDisposable
{
    public bool NeedsUpdate { get; set; } = true;
    public readonly Transform Finder;
    private Transform Target => OctreeManager.Instance.DefaultPathFindingTarget;
    public bool IsValid { get; set; }
    public List<Node> CurrentPath => OctreeManager.Instance.GetPathForIndex(_pathIndex);
    public int _pathIndex;


    public event PathfindingHandleUpdated UpdatedEvent;
    
    public PathfindingHandle(Transform finder, int pathIndex)
    {
        Finder = finder;
    }

    public void Dispose()
    {
        OctreeManager.Instance.RemovePathfinder(this);
    }

    public void DrawPath()
    {
        Debug.DrawLine(Finder.position, Target.position, Color.green);
        if (CurrentPath == null)
        {
            return;
        }

        for (int i = 0; i < CurrentPath.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            Debug.DrawLine(CurrentPath[i].center, CurrentPath[i - 1].center,
                Color.magenta, 10);
        }
    }

    public void Updated()
    {
        UpdatedEvent?.Invoke(this, new PathfindingHandleUpdatedArgs());
    }
}

public delegate void PathfindingHandleUpdated(object sender, PathfindingHandleUpdatedArgs args);

public class PathfindingHandleUpdatedArgs
{
}