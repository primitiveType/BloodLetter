﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentWrapper : MonoBehaviour, INavigationAgent
{
    private NavMeshAgent NavMeshAgent;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
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
}