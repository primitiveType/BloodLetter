﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;
    private List<IInventoryItem> Items { get; } = new List<IInventoryItem>();
    private EquipStatus CurrentEquip; //will have to make changes if you can later equip multiple things

    
    private PlayerInventoryData InventoryData { get; set; }

    private void Awake()
    {
        //InventoryData = SaveState.Instance.InventoryData;
        Toolbox.Instance.SetPlayerInventory(this);
        InventoryData = new PlayerInventoryData(SaveState.Instance.SaveData.InventoryData);
        LevelManager.Instance.LevelEnd += InstanceOnLevelEnd;
    }

    private void InstanceOnLevelEnd(object sender, LevelEndEventArgs args) 
    {
        if (args.Success)
        {
            SaveState.Instance.SaveData.InventoryData = InventoryData;
        }
    }

    private void OnDestroy()
    {
        LevelManager.Instance.LevelEnd -= InstanceOnLevelEnd;
    }


    private KeyType currentKeys { get; set; }
    
    public List<IInventoryItem> GetItems()
    {
        return Items.ToList();
    }

    public void GetWeapon(WeaponId weapon)
    {
        var prevValue = InventoryData.Weapons;
        InventoryData.Weapons |= weapon;

        if (prevValue != InventoryData.Weapons)
        {
            Events.OnWeaponsChanged(prevValue, InventoryData.Weapons);
        }
    }

    public bool HasWeapon(WeaponId weapon)
    {
        return InventoryData.Weapons.HasFlag(weapon);
    }
    

    public void AddItem(IInventoryItem toAdd)
    {
        Items.Add(toAdd);
    }

    public void AddKey(IKey toAdd)
    {
        currentKeys |= toAdd.KeyType;
    }

    public bool HasKey(KeyType keyType)
    {
        return currentKeys.HasFlag(keyType);
    }

    public int GetAmmoAmount(AmmoType type)
    {
        InventoryData.Ammo.TryGetValue(type, out int ammo);
        return ammo;
    }

    public void UseAmmo(AmmoType type, int amount)
    {
        ChangeAmmoAmount(type, -amount);
    }

    public void GainAmmo(AmmoType type, int amount)
    {
        ChangeAmmoAmount(type, amount);
    }

    private void ChangeAmmoAmount(AmmoType type, int amount)
    {
        if (!InventoryData.Ammo.TryGetValue(type, out int prevAmount))
        {
            InventoryData.Ammo.Add(type, 0);
        }
        InventoryData.Ammo[type] = Mathf.CeilToInt(Mathf.Clamp((float) InventoryData.Ammo[type] + amount, 0, (float) GetMaxAmmoAmount(type)));
        if (InventoryData.Ammo[type] != prevAmount)
        {
            Events.OnAmmoChanged(prevAmount, InventoryData.Ammo[type], type);
        }
    }

    public int GetMaxAmmoAmount(AmmoType ammoType)
    {
        return 100;
    }
    
    public void EquipThing(EquipStatus thing)
    {
        if (!equipping)
        {
            StartCoroutine(EquipThingCR(thing));
        }
    }

    private bool equipping;
    private IEnumerator EquipThingCR(EquipStatus thing)
    {
        
        if (CurrentEquip == thing || !thing.CanEquip())
        {
            yield break;
        }

        equipping = true;
        var prev = CurrentEquip != null ? CurrentEquip.WeaponId : 0;
        
        if (CurrentEquip != null)
        {
            yield return StartCoroutine(CurrentEquip.UnEquip());
        }

        Events.OnEquippedWeaponChanged(prev, thing.WeaponId);
        yield return StartCoroutine(thing.Equip());
        CurrentEquip = thing;
        InventoryData.EquippedWeapon = CurrentEquip.WeaponId;
        equipping = false;
    }
}

[Serializable]
public class PlayerInventoryData
{
    [SerializeField] public WeaponId EquippedWeapon;
    [SerializeField] public WeaponId Weapons;
    [SerializeField] public AmmoDictionary Ammo;

    public PlayerInventoryData(PlayerInventoryData toCopy)
    {
        EquippedWeapon = toCopy.EquippedWeapon;
        Weapons = toCopy.Weapons;
        Ammo = new AmmoDictionary();
        foreach (var kvp in toCopy.Ammo)
        {
            Ammo.Add(kvp.Key, kvp.Value);
        }
    }
}

[Serializable]
public enum AmmoType
{
    Lead,
    Mana
}

public interface IInventoryItem
{
}

public interface IKey : IInventoryItem
{
    KeyType KeyType { get; }
}

[Flags]
public enum KeyType
{
    None = 0x0000,
    Blue = 0x0001,
    Red = 0x0010,
    Yellow = 0x0100
}