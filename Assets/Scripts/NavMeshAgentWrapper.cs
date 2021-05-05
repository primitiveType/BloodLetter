using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentWrapper : MonoBehaviour, INavigationAgent
{
    private NavMeshAgent m_NavMeshAgent;

    public bool IsGrounded
    {
        get => NavMeshAgent.isOnNavMesh;
    }

    public bool isStopped
    {
        get => NavMeshAgent.isStopped;
        set => NavMeshAgent.isStopped = value;
    }

    public bool updateRotation
    {
        get => NavMeshAgent.updateRotation;
        set => NavMeshAgent.updateRotation = value;
    }

    public Vector3 velocity
    {
        get => NavMeshAgent.velocity;
        set => NavMeshAgent.velocity = value;
    }

    public void SetDestination(Vector3 targetPosition)
    {
        NavMeshAgent.SetDestination(targetPosition);
    }

    public void Warp(Vector3 position)
    {
        NavMeshAgent.Warp(position);
    }

    public float MoveSpeed => NavMeshAgent.speed;
    public float StoppingDistance => NavMeshAgent.stoppingDistance;
    public float Acceleration => NavMeshAgent.acceleration;

    public NavMeshAgent NavMeshAgent =>
        m_NavMeshAgent != null ? m_NavMeshAgent : m_NavMeshAgent = GetComponent<NavMeshAgent>();

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
        }

        EnemyData data = dataProvider.Data;

        NavMeshAgent.acceleration = data.Acceleration;
        NavMeshAgent.speed = data.MoveSpeed;
        NavMeshAgent.stoppingDistance = data.StoppingDistance;
    }
}