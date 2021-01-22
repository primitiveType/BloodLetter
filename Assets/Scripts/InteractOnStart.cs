using UnityEngine;

public class InteractOnStart : MonoBehaviour
{
    private void Start()
    {
        foreach (var interactable in GetComponents<IInteractable>()) interactable.Interact();
    }
}