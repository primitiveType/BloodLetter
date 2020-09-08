using System.Collections;
using UnityEngine;

public class EnemyAggroHandler : MonoBehaviour
{
    private static readonly int Aggro = Animator.StringToHash("IsAggro");

    [SerializeField] private float AggroDelayVariance = 1f;

    [SerializeField] private float AggroRange;
    [SerializeField] private float EarshotAggroRange = 20;


    private bool isInitialized;
    private float m_distance;
    private bool m_isAggro;
    [SerializeField] private MonsterVisibilityHandler VisibilityHandler;
    public IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot { get; set; }
    private Animator Animator => ActorRoot.Animator;

    public bool IsAggro
    {
        get => m_isAggro;
        set
        {
            Initialize();
            var prevAggro = IsAggro;
            m_isAggro = value;
            Animator.SetBool(Aggro, IsAggro);
            if (IsAggro && !prevAggro) //this event should take args or there should be a deaggro event
                Events.OnAggro();

            PreAggro = value; //if un-aggro'd, reset this
        }
    }

    private bool PreAggro { get; set; }


    public Transform Target { get; set; }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs args)
    {
        if (Vector3.Distance(Target.position, transform.position) > EarshotAggroRange) return;

        if (VisibilityHandler.CanSeePlayer(true, true)) SetAggro();
    }

    private void SetAggro()
    {
        if (!PreAggro)
        {
            PreAggro = true;
            StartCoroutine(AggroAfterRandomDelay());
        }
    }

    private IEnumerator AggroAfterRandomDelay()
    {
        yield return new WaitForSeconds(Random.Range(0, AggroDelayVariance));
        IsAggro = true;
    }

    private void Initialize()
    {
        if (isInitialized) return;

        isInitialized = true;

        ActorRoot = GetComponentInParent<ActorRoot>();
        Toolbox.Instance.PlayerEvents.PlayerShootEvent += OnPlayerShoot;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnDeathEvent += OnEnemyDeath;

        Target = Toolbox.Instance.PlayerTransform;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        m_distance = Vector3.Distance(Target.transform.position, transform.position);
        if (m_distance < AggroRange && VisibilityHandler.CanSeePlayer()) SetAggro();
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        IsAggro = false;
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true; //do we need to check if alive?
        VisibilityHandler.LastSeenPosition =
            Toolbox.Instance.PlayerHeadTransform.position; //hack, should be based on who shot them. 
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnDeathEvent -= OnEnemyDeath;
    }
}