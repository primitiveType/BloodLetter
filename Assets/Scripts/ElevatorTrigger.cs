using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private Elevator Elevator;

    private void OnTriggerEnter(Collider other)
    {
        Elevator.Trigger();
    }
}