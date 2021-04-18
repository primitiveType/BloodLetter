using UnityEngine;

public class ActorEventsMediator : MonoBehaviour, IActorEvents
{
    [SerializeField] private ActorEvents _actorEventsImplementation;

    public event OnShotEvent OnShotEvent
    {
        add => _actorEventsImplementation.OnShotEvent += value;
        remove => _actorEventsImplementation.OnShotEvent -= value;
    }

    public event OnHealthChangedEvent OnHealthChangedEvent
    {
        add => _actorEventsImplementation.OnHealthChangedEvent += value;
        remove => _actorEventsImplementation.OnHealthChangedEvent -= value;
    }

    public event OnArmorChangedEvent OnArmorChangedEvent
    {
        add => _actorEventsImplementation.OnArmorChangedEvent += value;
        remove => _actorEventsImplementation.OnArmorChangedEvent -= value;
    }

    public event OnStepEvent OnStepEvent
    {
        add => _actorEventsImplementation.OnStepEvent += value;
        remove => _actorEventsImplementation.OnStepEvent -= value;
    }

    public event OnAttackEvent OnAttackEvent
    {
        add => _actorEventsImplementation.OnAttackEvent += value;
        remove => _actorEventsImplementation.OnAttackEvent -= value;
    }

    public event OnDeathEvent OnDeathEvent
    {
        add => _actorEventsImplementation.OnDeathEvent += value;
        remove => _actorEventsImplementation.OnDeathEvent -= value;
    }

    public event OnAggroEvent OnAggroEvent
    {
        add => _actorEventsImplementation.OnAggroEvent += value;
        remove => _actorEventsImplementation.OnAggroEvent -= value;
    }

    public event OnAmmoChangedEvent OnAmmoChangedEvent
    {
        add => _actorEventsImplementation.OnAmmoChangedEvent += value;
        remove => _actorEventsImplementation.OnAmmoChangedEvent -= value;
    }

    public event OnWeaponsChangedEvent OnWeaponsChangedEvent
    {
        add => _actorEventsImplementation.OnWeaponsChangedEvent += value;
        remove => _actorEventsImplementation.OnWeaponsChangedEvent -= value;
    }

    public event OnEquippedWeaponChangedEvent OnEquippedWeaponChangedEvent
    {
        add => _actorEventsImplementation.OnEquippedWeaponChangedEvent += value;
        remove => _actorEventsImplementation.OnEquippedWeaponChangedEvent -= value;
    }

    public void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        _actorEventsImplementation.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void OnStep()
    {
        OnStep(null, null);
    }
    public void OnStep(Vector3? lastPosition, Vector3? newPosition)
    {
        _actorEventsImplementation.OnStep(lastPosition, newPosition);//fix this later
    }

    public void OnAttack()
    {
        _actorEventsImplementation.OnAttack();
    }

    public void OnDeath()
    {
        _actorEventsImplementation.OnDeath();
    }

    public void OnAggro()
    {
        _actorEventsImplementation.OnAggro();
    }

    public void OnHealthChanged(float amount, bool isHealing = false)
    {
        _actorEventsImplementation.OnHealthChanged(amount, isHealing);
    }

    public void OnArmorChanged()
    {
        _actorEventsImplementation.OnArmorChanged();
    }

    public void OnAmmoChanged(float oldValue, float newValue, AmmoType type)
    {
        _actorEventsImplementation.OnAmmoChanged(oldValue, newValue, type);
    }

    public void OnWeaponsChanged(WeaponId oldValue, WeaponId newValue)
    {
        _actorEventsImplementation.OnWeaponsChanged(oldValue, newValue);
    }

    public void OnEquippedWeaponChanged(WeaponId oldValue, WeaponId newValue, PlayerInventory.EquipmentSlot slot)
    {
        _actorEventsImplementation.OnEquippedWeaponChanged(oldValue, newValue, slot);
    }
}