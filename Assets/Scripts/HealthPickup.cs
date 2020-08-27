using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup<ActorHealth>
{
    [SerializeField] private float HealAmount;
    [SerializeField] private bool CanOverheal;

    protected override string toastMessage => $"Pickep up {HealAmount} Health";

    protected override bool CanBePickedUp()
    {
        return !currentActor.IsFullHealth && currentActor.IsAlive;
    }

    protected override void PickupItem()
    {
        currentActor.Heal(HealAmount, CanOverheal);
    }
}