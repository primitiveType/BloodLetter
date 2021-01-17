using UnityEngine;

public class InvulnPickup : Pickup<PlayerRoot>
{
    [SerializeField] private DamageType damageToIgnore;
    [SerializeField] private string toastString;
    protected override string toastMessage => toastString;

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        var invuln = currentActor.Armor.gameObject.AddComponent<Invulnerable>();
        invuln.DamageToIgnore = damageToIgnore;
    }
}