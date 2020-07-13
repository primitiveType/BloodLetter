using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : Pickup<PlayerInventory>
{
    [SerializeField] private KeyType KeyType;
    protected override string toastMessage => $"Picked up the {KeyType} Key!";

    protected override bool CanBePickedUp()
    {
        return true;
    }

    protected override void PickupItem()
    {
        currentActor.AddKey(new Key(KeyType));
    }
}

public class Key : IKey
{
    public Key(KeyType keyType)
    {
        KeyType = keyType;
    }

    public KeyType KeyType { get; }
}