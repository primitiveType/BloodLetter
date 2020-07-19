using UnityEngine;

public class ActorEvents : MonoBehaviour
{
    public event OnShotEvent OnShotEvent;
    public event OnHealthChangedEvent OnHealthChangedEvent;
    public event OnArmorChangedEvent OnArmorChangedEvent;
    public event OnStepEvent OnStepEvent;
    public event OnAttackEvent OnAttackEvent;
    public event OnDeathEvent OnDeathEvent;
    [SerializeField] private EnemySounds Sounds;
    [SerializeField] private AudioSource Source;
    public event OnAggroEvent OnAggroEvent;
    public event OnAmmoChangedEvent OnAmmoChangedEvent;
    public event OnWeaponsChangedEvent OnWeaponsChangedEvent;
    public event OnEquippedWeaponChangedEvent OnEquippedWeaponChangedEvent;

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
    
    public void OnArmorChanged()
    {
        OnArmorChangedEvent?.Invoke(this, new OnArmorChangedEventArgs());
    }

    public void OnAmmoChanged(int oldValue, int newValue, AmmoType type)
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

public delegate void OnEquippedWeaponChangedEvent(object sender, OnEquippedWeaponChangedEventArgs args);

public class OnEquippedWeaponChangedEventArgs
{
    public OnEquippedWeaponChangedEventArgs(WeaponId oldValue, WeaponId newValue)
    {
        NewValue = newValue;
        OldValue = oldValue;
    }

    public WeaponId NewValue { get; }
    public WeaponId OldValue { get; }
}

public delegate void OnArmorChangedEvent(object sender, OnArmorChangedEventArgs args);

public class OnArmorChangedEventArgs
{
}

public delegate void OnWeaponsChangedEvent(object sender, OnWeaponsChangedEventArgs args);

public class OnWeaponsChangedEventArgs
{
    public OnWeaponsChangedEventArgs(WeaponId oldValue, WeaponId newValue)
    {
        NewValue = newValue;
        OldValue = oldValue;
    }

    public WeaponId NewValue { get; }
    public WeaponId OldValue { get; }
}

public delegate void OnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args);

public class OnAmmoChangedEventArgs
{
    public OnAmmoChangedEventArgs(int oldValue, int newValue, AmmoType type)
    {
        NewValue = newValue;
        OldValue = oldValue;
        Type = type;
    }

    public int NewValue { get; }
    public int OldValue { get; }
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

