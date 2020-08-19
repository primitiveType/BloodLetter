using UnityEngine;

public class FlyingNavigation : MonoBehaviour, INavigationAgent
{
    [SerializeField] private float MaxRotationSpeed = 1f;
    [SerializeField] private float BreakDistance = 1f;
    [SerializeField] private bool AimForBreakDistance = true;
    [SerializeField] private float MaxAcceleration = 1f;
    private FlyingSteeringComponent Steering { get; set; }
    private ActorRoot ActorRoot { get; set; }

    private Transform myTransform;

    private Rigidbody rb;

    [SerializeField] private float _maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Steering = GetComponent<FlyingSteeringComponent>();
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

        TargetPosition = dest;
        HandleRotation(lookDest);
        HandleVelocity(dest);
    }

    public Vector3 TargetPosition { get; private set; }

    private void HandleVelocity(Vector3 dest)
    {
        if (Vector3.Distance(myTransform.position, dest) <= BreakDistance)
        {
            TrySetVelocity(Vector3.zero);
        }
        else
        {
            var desiredVelocity =
                Steering.GetAdjustedDirectionToTarget(dest) * MaxSpeed; //((dest) - prevLocation).normalized * MaxSpeed;
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

        myTransform.forward = Vector3.Slerp(myTransform.forward, targetLook, tValue);
    }

    public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
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