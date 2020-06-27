﻿using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event OnShotEvent OnShotEvent;
    public event OnStepEvent OnStepEvent;
    public event OnAttackEvent OnAttackEvent;
    public event OnDeathEvent OnDeathEvent;
    [SerializeField] private EnemySounds Sounds;
    [SerializeField] private AudioSource Source;
    public event OnAggroEvent OnAggroEvent;

    public void OnShot(HitscanProjectileInfo projectileInfo)
    {//TODO: this should probably set an animator bool that fires an event
        OnShotEvent?.Invoke(this, new OnShotEventArgs(projectileInfo));
    }

    public void OnStep()
    {
        OnStepEvent?.Invoke(this, new OnStepEventArgs());
    }

    public void OnAttack()
    {
        OnAttackEvent?.Invoke(this, new OnAttackEventArgs());
    }

    public void OnDeath()
    {
        OnDeathEvent?.Invoke(this, new OnDeathEventArgs());
    }

    public void OnAggro()
    {
        OnAggroEvent?.Invoke(this, new OnAggroEventArgs());
    }
}

public delegate void OnAggroEvent(object sender, OnAggroEventArgs args);

public class OnAggroEventArgs
{
}

public delegate void OnDeathEvent(object sender, OnDeathEventArgs args);

public class OnDeathEventArgs
{
}

public delegate void OnAttackEvent(object sender, OnAttackEventArgs args);

public class OnAttackEventArgs
{
}

public delegate void OnStepEvent(object sender, OnStepEventArgs args);

public class OnStepEventArgs
{
}

public delegate void OnShotEvent(object sender, OnShotEventArgs args);

public class OnShotEventArgs
{
    public HitscanProjectileInfo HitscanProjectileInfo { get; }

    public OnShotEventArgs(HitscanProjectileInfo projectileInfo)
    {
        HitscanProjectileInfo = projectileInfo;
    }
}