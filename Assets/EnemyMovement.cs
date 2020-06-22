using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private float AggroRange;
    [SerializeField] private float EarshotAggroRange = 20;

    [SerializeField] private AttackData Attack;
    [SerializeField] private float AttackCooldown;
    [SerializeField] private float DeAggroRange;

    private float LastAttackTimeStamp;
    [SerializeField] private EnemyEvents m_Events;

    [SerializeField] private float m_Health;
    private bool m_isAggro;

    [SerializeField] private Transform Target;
    private Animator Animator { get; set; }

    private bool IsAggro
    {
        get => m_isAggro && IsAlive;
        set
        {
            var wasAggro = m_isAggro;
            m_isAggro = value && IsAlive;
            if (!wasAggro && m_isAggro)
            {
                Events.OnAggro();
            }

                Debug.Log($"Aggro {IsAggro}");
        }
    }

    public bool IsAlive { get; set; } = true;


    public float Health
    {
        get => m_Health;
        set
        {
            m_Health = value;
            if (m_Health <= 0 && IsAlive) Die();
        }
    }

    public EnemyEvents Events => m_Events;


    // Start is called before the first frame update
    private void Start()
    {
        Toolbox.PlayerEvents.PlayerShootEvent += OnPlayerShoot;
        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnAttackEvent += OnEnemyAttack;
        Animator = GetComponentInChildren<Animator>();
        Target = Toolbox.PlayerTransform;
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
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

    private void Update()
    {
        if (Agent && Target) Agent.SetDestination(Target.position);

        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance < AggroRange) IsAggro = true;

            if (IsAggro && Agent.remainingDistance > DeAggroRange) IsAggro = false;
        }

        Agent.isStopped = !IsAggro;

        if (Time.time - LastAttackTimeStamp > AttackCooldown) TryAttack();

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
        else
        {
            Animator.SetBool(Attacking, false);
        }
    }


    private void UpdateAnimationStates()
    {
        if (Agent.velocity.sqrMagnitude > 2f) //TODO: base this on something
            Animator.SetBool(Moving, true);
        else
            Animator.SetBool(Moving, false);

        Animator.SetBool(IsDead, !IsAlive); //kinda dumb rofl
    }

    private void OnDestroy()
    {
        Events.OnAggroEvent -= OnEnemyAggro;
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnAttackEvent -= OnEnemyAttack;
        Toolbox.PlayerEvents.PlayerShootEvent -= OnPlayerShoot;
    }
    private void Die()
    {
        IsAlive = false;
        Events.OnDeath();
        IsAggro = false;
        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;   
        }
    }

}

[Serializable]
public class AttackData //first pass
{
    private Collider[] hitResults = new Collider[10];
    public float Radius;
    public float Range;

    public void DoAttack(Transform target)
    {
        int size = Physics.OverlapSphereNonAlloc(target.position, Radius, hitResults, LayerMask.GetMask("Player"),
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < size; i++)
        {
            Debug.Log($"Hit {hitResults[i]}");
            
        }
    }
}