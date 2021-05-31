using SensorToolkit;
using UnityEngine;

public class UsesAmmoOnAllEnemies : MonoBehaviour, IShootCondition
{
    [SerializeField] private AmmoType AmmoType;
    [SerializeField] private float AmmoUsed = 1;
    [SerializeField] private EquipStatus EquipStatus;
    private PlayerEvents Events;
    private PlayerInventory Inventory;
    [SerializeField] private TriggerSensor m_Sensor;

    private void Start()
    {
        Inventory = Toolbox.Instance.PlayerInventory;
        Events = Toolbox.Instance.PlayerEvents;
        Events.PlayerShootEvent += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs playerShootEventArgs)
    {
        if (playerShootEventArgs.WeaponId != EquipStatus.WeaponId)
        {
            return;
        }
        if (isActiveAndEnabled && EquipStatus.IsEquipped) //hope this is good enough for now!    
            Inventory.UseAmmo(AmmoType, ActualAmmoUsed);
    }

    private float ActualAmmoUsed => AmmoUsed * m_Sensor.DetectedObjects.Count;

    public bool HasAmmo()
    {
        return Inventory.GetAmmoAmount(AmmoType) >= AmmoUsed && m_Sensor.DetectedObjects.Count > 0;
    }

    public bool CanShoot()
    {
        return HasAmmo() && !Toolbox.Instance.IsPlayerDead;
    }
}