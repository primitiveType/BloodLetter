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
        if (!currentActor.IsAlive)
        {
            return false;
        }

        if (CanOverheal)
        {
            return !currentActor.IsFullOverHealth;
        }
        
        
        return !currentActor.IsFullHealth ;
    }

    protected override void PickupItem()
    {
        currentActor.Heal(HealAmount, CanOverheal);
    }
}