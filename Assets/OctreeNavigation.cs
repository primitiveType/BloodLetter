using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodingEssentials;
using UnityEngine;

public class OctreeNavigation : MonoBehaviour, INavigationAgent
{
    private PathfindingHandle PathfindingHandle { get; set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private ActorEvents Events;

    // Start is called before the first frame update
    void Start()
    {
        PathfindingHandle = OctreeManager.Instance.StartPathfindingToPlayer(gameObject);
        DrawPath();
        myObjectTransform = transform;
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }

    private void DrawPath()
    {
        for (int i = 0; i < PathfindingHandle.CurrentPath.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            Debug.DrawLine(PathfindingHandle.CurrentPath[i].center, PathfindingHandle.CurrentPath[i - 1].center,
                Color.magenta, 10);
        }
    }


    int pathIndex = 0;
    bool moveTowards = true; // or -1 if backwards
    Transform myObjectTransform;
    float distanceErr = .1f; // this should probably be half node size
    float stoppingDistance = 1f;
    private Vector3 positionOffset = Vector3.up;
    private Vector3 myPosition => myObjectTransform.position + positionOffset;

    private void Update()
    {
        if (updateRotation)
        {
            var rot = myObjectTransform.rotation.eulerAngles;
            myObjectTransform.LookAt(PathfindingHandle.CurrentPath[pathIndex].center, Vector3.up);
            myObjectTransform.rotation = Quaternion.Euler(rot.x, myObjectTransform.rotation.eulerAngles.y, rot.z);
        }
        
        if (Vector3.Distance(myPosition, PathfindingHandle.CurrentPath.Last().center) <=
            stoppingDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }


        var myDesiredLocation = PathfindingHandle.CurrentPath[pathIndex].center;
        rb.velocity = Vector3.Lerp((myDesiredLocation - myPosition).normalized * MaxSpeed,
            rb.velocity, .5f);

        if (Vector3.Distance(myPosition, myDesiredLocation) < distanceErr)
        {
            // My object reach the of path

            pathIndex += moveTowards ? 1 : -1; // My next location in the path
            if (pathIndex <= 0 || pathIndex >= PathfindingHandle.CurrentPath.Count)
            {
                // stop doing the above because the player reach the end of the path
                rb.velocity = Vector3.zero;
            }
        }

       
    }

    public float MaxSpeed;
    public bool isStopped { get; set; }
    public bool updateRotation { get; set; }
    public Vector3 velocity => rb.velocity;

    public void SetDestination(Vector3 targetPosition)
    {
        //Do nothing for now. We alays use the player as the target for now.
    }
}