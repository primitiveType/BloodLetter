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
    public event OnWeaponsChangedEvent OnWeaponsChangedEvent;
    public event OnEquippedWeaponChangedEvent OnEquippedWeaponChangedEvent;

    /// <summary>
    ///     Should really be called "OnDamaged"
    /// </summary>
    /// <param name="projectileInfo"></param>
    public void OnShot(IDamageSource projectileInfo, Vector3 worldPos)
    {
        //TODO: this should probably set an animator bool that fires an event
        OnShotEvent?.Invoke(this, new OnShotEventArgs(projectileInfo, worldPos));
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

    public void OnArmorChanged()
    {
        OnArmorChangedEvent?.Invoke(this, new OnArmorChangedEventArgs());
    }

    public void OnAmmoChanged(float oldValue, float newValue, AmmoType type)
    {
        OnAmmoChangedEvent?.Invoke(this, new OnAmmoChangedEventArgs(oldValue, newValue, type));
    }

    public void OnWeaponsChanged(WeaponId oldValue, WeaponId newValue)
    {
        OnWeaponsChangedEvent?.Invoke(this, new OnWeaponsChangedEventArgs(oldValue, newValue));
    }

    public void OnEquippedWeaponChanged(WeaponId oldValue, WeaponId newValue)
    {
        OnEquippedWeaponChangedEvent?.Invoke(this, new OnEquippedWeaponChangedEventArgs(oldValue, newValue));
    }
}