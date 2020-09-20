using UnityEngine;

public interface IActorEvents
{
    event OnShotEvent OnShotEvent;
    event OnHealthChangedEvent OnHealthChangedEvent;
    event OnArmorChangedEvent OnArmorChangedEvent;
    event OnStepEvent OnStepEvent;
    event OnAttackEvent OnAttackEvent;
    event OnDeathEvent OnDeathEvent;
    event OnAggroEvent OnAggroEvent;
    event OnAmmoChangedEvent OnAmmoChangedEvent;
    event OnWeaponsChangedEvent OnWeaponsChangedEvent;
    event OnEquippedWeaponChangedEvent OnEquippedWeaponChangedEvent;

    /// <summary>
    ///     Should really be called "OnDamaged"
    /// </summary>
    /// <param name="projectileInfo"></param>
    void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal);

    void OnStep(Vector3? prevPosition, Vector3? newPosition);
    void OnAttack();
    void OnDeath();
    void OnAggro();
    void OnHealthChanged(float amount, bool isHealing = false);
    void OnArmorChanged();
    void OnAmmoChanged(float oldValue, float newValue, AmmoType type);
    void OnWeaponsChanged(WeaponId oldValue, WeaponId newValue);
    void OnEquippedWeaponChanged(WeaponId oldValue, WeaponId newValue);
}