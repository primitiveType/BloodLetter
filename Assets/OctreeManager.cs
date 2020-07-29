// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// public class DynamicOctreeBounds : MonoBehaviour
// {
// 	[SerializeField] private Collider _collider;
//
// 	private void Start()
// 	{
// 		if (!_collider)
// 		{
// 			_collider = GetComponent<Collider>();
// 			if (!_collider)
// 			{
// 				_collider = gameObject.AddComponent<BoxCollider>();
// 			}
// 		}
// 	
// 	}
//
// 	private void Update()
// 	{
// 		OctreeManager.Instance.Add();
// 	}
// }
// public class OctreeManager : MonoBehaviourSingleton<OctreeManager>
// {
// 	private BoundsOctree<GameObject> boundsTree;
//
// 	[SerializeField] private BoxCollider Levelbounds;
//     // Start is called before the first frame update
//     void Start()
//     {
// 	    boundsTree = new BoundsOctree<GameObject>(150, transform.position, 1, 1);
// 	    var colliders = Physics.OverlapBox(Levelbounds.center, Levelbounds.size / 2f, Quaternion.identity);
// 	    foreach (var other in colliders)
// 	    {
// 			Add(other.gameObject, other.bounds);
// 	    }
//     }
//
//     public void Add(GameObject go, Bounds bounds)
//     {
// 	    boundsTree.Add(go, bounds);
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//     }
//     
//     void OnDrawGizmos() {
//     	boundsTree.DrawAllBounds(); // Draw node boundaries
//     	boundsTree.DrawAllObjects(); // Draw object boundaries
//     	boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries
//     
//     	
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using CodingEssentials;
using CodingEssentials.Trees;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class OctreeManager : MonoBehaviourSingleton<OctreeManager>
{
    // private NonSparseOctree<IOctreeObject> Octree { get; set; }

    public GameObject Scene;

    [FormerlySerializedAs("desiredSize")] public float desiredUnitSize = 1; //meters

    private void OnDrawGizmos()
    {
        // if(Octree != null) Octree.DebugDraw();
    }

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
        dynamicWorld = new World(Scene, size, this.transform.position, maxLevelInt, 0, true, Graph.GraphType.CENTER,
            false);

        staticWorld.space.DisplayVoxels(maxLevelInt);
        // Octree = new NonSparseOctree<IOctreeObject>(1, new Vector3(minsize,minsize,minsize), col.bounds);
        //    var colliders = Physics.OverlapBox(col.center, col.size / 2f, Quaternion.identity);
        // foreach (var other in colliders)
        //    {
        //        if (other.isTrigger) continue;
        //        
        //        var oo = other.GetOrAddComponent<OctreeObject>();
        //        Octree.Add(oo);
        //    }
    }

    // List<Vector3> currentV1 = null;
    // List<List<Vector3>> currentV2 = null;

    public void TestPathFinding(Vector3 start, Vector3 dest)
    {
        Graph.PathFindingMethod method = staticWorld.spaceGraph.LazyThetaStar;
        float totalLength = 0;
        float startTime = Time.realtimeSinceStartup;
        List<Node> paths = staticWorld
            .spaceGraph //TODO: consider using the one that takes a list of source-dest. check performance
            .FindPath(method, start, dest, staticWorld.space);

        if (paths == null)
        {
            return;
        }

        for (int i = 0; i < paths.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            Debug.DrawLine(paths[i].center, paths[i - 1].center, Color.magenta, 10);
        }
    }


    // public void UpdateTree(IOctreeObject obj, Bounds bounds)
    // {
    //     Octree.Update(obj, bounds);
    //     
    // }

    // public ISpatialTree<IOctreeObject, Bounds> FindSharedRoot(IOctreeObject first, IOctreeObject second)
    // {
    //     var val = Octree.FindSharedRoot(first, second);
    //     return val;
    // }
    public void UpdateDynamic(Bounds colliderBounds)
    {
        var node = staticWorld.space.Find(colliderBounds.center);
        node.dynamicBlocked = true;
    }
}

