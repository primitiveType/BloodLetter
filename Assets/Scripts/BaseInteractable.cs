using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType RequiredKeys;

    public void Interact()
    {
        if (!Toolbox.Instance.PlayerInventory.HasKey(RequiredKeys))
        {
            ToastHandler.Instance.PopToast($"{RequiredKeys} Key Required!");
            return;
        }
        DoInteraction();
    }

    protected abstract void DoInteraction();
}