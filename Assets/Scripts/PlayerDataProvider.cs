using System;
using UnityEngine;

[Serializable]
public class PlayerDataProvider : MonoBehaviour, IHealthDataProvider
{
    [SerializeField] private PlayerData m_Data;

    public bool Populated { get; set; }

    public PlayerData Data
    {
        get
        {
            if (!Populated && Application.isPlaying)
            {
                Populated = true;
                Data = GameConstants.GetPlayerData("Normal");
            }

            return m_Data;
        }
        set => m_Data = value;
    }

    public int MaxHealth => Data.MaxHealth;

    public int StartHealth => Data.StartHealth;

    public int MaxArmor => Data.MaxArmor;

    public int StartArmor => Data.StartArmor;
    public float OverhealMaxHealth => Data.OverhealMaxHealth;

    public float OverhealMaxArmor => Data.OverhealMaxArmor;
}