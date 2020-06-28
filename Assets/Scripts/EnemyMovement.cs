using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private float AggroRange;
    [SerializeField] private float EarshotAggroRange = 20;

    [SerializeField] private AttackData Attack;
    [SerializeField] private float DeAggroRange;

    [SerializeField] private ActorEvents m_Events;

    private bool m_isAggro;

    [SerializeField] private Transform Target;
    private Animator Animator { get; set; }

    private ActorHealth m_Health;

    public ActorHealth Health => m_Health != null ? m_Health : m_Health = GetComponent<ActorHealth>();

    private bool IsAggro
    {
        get => m_isAggro && Health.IsAlive;
        set
        {
            var wasAggro = m_isAggro;
            m_isAggro = value && Health.IsAlive;
            if (!wasAggro && m_isAggro)
            {
                Events.OnAggro();
            }
        }
    }


    public ActorEvents Events => m_Events;


    private void Start()
    {
        Toolbox.PlayerEvents.PlayerShootEvent += OnPlayerShoot;
        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnAttackEvent += OnEnemyAttack;
        Events.OnDeathEvent += OnEnemyDeath;
        Animator = GetComponentInChildren<Animator>();
        Target = Toolbox.PlayerTransform;
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        IsAggro = false;
        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;
        }
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
    }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs args)
    {
        if (Vector3.Distance(Target.position, transform.position) < EarshotAggroRange)
        {
            IsAggro = true;
        }
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true;
        // Health -= args.ProjectileInfo.Damage;
    }

    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
    }

    private void Update()
    {
        if (Agent && Target) Agent.SetDestination(Target.position);

        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance < AggroRange) IsAggro = true;

            if (IsAggro && Agent.remainingDistance > DeAggroRange) IsAggro = false;
        }

        Agent.isStopped = !IsAggro;


        UpdateAnimationStates();
    }


    // private void TryMeleeAttack()
    // {
    //     if (Attack.Range >= Vector3.Distance(transform.position, Target.position))
    //         //should probably also raycast for line of sight , move this code into the Attack class
    //     {
    //         //Attack.DoAttack(Target);
    //         Animator.SetBool(Attacking, true);
    //         LastAttackTimeStamp = Time.time;
    //     }
    //     else
    //     {
    //         Animator.SetBool(Attacking, false);
    //     }
    // }


    private void UpdateAnimationStates()
    {
        if (Agent.velocity.sqrMagnitude > 2f) //TODO: base this on something
            Animator.SetBool(Moving, true);
        else
            Animator.SetBool(Moving, false);

        Animator.SetBool(IsDead, !Health.IsAlive); //kinda dumb rofl
    }

    private void OnDestroy()
    {
        Events.OnAggroEvent -= OnEnemyAggro;
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnAttackEvent -= OnEnemyAttack;
        Toolbox.PlayerEvents.PlayerShootEvent -= OnPlayerShoot;
    }
}