using UnityEngine;

public class HealthPickup : Pickup<ActorHealth>
{
    [SerializeField] private bool CanOverheal;
    [SerializeField] private float HealAmount;

    protected override string toastMessage => $"Pickep up {HealAmount} Health";

    protected override bool CanBePickedUp()
    {
        if (!currentActor.IsAlive) return false;

        if (CanOverheal) return !currentActor.IsFullOverHealth;


        return !currentActor.IsFullHealth;
    }

    protected override void PickupItem()
    {
        currentActor.Heal(HealAmount, CanOverheal);
    }
}