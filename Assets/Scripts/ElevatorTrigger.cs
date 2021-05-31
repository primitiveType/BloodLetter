using UnityEngine;
using UnityEngine.Serialization;

public class ElevatorTrigger : MonoBehaviour
{
    [FormerlySerializedAs("Elevator")] [SerializeField] private Elevator m_Elevator;

    public Elevator Elevator
    {
        get => m_Elevator;
        set => m_Elevator = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !other.isTrigger)
            m_Elevator.Trigger();
    }
}