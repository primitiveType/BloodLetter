using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventory : MonoBehaviour
{
    private List<IInventoryItem> Items { get; } = new List<IInventoryItem>();


    private void Awake()
    {
        Toolbox.SetPlayerInventory(this);
    }

    private List<IKey> Keys { get; } = new List<IKey>();
    public List<IInventoryItem> GetItems()
    {
        return Items.ToList();
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
        return Keys.Count(key =>key.KeyType == keyType) > 0;
    }
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