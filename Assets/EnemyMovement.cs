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

    public bool IsAlive { get; set; } = true;

    [SerializeField] private float m_Health;


    public float Health
    {
        get => m_Health;
        set
        {
            m_Health = value;
            if (m_Health <= 0 && IsAlive)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        IsAlive = false;
        Events.OnDeath();
        IsAggro = false;
    }

    [SerializeField] private float AggroRange;
    [SerializeField] private float DeAggroRange;
    [SerializeField] private float AttackCooldown;

    [SerializeField] private AttackData Attack;

    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    [SerializeField] private EnemyEvents m_Events;
    private bool m_isAggro;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    // Start is called before the first frame update
    void Start()
    {
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnAttackEvent += OnEnemyAttack;
        Animator = GetComponentInChildren<Animator>();
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        Attack.DoAttack(Target);
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true;
        Health -= args.ProjectileInfo.Damage;
    }

    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
    }

    public EnemyEvents Events => m_Events;

    private float LastAttackTimeStamp;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

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

        if (Time.time - LastAttackTimeStamp > AttackCooldown)
        {
            TryAttack();
        }

        UpdateAnimationStates();
    }

    private void TryAttack()
    {
        if (Attack.Range >= Vector3.Distance(transform.position, Target.position))
            //should probably also raycast for line of sight , move this code into the Attack class
        {
            //Attack.DoAttack(Target);
            Animator.SetBool(Attacking, true);
            LastAttackTimeStamp = Time.time;
        }
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

        Animator.SetBool(IsDead, !IsAlive); //kinda dumb rofl
    }

    private void OnDestroy()
    {
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}

[Serializable]
public class AttackData //first pass
{
    public float Range;
    public float Radius;

    private Collider[] hitResults = new Collider[10];

    public void DoAttack(Transform target)
    {
        var size = Physics.OverlapSphereNonAlloc(target.position, Radius, hitResults, LayerMask.GetMask("Player"),
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < size; i++)
        {
            Debug.Log($"Hit {hitResults[i]}");
        }
    }
}