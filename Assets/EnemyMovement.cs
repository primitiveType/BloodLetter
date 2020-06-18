using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Animator Animator { get; set; }

    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private Transform Target;

    private bool IsAggro
    {
        get => m_isAggro && IsAlive;
        set => m_isAggro = value && IsAlive;
    }

    public bool IsAlive => Health > 0;

    [SerializeField] private float m_Health;

    public float Health
    {
        get => m_Health;
        set
        {
            m_Health = value;
            if (m_Health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Events.OnDeath();
        Animator.SetBool(IsDead, true);
        IsAggro = false;
    }

    [SerializeField] private float AggroRange;
    [SerializeField] private float DeAggroRange;
    [SerializeField] private float AttackCooldown;

    [SerializeField] private List<AttackData> Attacks;

    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    [SerializeField] private EnemyEvents m_Events;
    private bool m_isAggro;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    // Start is called before the first frame update
    void Start()
    {
        Events.OnStepEvent += OnStepped;
        Events.OnShotEvent += OnShot;
        Events.OnAttackEvent += OnAttack;
        Animator = GetComponentInChildren<Animator>();
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }
    }

    private void OnAttack(object sender, OnAttackEventArgs args)
    {
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true;
        Health -= args.ProjectileInfo.Damage;
    }

    private void OnStepped(object sender, OnStepEventArgs args)
    {
    }

    public EnemyEvents Events => m_Events;

    void Update()
    {
        if (Agent && Target)
        {
            Agent.SetDestination(Target.position);
        }

        if (Agent.remainingDistance < AggroRange)
        {
            IsAggro = true;
        }

        if (IsAggro && Agent.remainingDistance > DeAggroRange)
        {
            IsAggro = false;
        }

        Agent.isStopped = !IsAggro;

        UpdateAnimationStates();
    }


    private void UpdateAnimationStates()
    {
        if (Agent.velocity.sqrMagnitude > 2f) //TODO: base this on something
        {
            Animator.SetBool(Moving, true);
        }
        else
        {
            Animator.SetBool(Moving, false);
        }
    }

    private void OnDestroy()
    {
        Events.OnStepEvent -= OnStepped;
        Events.OnShotEvent -= OnShot;
        Events.OnAttackEvent -= OnAttack;
    }
}

internal class AttackData //first pass
{
    public float Range;
    public float Radius;

    public void DoAttack()
    {
        // Physics.check
    }
}