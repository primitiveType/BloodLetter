using UnityEngine;

public class MonsterVisibilityHandler : MonoBehaviour
{
    [SerializeField] private float _degreesVisibility = 180;
    private readonly int checkFrequency = 2;

    private int lastFrameCheck;

    private bool m_CanSeePlayer;
    [SerializeField] private Transform m_monsterTransform;
    [SerializeField] private Transform m_EyesTransform;
    private Transform Target { get; set; }

    public Vector3? LastSeenPosition { get; set; }


    private float DegreesVisibility => _degreesVisibility;

    private Transform MonsterTransform
    {
        get => m_monsterTransform;
        set => m_monsterTransform = value;
    }

    private Transform TargetCollider { get; set; }

    public Transform EyesTransform
    {
        get => m_EyesTransform;
        set => m_EyesTransform = value;
    }

    private void Start()
    {
        Target = Toolbox.Instance.PlayerHeadTransform;
        TargetCollider = Toolbox.Instance.PlayerTransform;
        //MonsterTransform = transform;
    }

    public bool CanSeePlayer(bool ignoreDirection = false, bool forceCheck = false)
    {
        if (!forceCheck)
            if (Time.frameCount < lastFrameCheck + checkFrequency)
                return m_CanSeePlayer;

        lastFrameCheck = Time.frameCount;

        if (!ignoreDirection)
        {
            var direction = (MonsterTransform.position - Target.position).normalized;
            // Debug.Log(direction);
            var angle = Vector3.Dot(direction, MonsterTransform.forward);

            if (angle > DegreesVisibility / 180f - 1) //if monster isn't facing player
                return m_CanSeePlayer = false;
        }

        //might want to offset monster position so they can see over low walls, etc.
        var position = EyesTransform.position;
        var ray = new Ray(position, Target.position - position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
        if (Physics.Raycast(ray, out var hitInfo, 100,
            LayerMask.GetMask("Player", "Default", "Interactable", "Hazard", "Destructible")))
        {
            // Debug.Log(hitInfo.transform.name);
            m_CanSeePlayer = hitInfo.transform == TargetCollider;
            if (m_CanSeePlayer)
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green, 20);
                LastSeenPosition = hitInfo.transform.position;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.grey, 20);
            }

            return m_CanSeePlayer;
        }


        m_CanSeePlayer = false;

        return m_CanSeePlayer;
    }
}