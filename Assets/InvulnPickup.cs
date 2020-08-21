using System;
using System.Collections;
using System.Collections.Generic;
using CodingEssentials;

public class InvulnPickup : Pickup<PlayerRoot>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override string toastMessage => "Pickup up limited Invulnerability!";
    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        currentActor.Armor.gameObject.AddComponent<Invulnerable>();
    }
}