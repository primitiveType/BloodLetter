﻿using System;
using UnityEngine;

public class ActorEvents : MonoBehaviour, IActorEvents
{
    [SerializeField] private EnemySounds Sounds;
    [SerializeField] private AudioSource Source;
    public event OnShotEvent OnShotEvent;
    public event OnHealthChangedEvent OnHealthChangedEvent;
    public event OnArmorChangedEvent OnArmorChangedEvent;
    public event OnStepEvent OnStepEvent;
    public event OnAttackEvent OnAttackEvent;
    public event OnDeathEvent OnDeathEvent;
    public event OnAggroEvent OnAggroEvent;
    public event OnAmmoChangedEvent OnAmmoChangedEvent;
    public event OnKeysChangedEvent OnKeysChangedEvent;
    public event OnWeaponsChangedEvent OnWeaponsChangedEvent;
    public event OnEquippedWeaponChangedEvent OnEquippedWeaponChangedEvent;

    /// <summary>
    ///     Should really be called "OnDamaged"
    /// </summary>
    /// <param name="projectileInfo"></param>
    public void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        OnShotEvent?.Invoke(this, new OnShotEventArgs(projectileInfo, worldPos, hitNormal));
    }

    public void OnStep(Vector3? lastPosition, Vector3? newPosition)
    {
        OnStepEvent?.Invoke(this, new OnStepEventArgs(lastPosition, newPosition));
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

    public void OnArmorChanged()
    {
        OnArmorChangedEvent?.Invoke(this, new OnArmorChangedEventArgs());
    }

    public void OnAmmoChanged(float oldValue, float newValue, AmmoType type)
    {
        OnAmmoChangedEvent?.Invoke(this, new OnAmmoChangedEventArgs(oldValue, newValue, type));
    }
    
    public void OnKeysChanged(KeyType oldValue, KeyType newValue)
    {
        OnKeysChangedEvent.Invoke(this, new OnKeysChangedEventArgs(oldValue, newValue));
    }

    public void OnWeaponsChanged(WeaponId oldValue, WeaponId newValue)
    {
        OnWeaponsChangedEvent?.Invoke(this, new OnWeaponsChangedEventArgs(oldValue, newValue));
    }

    public void OnEquippedWeaponChanged(WeaponId oldValue, WeaponId newValue, PlayerInventory.EquipmentSlot slot)
    {
        OnEquippedWeaponChangedEvent?.Invoke(this, new OnEquippedWeaponChangedEventArgs(oldValue, newValue, slot));
    }
}