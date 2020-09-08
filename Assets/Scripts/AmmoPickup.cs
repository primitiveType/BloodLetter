using UnityEngine;

public class AmmoPickup : Pickup<PlayerInventory>
{
    [SerializeField] private int Amount;
    [SerializeField] private AmmoType Type;
    protected override string toastMessage => $"Picked up {Amount} {Type.ToString()}";

    protected override bool CanBePickedUp()
    {
        return currentActor.GetAmmoAmount(Type) < currentActor.GetMaxAmmoAmount(Type);
    }

    protected override void PickupItem()
    {
        currentActor.GainAmmo(Type, Amount);
    }
}