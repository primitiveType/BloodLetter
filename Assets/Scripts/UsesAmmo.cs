using UnityEngine;

public class UsesAmmo : MonoBehaviour
{

    [SerializeField] private AmmoType AmmoType;
    [SerializeField] private float AmmoUsed = 1;
    private PlayerInventory Inventory;
    private PlayerEvents Events;
    [SerializeField] private EquipStatus EquipStatus;

    private void Start()
    {
        Inventory = Toolbox.Instance.PlayerInventory;
        Events = Toolbox.Instance.PlayerEvents;
        Events.PlayerShootEvent += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs playerShootEventArgs)
    {
        if (isActiveAndEnabled && EquipStatus.IsEquipped)//hope this is good enough for now!    
        {
            Inventory.UseAmmo(AmmoType, AmmoUsed);
        }
    }

    public bool HasAmmo()
    {
        return Inventory.GetAmmoAmount(AmmoType) > 0;
    }
}