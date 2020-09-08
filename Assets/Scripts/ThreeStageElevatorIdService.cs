using System.Collections.Generic;

public class ThreeStageElevatorIdService : MonoBehaviourSingleton<ThreeStageElevatorIdService>
{
    private readonly Dictionary<InteractableKey, List<ThreeStageElevator>> Elevators =
        new Dictionary<InteractableKey, List<ThreeStageElevator>>();

    private void Start()
    {
        LevelManager.Instance.LevelBegin += InstanceOnLevelBegin;
    }

    private void InstanceOnLevelBegin(object sender, LevelBeginEventArgs args)
    {
        Elevators.Clear();
    }

    public void RegisterInteractable(ThreeStageElevator interactbale, InteractableKey key)
    {
        if (!Elevators.ContainsKey(key)) Elevators.Add(key, new List<ThreeStageElevator>());

        Elevators[key].Add(interactbale);
    }

    public void TriggerAll(InteractableKey key, ElevatorState state)
    {
        if (!Elevators.ContainsKey(key)) return;

        foreach (var elevator in Elevators[key]) elevator.MoveTo(state);
    }
}