using UnityEngine;

public class WeaponPickup : Pickup<PlayerInventory>
{
    [SerializeField] private WeaponId WeaponId;
    protected override string toastMessage => $"Picked up the {WeaponId.ToString()} !";

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        currentActor.GetWeapon(WeaponId);
    }
}