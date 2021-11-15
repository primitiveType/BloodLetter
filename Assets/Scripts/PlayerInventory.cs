using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private class EquipInfo
    {
        public bool equipping;
        public EquipStatus current;
    }

    public enum EquipmentSlot
    {
        LeftHand,
        RightHand
    }

    private Dictionary<EquipmentSlot, EquipInfo> EquippedItems = new Dictionary<EquipmentSlot, EquipInfo>
    {
    };


    // private bool equipping;
    [SerializeField] private PlayerEvents Events;
    private List<IInventoryItem> Items { get; } = new List<IInventoryItem>();


    private PlayerInventoryData InventoryData { get; set; }


    private KeyType currentKeys { get; set; }

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
            if(!SaveState.Instance.SaveData.BeatenLevels.Contains(args.LevelName))
            {
                SaveState.Instance.SaveData.BeatenLevels.Add(args.LevelName);
            }

            SaveState.Instance.Save();
        }
    }

    private void OnDestroy()
    {
        LevelManager.Instance.LevelEnd -= InstanceOnLevelEnd;
    }

    public List<IInventoryItem> GetItems()
    {
        return Items.ToList();
    }

    public void GetWeapon(WeaponId weapon)
    {
        var prevValue = InventoryData.Weapons;
        InventoryData.Weapons |= weapon;

        if (prevValue != InventoryData.Weapons) Events.OnWeaponsChanged(prevValue, InventoryData.Weapons);
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
        var prevKeys = currentKeys;
        currentKeys |= toAdd.KeyType;
        Events.OnKeysChanged(prevKeys, currentKeys);
    }

    public bool HasKey(KeyType keyType)
    {
        return currentKeys.HasFlag(keyType);
    }

    public float GetAmmoAmount(AmmoType type)
    {
        InventoryData.Ammo.TryGetValue(type, out float ammo);
        return ammo;
    }

    public void UseAmmo(AmmoType type, float amount)
    {
        ChangeAmmoAmount(type, -amount);
    }

    public void GainAmmo(AmmoType type, int amount)
    {
        ChangeAmmoAmount(type, amount);
    }

    private void ChangeAmmoAmount(AmmoType type, float amount)
    {
        if (!InventoryData.Ammo.TryGetValue(type, out var prevAmount)) InventoryData.Ammo.Add(type, 0);
        InventoryData.Ammo[type] = Mathf.Clamp(InventoryData.Ammo[type] + amount, 0, GetMaxAmmoAmount(type));
        if (Math.Abs(InventoryData.Ammo[type] - prevAmount) > float.Epsilon)
            Events.OnAmmoChanged(prevAmount, InventoryData.Ammo[type], type);
    }

    public int GetMaxAmmoAmount(AmmoType ammoType)
    {
        switch (ammoType)
        {
            case AmmoType.Lead:
                return 10;
                break;
            case AmmoType.Blood:
                return 100;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ammoType), ammoType, null);
        }
    }

    public bool IsEquipped(EquipStatus thing, EquipmentSlot slot)
    {
        return EquippedItems[slot].current == thing;
    }

    public WeaponId CurrentEquip(EquipmentSlot slot)
    {
        if (!EquippedItems.ContainsKey(slot))
        {
            return 0;
        }

        if (EquippedItems[slot].current == null)
        {
            return 0;
        }

        return EquippedItems[slot].current.WeaponId;
    }

    public bool IsEquipped(WeaponId weaponId, EquipmentSlot slot)
    {
        if (!EquippedItems.ContainsKey(slot))
        {
            return false;
        }

        var equipped = EquippedItems[slot]?.current?.WeaponId.HasFlag(weaponId);
        return equipped ?? false;
    }

    public void EquipThing(EquipStatus thing, EquipmentSlot slot)
    {
        if (!EquippedItems.ContainsKey(slot))
        {
            EquippedItems.Add(slot, new EquipInfo());
        }

        if (!EquippedItems[slot].equipping)
        {
            StartCoroutine(EquipThingCR(thing, slot));
        }
    }

    private IEnumerator EquipThingCR(EquipStatus thing, EquipmentSlot slot)
    {
        if (EquippedItems[slot].current == thing || !thing.CanEquip()) yield break;

        EquippedItems[slot].equipping = true;
        var prev = EquippedItems[slot].current != null ? EquippedItems[slot].current.WeaponId : 0;

        if (EquippedItems[slot].current != null) yield return StartCoroutine(EquippedItems[slot].current.UnEquip());

        EquippedItems[slot].current = thing;
        // InventoryData.EquippedWeapon = EquippedItems[EquipmentSlot.LeftHand].current.WeaponId |
        //                                EquippedItems[EquipmentSlot.RightHand].current.WeaponId;
        Events.OnEquippedWeaponChanged(prev, thing.WeaponId, slot);
        // yield return StartCoroutine(thing.Equip());
        EquippedItems[slot].equipping = false;
    }

    public void GainBlood(int i)
    {
    }
}