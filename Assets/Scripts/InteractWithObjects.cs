using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : BaseInteractable
{
    [SerializeField] private List<GameObject> Interactables;

    protected override bool DoInteraction()
    {
        foreach (var go in Interactables)
        {
            foreach (var interactable in go.GetComponentsInChildren<IInteractable>())
            {
                interactable.Interact();
            }
        }

        return true;
    }
}
