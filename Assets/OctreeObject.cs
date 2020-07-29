using System.Collections.Generic;
using CodingEssentials.Trees;
using UnityEngine;
using UnityEngine.Networking.Match;

public class OctreeObject : MonoBehaviour, IOctreeObject, IHasNeighbours<OctreeObject>
{
    private Collider m_collider;
    private Collider Collider => m_collider; 
    public Bounds Bounds { get; set; }

    private void Awake() {
        m_collider = GetComponent<Collider>();
        Bounds = Collider.bounds;
    }

    private void Update()
    {
        OctreeManager.Instance.UpdateDynamic(Collider.bounds);
        if(Bounds.center != Collider.bounds.center) {
            
            // OctreeManager.Instance.UpdateTree(this, Collider.bounds);
            // if(!Tree.Contains(Bounds)) {
            //     Tree.Remove(this);
            //     gameObject.Destroy();
            // }
        }
    }

    public IEnumerable<OctreeObject> Neighbours { get; } 
}