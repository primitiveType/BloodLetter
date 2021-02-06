using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    [SerializeField] protected Animator _animator;

    [SerializeField] protected INavigationAgent Agent;

    [SerializeField] private EnemyAggroHandler AggroHandler;
    [SerializeField] private float TargetingVariance = 10;

    private ActorHealth m_Health;


    [SerializeField] private Transform Target;

    public IActorEvents Events => ActorRoot.ActorEvents;
    protected ActorRoot ActorRoot { get; set; }

    private Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public ActorHealth Health => m_Health != null ? m_Health : m_Health = GetComponent<ActorHealth>();

    private bool IsAggro => AggroHandler.IsAggro && Health.IsAlive;

    protected bool IsAttacking => ActorRoot.Attack != null && ActorRoot.Attack.IsAttacking;

    protected virtual void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Agent = ActorRoot.Navigation;
        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnDeathEvent += OnEnemyDeath;
        Events.OnAttackEvent += OnEnemyAttack;
        if (Animator == null)
            Animator = GetComponentInChildren<Animator>();
        Target = Toolbox.Instance.PlayerTransform;
        if (Agent != null && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(GetDestination());
            Agent.updateRotation = true;
        }

        Toolbox.Instance.AddEnemy(Health);
        StartCoroutine(UpdateAnimationStates());
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
    }


    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
    }


    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
    }

    protected virtual void Update()
    {
        if (Agent != null && Target)
        {
            Agent.SetDestination(GetDestination());

            Agent.isStopped = ShouldStop;
        }
    }

    private void Awake()
    {
        Seed = GetInstanceID();
    }

    private int Seed { get; set; }
    private Vector3 GetDestination()
    {
        float xOffset = (.5f - Mathf.PerlinNoise(Time.time,Seed)) * TargetingVariance;
        float zOffset = (.5f - Mathf.PerlinNoise(Seed, Time.time)) * TargetingVariance;
        return new Vector3(xOffset, 0, zOffset) + Target.position;
    }

    public virtual bool ShouldStop => !IsAggro || IsAttacking || IsFlinching;

    private bool IsFlinching => ActorRoot.Flinch != null && ActorRoot.Flinch.IsFlinching;

    private bool AnimatorIsMoving { get; set; }
    private float DelayBetweenUpdatingMovementState = .1f;

    protected virtual IEnumerator UpdateAnimationStates()
    {
        while (true)
        {
            if (!Agent.isStopped)
            {
                if (AnimatorIsMoving)
                {
                    yield return null;
                    continue;
                }

                AnimatorIsMoving = true;
                Animator.SetBool(Moving, AnimatorIsMoving);
                yield return new WaitForSeconds(DelayBetweenUpdatingMovementState);
            }
            else
            {
                if (!AnimatorIsMoving)
                {
                    yield return null;
                    continue;
                }

                AnimatorIsMoving = false;
                Animator.SetBool(Moving, AnimatorIsMoving);
                yield return new WaitForSeconds(DelayBetweenUpdatingMovementState);
            }

            yield return null;
        }
    }

    protected virtual void OnDestroy()
    {
        Events.OnAggroEvent -= OnEnemyAggro;
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnDeathEvent -= OnEnemyDeath;
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}