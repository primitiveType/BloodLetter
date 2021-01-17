using UnityEngine;

public class InteractOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject Interactable;
    private void OnDestroy()
    {
        foreach (var interactable in Interactable.GetComponents<IInteractable>()) interactable.Interact();

    }
}