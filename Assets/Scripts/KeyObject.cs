using UnityEngine;

public class KeyObject : Pickup<PlayerInventory>
{
    [SerializeField] private KeyType KeyType;
    protected override string toastMessage => $"Picked up the {KeyType} Key!";

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        currentActor.AddKey(new Key(KeyType));
    }
}