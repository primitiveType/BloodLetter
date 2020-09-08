using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");

    [SerializeField] private INavigationAgent Agent;

    [SerializeField] private EnemyAggroHandler AggroHandler;

    public IActorEvents Events => ActorRoot.ActorEvents;


    [SerializeField] private Transform Target;
    private ActorRoot ActorRoot { get; set; }

    private Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    private ActorHealth m_Health;
    [SerializeField] private Animator _animator;

    public ActorHealth Health => m_Health != null ? m_Health : m_Health = GetComponent<ActorHealth>();

    private bool IsAggro => AggroHandler.IsAggro && Health.IsAlive;


    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Agent = ActorRoot.Navigation;

        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnAttackEvent += OnEnemyAttack;
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
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
    }


    private bool isAttacking;
    private float attackTime = .5f;
    private Coroutine AttackRoutine;

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        if (AttackRoutine != null)
        {
            StopCoroutine(AttackRoutine);
        }

        AttackRoutine = StartCoroutine(AttackTimerCR());
    }

    private IEnumerator AttackTimerCR()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
    }


    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
    }

    private void Update()
    {
        if (Agent != null && Target)
        {
            Agent.SetDestination(Target.position);

            Agent.isStopped = !IsAggro || isAttacking;
        }


        UpdateAnimationStates();
    }

    private void UpdateAnimationStates()
    {
        if (Agent.velocity.sqrMagnitude > 2f) //TODO: base this on something
            Animator.SetBool(Moving, true);
        else
            Animator.SetBool(Moving, false);
    }

    private void OnDestroy()
    {
        Events.OnAggroEvent -= OnEnemyAggro;
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}