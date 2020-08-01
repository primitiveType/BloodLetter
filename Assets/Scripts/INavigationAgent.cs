using UnityEngine;

public interface INavigationAgent
{
    bool isStopped { get; set; }
    bool updateRotation { get; set; }
    Vector3 velocity { get; }
    void SetDestination(Vector3 targetPosition);
}