using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType RequiredKeys;

    public bool Interact()
    {
        if (!Toolbox.Instance.PlayerInventory.HasKey(RequiredKeys))
        {
            ToastHandler.Instance.PopToast($"{RequiredKeys} Key Required!");
            return false;
        }
        DoInteraction();
        return DoInteraction();
    }

    protected abstract bool DoInteraction();
}