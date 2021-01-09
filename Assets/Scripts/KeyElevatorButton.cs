using System;
using UnityEngine;

[ExecuteAlways]
public class KeyElevatorButton : ElevatorButton
{
    // [SerializeField] private KeyType KeyType;
    //
    // public override bool Interact()
    // {
    //     if (Toolbox.Instance.PlayerInventory.HasKey(KeyType)) return base.Interact();
    //
    //     return false;
    // }

    private void Awake()
    {
        throw new Exception("key elevator button is obsolete! replace it with elevator button.");
    }
}