public class NonSparseOctree<T> : Octree<T> where T : class, IOctreeObject
{
    private Vector3 CellSize { get; }

    public NonSparseOctree(int maxObjects, Vector3 cellSize, Bounds bounds) : base(maxObjects, cellSize, bounds)
    {
        CellSize = cellSize;
        SubdivideUntilCellSizeValid();
    }

    public NonSparseOctree(int maxObjects, int mergeThreshold, Vector3 cellSize, Bounds bounds) : base(maxObjects,
        mergeThreshold, cellSize, bounds)
    {
        CellSize = cellSize;
        SubdivideUntilCellSizeValid();
    }

    private void SubdivideUntilCellSizeValid()
    {
        if (Bounds.size.x > CellSize.x || Bounds.size.y > CellSize.y || Bounds.size.z > CellSize.z)
        {
            Split();
        }
    }

    protected override void Split()
    {
        var c = TreeBounds.center;
        var size = TreeBounds.extents;
        var half = size / 2;
        Cells = new SpatialTree<T, Bounds>[8];
        Cells[0] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(half.x, half.y, half.z), size));
        Cells[1] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(-half.x, half.y, half.z), size));
        Cells[2] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(half.x, -half.y, half.z), size));
        Cells[3] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(-half.x, -half.y, half.z), size));
        Cells[4] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(half.x, half.y, -half.z), size));
        Cells[5] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(-half.x, half.y, -half.z), size));
        Cells[6] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(half.x, -half.y, -half.z), size));
        Cells[7] = new NonSparseOctree<T>(MaxObjects, MergeThreshold, CellSize,
            new Bounds(c + new Vector3(-half.x, -half.y, -half.z), size));
    }
}

public class Path<Node> : IEnumerable<Node>
{
    public Node LastStep { get; private set; }
    public Path<Node> PreviousSteps { get; private set; }
    public double TotalCost { get; private set; }

    private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
    {
        LastStep = lastStep;
        PreviousSteps = previousSteps;
        TotalCost = totalCost;
    }

    public Path(Node start) : this(start, null, 0)
    {
    }

    public Path<Node> AddStep(Node step, double stepCost)
    {
        return new Path<Node>(step, this, TotalCost + stepCost);
    }

    public IEnumerator<Node> GetEnumerator()
    {
        for (Path<Node> p = this; p != null; p = p.PreviousSteps)
            yield return p.LastStep;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public interface IHasNeighbours<N>
{
    IEnumerable<N> Neighbours { get; }
}


public class AStar
{
    public static Path<Node> FindPath<Node>(
        Node start,
        Node destination,
        Func<Node, Node, double> distance,
        Func<Node, double> estimate)
        where Node : IHasNeighbours<Node>
    {
        var closed = new HashSet<Node>();
        var queue = new PriorityQueue<double, Path<Node>>();
        queue.Enqueue(0, new Path<Node>(start));
        while (!queue.IsEmpty)
        {
            var path = queue.Dequeue();
            if (closed.Contains(path.LastStep))
                continue;
            if (path.LastStep.Equals(destination))
                return path;
            closed.Add(path.LastStep);
            foreach (Node n in path.LastStep.Neighbours)
            {
                double d = distance(path.LastStep, n);
                var newPath = path.AddStep(n, d);
                queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
            }
        }

        return null;
    }
}

class PriorityQueue<P, V>
{
    private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();

    public void Enqueue(P priority, V value)
    {
        Queue<V> q;
        if (!list.TryGetValue(priority, out q))
        {
            q = new Queue<V>();
            list.Add(priority, q);
        }

        q.Enqueue(value);
    }

    public V Dequeue()
    {
        // will throw if there isn’t any first element!
        var pair = list.First();
        var v = pair.Value.Dequeue();
        if (pair.Value.Count == 0) // nothing left of the top priority.
            list.Remove(pair.Key);
        return v;
    }

    public bool IsEmpty
    {
        get { return !list.Any(); }
    }
}