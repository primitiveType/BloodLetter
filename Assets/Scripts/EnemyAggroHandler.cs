using UnityEngine;

public class EnemyAggroHandler : MonoBehaviour
{
    [SerializeField] private MonsterVisibilityHandler VisibilityHandler;
    private bool m_isAggro;
    private static readonly int Aggro = Animator.StringToHash("IsAggro");
    [SerializeField] private ActorEvents m_Events;
    public ActorEvents Events => m_Events;

    private Animator Animator
    {
        get => m_animator;
        set => m_animator = value;
    }

    [SerializeField] private float AggroRange;
    [SerializeField] private float EarshotAggroRange = 20;
    private float m_distance;
    [SerializeField] private Animator m_animator;

    public bool IsAggro
    {
        get => m_isAggro;
        set
        {
            bool prevAggro = IsAggro;
            m_isAggro = value;
            Animator.SetBool(Aggro, IsAggro);
            if (IsAggro && !prevAggro) //this event should take args or there should be a deaggro event
            {
                Events.OnAggro();
            }
        }
    }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs args)
    {
        if (Vector3.Distance(Target.position, transform.position) > EarshotAggroRange)
        {
            return;
        }

        if (VisibilityHandler.CanSeePlayer())
        {
            IsAggro = true;
        }
    }

    private void Start()
    {
        Toolbox.Instance.PlayerEvents.PlayerShootEvent += OnPlayerShoot;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnDeathEvent += OnEnemyDeath;

        Target = Toolbox.Instance.PlayerTransform;
    }

    private void Update()
    {
        m_distance = Vector3.Distance(Target.transform.position, transform.position);
        if (m_distance < AggroRange) IsAggro = true;
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        IsAggro = false;
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true; //do we need to check if alive?
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnDeathEvent -= OnEnemyDeath;
    }


    public Transform Target { get; set; }
}