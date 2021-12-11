using System;
using UnityEngine;


public class TestUnequip : MonoBehaviour
{
    [SerializeField] private KeyCode EquipKey;
    [SerializeField] private bool equipOnStart;

    [SerializeField] private EquipStatus Equippable;
    [SerializeField] private PlayerInventory.EquipmentSlot Slot = PlayerInventory.EquipmentSlot.RightHand;

    private UltimateRadialButtonInfo RadialButtonInfo { get; set; }

    public PlayerRoot PlayerRoot { get; private set; }
    
    private WeaponId PreviousWeapon { get; set; }

    // Update is called once per frame
    private void Update()
    {
        // if (Input.GetKeyDown(EquipKey))
        // {
        //     Equip();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Q) && PreviousWeapon == Equippable.WeaponId)
        // {
        //     Equip();
        // }
    }

    private void Equip()
    {
        Toolbox.Instance.PlayerInventory.EquipThing(Equippable.WeaponId, Slot);
    }

    private void Start()
    {
        RadialButtonInfo = new UltimateRadialButtonInfo();
        RadialButtonInfo.icon = Equippable.WeaponSprite;
        RadialButtonInfo.name = WeaponPickup.GetWeaponName(Equippable.WeaponId);
        RadialButtonInfo.id = (int) Equippable.WeaponId;
        PlayerRoot = GetComponentInParent<PlayerRoot>();
        PlayerRoot.ActorEvents.OnEquippedWeaponChangedEvent += OnWeaponChanged;
        PlayerRoot.ActorEvents.OnWeaponsChangedEvent += OnInventoryChanged;
        var savedWeapon = SaveState.Instance.SaveData.InventoryData.EquippedWeapon;
        if (savedWeapon != 0)
            equipOnStart = SaveState.Instance.SaveData.InventoryData.EquippedWeapon.HasFlag(Equippable.WeaponId);

        if (equipOnStart)
            Equip();

        UpdateButtonVisibility();
    }

    private void OnInventoryChanged(object sender, OnWeaponsChangedEventArgs args)
    {
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        bool enable = Toolbox.Instance.PlayerInventory.HasWeapon(Equippable.WeaponId);
        if (enable && RadialButtonInfo.radialButton == null)
        {
            UltimateRadialMenu.RegisterToRadialMenu("WeaponWheel", RadialCallback, RadialButtonInfo);
        }
        else if (!enable && RadialButtonInfo.radialButton != null)
        {
            RadialButtonInfo.RemoveRadialButton();
        }
    }

    private void OnWeaponChanged(object sender, OnEquippedWeaponChangedEventArgs args)
    {
        if (args.NewValue == Equippable.WeaponId)
        {
            RadialButtonInfo.SelectButton();
        }

        PreviousWeapon = args.OldValue;
    }

    private void RadialCallback()
    {
        Equip();
    }
}