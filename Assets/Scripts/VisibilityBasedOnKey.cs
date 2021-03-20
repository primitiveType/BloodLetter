using UnityEngine;
using UnityEngine.UI;

public class VisibilityBasedOnKey : MonoBehaviour
{
    [SerializeField] private KeyType Key;
    [SerializeField] private Image Image;

    private void Start()
    {
        Toolbox.Instance.PlayerEvents.OnKeysChangedEvent += PlayerEventsOnOnKeysChangedEvent;
        UpdateVisibility();
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerEvents.OnKeysChangedEvent -= PlayerEventsOnOnKeysChangedEvent;
    }

    private void PlayerEventsOnOnKeysChangedEvent(object sender, OnKeysChangedEventArgs args)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        Image.enabled = Toolbox.Instance.PlayerInventory.HasKey(Key);
    }
}