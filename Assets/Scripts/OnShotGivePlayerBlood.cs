using System;
using UnityEngine;

public class OnShotGivePlayerBlood : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;
    [SerializeField] private ActorHealth m_ActorHealth;


    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
        }

        EnemyData data = dataProvider.Data;
        enabled = data.CanBleed;
    }

    private void Start()
    {
        m_Events.OnShotEvent += OnShot;
        
    }

    private void OnDestroy()
    {
        m_Events.OnShotEvent -= OnShot;
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        if (!m_ActorHealth.IsAlive)
        {
            return;
        }
        if (args.ProjectileInfo.GetDamage().Type.HasFlag(DamageType.Physical))
        {
            Toolbox.Instance.PlayerEvents.OnBloodGained();
        }
    }
}