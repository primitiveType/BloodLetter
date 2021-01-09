using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [FormerlySerializedAs("RequiredKeys")] [SerializeField] private KeyType m_RequiredKeys;
    
    public KeyType RequiredKeys
    {
        get => m_RequiredKeys;
        set => m_RequiredKeys = value;
    }

    public bool Interact()
    {
        Debug.Log($"Getting player inventory on toolbox {Toolbox.Instance.GetInstanceID()}");
        if (m_RequiredKeys != KeyType.None && !Toolbox.Instance.PlayerInventory.HasKey(m_RequiredKeys))
        {
            ToastHandler.Instance.PopToast($"{m_RequiredKeys} Key Required!");
            return false;
        }

        return DoInteraction();
    }

    protected abstract bool DoInteraction();
}