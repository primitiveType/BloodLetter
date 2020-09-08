using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType RequiredKeys;

    public bool Interact()
    {
        Debug.Log($"Getting player inventory on toolbox {Toolbox.Instance.GetInstanceID()}");
        if (!Toolbox.Instance.PlayerInventory.HasKey(RequiredKeys))
        {
            ToastHandler.Instance.PopToast($"{RequiredKeys} Key Required!");
            return false;
        }

        return DoInteraction();
    }

    protected abstract bool DoInteraction();
}