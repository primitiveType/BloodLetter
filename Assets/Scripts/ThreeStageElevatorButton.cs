using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private ThreeStageElevator Elevator;

    [SerializeField] private ElevatorState state;

    public void Interact()
    {
        Elevator.MoveTo(state);
    }
}


public enum ElevatorState
{
    Start,
    Middle,
    End
}