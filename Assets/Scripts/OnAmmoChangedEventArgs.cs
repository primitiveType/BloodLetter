public class OnAmmoChangedEventArgs
{
    public OnAmmoChangedEventArgs(float oldValue, float newValue, AmmoType type)
    {
        NewValue = newValue;
        OldValue = oldValue;
        Type = type;
    }

    public float NewValue { get; }
    public float OldValue { get; }
    public AmmoType Type { get; }
}