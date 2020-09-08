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