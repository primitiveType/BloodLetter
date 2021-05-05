using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(Seeker))]
public class SeekerAgentWrapper : MonoBehaviour, INavigationAgent
{
    public bool isStopped
    {
        get => Seeker.isStopped;
        set => Seeker.isStopped = value;
    }

    public bool updateRotation { get; set; }
    public Vector3 velocity => Seeker.velocity;
    public bool IsGrounded { get; } = true;

    public void SetDestination(Vector3 targetPosition)
    {
        Seeker.destination = targetPosition;
    }

    public void Warp(Vector3 position)
    {
        Seeker.Teleport(position);
    }

    public float MoveSpeed
    {
        get => Seeker.maxSpeed;
        private set => Seeker.maxSpeed = value;
    }

    public float StoppingDistance
    {
        get => Seeker.endReachedDistance;
        private set => Seeker.endReachedDistance = value;
    }

    public float Acceleration
    {
        get => Seeker.maxAcceleration;
        set => Seeker.maxAcceleration = value;
    }

    private AIPath Seeker => m_Seeker != null ? m_Seeker : m_Seeker = GetComponent<AIPath>();

    private AIPath m_Seeker;

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
        }

        EnemyData data = dataProvider.Data;

        Acceleration = data.Acceleration;
        MoveSpeed = data.MoveSpeed;
        StoppingDistance = data.StoppingDistance;
    }
}