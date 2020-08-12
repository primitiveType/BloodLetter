using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevatorButton : BaseInteractable, IInteractable
{
    [SerializeField] private List<ThreeStageElevator> Elevators;

    [SerializeField] private ElevatorState state;

    protected override void DoInteraction()
    {
        foreach (var elevator in Elevators)
        {
            elevator.MoveTo(state);
        }
    }
}


public enum ElevatorState
{
    Start,
    Middle,
    End
}