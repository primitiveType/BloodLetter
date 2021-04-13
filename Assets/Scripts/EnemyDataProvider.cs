using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyDataProvider : MonoBehaviour, IHealthDataProvider
{
    [SerializeField] private string m_EnemyName;
    [SerializeField] private EnemyData m_Data;

    public string EnemyName => m_EnemyName;
    public bool Populated { get; set; }

    public EnemyData Data
    {
        get
        {
            if (!Populated && Application.isPlaying)
            {
                Populated = true;
                Data = GameConstants.GetEnemyDataByName(m_EnemyName, "THis does nothing right now.");
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

public interface IHealthDataProvider
{
    int MaxHealth { get; }


    int StartHealth { get; }


    int MaxArmor { get; }

    int StartArmor { get; }

    float OverhealMaxHealth { get; }
    float OverhealMaxArmor { get; }
}

[Serializable]
public struct EnemyData
{
    [SerializeField] public string Name;

    [SerializeField] public bool CanAggro;


    [SerializeField] public bool IsFlying;


    [SerializeField] public int MaxHealth;


    [SerializeField] public int StartHealth;


    [SerializeField] public int MaxArmor;

    [SerializeField] public int StartArmor;

    [SerializeField] public bool CanBleed;


    [SerializeField] public float MoveSpeed;

    [SerializeField] public float Acceleration;

    [SerializeField] public float StoppingDistance;


    [SerializeField] public int DamageToFlinch;

    [SerializeField] public float TimeToResetFlinch;

    [SerializeField] public float FlinchDuration;

    [SerializeField] public float DegreesVisibility;


    [SerializeField] public float AggroDelayVariance;

    [SerializeField] public float AggroRange;

    [SerializeField] public float EarshotAggroRange;

    [SerializeField] public float OverhealMaxHealth;
    [SerializeField] public float OverhealMaxArmor;
    [SerializeField] public List<string> Attacks;
}


[Serializable]
public struct PlayerData
{
    [SerializeField] public int MaxHealth;

    [SerializeField] public int OverhealMaxHealth;
    [SerializeField] public int StartHealth;
    [SerializeField] public string difficulty;


    [SerializeField] public int MaxArmor;
    [SerializeField] public float OverhealMaxArmor;

    [SerializeField] public int StartArmor;


    [SerializeField] public float MoveSpeed;

    [SerializeField] public float Acceleration;
}