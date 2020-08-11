using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CodingEssentials;
using UnityEngine;
using UnityEngine.Profiling;

public class OctreeNavigation : MonoBehaviour, INavigationAgent
{
    private PathfindingHandle PathfindingHandle { get; set; }

    [SerializeField] private Rigidbody rb;
    //[SerializeField] private Transform pathfindingAnchor;
    [SerializeField] private bool debug;
    [SerializeField] private IActorEvents Events;
    [SerializeField] private float VelocityUpdateInterval = .2f;

    // Start is called before the first frame update
    void Start()
    {
        PathfindingHandle = OctreeManager.Instance.StartPathfindingToPlayer(transform);
        PathfindingHandle.NeedsUpdate = false;
        PathfindingHandle.UpdatedEvent += PathfindingHandleOnUpdatedEvent;
        if (debug)
        {
            PathfindingHandle.DrawPath();
        }

        myObjectTransform = transform;
        Events.OnAggroEvent -= EventsOnOnAggroEvent;
        Events.OnAggroEvent += EventsOnOnAggroEvent;
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void PathfindingHandleOnUpdatedEvent(object sender, PathfindingHandleUpdatedArgs args)
    {
        CurrentNodeIndex = PathfindingHandle.CurrentPath.Count - 2; //ignore the first one
    }

    private void EventsOnOnAggroEvent(object sender, OnAggroEventArgs args)
    {
        PathfindingHandle.NeedsUpdate = true;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        enabled = false;
        PathfindingHandle.Dispose();
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
        Events.OnAggroEvent -= EventsOnOnAggroEvent;
    }


    int currentNodeIndex = 0;
    bool moveTowards = true; // or -1 if backwards
    Transform myObjectTransform;
    float distanceErr = .1f; // this should probably be half node size
    float stoppingDistance = 1f;
    private Vector3 positionOffset = Vector3.down * .5f;
    private Vector3 myPosition => myObjectTransform.position + positionOffset;

    private Vector3 lastDesiredVelocity;

    private void Update()
    {
        if (!ShouldPathfind())
        {
            return;
        }

        myDesiredLocation = PathfindingHandle.CurrentPath[CurrentNodeIndex].center;

        if (Vector3.Distance(myPosition, myDesiredLocation) < distanceErr)
        {
            // My object reach the of path

            CurrentNodeIndex--; // My next location in the path
        }
    }

    private void FixedUpdate()
    {
        if (!ShouldPathfind())
        {
            return;
        }


        myDesiredLocation = PathfindingHandle.CurrentPath[CurrentNodeIndex].center;


        if (updateRotation)
        {
            var rot = myObjectTransform.rotation.eulerAngles;
            myObjectTransform.LookAt(PathfindingHandle.CurrentPath[CurrentNodeIndex].center, Vector3.up);
            myObjectTransform.rotation = Quaternion.Euler(rot.x, myObjectTransform.rotation.eulerAngles.y, rot.z);
        }

        if (Vector3.Distance(myPosition, PathfindingHandle.CurrentPath.First().center) <=
            stoppingDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }


        myDesiredLocation = PathfindingHandle.CurrentPath[CurrentNodeIndex].center;
        var prevLocation = myPosition;
        var desiredVelocity = ((myDesiredLocation) - prevLocation).normalized * MaxSpeed;
        rb.velocity = desiredVelocity;


        //  Debug.DrawLine(rb.transform.position + rb.velocity.normalized, rb.transform.position, Color.blue, 10);
    }

    private bool ShouldPathfind()
    {
        if (isStopped)
        {
            return false;
        }

        if (!enabled)
        {
            return false;
        }

        if (PathfindingHandle == null || !PathfindingHandle.IsValid)
        {
            return false;
        }
        
        
        if (PathfindingHandle._pathIndex < 0)
        {
            return false;
        }


        return true;
    }

    public float MaxSpeed;
    private Vector3 myDesiredLocation;
    public bool isStopped { get; set; }
    public bool updateRotation { get; set; }
     public Vector3 velocity => rb.velocity;

    public int CurrentNodeIndex
    {
        get => currentNodeIndex;
        set => currentNodeIndex = Mathf.Clamp(value, 0, Int32.MaxValue);
    }

    public void SetDestination(Vector3 targetPosition)
    {
        //Do nothing for now. We alays use the player as the target for now.
    }
}

public class ProfilerSample : IDisposable
{
    public ProfilerSample(string name)
    {
        Profiler.BeginSample(name);
    }

    public void Dispose()
    {
        Profiler.EndSample();
    }
}