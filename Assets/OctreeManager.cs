using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingEssentials.Trees;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
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
        staticWorld = new World(Scene, size, this.transform.position, maxLevelInt, 0, true, Graph.GraphType.CORNER,
            true);


        // string test = JsonConvert.SerializeObject(staticWorld);

        // string path = Path.Combine(Application.dataPath, "test.json");
        // File.WriteAllText(path, test);
        // Debug.Log($"wrote test json to {path}");
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

            var targetPos = DefaultPathFindingTarget.transform.position;
            Task.Run(() =>
                {
                    UpdatePathfindingHandles(targetPos);

                    foreach (var pathfinder in currentPathfinders)
                    {
                        if (pathfinder.NeedsUpdate)
                        {
                            pathfinder.IsValid = true;
                            pathfinder.Updated();
                        }
                    }
                }
            );


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

    private void UpdatePathfindingHandles(Vector3 targetPosition)
    {
        Graph.PathFindingMethod method = staticWorld.spaceGraph.LazyThetaStar;
        Profiler.BeginSample("FindPath");
        // handle.CurrentPath
        Paths = staticWorld
            .spaceGraph //TODO: consider using the one that takes a list of source-dest. check performance
            .FindPath(method, targetPosition , CachedDestinationsList, staticWorld.space);
        Profiler.EndSample(); //("FindPath");
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
    public bool IsValid { get; set; } = false;
    private static List<Node> EmptyList = new List<Node>();
    public List<Node> CurrentPath => IsValid ? OctreeManager.Instance.GetPathForIndex(_pathIndex) : EmptyList;
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
        Debug.DrawLine(Finder.position, Finder.position + Vector3.up, Color.green);
        Debug.DrawLine(Target.position, Target.position + Vector3.up, Color.red);
        if (CurrentPath == null)
        {
            return;
        }

        for (int i = 0; i < CurrentPath.Count; i++)
        {
            Color color = Color.magenta;
            if (i == 0)
            {
                continue;
            }
            else if (i == CurrentPath.Count - 1)
            {
                color = Color.red;
            }
            else if (i == 1)
            {
                color = Color.green;
            }

            Debug.DrawLine(CurrentPath[i].center, CurrentPath[i - 1].center,
                color, 10);
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