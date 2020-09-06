using UnityEngine;

public class WeaponPickup : Pickup<PlayerInventory>
{
    [SerializeField] private WeaponId WeaponId;
    protected override string toastMessage => $"Picked up the {GetWeaponName(WeaponId)} !";

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        currentActor.GetWeapon(WeaponId);
    }

    private string GetWeaponName(WeaponId id)
    {
        if (id == WeaponId.Staff)
        {
            return "Taipala Staff";
        }

        return WeaponId.ToString();
    }
}