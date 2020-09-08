using UnityEngine;

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