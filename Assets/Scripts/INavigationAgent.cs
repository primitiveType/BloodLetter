using UnityEngine;

public interface INavigationAgent
{
    bool isStopped { get; set; }
    bool updateRotation { get; set; }
    Vector3 velocity { get; }
    bool IsGrounded { get; }
    void SetDestination(Vector3 targetPosition);
    void Warp(Vector3 position);
}