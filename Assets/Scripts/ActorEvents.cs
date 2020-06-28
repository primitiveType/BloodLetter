﻿using UnityEngine;

public class ActorEvents : MonoBehaviour
{
    public event OnShotEvent OnShotEvent;
    public event OnHealthChangedEvent OnHealthChangedEvent;
    public event OnStepEvent OnStepEvent;
    public event OnAttackEvent OnAttackEvent;
    public event OnDeathEvent OnDeathEvent;
    [SerializeField] private EnemySounds Sounds;
    [SerializeField] private AudioSource Source;
    public event OnAggroEvent OnAggroEvent;
    public event OnAmmoChangedEvent OnAmmoChangedEvent;

    /// <summary>
    /// Should really be called "OnDamaged"
    /// </summary>
    /// <param name="projectileInfo"></param>
    public void OnShot(IDamageSource projectileInfo)
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

    public void OnHealthChanged(float amount, bool isHealing = false)
    {
        OnHealthChangedEvent?.Invoke(this, new OnHealthChangedEventArgs(amount, isHealing));
    }

    public void OnAmmoChanged(int newValue, AmmoType type)
    {
        OnAmmoChangedEvent?.Invoke(this, new OnAmmoChangedEventArgs(newValue, type));
    }
}

public delegate void OnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args);

public class OnAmmoChangedEventArgs
{
    public OnAmmoChangedEventArgs(int newValue, AmmoType type)
    {
        NewValue = newValue;
        Type = type;
    }

    public int NewValue { get; }
    public AmmoType Type { get; }
}

public delegate void OnHealthChangedEvent(object sender, OnHealthChangedEventArgs args);

public class OnHealthChangedEventArgs
{
    public OnHealthChangedEventArgs(float amount, bool isHealing = false)
    {
        IsHealing = isHealing;
        Amount = amount;
    }

    public float Amount { get; }
    public bool IsHealing { get; }
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
    public IDamageSource ProjectileInfo { get; }

    public OnShotEventArgs(IDamageSource projectileInfo)
    {
        ProjectileInfo = projectileInfo;
    }
}

public interface IDamageSource
{
    float GetDamage(ActorHealth actorToDamage);
}