using System;
using UnityEngine;

[Serializable]
public class PlayerInventoryData
{
    [SerializeField] public AmmoDictionary Ammo;
    [SerializeField] public WeaponId EquippedWeapon;
    [SerializeField] public WeaponId Weapons;

    public PlayerInventoryData(PlayerInventoryData toCopy)
    {
        EquippedWeapon = toCopy.EquippedWeapon;
        Weapons = toCopy.Weapons;
        Ammo = new AmmoDictionary();
        foreach (var kvp in toCopy.Ammo) Ammo.Add(kvp.Key, kvp.Value);
    }

    public PlayerInventoryData()
    {
        
    }
}