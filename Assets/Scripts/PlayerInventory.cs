using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private EquipStatus CurrentEquip; //will have to make changes if you can later equip multiple things

    private bool equipping;
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
        if (args.Success) SaveState.Instance.SaveData.InventoryData = InventoryData;
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
        currentKeys |= toAdd.KeyType;
    }

    public bool HasKey(KeyType keyType)
    {
        return currentKeys.HasFlag(keyType);
    }

    public float GetAmmoAmount(AmmoType type)
    {
        InventoryData.Ammo.TryGetValue(type, out var ammo);
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
        return 100;
    }

    public void EquipThing(EquipStatus thing)
    {
        if (!equipping) StartCoroutine(EquipThingCR(thing));
    }

    private IEnumerator EquipThingCR(EquipStatus thing)
    {
        if (CurrentEquip == thing || !thing.CanEquip()) yield break;

        equipping = true;
        var prev = CurrentEquip != null ? CurrentEquip.WeaponId : 0;

        if (CurrentEquip != null) yield return StartCoroutine(CurrentEquip.UnEquip());

        Events.OnEquippedWeaponChanged(prev, thing.WeaponId);
        yield return StartCoroutine(thing.Equip());
        CurrentEquip = thing;
        InventoryData.EquippedWeapon = CurrentEquip.WeaponId;
        equipping = false;
    }
}