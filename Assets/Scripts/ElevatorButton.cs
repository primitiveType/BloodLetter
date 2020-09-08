using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Elevator Elevator;

    public virtual bool Interact()
    {
        Elevator.Trigger();
        return true;
        //play button sounds
    }
}