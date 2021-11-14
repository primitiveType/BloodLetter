﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private PlayerInventoryData _inventoryData;
    [SerializeField] private float _playerArmor;

    [SerializeField] private float _playerHealth;
    [SerializeField] private List<string> m_BeatenLevels;

    public SaveData()
    {
    }

    public SaveData(SaveData newGameSaveData)
    {
        _playerArmor = newGameSaveData._playerArmor;
        _playerHealth = newGameSaveData._playerHealth;
        BeatenLevels = newGameSaveData.BeatenLevels;
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

    public List<string> BeatenLevels
    {
        get => m_BeatenLevels;
        set => m_BeatenLevels = value;
    }
}