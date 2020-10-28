using UnityEngine;
using UnityEngine.Serialization;

public class FlyingNavigation : MonoBehaviour, INavigationAgent
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private bool AimForBreakDistance = true;
    [SerializeField] private float BreakDistance = 1f;
    [SerializeField] private float MaxAcceleration = 1f;
    [SerializeField] private float MaxRotationSpeed = 1f;

    [FormerlySerializedAs("myTransform")] [SerializeField]
    private Transform myEyes;

    [SerializeField] private Transform rigidBodyTransform;

    private Rigidbody rb;
    private FlyingSteeringComponent Steering { get; set; }
    private ActorRoot ActorRoot { get; set; }

    public Vector3 TargetPosition { get; private set; }

    public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }

    public bool isStopped { get; set; }
    public bool updateRotation { get; set; }
    public Vector3 velocity => rb.velocity;

    public void SetDestination(Vector3 targetPosition)
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        Steering = GetComponent<FlyingSteeringComponent>();
        rb = GetComponentInParent<Rigidbody>();
        myEyes = transform;
        ActorRoot = GetComponentInParent<ActorRoot>();
    }

    private void FixedUpdate()
    {
        if (!ActorRoot.VisibilityHandler.LastSeenPosition.HasValue) return;
        if (!ActorRoot.Health.IsAlive) return;

        var currentlyHasVision = ActorRoot.VisibilityHandler.CanSeePlayer();

        var prevLocation = myEyes.position;

        Vector3 dest;
        var lookDest = ActorRoot.VisibilityHandler.LastSeenPosition.Value;
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

    private void HandleVelocity(Vector3 dest)
    {
        var currentlyHasVision = ActorRoot.VisibilityHandler.CanSeePlayer();

        if (currentlyHasVision && Vector3.Distance(myEyes.position, dest) <= BreakDistance)
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
            rb.velocity = Vector3.Lerp(rb.velocity, newVelocity, MaxAcceleration / diff);
        else
            rb.velocity = newVelocity;
    }

    private void HandleRotation(Vector3 targetDestination)
    {
        var prevLocation = myEyes.position;
        var targetLook = (targetDestination - prevLocation).normalized;
        var angleBetween = Vector3.Angle(targetLook, myEyes.forward);
        var tValue = Time.deltaTime;
        var maxRotationsThisframe = MaxRotationSpeed * tValue;
        var maxDegreesThisFrame = maxRotationsThisframe * 360f;
        if (angleBetween > maxDegreesThisFrame)
        {
            //if we are turning too fast, adjust t by the same ratio to slow turning down to the max turn speed
            var diffRatio = maxDegreesThisFrame / angleBetween;
            tValue *= diffRatio;
        }

        var forward = rigidBodyTransform.forward;
        forward = Vector3.Slerp(forward, targetLook, tValue);

        //forward = new Vector3(forward.x, newY, forward.z);
        rigidBodyTransform.forward = forward;
        //    myTransform.rotation = Quaternion.Euler(0, myTransform.rotation.y, 0 );//HACK
    }


    // Update is called once per frame
    private void Update()
    {
    }
}