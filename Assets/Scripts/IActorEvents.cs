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
    /// Should really be called "OnDamaged"
    /// </summary>
    /// <param name="projectileInfo"></param>
    void OnShot(IDamageSource projectileInfo);

    void OnStep();
    void OnAttack();
    void OnDeath();
    void OnAggro();
    void OnHealthChanged(float amount, bool isHealing = false);
    void OnArmorChanged();
    void OnAmmoChanged(int oldValue, int newValue, AmmoType type);
    void OnWeaponsChanged(WeaponId oldValue, WeaponId newValue);
    void OnEquippedWeaponChanged(WeaponId oldValue, WeaponId newValue);
}

