using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevatorButton : BaseInteractable, IInteractable
{
    [SerializeField] private InteractableKey key;
    //[SerializeField] private List<ThreeStageElevator> Elevators;

    [SerializeField] private ElevatorState state;

    protected override bool DoInteraction()
    {
        ThreeStageElevatorIdService.Instance.TriggerAll(key, state);

        return true;
    }
}

public enum InteractableKey
{
    Blue,
    Yellow,
    Red,
    Hazard,
    Plywood
}


public enum ElevatorState
{
    Start,
    Middle,
    End
}