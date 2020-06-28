using UnityEngine;

public class AmmoPickup : Pickup<PlayerInventory>
{
    [SerializeField] private AmmoType Type;
    [SerializeField] private int Amount;
    protected override bool CanBePickedUp()
    {
        return currentActor.GetAmmoAmount(Type) < currentActor.GetMaxAmmoAmount(Type);
    }

    protected override void PickupItem()
    {
        currentActor.GainAmmo(Type, Amount);
    }
}