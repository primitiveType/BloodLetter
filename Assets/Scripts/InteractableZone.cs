using UnityEngine;

public class InteractableZone : PlayerTrigger
{
    private bool triggered;

    protected override void Trigger(Collider other)
    {
        triggered = true;
        foreach (var interactable in GetComponents<IInteractable>())
        {
            interactable.Interact();
        }
    }
}