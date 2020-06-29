using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup<ActorHealth>
{
    [SerializeField] private float HealAmount;

    protected override bool CanBePickedUp()
    {
        return !currentActor.IsFullHealth && currentActor.IsAlive;
    }

    protected override void PickupItem()
    {
        currentActor.Heal(HealAmount);
    }
}