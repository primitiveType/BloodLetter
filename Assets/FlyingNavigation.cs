using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMovementHandler : MonoBehaviour
{
    private FlyingNavigation Navigation;

    private void Start()
    {
        Navigation = GetComponent<FlyingNavigation>();
    }
    
    
}

public class FlyingNavigation : MonoBehaviour, INavigationAgent
{
    [SerializeField] private float RaycastDistance = 5f;
    [SerializeField] private float MaxRotationSpeed = 1f;
    [SerializeField] private float BreakDistance = 1f;
    [SerializeField] private bool AimForBreakDistance = true;
    [SerializeField] private float MaxAcceleration = 1f;

    public Vector3 TargetPosition { get; private set; }
    private ActorRoot ActorRoot { get; set; }

    private Transform myTransform;

    private Rigidbody rb;

    [SerializeField] private float _maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myTransform = transform;
        ActorRoot = GetComponentInParent<ActorRoot>();
    }

    private void FixedUpdate()
    {
        if (!ActorRoot.VisibilityHandler.LastSeenPosition.HasValue)
        {
            return;
        }

        bool currentlyHasVision = ActorRoot.VisibilityHandler.CanSeePlayer();

        var prevLocation = myTransform.position;

        Vector3 dest;
        Vector3 lookDest = ActorRoot.VisibilityHandler.LastSeenPosition.Value;
        if (AimForBreakDistance && currentlyHasVision)
        {
            var offset = (prevLocation - ActorRoot.VisibilityHandler.LastSeenPosition.Value).normalized * BreakDistance;
            dest = offset + ActorRoot.VisibilityHandler.LastSeenPosition.Value;
        }
        else
        {
            dest = lookDest;
        }

        HandleRotation(lookDest);
        HandleVelocity(dest);
    }

    private void HandleVelocity(Vector3 dest)
    {
        if (Vector3.Distance(myTransform.position, dest) <= BreakDistance)
        {
            TrySetVelocity(Vector3.zero);
        }
        else
        {
            var desiredVelocity =
                (dest - myTransform.position).normalized * MaxSpeed; //((dest) - prevLocation).normalized * MaxSpeed;
            TrySetVelocity(desiredVelocity);
        }
    }

    private void TrySetVelocity(Vector3 newVelocity)
    {
        var diff = Vector3.Distance(rb.velocity, newVelocity);
        if (diff > MaxAcceleration)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, newVelocity, (MaxAcceleration / diff));
        }
        else
        {
            rb.velocity = newVelocity;
        }
    }

    private void HandleRotation(Vector3 targetDestination)
    {
        var prevLocation = myTransform.position;
        var targetLook = (targetDestination - prevLocation).normalized;
        var angleBetween = Vector3.Angle(targetLook, myTransform.forward);
        var tValue = Time.deltaTime;
        var maxRotationsThisframe = MaxRotationSpeed * (tValue);
        var maxDegreesThisFrame = maxRotationsThisframe * 360f;
        if (angleBetween > maxDegreesThisFrame)
        {
            //if we are turning too fast, adjust t by the same ratio to slow turning down to the max turn speed
            var diffRatio = maxDegreesThisFrame / angleBetween;
            tValue *= diffRatio;
        }

        Debug.Log(tValue);
        myTransform.forward = Vector3.Slerp(myTransform.forward, targetLook, tValue);
    }

    public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }

    private void OnDrawGizmosSelected()
    {
        myTransform = transform;
        foreach (var ray in GetRays())
        {
            Debug.DrawRay(ray.origin, ray.direction * RaycastDistance);
        }
    }

    private IEnumerable<Ray> GetRays()
    {
        var position = myTransform.position;
        yield return new Ray(position, myTransform.forward);
        var up = myTransform.up;
        yield return new Ray(position, up);
        var right = myTransform.right;
        yield return new Ray(position, right);
        yield return new Ray(position, -up);
        yield return new Ray(position, -right);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool isStopped { get; set; }
    public bool updateRotation { get; set; }
    public Vector3 velocity => rb.velocity;

    public void SetDestination(Vector3 targetPosition)
    {
    }
}