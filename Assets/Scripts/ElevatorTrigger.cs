using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private Elevator Elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer ==LayerMask.NameToLayer("Player"))
            Elevator.Trigger();
    }
}