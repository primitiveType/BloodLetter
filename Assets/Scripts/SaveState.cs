using System;
using UnityEngine;

public class SaveState : MonoBehaviourSingleton<SaveState>
{
    [SerializeField] private SaveData _saveData;
    [SerializeField] private SaveData _newGameSaveData;

    public SaveData SaveData => _saveData;

    public void StartNewGame()
    {
        _saveData = new SaveData(_newGameSaveData);
    }
}

[Serializable]
public class SaveData
{
    [SerializeField] private PlayerInventoryData _inventoryData;

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

    [SerializeField] private float _playerHealth;
    [SerializeField] private float _playerArmor;

    public SaveData(SaveData newGameSaveData)
    {
        _playerArmor = newGameSaveData._playerArmor;
        _playerHealth = newGameSaveData._playerHealth;
        InventoryData = new PlayerInventoryData(newGameSaveData.InventoryData);
    }
}