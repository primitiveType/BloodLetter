using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ThreeStageElevator> Elevators;

    [SerializeField] private ElevatorState state;
    [SerializeField] private KeyType RequiredKeys;

    public void Interact()
    {
        if (!Toolbox.Instance.PlayerInventory.HasKey(RequiredKeys))
        {
            ToastHandler.Instance.PopToast($"{RequiredKeys} Key Required!");
            return;
        }
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