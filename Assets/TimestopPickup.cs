using UnityEngine;

public class TimestopPickup : Pickup<PlayerInventory>
{
    protected override string toastMessage => "Picked up Time Stop Potion!";

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        Toolbox.Instance.TimestopTimeStamp = Time.time;
    }
}