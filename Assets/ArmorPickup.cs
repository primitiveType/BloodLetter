using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : Pickup<ActorArmor>
{
    public float ArmorAmount;

    protected override string toastMessage => $"Picked up {ArmorAmount} Armor.";
    protected override bool CanBePickedUp()
    {//check for max armor
        return !currentActor.IsFull();
    }

    protected override void PickupItem()
    {
        currentActor.GainArmor(ArmorAmount);
    }
}
