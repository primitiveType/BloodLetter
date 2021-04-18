public class OnEquippedWeaponChangedEventArgs
{
    public OnEquippedWeaponChangedEventArgs(WeaponId oldValue, WeaponId newValue, PlayerInventory.EquipmentSlot slot)
    {
        NewValue = newValue;
        Slot = slot;
        OldValue = oldValue;
    }

    public WeaponId NewValue { get; }
    public PlayerInventory.EquipmentSlot Slot { get; }
    public WeaponId OldValue { get; }
}