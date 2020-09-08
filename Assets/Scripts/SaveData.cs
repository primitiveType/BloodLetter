using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private PlayerInventoryData _inventoryData;
    [SerializeField] private float _playerArmor;

    [SerializeField] private float _playerHealth;

    public SaveData(SaveData newGameSaveData)
    {
        _playerArmor = newGameSaveData._playerArmor;
        _playerHealth = newGameSaveData._playerHealth;
        InventoryData = new PlayerInventoryData(newGameSaveData.InventoryData);
    }

    public PlayerInventoryData InventoryData
    {
        get => _inventoryData;
        set => _inventoryData = value;
    }

    public float PlayerArmor
    {
        get => _playerArmor;
        set => _playerArmor = value;
    }

    public float PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }
}