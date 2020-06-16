using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Animator Animator { get; set; }

    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private Transform Target;

    private bool IsAggro { get; set; }

    [SerializeField] private float AggroRange;
    [SerializeField] private float DeAggroRange;
    //TODO:move animation state constants into new file?
    private static readonly int Moving = Animator.StringToHash("Moving");

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        if (Agent && Target)
        {
            Agent.isStopped = true;
            Agent.SetDestination(Target.position);
            Agent.updateRotation = true;
        }
    }

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

        if (IsAggro)
        {
            Agent.isStopped = false;
        }

        UpdateAnimationStates();
    }


    private void UpdateAnimationStates()
    {
        if (Agent.velocity.sqrMagnitude > 2f)//TODO: base this on something
        {
            Animator.SetBool(Moving, true);
        }
        else
        {
            Animator.SetBool(Moving, false);
        }
    }
}