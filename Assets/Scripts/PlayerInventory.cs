using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;
    private List<IInventoryItem> Items { get; } = new List<IInventoryItem>();

    private Dictionary<AmmoType, int> Ammo { get; } = new Dictionary<AmmoType, int>();

    [SerializeField] private WeaponId Weapons;

    private void Awake()
    {
        Toolbox.Instance.SetPlayerInventory(this);
        GainAmmo(AmmoType.Lead, 40);
    }

    private List<IKey> Keys { get; } = new List<IKey>();

    public List<IInventoryItem> GetItems()
    {
        return Items.ToList();
    }

    public void GetWeapon(WeaponId weapon)
    {
        var prevValue = Weapons;
        Weapons |= weapon;

        if (prevValue != Weapons)
        {
            Events.OnWeaponsChanged(prevValue, Weapons);
        }
    }

    public bool HasWeapon(WeaponId weapon)
    {
        return Weapons.HasFlag(weapon);
    }
    

    public void AddItem(IInventoryItem toAdd)
    {
        Items.Add(toAdd);
    }

    public void AddKey(IKey toAdd)
    {
        Keys.Add(toAdd);
    }

    public bool HasKey(KeyType keyType)
    {
        return Keys.Count(key => key.KeyType == keyType) > 0;
    }

    public int GetAmmoAmount(AmmoType type)
    {
        Ammo.TryGetValue(type, out int ammo);
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
        if (!Ammo.TryGetValue(type, out int prevAmount))
        {
            Ammo.Add(type, 0);
        }
        Ammo[type] = Mathf.CeilToInt(Mathf.Clamp((float) Ammo[type] + amount, 0, (float) GetMaxAmmoAmount(type)));
        if (Ammo[type] != prevAmount)
        {
            Events.OnAmmoChanged(prevAmount, Ammo[type], type);
        }
    }

    public int GetMaxAmmoAmount(AmmoType ammoType)
    {
        return 100;
    }
}

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

public enum KeyType
{
    Blue,
    Red,
    Yellow
}