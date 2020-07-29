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

    [SerializeField] private EnemyAggroHandler AggroHandler;

    [SerializeField] private ActorEvents m_Events;
    public ActorEvents Events => m_Events;


    [SerializeField] private Transform Target;
    [SerializeField] private bool isFlying;
    private Animator Animator { get; set; }

    private ActorHealth m_Health;

    public ActorHealth Health => m_Health != null ? m_Health : m_Health = GetComponent<ActorHealth>();

    private bool IsAggro => AggroHandler.IsAggro && Health.IsAlive;


    private void Start()
    {
        Events.OnAggroEvent += OnEnemyAggro;
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnAttackEvent += OnEnemyAttack;
        Events.OnDeathEvent += OnEnemyDeath;
        Animator = GetComponentInChildren<Animator>();
        Target = Toolbox.Instance.PlayerTransform;
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }
        Toolbox.Instance.AddEnemy(Health);
        
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;
        }
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
    }


    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
    }


    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
    }

    private void Update()
    {
        if (Agent && Target)
        {
            Agent.SetDestination(Target.position);
            if (isFlying)
            {
                var transform1 = transform;

                float prevY = transform1.position.y;
                Agent.baseOffset += prevY - Target.position.y;
                //transform1.position = new Vector3(transform1.position.x, Target.position.y, transform1.position.z);
                Agent.Raycast(transform1.position, out NavMeshHit hit);
                Agent.baseOffset = hit.distance;
            }
        }

        Agent.isStopped = !IsAggro;


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