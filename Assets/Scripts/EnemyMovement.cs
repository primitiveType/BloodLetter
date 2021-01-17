using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    [SerializeField] private Animator _animator;

    [SerializeField] private INavigationAgent Agent;

    [SerializeField] private EnemyAggroHandler AggroHandler;

    private ActorHealth m_Health;


    [SerializeField] private Transform Target;

    public IActorEvents Events => ActorRoot.ActorEvents;
    private ActorRoot ActorRoot { get; set; }

    private Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public ActorHealth Health => m_Health != null ? m_Health : m_Health = GetComponent<ActorHealth>();

    private bool IsAggro => AggroHandler.IsAggro && Health.IsAlive;

    private bool IsAttacking => ActorRoot.Attack != null && ActorRoot.Attack.IsAttacking;

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Agent = ActorRoot.Navigation;
        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnDeathEvent += OnEnemyDeath;
        if (Animator == null)
            Animator = GetComponentInChildren<Animator>();
        Target = Toolbox.Instance.PlayerTransform;
        if (Agent != null && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }

        Toolbox.Instance.AddEnemy(Health);
        StartCoroutine(UpdateAnimationStates());
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

    private void Update()
    {
        if (Agent != null && Target)
        {
            Agent.SetDestination(Target.position);

            Agent.isStopped = ShouldStop;
        }
    }

    public bool ShouldStop => !IsAggro || IsAttacking || IsFlinching;

    private bool IsFlinching => ActorRoot.Flinch != null && ActorRoot.Flinch.IsFlinching;
    
    private bool AnimatorIsMoving { get; set; }
    private float DelayBetweenUpdatingMovementState = .1f;

    private IEnumerator UpdateAnimationStates()
    {
        while (true)
        {
            if (!Agent.isStopped ) //TODO: base this on something
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

    private void OnDestroy()
    {
        Events.OnAggroEvent -= OnEnemyAggro;
        Events.OnStepEvent -= OnEnemyStepped;
    }
}